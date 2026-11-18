namespace RiskMap.Core.Models;

/// <summary>Un port ouvert sur une machine et le logiciel qui l'écoute.</summary>
public class Service
{
    // --- Rempli par le scanner -----------------------------------

    public int Port { get; set; }

    public string Protocol { get; set; } = "tcp";

    /// <summary>Nom usuel déduit du port (ssh, http, smb...).</summary>
    public string? Name { get; set; }

    /// <summary>
    /// Texte brut renvoyé par le service, sans modification.
    /// Ex. « SSH-2.0-OpenSSH_8.9p1 Ubuntu-3ubuntu0.10 » ou, pour HTTP, la valeur de l'en-tête Server.
    /// Null si le service n'a rien envoyé.
    /// </summary>
    public string? RawBanner { get; set; }

    // --- Rempli par l'analyse ---------------------------------------

    public InfoState VersionState { get; set; } = InfoState.Undetermined;

    /// <summary>Logiciel identifié (ex. « OpenSSH », « Apache httpd »).</summary>
    public string? Product { get; set; }

    /// <summary>Version identifiée (ex. « 8.9p1 »). Null si masquée ou non déterminée.</summary>
    public string? Version { get; set; }

    public List<Vulnerability> Vulnerabilities { get; set; } = new();
}
