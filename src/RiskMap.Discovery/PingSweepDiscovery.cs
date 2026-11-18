using System.Net;
using System.Net.NetworkInformation;
using RiskMap.Core.Contracts;
using RiskMap.Core.Models;

namespace RiskMap.Discovery;

/// <summary>
/// Module Découverte — responsable : Aida.
/// Trouve les machines actives d'un réseau en envoyant un ping à chaque adresse.
/// </summary>
public class PingSweepDiscovery : IHostDiscovery
{
    /// <summary>Temps d'attente maximum d'une réponse au ping, en millisecondes.</summary>
    public int TimeoutMs { get; set; } = 1000;

    /// <summary>Nombre de pings envoyés en même temps.</summary>
    public int MaxParallelPings { get; set; } = 64;

    private readonly MacVendorLookup _vendors = new();

    public async Task<List<Host>> DiscoverAsync(string target, IProgress<string>? progress, CancellationToken ct)
    {
        // TODO (Aida) :
        // 1. Transformer « 192.168.56.0/24 » en liste d'adresses avec SubnetHelper.Expand (à écrire).
        // 2. Pour chaque adresse, en parallèle (SemaphoreSlim avec MaxParallelPings) :
        //      using var ping = new Ping();
        //      PingReply reply = await ping.SendPingAsync(ip, TimeoutMs);
        //      si reply.Status == IPStatus.Success :
        //          new Host { IpAddress = ..., Ttl = reply.Options?.Ttl }
        // 3. Pour chaque machine trouvée : MacAddress avec ArpTable.GetMac(ip), puis Vendor avec _vendors.Find(mac).
        // 4. Bonus : Hostname avec Dns.GetHostEntryAsync(ip) (dans un try/catch, souvent ça échoue).
        // 5. Trier la liste par adresse IP avant de la renvoyer.
        //
        // Pour tester sans le reste de l'application :
        //   dotnet run --project src/RiskMap.Cli -- discover 192.168.56.0/24
        await Task.CompletedTask;
        throw new NotImplementedException("Découverte pas encore écrite (PingSweepDiscovery).");
    }
}

/// <summary>Découpe une notation CIDR en adresses IP.</summary>
public static class SubnetHelper
{
    /// <summary>
    /// « 192.168.56.0/24 » → 192.168.56.1 à 192.168.56.254 (sans l'adresse réseau ni le broadcast).
    /// Accepte aussi une adresse seule (« 192.168.56.101 »).
    /// </summary>
    public static List<IPAddress> Expand(string cidr)
    {
        // TODO (Aida) : convertir l'adresse en entier 32 bits, appliquer le masque,
        // puis énumérer de (réseau + 1) à (broadcast - 1).
        // Refuser les réseaux plus grands que /16 pour éviter un scan interminable.
        throw new NotImplementedException();
    }
}

/// <summary>Lit l'adresse MAC d'une machine du même sous-réseau.</summary>
public static class ArpTable
{
    public static string? GetMac(IPAddress ip)
    {
        // TODO (Aida) : deux possibilités.
        //   - Simple : lancer « arp -a » avec Process.Start et lire la ligne de l'adresse.
        //   - Propre : appeler la fonction Windows SendARP (iphlpapi.dll) avec [DllImport].
        // Renvoyer null si la machine est sur un autre réseau (pas d'adresse MAC visible).
        return null;
    }
}

/// <summary>Trouve le fabricant à partir des 3 premiers octets de l'adresse MAC (OUI).</summary>
public class MacVendorLookup
{
    public string? Find(string? mac)
    {
        // TODO (Aida) : charger une fois la liste officielle de l'IEEE (fichier oui.csv,
        // à mettre dans le dossier data/), puis chercher le préfixe « 08:00:27 » → « PCS Systemtechnik ».
        // Pas besoin d'internet pendant le scan.
        return null;
    }
}
