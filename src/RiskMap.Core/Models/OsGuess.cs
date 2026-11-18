namespace RiskMap.Core.Models;

/// <summary>Estimation du système d'exploitation, construite en croisant plusieurs indices.</summary>
public class OsGuess
{
    public InfoState State { get; set; } = InfoState.Undetermined;

    /// <summary>Ex. « Windows », « Linux (Ubuntu) », « Imprimante ».</summary>
    public string? Name { get; set; }

    /// <summary>Confiance de 0 à 100.</summary>
    public int Confidence { get; set; }

    /// <summary>Indices utilisés, affichés dans le rapport. Ex. « TTL 128 », « ports 135 + 445 ».</summary>
    public List<string> Clues { get; set; } = new();
}
