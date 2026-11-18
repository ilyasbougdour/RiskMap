using System;

namespace RiskMap
{
	/// <summary>
	/// Textes affichés à l'utilisateur (avec les accents, que les enum ne peuvent pas avoir).
	/// </summary>
	public class Textes
	{
		public static string niveau(NiveauRisque n){
			switch(n){
				case NiveauRisque.Critique: return "Critique";
				case NiveauRisque.Eleve: return "Élevé";
				case NiveauRisque.Moyen: return "Moyen";
				default: return "Faible";
			}
		}

		public static string etat(EtatInfo e){
			switch(e){
				case EtatInfo.Identifie: return "Identifié";
				case EtatInfo.Masque: return "Masqué";
				default: return "Non déterminé";
			}
		}
	}
}
