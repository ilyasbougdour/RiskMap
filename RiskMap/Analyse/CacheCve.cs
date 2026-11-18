using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RiskMap
{
	/// <summary>
	/// Une entrée du cache : un nom CPE et les CVE trouvées pour lui.
	/// </summary>
	public class EntreeCache
	{
		private string cpe = "";
		private DateTime dateRequete = DateTime.Now;
		private List<Vulnerabilite> vulnerabilites = new List<Vulnerabilite>();

		public EntreeCache()
		{
		}

		public string Cpe{
			get { return cpe; }
			set { cpe = value; }
		}

		public DateTime DateRequete{
			get { return dateRequete; }
			set { dateRequete = value; }
		}

		public List<Vulnerabilite> Vulnerabilites{
			get { return vulnerabilites; }
			set { vulnerabilites = value; }
		}
	}

	/// <summary>
	/// MODULE ANALYSE (Abdelali) - cache local des réponses NVD (fichier cache_cve.xml).
	/// Scans suivants plus rapides, et l'outil reste utilisable sans connexion.
	/// </summary>
	public class CacheCve
	{
		private List<EntreeCache> entrees = new List<EntreeCache>();

		public CacheCve()
		{
		}

		public List<EntreeCache> Entrees{
			get { return entrees; }
			set { entrees = value; }
		}

		/// <summary>
		/// CVE en cache pour ce CPE, null si absent du cache.
		/// </summary>
		public List<Vulnerabilite> chercher(string cpe){
			// TODO (Abdelali) : parcourir entrees ; décider d'une durée de validité (ex. 7 jours)
			return null;
		}

		/// <summary>
		/// Ajoute (ou remplace) la réponse NVD pour ce CPE.
		/// </summary>
		public void ajouter(string cpe, List<Vulnerabilite> vulnerabilites){
			// TODO (Abdelali)
		}

		public void saveToXml(string nomFichier){
			FileStream file = File.Open(nomFichier, FileMode.Create);
			XmlSerializer serializer = new XmlSerializer(typeof(CacheCve));
			serializer.Serialize(file, this);
			file.Close();
		}

		//chargement du cache, cache vide si le fichier n'existe pas encore
		public static CacheCve charger(string nomFichier){
			if (File.Exists(nomFichier) == false)
				return new CacheCve();
			FileStream file = File.Open(nomFichier, FileMode.Open);
			XmlSerializer serializer = new XmlSerializer(typeof(CacheCve));
			CacheCve obj = (CacheCve) serializer.Deserialize(file);
			file.Close();
			return obj;
		}
	}
}
