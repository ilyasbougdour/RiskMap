using RiskMap.Core.Contracts;
using RiskMap.Core.Models;

namespace RiskMap.Scanner;

/// <summary>
/// Module Scanner — responsable : Ilyass.
/// Devine le système d'exploitation en croisant les indices déjà collectés.
/// Chaque indice donne des points à une famille de systèmes ; la famille qui a le plus de points gagne.
/// </summary>
public class OsDetector : IOsDetector
{
    private const string Windows = "Windows", Linux = "Linux", MacOs = "macOS",
                         Printer = "Imprimante", Network = "Équipement réseau";

    // Poids des indices. Une bannière nomme le système : c'est l'indice le plus fort.
    private const int TtlWeight = 30, FirstBannerWeight = 40, OtherBannerWeight = 15,
                      PortsWeight = 30, VendorWeight = 30;

    /// <summary>Mots cherchés dans les bannières → famille, et distribution pour Linux. L'ordre compte : Ubuntu avant Debian.</summary>
    private static readonly (string Keyword, string Family, string? Detail)[] BannerKeywords =
    {
        ("ubuntu", Linux, "Ubuntu"), ("debian", Linux, "Debian"), ("centos", Linux, "CentOS"),
        ("red hat", Linux, "Red Hat"), ("fedora", Linux, "Fedora"), ("raspbian", Linux, "Raspbian"),
        ("linux", Linux, null),
        ("microsoft", Windows, null), ("windows", Windows, null), ("win64", Windows, null), ("win32", Windows, null),
        ("darwin", MacOs, null), ("mac os", MacOs, null),
        ("laserjet", Printer, null), ("jetdirect", Printer, null), ("printer", Printer, null),
        ("cisco", Network, null), ("routeros", Network, null), ("mikrotik", Network, null),
    };

    /// <summary>Mots cherchés dans le fabricant de la carte réseau (adresse MAC) → famille et marque affichée.</summary>
    private static readonly (string Keyword, string Family, string Brand)[] VendorKeywords =
    {
        ("hewlett", Printer, "HP"), ("epson", Printer, "Epson"), ("canon", Printer, "Canon"),
        ("brother", Printer, "Brother"), ("lexmark", Printer, "Lexmark"), ("xerox", Printer, "Xerox"),
        ("cisco", Network, "Cisco"), ("netgear", Network, "Netgear"), ("tp-link", Network, "TP-Link"),
        ("ubiquiti", Network, "Ubiquiti"), ("mikrotik", Network, "MikroTik"),
        ("apple", MacOs, "Apple"),
    };

    public void Detect(Host host)
    {
        var scores = new Dictionary<string, int>();
        var clues = new Dictionary<string, List<string>>(); // indices, rangés par famille
        string? linuxDistro = null, brand = null;

        void Add(string family, int points, string clue)
        {
            scores[family] = scores.GetValueOrDefault(family) + points;
            if (!clues.ContainsKey(family)) clues[family] = new List<string>();
            clues[family].Add(clue);
        }

        // 1. TTL : valeur de départ du système, diminuée de 1 par routeur traversé.
        if (host.Ttl is int ttl)
        {
            if (ttl <= 64) Add(Linux, TtlWeight, $"TTL {ttl}");          // Linux et macOS partent de 64
            else if (ttl <= 128) Add(Windows, TtlWeight, $"TTL {ttl}");  // Windows part de 128
            else
            {
                // 255 : imprimantes et équipements réseau. On partage les points entre les deux.
                Add(Printer, TtlWeight / 2, $"TTL {ttl}");
                Add(Network, TtlWeight / 2, $"TTL {ttl}");
            }
        }

        // 2. Bannières : un seul indice par bannière (le premier mot trouvé).
        var bannerFamilies = new HashSet<string>();
        foreach (var service in host.Services.Where(s => !string.IsNullOrWhiteSpace(s.RawBanner)))
        {
            var banner = service.RawBanner!;
            foreach (var (keyword, family, detail) in BannerKeywords)
            {
                var index = banner.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
                if (index < 0) continue;

                // La 1re bannière d'une famille compte beaucoup ; les suivantes confirment.
                var points = bannerFamilies.Add(family) ? FirstBannerWeight : OtherBannerWeight;
                Add(family, points, $"bannière {ServiceLabel(service)} « {WordAt(banner, index)} »");
                if (family == Linux) linuxDistro ??= detail;
                break;
            }
        }

        // 3. Combinaisons de ports typiques.
        var ports = host.Services.Select(s => s.Port).ToHashSet();
        if (ports.Contains(135) && ports.Contains(445))
            Add(Windows, PortsWeight, ports.Contains(3389) ? "ports 135 + 445 + 3389 ouverts" : "ports 135 + 445 ouverts");
        else if (ports.Contains(3389))
            Add(Windows, PortsWeight / 2, "port 3389 (bureau à distance) ouvert");
        if (ports.Contains(9100))
            Add(Printer, PortsWeight, "port 9100 (impression)");

        // 4. Fabricant de la carte réseau.
        if (!string.IsNullOrWhiteSpace(host.Vendor))
        {
            foreach (var (keyword, family, vendorBrand) in VendorKeywords)
            {
                if (!host.Vendor.Contains(keyword, StringComparison.OrdinalIgnoreCase)) continue;
                Add(family, VendorWeight, $"fabricant MAC {host.Vendor}");
                brand = vendorBrand;
                break;
            }
        }

        // 5. Résultat : la famille avec le plus de points.
        if (scores.Count == 0)
        {
            host.Os = new OsGuess { State = InfoState.Undetermined };
            return;
        }

        var ranked = scores.OrderByDescending(s => s.Value).ToList();
        var (best, bestScore) = (ranked[0].Key, ranked[0].Value);
        var runnerUp = ranked.Count > 1 ? ranked[1].Value : 0;

        host.Os = new OsGuess
        {
            State = InfoState.Identified,
            Name = best switch
            {
                Linux when linuxDistro != null => $"Linux ({linuxDistro})",
                Printer or Network when brand != null => $"{best} ({brand})",
                _ => best,
            },
            // Plus il y a de points, plus on est sûr ; les indices contradictoires font baisser la confiance.
            Confidence = Math.Clamp(bestScore - runnerUp / 2, 10, 95),
            Clues = clues[best],
        };
    }

    /// <summary>Nom court du service pour l'indice : « SSH », « HTTP »...</summary>
    private static string ServiceLabel(Service s) => (s.Name ?? $"port {s.Port}").ToUpperInvariant();

    /// <summary>Le mot (séparé par des espaces) qui contient la position donnée. Ex. « Debian-8ubuntu1 ».</summary>
    private static string WordAt(string text, int index)
    {
        var start = text.LastIndexOf(' ', index) + 1;
        var end = text.IndexOf(' ', index);
        return text[start..(end < 0 ? text.Length : end)];
    }
}
