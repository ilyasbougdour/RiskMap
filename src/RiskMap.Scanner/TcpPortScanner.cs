using System.Net.Sockets;
using RiskMap.Core.Contracts;
using RiskMap.Core.Models;

namespace RiskMap.Scanner;

/// <summary>
/// Module Scanner — responsable : Ilyass.
/// Teste une liste de ports TCP et lit ce que chaque service annonce (sa bannière).
/// </summary>
public class TcpPortScanner : IPortScanner
{
    /// <summary>Ports testés, avec leur nom usuel.</summary>
    public Dictionary<int, string> Ports { get; set; } = new()
    {
        [21] = "ftp", [22] = "ssh", [23] = "telnet", [25] = "smtp", [53] = "dns",
        [80] = "http", [110] = "pop3", [135] = "msrpc", [139] = "netbios-ssn", [143] = "imap",
        [443] = "https", [445] = "smb", [631] = "ipp", [3306] = "mysql", [3389] = "rdp",
        [5432] = "postgresql", [5900] = "vnc", [8080] = "http-alt", [9100] = "jetdirect",
    };

    public int ConnectTimeoutMs { get; set; } = 800;

    public int BannerTimeoutMs { get; set; } = 2000;

    public async Task ScanAsync(Host host, CancellationToken ct)
    {
        // TODO (Ilyass) :
        // 1. Pour chaque port, en parallèle :
        //      using var client = new TcpClient();
        //      using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
        //      timeout.CancelAfter(ConnectTimeoutMs);
        //      await client.ConnectAsync(host.IpAddress, port, timeout.Token);
        //    Connexion réussie = port ouvert. Exception ou délai dépassé = fermé ou filtré.
        // 2. Pour chaque port ouvert, appeler BannerGrabber.ReadAsync.
        // 3. Remplir host.Services (Port, Name, RawBanner), trié par port.
        //    Ne PAS interpréter la bannière : l'analyse s'en charge. On garde le texte brut.
        //
        // Pour tester sans le reste de l'application :
        //   dotnet run --project src/RiskMap.Cli -- scan 192.168.56.101
        await Task.CompletedTask;
        throw new NotImplementedException("Scan des ports pas encore écrit (TcpPortScanner).");
    }
}

/// <summary>Lit la bannière d'un service sur une connexion ouverte.</summary>
public static class BannerGrabber
{
    public static Task<string?> ReadAsync(TcpClient client, int port, int timeoutMs, CancellationToken ct)
    {
        // TODO (Ilyass) : trois cas.
        //   - Services qui parlent en premier (FTP, SSH, SMTP, Telnet, MySQL) : lire ce qui arrive
        //     pendant timeoutMs. Ex. « SSH-2.0-OpenSSH_8.9p1 Ubuntu-3ubuntu0.10 ».
        //   - HTTP (80, 8080) : envoyer « HEAD / HTTP/1.0\r\n\r\n » et renvoyer la valeur de l'en-tête « Server: ».
        //   - HTTPS (443) : même chose à travers un SslStream (accepter tous les certificats, c'est un scanner).
        // Renvoyer null si rien n'arrive. Nettoyer les caractères non imprimables (MySQL envoie du binaire).
        throw new NotImplementedException();
    }
}
