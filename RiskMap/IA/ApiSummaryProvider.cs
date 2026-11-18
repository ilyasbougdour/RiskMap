using System;
using System.Collections.Generic;

namespace RiskMap
{
	/// <summary>
	/// MODULE ANALYSE (Abdelali) - synthèse par une IA en ligne.
	/// Étapes : anonymiser -> construire le texte -> appeler l'API -> lire la réponse -> désanonymiser.
	/// Les données envoyées sont TOUJOURS la copie anonymisée.
	/// </summary>
	public class ApiSummaryProvider : ISummaryProvider
	{
		private Configuration config;

		public ApiSummaryProvider(Configuration prmConfig)
		{
			config = prmConfig;
		}

		public string Nom{
			get { return "IA en ligne (" + config.ModeleIa + ")"; }
		}

		public bool EnvoieDonneesExternes{
			get { return true; }
		}

		public void genererSynthese(ResultatScan r){
			Anonymiseur anonymiseur = new Anonymiseur();
			ResultatScan copie = anonymiseur.anonymiser(r);
			string texte = ConstructeurPrompt.donnees(copie);

			//sécurité : on refuse d'envoyer si une vraie adresse ou un vrai nom est encore présent
			if (ApiSummaryProvider.contientDonneesReelles(r, texte))
				throw new Exception("Anonymisation incomplète : envoi annulé.");

			string reponse = this.appelerApi(ConstructeurPrompt.consignes(), texte);

			r.SyntheseGlobale = anonymiseur.desanonymiser(reponse);
			r.SourceSynthese = this.Nom;
			// TODO (Abdelali, plus tard) : découper la réponse pour remplir aussi m.Synthese de chaque machine
		}

		//vrai si le texte contient une IP, un nom d'hôte ou une MAC réels du résultat d'origine
		public static bool contientDonneesReelles(ResultatScan original, string texte){
			foreach(Machine m in original.Machines){
				if (m.AdresseIp != "" && texte.Contains(m.AdresseIp))
					return true;
				if (m.NomHote != "" && texte.Contains(m.NomHote))
					return true;
				if (m.AdresseMac != "" && texte.Contains(m.AdresseMac))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Appel HTTP à l'API choisie, renvoie le texte rédigé par le modèle.
		/// Lève une exception en cas d'échec : l'interface bascule alors sur SyntheseSansIA.
		/// </summary>
		private string appelerApi(string consignes, string donnees){
			// TODO (Abdelali) : dépend du fournisseur choisi (à décider en équipe, voir ORGANISATION.md).
			// 1. construire le JSON de la requête (consignes + données, modèle = config.ModeleIa)
			//    avec JavaScriptSerializer.Serialize(...) pour bien échapper les guillemets et retours à la ligne
			// 2. en-têtes : la clé config.CleApiIa (nom de l'en-tête selon le fournisseur)
			// 3. string json = ClientHttp.post(config.UrlApiIa, corps, entetes, 60000);
			// 4. extraire le texte de la réponse JSON
			// Ne jamais écrire la clé dans un fichier de log ou dans le rapport.
			throw new NotImplementedException("Appel à l'API d'IA pas encore écrit");
		}
	}
}
