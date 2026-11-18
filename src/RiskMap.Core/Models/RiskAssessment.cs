namespace RiskMap.Core.Models;

/// <summary>Score de risque d'une machine et le détail de son calcul.</summary>
public class RiskAssessment
{
    public int Score { get; set; }

    public RiskLevel Level { get; set; }

    /// <summary>Chaque ligne explique d'où viennent des points (règle de port ou CVE).</summary>
    public List<RiskFactor> Factors { get; set; } = new();
}

public class RiskFactor
{
    /// <summary>« port » pour une règle du fichier de règles, « cve » pour une vulnérabilité.</summary>
    public string Source { get; set; } = "";

    public int Points { get; set; }

    public string Reason { get; set; } = "";
}
