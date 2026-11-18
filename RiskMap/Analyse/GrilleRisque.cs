using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RiskMap
{
	/// <summary>
	/// Une règle : si ce port est ouvert, on ajoute des points.
	/// </summary>
	public class RegleRisque
	{
		private int port = 0;
		private int points = 0;
		private string raison = "";

		public RegleRisque()
		{
		}

		public RegleRisque(int prmPort, int prmPoints, string prmRaison)
		{
			port = prmPort;
			points = prmPoints;
			raison = prmRaison;
		}

		public int Port{
			get { return port; }
			set { port = value; }
		}

		public int Points{
			get { return points; }
			set { points = value; }
		}

		public string Raison{
			get { return raison; }
			set { raison = value; }
		}
	}

	/// <summary>
	/// Toute la grille de risque, stockée dans Donnees/regles.xml : on l'ajuste pendant les tests
	/// sans toucher au code. Valeurs de départ = celles de la proposition de projet.
	/// </summary>
	public class GrilleRisque
	{
		private List<RegleRisque> regles = new List<RegleRisque>();

		//points ajoutés selon la gravité de la CVE la plus grave d'un service
		private int pointsCveCritique = 40;
		private int pointsCveElevee = 25;
		private int pointsCveMoyenne = 10;

		//seuils des niveaux (score >= seuil)
		private int seuilMoyen = 20;
		private int seuilEleve = 50;
		private int seuilCritique = 80;

		public GrilleRisque()
		{
		}

		public List<RegleRisque> Regles{
			get { return regles; }
			set { regles = value; }
		}

		public int PointsCveCritique{
			get { return pointsCveCritique; }
			set { pointsCveCritique = value; }
		}

		public int PointsCveElevee{
			get { return pointsCveElevee; }
			set { pointsCveElevee = value; }
		}

		public int PointsCveMoyenne{
			get { return pointsCveMoyenne; }
			set { pointsCveMoyenne = value; }
		}

		public int SeuilMoyen{
			get { return seuilMoyen; }
			set { seuilMoyen = value; }
		}

		public int SeuilEleve{
			get { return seuilEleve; }
			set { seuilEleve = value; }
		}

		public int SeuilCritique{
			get { return seuilCritique; }
			set { seuilCritique = value; }
		}

		public void saveToXml(string nomFichier){
			FileStream file = File.Open(nomFichier, FileMode.Create);
			XmlSerializer serializer = new XmlSerializer(typeof(GrilleRisque));
			serializer.Serialize(file, this);
			file.Close();
		}

		public static GrilleRisque createFromXMLFile(string nomFichier){
			FileStream file = File.Open(nomFichier, FileMode.Open);
			XmlSerializer serializer = new XmlSerializer(typeof(GrilleRisque));
			GrilleRisque obj = (GrilleRisque) serializer.Deserialize(file);
			file.Close();
			return obj;
		}

		//grille de départ (sert à générer regles.xml)
		public static GrilleRisque grilleDeDepart(){
			GrilleRisque g = new GrilleRisque();
			g.Regles.Add(new RegleRisque(23, 30, "Telnet : mots de passe envoyés en clair"));
			g.Regles.Add(new RegleRisque(445, 25, "SMB exposé : failles connues (ex. WannaCry)"));
			g.Regles.Add(new RegleRisque(21, 20, "FTP : transfert non chiffré"));
			g.Regles.Add(new RegleRisque(22, 0, "SSH : protocole sécurisé"));
			//règles ajoutées, à valider en équipe
			g.Regles.Add(new RegleRisque(3389, 15, "Bureau à distance (RDP) exposé"));
			g.Regles.Add(new RegleRisque(5900, 20, "VNC : accès à distance souvent mal protégé"));
			g.Regles.Add(new RegleRisque(3306, 15, "Base de données MySQL exposée sur le réseau"));
			g.Regles.Add(new RegleRisque(139, 10, "NetBIOS : partage Windows ancien"));
			g.Regles.Add(new RegleRisque(80, 5, "HTTP : site non chiffré"));
			return g;
		}
	}
}
