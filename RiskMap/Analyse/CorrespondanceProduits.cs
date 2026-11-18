using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RiskMap
{
	/// <summary>
	/// Une ligne de la table : nom de produit vu dans la bannière -> vendeur et produit dans la NVD (CPE).
	/// </summary>
	public class ProduitNvd
	{
		//nom tel que rempli par LecteurBanniere, ex. "OpenSSH"
		private string produit = "";
		//vendeur NVD, ex. "openbsd"
		private string vendeur = "";
		//produit NVD, ex. "openssh"
		private string produitCpe = "";

		public ProduitNvd()
		{
		}

		public ProduitNvd(string prmProduit, string prmVendeur, string prmProduitCpe)
		{
			produit = prmProduit;
			vendeur = prmVendeur;
			produitCpe = prmProduitCpe;
		}

		public string Produit{
			get { return produit; }
			set { produit = value; }
		}

		public string Vendeur{
			get { return vendeur; }
			set { vendeur = value; }
		}

		public string ProduitCpe{
			get { return produitCpe; }
			set { produitCpe = value; }
		}

		//nom CPE 2.3 pour une version donnée, ex. cpe:2.3:a:openbsd:openssh:8.9:*:*:*:*:*:*:*
		public string cpe(string version){
			// TODO (Abdelali) : gérer les versions du type "8.9p1" (dans la NVD : version 8.9, mise à jour p1)
			return "cpe:2.3:a:" + vendeur + ":" + produitCpe + ":" + version + ":*:*:*:*:*:*:*";
		}
	}

	/// <summary>
	/// MODULE ANALYSE (Abdelali) - table de correspondance chargée depuis Donnees/produits.xml.
	/// La NVD ne comprend pas une bannière : il faut lui donner un nom CPE. On le fait avec cette table
	/// pour une dizaine de produits courants (c'est volontairement limité, voir « Limites » dans le README).
	/// </summary>
	public class CorrespondanceProduits
	{
		private List<ProduitNvd> table = new List<ProduitNvd>();

		public CorrespondanceProduits()
		{
		}

		public List<ProduitNvd> Table{
			get { return table; }
			set { table = value; }
		}

		//recherche d'un produit (sans tenir compte des majuscules), null si inconnu
		public ProduitNvd trouver(string produit){
			foreach(ProduitNvd p in table)
				if (p.Produit.ToLower() == produit.ToLower())
					return p;
			return null;
		}

		public void saveToXml(string nomFichier){
			FileStream file = File.Open(nomFichier, FileMode.Create);
			XmlSerializer serializer = new XmlSerializer(typeof(CorrespondanceProduits));
			serializer.Serialize(file, this);
			file.Close();
		}

		public static CorrespondanceProduits createFromXMLFile(string nomFichier){
			FileStream file = File.Open(nomFichier, FileMode.Open);
			XmlSerializer serializer = new XmlSerializer(typeof(CorrespondanceProduits));
			CorrespondanceProduits obj = (CorrespondanceProduits) serializer.Deserialize(file);
			file.Close();
			return obj;
		}

		//table de départ (sert à générer produits.xml) - vendeurs à vérifier sur nvd.nist.gov (recherche CPE)
		public static CorrespondanceProduits tableDeDepart(){
			CorrespondanceProduits c = new CorrespondanceProduits();
			c.Table.Add(new ProduitNvd("OpenSSH", "openbsd", "openssh"));
			c.Table.Add(new ProduitNvd("vsftpd", "vsftpd_project", "vsftpd"));
			c.Table.Add(new ProduitNvd("ProFTPD", "proftpd", "proftpd"));
			c.Table.Add(new ProduitNvd("Apache httpd", "apache", "http_server"));
			c.Table.Add(new ProduitNvd("nginx", "f5", "nginx"));
			c.Table.Add(new ProduitNvd("Microsoft IIS", "microsoft", "internet_information_services"));
			c.Table.Add(new ProduitNvd("MySQL", "oracle", "mysql"));
			c.Table.Add(new ProduitNvd("Samba", "samba", "samba"));
			c.Table.Add(new ProduitNvd("Postfix", "postfix", "postfix"));
			c.Table.Add(new ProduitNvd("Exim", "exim", "exim"));
			return c;
		}
	}
}
