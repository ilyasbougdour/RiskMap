using System;
using System.Collections.Generic;

namespace RiskMap
{
	/// <summary>
	/// Anonymisation AVANT tout envoi à l'API d'IA (Ilyas, semaine 3).
	/// Les adresses IP, noms d'hôtes et adresses MAC sont remplacés par des alias
	/// ("Machine 1", "Machine 2"...). L'IA n'en a pas besoin pour expliquer une CVE.
	/// Au retour, les alias du texte de l'IA sont remplacés par les vrais noms.
	/// </summary>
	public class Anonymiseur
	{
		//alias -> vrai nom affiché (ex. "Machine 1" -> "metasploitable (192.168.56.101)")
		private Dictionary<string, string> correspondances = new Dictionary<string, string>();

		public Anonymiseur()
		{
		}

		/// <summary>
		/// Renvoie une COPIE anonymisée du résultat (l'original n'est pas modifié).
		/// </summary>
		public ResultatScan anonymiser(ResultatScan r){
			ResultatScan copie = r.copie();
			correspondances.Clear();

			// TODO (Ilyas) : pour chaque machine i de la copie
			//   alias = "Machine " + (i + 1)
			//   correspondances[alias] = vrai nom (nomAffiche + IP) de la machine d'origine
			//   copie.Machines[i].AdresseIp = alias ; NomHote = "" ; AdresseMac = "" ;
			//   attention aussi aux bannières qui peuvent contenir un nom d'hôte.
			// copie.PlageReseau = "réseau local"

			return copie;
		}

		/// <summary>
		/// Remet les vrais noms dans un texte rédigé par l'IA.
		/// </summary>
		public string desanonymiser(string texte){
			// TODO (Ilyas) : remplacer chaque alias par son vrai nom (String.Replace).
			// Attention : remplacer "Machine 10" avant "Machine 1".
			return texte;
		}
	}
}
