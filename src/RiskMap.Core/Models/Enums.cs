namespace RiskMap.Core.Models;

/// <summary>
/// État d'une information (version d'un service, système d'exploitation).
/// On distingue « masqué » (choix de l'administrateur) et « non déterminé » (échec de notre outil).
/// </summary>
public enum InfoState
{
    /// <summary>Notre outil n'a pas réussi à lire l'information. Cela ne dit rien sur la machine.</summary>
    Undetermined,

    /// <summary>L'information est connue.</summary>
    Identified,

    /// <summary>Le service répond mais cache volontairement sa version. Bonne pratique, mais le risque reste inconnu.</summary>
    Hidden,
}

/// <summary>Niveau de risque d'une machine : faible / moyen / élevé / critique.</summary>
public enum RiskLevel
{
    Low,
    Medium,
    High,
    Critical,
}
