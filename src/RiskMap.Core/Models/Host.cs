namespace RiskMap.Core.Models;

/// <summary>
/// Une machine du réseau. Chaque module remplit sa partie :
///   Découverte : IpAddress, Ttl, MacAddress, Vendor, Hostname
///   Scanner    : Services (Port + RawBanner), Os
///   Analyse    : Services (Product, Version, Vulnerabilities), Risk, AiSummary
/// </summary>
public class Host
{
    // --- Découverte ---------------------------------------------------------

    public string IpAddress { get; set; } = "";

    public string? Hostname { get; set; }

    /// <summary>Format « AA:BB:CC:DD:EE:FF ». Disponible seulement sur le même sous-réseau.</summary>
    public string? MacAddress { get; set; }

    /// <summary>Fabricant déduit de l'adresse MAC (ex. « Hewlett Packard »).</summary>
    public string? Vendor { get; set; }

    /// <summary>TTL lu dans la réponse au ping. Environ 64 = Linux/macOS, 128 = Windows, 255 = équipement réseau.</summary>
    public int? Ttl { get; set; }

    // --- Scanner ------------------------------------------------------------

    public List<Service> Services { get; set; } = new();

    public OsGuess Os { get; set; } = new();

    // --- Analyse ------------------------------------------------------------

    public RiskAssessment? Risk { get; set; }

    /// <summary>Synthèse rédigée par l'IA pour cette machine.</summary>
    public string? AiSummary { get; set; }
}
