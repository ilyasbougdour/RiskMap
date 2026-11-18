using System;

namespace RiskMap
{
	/// <summary>
	/// Synthèse de secours, sans IA et sans réseau : texte construit à partir du score et des CVE.
	/// Utilisée si l'IA est désactivée, s'il n'y a pas de clé, pas de connexion ou plus de quota.
	/// </summary>
	public class SyntheseSansIA : ISummaryProvider
	{
		public string Nom{
			get { return "Synthèse automatique (sans IA)"; }
		}

		public bool EnvoieDonneesExternes{
			get { return false; }
		}

		public void genererSynthese(ResultatScan r){
			foreach(Machine m in r.Machines){
				string texte = m.nomAffiche() + " : risque " + Textes.niveau(m.Niveau).ToLower()
					+ " (score " + m.Score.ToString() + ")." + Environment.NewLine;
				foreach(string ligne in m.DetailScore)
					texte = texte + "  " + ligne + Environment.NewLine;
				if (m.nombreCve() > 0)
					texte = texte + "Machine potentiellement vulnérable : vérifier les mises à jour des services concernés." + Environment.NewLine;
				m.Synthese = texte;
			}
			r.SyntheseGlobale = r.Machines.Count.ToString() + " machine(s) analysée(s) : "
				+ r.nombreMachines(NiveauRisque.Critique).ToString() + " critique(s), "
				+ r.nombreMachines(NiveauRisque.Eleve).ToString() + " élevée(s), "
				+ r.nombreMachines(NiveauRisque.Moyen).ToString() + " moyenne(s), "
				+ r.nombreMachines(NiveauRisque.Faible).ToString() + " faible(s).";
			r.SourceSynthese = this.Nom;
		}
	}
}
