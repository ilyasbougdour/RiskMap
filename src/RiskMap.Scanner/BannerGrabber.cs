using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace RiskMap.Scanner;

/// <summary>
/// Module Scanner — responsable : Ilyass.
/// Lit la bannière d'un service sur une connexion déjà ouverte.
/// On renvoie le texte brut (nettoyé des caractères non imprimables) : c'est l'analyse qui l'interprète.
/// </summary>
public static class BannerGrabber
{
    /// <summary>Ports qui parlent HTTP : on doit envoyer une requête pour obtenir une réponse.</summary>
    private static readonly HashSet<int> HttpPorts = new() { 80, 631, 8080 };

    private static readonly HashSet<int> HttpsPorts = new() { 443 };

    /// <summary>
    /// Ports qui n'envoient jamais de texte lisible tout seuls (protocoles binaires ou silencieux).
    /// Inutile d'attendre timeoutMs pour rien.
    /// </summary>
    private static readonly HashSet<int> SilentPorts = new() { 53, 135, 139, 445, 3389, 5432, 9100 };

    /// <summary>Taille maximum lue : une bannière tient en quelques centaines d'octets.</summary>
    private const int MaxBytes = 4096;

    /// <returns>La bannière, ou null si le service n'a rien envoyé d'exploitable.</returns>
    public static async Task<string?> ReadAsync(TcpClient client, int port, int timeoutMs, CancellationToken ct)
    {
        if (SilentPorts.Contains(port)) return null;

        // Un seul délai pour toute la lecture de la bannière.
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeout.CancelAfter(timeoutMs);
        var stream = client.GetStream();

        if (HttpsPorts.Contains(port))
        {
            // Le certificat n'est pas vérifié : on veut juste lire l'en-tête Server, pas se protéger.
            await using var ssl = new SslStream(stream, leaveInnerStreamOpen: true);
            await ssl.AuthenticateAsClientAsync(new SslClientAuthenticationOptions
            {
                TargetHost = ((System.Net.IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString(),
                RemoteCertificateValidationCallback = (_, _, _, _) => true,
            }, timeout.Token);
            return await ReadHttpServerAsync(ssl, timeout.Token, ct);
        }

        if (HttpPorts.Contains(port))
            return await ReadHttpServerAsync(stream, timeout.Token, ct);

        if (port == 3306)
            return ParseMySqlVersion(await ReadUntilAsync(stream, IsMySqlPacketComplete, timeout.Token, ct));

        if (port == 23)
            return await ReadTelnetAsync(stream, timeout.Token, ct);

        // Cas général : le service parle en premier (FTP, SSH, SMTP, POP3, IMAP, VNC...). On lit la 1re ligne.
        var data = await ReadUntilAsync(stream, d => d.Contains((byte)'\n'), timeout.Token, ct);
        return FirstLine(Clean(data));
    }

    // --- HTTP ---------------------------------------------------------------

    /// <summary>Envoie HEAD / et renvoie la valeur de l'en-tête « Server: ».</summary>
    private static async Task<string?> ReadHttpServerAsync(Stream stream, CancellationToken timeout, CancellationToken ct)
    {
        var request = Encoding.ASCII.GetBytes("HEAD / HTTP/1.0\r\nUser-Agent: RiskMap\r\n\r\n");
        await stream.WriteAsync(request, timeout);

        // Les en-têtes se terminent par une ligne vide.
        var data = await ReadUntilAsync(stream, d => IndexOf(d, "\r\n\r\n"u8) >= 0, timeout, ct);
        var headers = Encoding.Latin1.GetString(data.ToArray()).Split('\n');
        var server = headers.FirstOrDefault(h => h.StartsWith("Server:", StringComparison.OrdinalIgnoreCase));
        return server == null ? null : Clean(server["Server:".Length..]);
    }

    // --- MySQL --------------------------------------------------------------

    // Le serveur MySQL envoie un paquet de bienvenue binaire :
    //   3 octets longueur + 1 octet numéro | 1 octet version du protocole (10) | version en texte terminée par 0 | ...
    private static bool IsMySqlPacketComplete(List<byte> d) =>
        d.Count >= 4 && d.Count >= 4 + (d[0] | d[1] << 8 | d[2] << 16);

    private static string? ParseMySqlVersion(List<byte> d)
    {
        if (d.Count < 6) return null;
        if (d[4] == 0xFF) return null; // paquet d'erreur (ex. « Host is not allowed to connect ») : pas de version
        var end = d.IndexOf(0, 5);
        if (end < 0) end = d.Count;
        return Clean(d.GetRange(5, end - 5));
    }

    // --- Telnet -------------------------------------------------------------

    private const byte Iac = 255, Will = 251, Wont = 252, Do = 253, Dont = 254, Sb = 250, Se = 240;

    /// <summary>
    /// Telnet commence par négocier des options en binaire (IAC DO ..., IAC WILL ...).
    /// On refuse tout (WONT / DONT) pour que le serveur affiche son invite, ex. « metasploitable login: ».
    /// </summary>
    private static async Task<string?> ReadTelnetAsync(NetworkStream stream, CancellationToken timeout, CancellationToken ct)
    {
        var text = new List<byte>();
        var buffer = new byte[512];
        int state = 0;      // 0 = texte, 1 = après IAC, 2 = après IAC + WILL/DO..., 3 = dans une sous-négociation
        byte command = 0;

        try
        {
            while (text.Count < MaxBytes)
            {
                var n = await stream.ReadAsync(buffer, timeout);
                if (n == 0) break;
                var reply = new List<byte>();

                for (var i = 0; i < n; i++)
                {
                    var b = buffer[i];
                    switch (state)
                    {
                        case 0:
                            if (b == Iac) state = 1; else text.Add(b);
                            break;
                        case 1:
                            if (b is Will or Wont or Do or Dont) { command = b; state = 2; }
                            else if (b == Sb) state = 3;
                            else state = 0;
                            break;
                        case 2:
                            if (command == Do) reply.AddRange(new[] { Iac, Wont, b });
                            if (command == Will) reply.AddRange(new[] { Iac, Dont, b });
                            state = 0;
                            break;
                        case 3:
                            if (b == Se) state = 0; // fin de la sous-négociation (simplifié : on ignore son contenu)
                            break;
                    }
                }

                if (reply.Count > 0) await stream.WriteAsync(reply.ToArray(), timeout);

                // On s'arrête dès qu'une invite apparaît (« login: », « Password: », « > »...).
                var current = Clean(text);
                if (current != null && (current.EndsWith(':') || current.EndsWith('>')))
                    break;
            }
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested) { }
        catch (IOException) { }

        // Les lignes sont mises bout à bout : « Ubuntu 8.04 metasploitable login: ».
        var lines = Encoding.Latin1.GetString(text.ToArray())
            .Split('\n')
            .Select(l => Clean(l))
            .Where(l => l != null);
        var result = string.Join(" ", lines);
        return result.Length == 0 ? null : result;
    }

    // --- Outils -------------------------------------------------------------

    /// <summary>
    /// Lit jusqu'à ce que <paramref name="done"/> soit vrai, que le serveur ferme, ou que le délai expire.
    /// Renvoie tout ce qui a été reçu (même partiel). L'annulation par l'utilisateur (ct) remonte.
    /// </summary>
    private static async Task<List<byte>> ReadUntilAsync(Stream stream, Func<List<byte>, bool> done, CancellationToken timeout, CancellationToken ct)
    {
        var data = new List<byte>();
        var buffer = new byte[1024];
        try
        {
            while (data.Count < MaxBytes && !done(data))
            {
                var n = await stream.ReadAsync(buffer, timeout);
                if (n == 0) break; // le serveur a fermé la connexion
                data.AddRange(buffer.AsSpan(0, n).ToArray());
            }
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested) { } // délai dépassé : on garde ce qu'on a
        catch (IOException) { }                                                   // connexion coupée
        return data;
    }

    private static string? Clean(List<byte> data) => Clean(Encoding.Latin1.GetString(data.ToArray()));

    /// <summary>Remplace les caractères non imprimables par des espaces (sauf retours à la ligne) et enlève les espaces autour.</summary>
    private static string? Clean(string text)
    {
        var sb = new StringBuilder(text.Length);
        foreach (var c in text)
            sb.Append(c == '\n' || (c >= ' ' && c != (char)127 && c < (char)256) ? c : ' ');
        var result = sb.ToString().Trim();
        return result.Length == 0 ? null : result;
    }

    private static string? FirstLine(string? text) => text?.Split('\n')[0].Trim();

    private static int IndexOf(List<byte> data, ReadOnlySpan<byte> pattern) =>
        data.ToArray().AsSpan().IndexOf(pattern);
}
