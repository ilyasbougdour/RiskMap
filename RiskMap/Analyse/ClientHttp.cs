using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace RiskMap
{
	/// <summary>
	/// MODULE ANALYSE (Abdelali) - requêtes HTTPS communes à la NVD et à l'API d'IA.
	/// Une seule classe pour les deux : pas de code en double.
	/// Les méthodes lèvent une exception en cas d'erreur réseau : à attraper avec try / catch chez l'appelant.
	/// </summary>
	public class ClientHttp
	{
		//.NET Framework 4.5 n'active pas TLS 1.2 par défaut : sans ça, la NVD et les API refusent la connexion
		private static void activerTls12(){
			ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
		}

		//requête GET, renvoie le corps de la réponse (JSON)
		public static string get(string url, Dictionary<string, string> entetes, int delaiMs){
			activerTls12();
			HttpWebRequest req = (HttpWebRequest) WebRequest.Create(url);
			req.Method = "GET";
			req.Timeout = delaiMs;
			req.UserAgent = "RiskMap (projet etudiant)";
			ajouterEntetes(req, entetes);
			return lireReponse(req);
		}

		//requête POST avec un corps JSON, renvoie le corps de la réponse (JSON)
		public static string post(string url, string json, Dictionary<string, string> entetes, int delaiMs){
			activerTls12();
			HttpWebRequest req = (HttpWebRequest) WebRequest.Create(url);
			req.Method = "POST";
			req.Timeout = delaiMs;
			req.UserAgent = "RiskMap (projet etudiant)";
			req.ContentType = "application/json";
			ajouterEntetes(req, entetes);
			byte[] corps = Encoding.UTF8.GetBytes(json);
			req.ContentLength = corps.Length;
			Stream flux = req.GetRequestStream();
			flux.Write(corps, 0, corps.Length);
			flux.Close();
			return lireReponse(req);
		}

		private static void ajouterEntetes(HttpWebRequest req, Dictionary<string, string> entetes){
			if (entetes != null)
				foreach(KeyValuePair<string, string> e in entetes)
					req.Headers.Add(e.Key, e.Value);
		}

		private static string lireReponse(HttpWebRequest req){
			HttpWebResponse rep = (HttpWebResponse) req.GetResponse();
			StreamReader sr = new StreamReader(rep.GetResponseStream(), Encoding.UTF8);
			string texte = sr.ReadToEnd();
			sr.Close();
			rep.Close();
			return texte;
		}
	}
}
