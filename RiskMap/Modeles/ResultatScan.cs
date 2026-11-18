using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RiskMap
{
	/// <summary>
	/// Résultat complet d'un scan : c'est l'objet qui passe d'un module à l'autre.
	/// Scanner le crée -> Analyse le complète -> Interface l'affiche.
	/// Il s'enregistre en XML : chacun peut travailler sur Donnees/scan_exemple.xml sans attendre les autres.
	/// CLASSE COMMUNE : ne pas modifier sans prévenir les deux autres (voir ORGANISATION.md).
	/// </summary>
	public class ResultatScan
	{
		private DateTime dateScan = DateTime.Now;
		//plage scannée, ex. "192.168.56.1-254"
		private string plageReseau = "";
		private List<Machine> machines = new List<Machine>();
		//synthèse globale du réseau (IA ou texte modèle)
		private string syntheseGlobale = "";
		//nom du fournisseur qui a rédigé la synthèse
		private string sourceSynthese = "";

		public ResultatScan()
		{
		}

		public DateTime DateScan{
			get { return dateScan; }
			set { dateScan = value; }
		}

		public string PlageReseau{
			get { return plageReseau; }
			set { plageReseau = value; }
		}

		public List<Machine> Machines{
			get { return machines; }
			set { machines = value; }
		}

		public string SyntheseGlobale{
			get { return syntheseGlobale; }
			set { syntheseGlobale = value; }
		}

		public string SourceSynthese{
			get { return sourceSynthese; }
			set { sourceSynthese = value; }
		}

		public void ajouterMachine(Machine m){
			machines.Add(m);
		}

		//nombre de machines d'un niveau donné
		public int nombreMachines(NiveauRisque niveau){
			int n = 0;
			foreach(Machine m in machines)
				if (m.Niveau == niveau)
					n++;
			return n;
		}

		//sauvegarde dans un fichier XML
		public void saveToXml(string nomFichier){
			FileStream file = File.Open(nomFichier, FileMode.Create);
			XmlSerializer serializer = new XmlSerializer(typeof(ResultatScan));
			serializer.Serialize(file, this);
			file.Close();
		}

		//chargement à partir d'un fichier XML -- méthode statique
		public static ResultatScan createFromXMLFile(string nomFichier){
			FileStream file = File.Open(nomFichier, FileMode.Open);
			XmlSerializer serializer = new XmlSerializer(typeof(ResultatScan));
			ResultatScan obj = (ResultatScan) serializer.Deserialize(file);
			file.Close();
			return obj;
		}

		//copie complète (indépendante) de l'objet, utile pour l'anonymisation
		public ResultatScan copie(){
			XmlSerializer serializer = new XmlSerializer(typeof(ResultatScan));
			StringWriter sw = new StringWriter();
			serializer.Serialize(sw, this);
			StringReader sr = new StringReader(sw.ToString());
			return (ResultatScan) serializer.Deserialize(sr);
		}
	}
}
