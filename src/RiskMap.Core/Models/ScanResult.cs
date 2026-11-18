namespace RiskMap.Core.Models;

/// <summary>Résultat complet d'un scan : c'est ce qui est enregistré en JSON et affiché par l'interface.</summary>
public class ScanResult
{
    /// <summary>Réseau scanné, par exemple « 192.168.56.0/24 ».</summary>
    public string Target { get; set; } = "";

    public DateTime StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public List<Host> Hosts { get; set; } = new();
}
