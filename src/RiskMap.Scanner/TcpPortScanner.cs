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
        // Tous les ports sont testés en même temps : le scan d'une machine dure
        // environ ConnectTimeoutMs (+ lecture des bannières), et non la somme des délais.
        var tasks = Ports.Select(p => ScanPortAsync(host.IpAddress, p.Key, p.Value, ct));
        var results = await Task.WhenAll(tasks);

        // Ports fermés ou filtrés = null : on ne garde que les ouverts, triés par numéro.
        // On garde la bannière brute : c'est le module Analyse qui l'interprète.
        host.Services = results.OfType<Service>().OrderBy(s => s.Port).ToList();
    }

    /// <summary>Teste un port. Renvoie le service s'il est ouvert, null s'il est fermé ou filtré.</summary>
    private async Task<Service?> ScanPortAsync(string ip, int port, string name, CancellationToken ct)
    {
        using var client = new TcpClient();
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeout.CancelAfter(ConnectTimeoutMs);

        try
        {
            await client.ConnectAsync(ip, port, timeout.Token);
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            return null; // délai dépassé : port filtré (pare-feu) ou machine qui ne répond pas
        }
        catch (SocketException)
        {
            return null; // connexion refusée : port fermé
        }
        // Si c'est l'utilisateur qui a annulé (ct), l'exception remonte et arrête le scan.

        var service = new Service { Port = port, Name = name };
        try
        {
            service.RawBanner = await BannerGrabber.ReadAsync(client, port, BannerTimeoutMs, ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            // Bannière illisible : le port reste ouvert, RawBanner reste null.
        }
        return service;
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
