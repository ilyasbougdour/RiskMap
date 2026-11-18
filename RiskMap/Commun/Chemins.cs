using System;
using System.IO;
using System.Windows.Forms;

namespace RiskMap
{
	/// <summary>
	/// Emplacements des fichiers utilisés par l'application (tous à côté de l'exécutable).
	/// </summary>
	public class Chemins
	{
		//dossier des fichiers de données livrés avec l'application (règles, produits, exemple)
		public static string dossierDonnees(){
			return Path.Combine(Application.StartupPath, "Donnees");
		}

		//grille des règles de risque (modifiable sans toucher au code)
		public static string fichierRegles(){
			return Path.Combine(dossierDonnees(), "regles.xml");
		}

		//table de correspondance bannière -> produit NVD
		public static string fichierProduits(){
			return Path.Combine(dossierDonnees(), "produits.xml");
		}

		//résultat de scan d'exemple
		public static string fichierExemple(){
			return Path.Combine(dossierDonnees(), "scan_exemple.xml");
		}

		//paramètres locaux, avec les clés API : JAMAIS sur GitHub (voir .gitignore)
		public static string fichierConfiguration(){
			return Path.Combine(Application.StartupPath, "config.xml");
		}

		//cache local des réponses NVD (créé au premier usage)
		public static string fichierCacheCve(){
			return Path.Combine(Application.StartupPath, "cache_cve.xml");
		}
	}
}
