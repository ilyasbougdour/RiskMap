using System;

namespace RiskMap
{
	/// <summary>
	/// Une vulnérabilité publique (CVE) trouvée dans la base NVD.
	/// CLASSE COMMUNE : ne pas modifier sans prévenir les deux autres (voir ORGANISATION.md).
	/// </summary>
	public class Vulnerabilite
	{
		//identifiant, ex. "CVE-2011-2523"
		private string idCve = "";
		//score de gravité CVSS, de 0 à 10
		private double scoreCvss = 0.0;
		//description courte (en anglais, telle que donnée par la NVD)
		private string description = "";

		//constructeur sans paramètres (obligatoire pour la sérialisation XML)
		public Vulnerabilite()
		{
		}

		//constructeur complet
		public Vulnerabilite(string prmIdCve, double prmScoreCvss, string prmDescription)
		{
			idCve = prmIdCve;
			scoreCvss = prmScoreCvss;
			description = prmDescription;
		}

		public string IdCve{
			get { return idCve; }
			set { idCve = value; }
		}

		public double ScoreCvss{
			get { return scoreCvss; }
			set { scoreCvss = value; }
		}

		public string Description{
			get { return description; }
			set { description = value; }
		}

		//lien vers la fiche NVD
		public string lien(){
			return "https://nvd.nist.gov/vuln/detail/" + idCve;
		}

		//gravité en clair selon l'échelle CVSS v3
		public string gravite(){
			if (scoreCvss >= 9.0)
				return "Critique";
			else if (scoreCvss >= 7.0)
				return "Élevée";
			else if (scoreCvss >= 4.0)
				return "Moyenne";
			else
				return "Faible";
		}

		public override string ToString(){
			return idCve + " (" + scoreCvss.ToString() + " - " + this.gravite() + ")";
		}
	}
}
