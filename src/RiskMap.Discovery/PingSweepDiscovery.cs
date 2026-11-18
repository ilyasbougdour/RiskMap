using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Net.Sockets;
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
    var addresses = SubnetHelper.Expand(target);
    progress?.Report($"Ping de {addresses.Count} adresse(s)...");

    // Le « portier » : au plus MaxParallelPings pings en même temps.
    using var limiter = new SemaphoreSlim(MaxParallelPings);

    // Collection prévue pour être remplie par plusieurs tâches à la fois.
    var found = new ConcurrentBag<Host>();

    var tasks = addresses.Select(async ip =>
    {
        await limiter.WaitAsync(ct);   // attend qu'une place se libère
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(ip, TimeoutMs);

            if (reply.Status == IPStatus.Success)
           {
               var host = new Host { IpAddress = ip.ToString(), Ttl = reply.Options?.Ttl };

               host.MacAddress = ArpTable.GetMac(ip);
               host.Vendor = _vendors.Find(host.MacAddress);

    // Le nom d'hôte échoue souvent : on limite l'attente à 2 secondes.
               try
             {
                 using var dnsCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                 dnsCts.CancelAfter(2000);
                 var entry = await Dns.GetHostEntryAsync(ip.ToString(), dnsCts.Token);
                 host.Hostname = entry.HostName;
         }
              catch { /* pas de nom : ce n'est pas grave */ }

            found.Add(host);
            progress?.Report($"Machine trouvée : {ip}");
         }  
        }
        catch (PingException) { /* adresse injoignable : on ignore */ }
        finally { limiter.Release(); } // toujours rendre la place
    });

    await Task.WhenAll(tasks);   // attend la fin de tous les pings

    // Tri par adresse IP.
    return found.OrderBy(h => SubnetHelper.ToUInt(IPAddress.Parse(h.IpAddress))).ToList();
 }
}

/// <summary>Découpe une notation CIDR en adresses IP.</summary>
public static class SubnetHelper
{
    public static List<IPAddress> Expand(string cidr)
    {
        if (string.IsNullOrWhiteSpace(cidr))
            throw new ArgumentException("La cible est vide.");

        // "192.168.56.0/24" devient ["192.168.56.0", "24"]
        var parts = cidr.Trim().Split('/');

        if (!IPAddress.TryParse(parts[0], out var ip) || ip.AddressFamily != AddressFamily.InterNetwork)
            throw new ArgumentException($"Adresse IPv4 invalide : {parts[0]}");

        // Une adresse seule : on renvoie une liste avec une seule machine.
        if (parts.Length == 1)
            return new List<IPAddress> { ip };

        if (parts.Length != 2 || !int.TryParse(parts[1], out var prefix) || prefix < 0 || prefix > 32)
            throw new ArgumentException($"Notation CIDR invalide : {cidr}");

        if (prefix < 16)
            throw new ArgumentException("Réseau trop grand (maximum /16).");

        uint address = ToUInt(ip);
        uint mask = uint.MaxValue << (32 - prefix);   // /24 donne 255.255.255.0
        uint network = address & mask;                // adresse du réseau
        uint broadcast = network | ~mask;             // adresse de broadcast

        var result = new List<IPAddress>();

        if (prefix == 32) { result.Add(FromUInt(network)); return result; }
        if (prefix == 31) { result.Add(FromUInt(network)); result.Add(FromUInt(broadcast)); return result; }

        for (uint a = network + 1; a < broadcast; a++)
            result.Add(FromUInt(a));

        return result;
    }

    // 192.168.56.1 devient un entier de 32 bits.
    internal static uint ToUInt(IPAddress ip)
    {
        var b = ip.GetAddressBytes();
        return ((uint)b[0] << 24) | ((uint)b[1] << 16) | ((uint)b[2] << 8) | b[3];
    }

    private static IPAddress FromUInt(uint v) =>
        new IPAddress(new[] { (byte)(v >> 24), (byte)(v >> 16), (byte)(v >> 8), (byte)v });
}

/// <summary>Lit l'adresse MAC d'une machine du même sous-réseau.</summary>
public static class ArpTable
{
    // On « emprunte » une fonction de Windows (iphlpapi.dll) pour demander la MAC d'une IP.
    [DllImport("iphlpapi.dll", ExactSpelling = true)]
    private static extern int SendARP(uint destIp, uint srcIp, byte[] macAddr, ref int physicalAddrLen);

    public static string? GetMac(IPAddress ip)
    {
        try
        {
            uint dest = BitConverter.ToUInt32(ip.GetAddressBytes(), 0);
            var mac = new byte[6];
            int len = mac.Length;

            // 0 = succès. Sinon la machine est sur un autre réseau ou ne répond pas à l'ARP.
            if (SendARP(dest, 0, mac, ref len) != 0 || len != 6)
                return null;

            return string.Join(":", mac.Select(b => b.ToString("X2")));   // AA:BB:CC:DD:EE:FF
        }
        catch
        {
            return null;
        }
    }
}
/// <summary>Trouve le fabricant à partir des 3 premiers octets de l'adresse MAC (OUI).</summary>
public class MacVendorLookup
{
    private readonly Dictionary<string, string> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["080027"] = "PCS Systemtechnik (VirtualBox)",
        ["525400"] = "QEMU/KVM",
        ["005056"] = "VMware",
        ["000C29"] = "VMware",
        ["000569"] = "VMware",
        ["001C42"] = "Parallels",
        ["00155D"] = "Microsoft (Hyper-V)",
    };

    public MacVendorLookup()
    {
        // Liste complète optionnelle : src/RiskMap.Discovery/data/oui.csv
        var path = Path.Combine(AppContext.BaseDirectory, "data", "oui.csv");
        if (!File.Exists(path)) return;

        foreach (var line in File.ReadLines(path).Skip(1))   // Skip(1) = on saute l'en-tête
        {
            var cols = SplitCsv(line);
            if (cols.Count >= 3 && cols[1].Length == 6)
                _map[cols[1]] = cols[2];                     // « 080027 » -> « PCS Systemtechnik GmbH »
        }
    }

    public string? Find(string? mac)
    {
        if (string.IsNullOrWhiteSpace(mac)) return null;

        // « 08:00:27:AA:BB:CC » -> « 080027 »
        var clean = mac.Replace(":", "").Replace("-", "");
        if (clean.Length < 6) return null;

        return _map.TryGetValue(clean[..6], out var vendor) ? vendor : null;
    }

    // Découpe une ligne CSV en tenant compte des guillemets : "Foo, Inc.",bar
    private static List<string> SplitCsv(string line)
    {
        var result = new List<string>();
        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        foreach (var c in line)
        {
            if (c == '"') inQuotes = !inQuotes;
            else if (c == ',' && !inQuotes) { result.Add(current.ToString()); current.Clear(); }
            else current.Append(c);
        }
        result.Add(current.ToString());
        return result;
    }
}