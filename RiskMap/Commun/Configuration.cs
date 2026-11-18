using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RiskMap
{
	/// <summary>
	/// Paramètres de l'application, enregistrés dans config.xml à côté de l'exécutable.
	/// config.xml contient les clés API : il est dans le .gitignore et ne doit jamais être poussé.
	/// Les clés peuvent aussi venir des variables d'environnement RISKMAP_CLE_IA et RISKMAP_CLE_NVD.
	/// </summary>
	public class Configuration
	{
		//--- synthèse par IA ---
		private bool utiliserIa = false;
		private string urlApiIa = "";
		private string modeleIa = "";
		private string cleApiIa = "";

		//--- base NVD ---
		//facultative : sans clé, 5 requêtes / 30 s ; avec clé gratuite, 50 requêtes / 30 s
		private string cleApiNvd = "";

		//--- scan ---
		private int delaiPingMs = 500;
		private int delaiPortMs = 800;
		private List<int> ports = new List<int>();

		public Configuration()
		{
		}

		public bool UtiliserIa{
			get { return utiliserIa; }
			set { utiliserIa = value; }
		}

		public string UrlApiIa{
			get { return urlApiIa; }
			set { urlApiIa = value; }
		}

		public string ModeleIa{
			get { return modeleIa; }
			set { modeleIa = value; }
		}

		public string CleApiIa{
			get { return cleApiIa; }
			set { cleApiIa = value; }
		}

		public string CleApiNvd{
			get { return cleApiNvd; }
			set { cleApiNvd = value; }
		}

		public int DelaiPingMs{
			get { return delaiPingMs; }
			set { delaiPingMs = value; }
		}

		public int DelaiPortMs{
			get { return delaiPortMs; }
			set { delaiPortMs = value; }
		}

		public List<int> Ports{
			get { return ports; }
			set { ports = value; }
		}

		//vrai si la synthèse IA est activée et qu'une clé est disponible
		public bool iaDisponible(){
			return utiliserIa && cleApiIa != "" && urlApiIa != "";
		}

		//ports scannés par défaut si config.xml n'en donne pas
		public static List<int> portsParDefaut(){
			List<int> l = new List<int>();
			int[] tab = { 21, 22, 23, 25, 53, 80, 110, 139, 143, 443, 445, 3306, 3389, 5900, 8080, 9100 };
			foreach(int p in tab)
				l.Add(p);
			return l;
		}

		//sauvegarde dans config.xml
		public void sauver(){
			FileStream file = File.Open(Chemins.fichierConfiguration(), FileMode.Create);
			XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
			serializer.Serialize(file, this);
			file.Close();
		}

		//chargement de config.xml (valeurs par défaut si le fichier n'existe pas) -- méthode statique
		public static Configuration charger(){
			Configuration c = new Configuration();
			string nomFichier = Chemins.fichierConfiguration();
			if (File.Exists(nomFichier)){
				try
				{
					FileStream file = File.Open(nomFichier, FileMode.Open);
					XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
					c = (Configuration) serializer.Deserialize(file);
					file.Close();
				}
				catch
				{
					c = new Configuration();
				}
			}
			if (c.Ports.Count == 0)
				c.Ports = portsParDefaut();
			//les variables d'environnement passent avant le fichier
			string cle = Environment.GetEnvironmentVariable("RISKMAP_CLE_IA");
			if (cle != null && cle != "")
				c.CleApiIa = cle;
			cle = Environment.GetEnvironmentVariable("RISKMAP_CLE_NVD");
			if (cle != null && cle != "")
				c.CleApiNvd = cle;
			return c;
		}
	}
}
