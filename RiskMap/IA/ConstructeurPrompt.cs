using System;

namespace RiskMap
{
	/// <summary>
	/// Texte envoyé à l'IA (Abdelali) : les consignes + les résultats (déjà anonymisés) sous forme structurée.
	/// </summary>
	public class ConstructeurPrompt
	{
		//consignes fixes : c'est ce qui empêche l'IA d'inventer des CVE
		public static string consignes(){
			return
				"Tu rédiges en français la synthèse d'un audit de sécurité d'un réseau de laboratoire, pour un lecteur non expert.\n" +
				"Règles obligatoires :\n" +
				"- Ne cite QUE les CVE présentes dans les données fournies. N'en invente aucune.\n" +
				"- Une machine avec des CVE est « potentiellement vulnérable », jamais « vulnérable » : le système a pu corriger la faille sans changer de version.\n" +
				"- Une version masquée est une bonne pratique, mais elle ne réduit pas le risque : le risque de ce service reste inconnu.\n" +
				"- « Non déterminé » veut dire que l'outil n'a pas pu lire l'information : n'en tire aucune conclusion.\n" +
				"- Garde les noms de machines tels qu'ils sont écrits (Machine 1, Machine 2...).\n" +
				"Plan : 1) résumé en 3 phrases ; 2) machines par ordre de priorité ; 3) correctifs concrets à appliquer pour chacune.";
		}

		//résultats sous forme de texte structuré (à appeler sur la copie ANONYMISÉE)
		public static string donnees(ResultatScan r){
			string t = "";
			foreach(Machine m in r.Machines){
				t = t + "== " + m.AdresseIp + " ==\n";
				t = t + "Système probable : " + m.SystemeProbable + " (confiance " + m.ConfianceSysteme.ToString() + " %, " + m.EtatSysteme.ToString() + ")\n";
				t = t + "Score : " + m.Score.ToString() + " - niveau " + m.Niveau.ToString() + "\n";
				foreach(Service s in m.Services){
					t = t + "Service " + s.description() + " [" + s.EtatVersion.ToString() + "]\n";
					foreach(Vulnerabilite v in s.Vulnerabilites)
						t = t + "   " + v.IdCve + " CVSS " + v.ScoreCvss.ToString() + " : " + v.Description + "\n";
				}
				foreach(string ligne in m.DetailScore)
					t = t + "Règle : " + ligne + "\n";
				t = t + "\n";
			}
			return t;
		}
	}
}
