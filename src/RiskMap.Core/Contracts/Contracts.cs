using RiskMap.Core.Models;

namespace RiskMap.Core.Contracts;

// Les contrats entre les modules. Chaque interface a :
//   - une vraie implémentation, écrite par le responsable du module ;
//   - une fausse implémentation (dossier Fakes) qui renvoie des données d'exemple.
// Tant qu'une vraie implémentation n'est pas prête, les autres utilisent la fausse.
//
// Ne modifiez pas ce fichier sans prévenir les deux autres : c'est le seul endroit
// où un changement peut casser le travail de tout le monde.

/// <summary>Module Découverte : trouve les machines actives d'un réseau.</summary>
public interface IHostDiscovery
{
    /// <param name="target">Réseau à scanner, ex. « 192.168.56.0/24 ».</param>
    /// <returns>Les machines qui répondent, avec IpAddress et Ttl remplis (MacAddress et Vendor si possible).</returns>
    Task<List<Host>> DiscoverAsync(string target, IProgress<string>? progress, CancellationToken ct);
}

/// <summary>Module Scanner : trouve les ports ouverts d'une machine et lit leurs bannières.</summary>
public interface IPortScanner
{
    /// <summary>Remplit host.Services (Port, Name, RawBanner). Ne touche pas aux autres champs.</summary>
    Task ScanAsync(Host host, CancellationToken ct);
}

/// <summary>Module Scanner : devine le système d'exploitation à partir des indices déjà collectés.</summary>
public interface IOsDetector
{
    /// <summary>Remplit host.Os à partir de host.Ttl, host.Vendor, host.Services (ports et bannières brutes).</summary>
    void Detect(Host host);
}

/// <summary>Module Analyse : versions, CVE, score de risque et synthèse IA.</summary>
public interface IHostAnalyzer
{
    /// <summary>
    /// Remplit, pour chaque service : VersionState, Product, Version, Vulnerabilities.
    /// Puis host.Risk et host.AiSummary.
    /// </summary>
    Task AnalyzeAsync(Host host, CancellationToken ct);
}
