using System;
using System.Collections.Generic;

namespace RiskMap
{
	/// <summary>
	/// Un service réseau ouvert sur une machine (un port TCP).
	/// CLASSE COMMUNE : ne pas modifier sans prévenir les deux autres (voir ORGANISATION.md).
	/// </summary>
	public class Service
	{
		//numéro de port, ex. 22
		private int port = 0;
		//nom usuel du service, ex. "ssh" (déduit du port)
		private string nomService = "";
		//texte brut annoncé par le service (bannière), vide si rien lu
		private string banniere = "";
		//logiciel identifié, ex. "OpenSSH"
		private string produit = "";
		//version identifiée, ex. "8.9p1"
		private string version = "";
		//état de l'identification de la version
		private EtatInfo etatVersion = EtatInfo.NonDetermine;
		//vulnérabilités trouvées pour ce produit + version
		private List<Vulnerabilite> vulnerabilites = new List<Vulnerabilite>();

		//constructeur sans paramètres (obligatoire pour la sérialisation XML)
		public Service()
		{
		}

		//constructeur avec le port et le nom
		public Service(int prmPort, string prmNomService)
		{
			port = prmPort;
			nomService = prmNomService;
		}

		public int Port{
			get { return port; }
			set { port = value; }
		}

		public string NomService{
			get { return nomService; }
			set { nomService = value; }
		}

		public string Banniere{
			get { return banniere; }
			set { banniere = value; }
		}

		public string Produit{
			get { return produit; }
			set { produit = value; }
		}

		public string Version{
			get { return version; }
			set { version = value; }
		}

		public EtatInfo EtatVersion{
			get { return etatVersion; }
			set { etatVersion = value; }
		}

		public List<Vulnerabilite> Vulnerabilites{
			get { return vulnerabilites; }
			set { vulnerabilites = value; }
		}

		//score CVSS le plus élevé parmi les CVE du service (0 si aucune)
		public double scoreMax(){
			double max = 0.0;
			foreach(Vulnerabilite v in vulnerabilites)
				if (v.ScoreCvss > max)
					max = v.ScoreCvss;
			return max;
		}

		//description courte, ex. "22/ssh OpenSSH 8.9p1"
		public string description(){
			string s = port.ToString() + "/" + nomService;
			if (etatVersion == EtatInfo.Identifie)
				s = s + " " + produit + " " + version;
			else if (etatVersion == EtatInfo.Masque)
				s = s + " (version masquée)";
			else
				s = s + " (version non déterminée)";
			return s;
		}

		public override string ToString(){
			return this.description();
		}
	}
}
