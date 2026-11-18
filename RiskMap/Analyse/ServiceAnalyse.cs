using System;
using System.Collections.Generic;
using System.IO;

namespace RiskMap
{
	/// <summary>
	/// MODULE ANALYSE (Abdelali) - point d'entrée du module : CVE puis score.
	/// C'est la seule méthode que l'interface appelle.
	/// </summary>
	public class ServiceAnalyse
	{
		private Configuration config;

		public ServiceAnalyse(Configuration prmConfig)
		{
			config = prmConfig;
		}

		/// <summary>
		/// Complète le résultat : CVE de chaque service identifié, puis score et niveau de chaque machine.
		/// Renvoie le nombre de services pour lesquels la recherche de CVE a échoué (réseau...).
		/// </summary>
		public int analyser(ResultatScan r){
			int echecs = 0;
			CorrespondanceProduits table = chargerProduits();
			CacheCve cache = CacheCve.charger(Chemins.fichierCacheCve());
			ClientNvd nvd = new ClientNvd(config.CleApiNvd);

			foreach(Machine m in r.Machines){
				foreach(Service s in m.Services){
					//pas de version -> pas de recherche possible
					if (s.EtatVersion != EtatInfo.Identifie || s.Version == "")
						continue;
					ProduitNvd p = table.trouver(s.Produit);
					if (p == null)
						continue;
					string cpe = p.cpe(s.Version);
					List<Vulnerabilite> trouvees = cache.chercher(cpe);
					if (trouvees == null){
						try
						{
							trouvees = nvd.rechercher(cpe);
							cache.ajouter(cpe, trouvees);
						}
						catch
						{
							echecs++;
							continue;
						}
					}
					s.Vulnerabilites = trouvees;
				}
			}
			cache.saveToXml(Chemins.fichierCacheCve());

			//score de chaque machine
			MoteurRisque moteur = new MoteurRisque(chargerGrille());
			moteur.evaluer(r);
			return echecs;
		}

		//grille de risque depuis regles.xml, grille de départ si le fichier manque
		public static GrilleRisque chargerGrille(){
			if (File.Exists(Chemins.fichierRegles()))
				return GrilleRisque.createFromXMLFile(Chemins.fichierRegles());
			else
				return GrilleRisque.grilleDeDepart();
		}

		//table des produits depuis produits.xml, table de départ si le fichier manque
		public static CorrespondanceProduits chargerProduits(){
			if (File.Exists(Chemins.fichierProduits()))
				return CorrespondanceProduits.createFromXMLFile(Chemins.fichierProduits());
			else
				return CorrespondanceProduits.tableDeDepart();
		}
	}
}
