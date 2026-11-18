using System;
using System.Collections.Generic;

namespace RiskMap
{
	/// <summary>
	/// MODULE ANALYSE (Abdelali) - interrogation de la base publique NVD (API 2.0).
	/// Limite : 5 requêtes / 30 s sans clé, 50 avec une clé gratuite -> toujours passer par CacheCve.
	/// </summary>
	public class ClientNvd
	{
		private const string URL_NVD = "https://services.nvd.nist.gov/rest/json/cves/2.0";
		private string cleApi;

		public ClientNvd(string prmCleApi)
		{
			cleApi = prmCleApi;
		}

		/// <summary>
		/// CVE connues pour un nom CPE. Liste vide si aucune ; exception si erreur réseau.
		/// </summary>
		public List<Vulnerabilite> rechercher(string cpe){
			List<Vulnerabilite> liste = new List<Vulnerabilite>();

			// TODO (Abdelali) :
			// 1. url = URL_NVD + "?cpeName=" + Uri.EscapeDataString(cpe)
			//    (essayer aussi virtualMatchString si cpeName ne renvoie rien)
			// 2. si cleApi != "" : en-tête "apiKey" = cleApi
			// 3. string json = ClientHttp.get(url, entetes, 20000);
			// 4. lire le JSON sans NuGet : System.Web.Script.Serialization.JavaScriptSerializer
			//    (référence System.Web.Extensions, déjà dans le projet) -> Deserialize<Dictionary<string, object>>
			//    vulnerabilities[i].cve.id
			//    vulnerabilities[i].cve.metrics.cvssMetricV31[0].cvssData.baseScore (sinon V30, sinon V2)
			//    vulnerabilities[i].cve.descriptions[0].value (lang = "en")
			// 5. liste.Add(new Vulnerabilite(id, score, description));
			// Tester d'abord avec un CPE connu : cpe:2.3:a:vsftpd_project:vsftpd:2.3.4:*:*:*:*:*:*:*

			return liste;
		}
	}
}
