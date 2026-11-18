using RiskMap.Core.Models;
using RiskMap.Core.Storage;

namespace RiskMap.Analysis.Scoring;

/// <summary>Contenu du fichier rules/port-rules.json (modifiable sans toucher au code).</summary>
public class RiskRules
{
    public List<PortRule> PortRules { get; set; } = new();

    public CvePoints CvePoints { get; set; } = new();

    public LevelThresholds Levels { get; set; } = new();

    public static RiskRules Load(string path) => JsonStore.Load<RiskRules>(path);
}

public class PortRule
{
    public int Port { get; set; }

    /// <summary>Nom affiché, ex. « Telnet ».</summary>
    public string Label { get; set; } = "";

    public int Points { get; set; }

    public string Reason { get; set; } = "";
}

/// <summary>Points ajoutés selon la gravité d'une CVE.</summary>
public class CvePoints
{
    public int Critical { get; set; }
    public int High { get; set; }
    public int Medium { get; set; }
    public int Low { get; set; }
}

/// <summary>Score minimum pour chaque niveau (en dessous de Medium : faible).</summary>
public class LevelThresholds
{
    public int Medium { get; set; }
    public int High { get; set; }
    public int Critical { get; set; }
}

/// <summary>Calcule le score de risque d'une machine : règles de ports + CVE.</summary>
public class RiskScorer
{
    private readonly RiskRules _rules;

    public RiskScorer(RiskRules rules) => _rules = rules;

    public RiskAssessment Score(Host host)
    {
        // TODO :
        // 1. Pour chaque service, chercher une règle sur son port → RiskFactor { Source = "port", ... }.
        // 2. Pour les CVE : décider la règle (toutes les CVE ? la plus grave par service ?).
        //    Apache 2.2.8 a des dizaines de CVE : tout additionner fait exploser le score.
        //    → RiskFactor { Source = "cve", ... }.
        // 3. Score = somme des points ; Level selon _rules.Levels.
        // Les données d'exemple (FakeData) montrent le résultat attendu.
        throw new NotImplementedException("Calcul du score pas encore écrit (RiskScorer).");
    }
}
