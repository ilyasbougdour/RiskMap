using System;

namespace RiskMap
{
	/// <summary>
	/// Fournisseur de synthèse. Deux implémentations :
	///  - ApiSummaryProvider : synthèse rédigée par une IA en ligne (données anonymisées avant l'envoi) ;
	///  - SyntheseSansIA     : texte modèle construit à partir des règles, sans réseau (solution de secours).
	/// Le reste du programme ne sait pas lequel est utilisé (même principe que ISavable en séance 4).
	/// </summary>
	public interface ISummaryProvider
	{
		//nom affiché dans l'interface et dans le rapport
		string Nom { get; }

		//vrai si des données quittent l'ordinateur (l'interface doit alors prévenir l'utilisateur)
		bool EnvoieDonneesExternes { get; }

		//remplit r.SyntheseGlobale, r.SourceSynthese et la Synthese de chaque machine
		void genererSynthese(ResultatScan r);
	}
}
