using System;

namespace RiskMap
{
	/// <summary>
	/// État d'une information déduite par le scan (version d'un service, système d'une machine).
	/// </summary>
	public enum EtatInfo
	{
		//l'outil n'a pas réussi à lire l'information (délai dépassé, format inconnu...)
		NonDetermine,
		//l'information est connue
		Identifie,
		//le service répond mais cache volontairement sa version
		Masque
	}

	/// <summary>
	/// Niveau de risque d'une machine, calculé par le moteur de risque.
	/// </summary>
	public enum NiveauRisque
	{
		Faible,
		Moyen,
		Eleve,
		Critique
	}
}
