using System;
using System.Collections.Generic;

namespace RiskMap
{
	/// <summary>
	/// Une machine active trouvée sur le réseau.
	/// CLASSE COMMUNE : ne pas modifier sans prévenir les deux autres (voir ORGANISATION.md).
	/// </summary>
	public class Machine
	{
		//--- remplis par le module Scanner ---
		private string adresseIp = "";
		private string nomHote = "";
		private string adresseMac = "";
		//fabricant déduit de l'adresse MAC (imprimante, routeur...)
		private string fabricant = "";
		//TTL de la réponse au ping (64 ~ Linux, 128 ~ Windows, 255 ~ équipement réseau)
		private int ttl = 0;
		//système d'exploitation probable, ex. "Linux (Ubuntu)"
		private string systemeProbable = "";
		//confiance dans l'estimation du système, de 0 à 100
		private int confianceSysteme = 0;
		private EtatInfo etatSysteme = EtatInfo.NonDetermine;
		//services ouverts
		private List<Service> services = new List<Service>();

		//--- remplis par le module Analyse ---
		private int score = 0;
		private NiveauRisque niveau = NiveauRisque.Faible;
		//explication du score, une ligne par règle appliquée
		private List<string> detailScore = new List<string>();
		//synthèse rédigée (IA ou texte modèle)
		private string synthese = "";

		//constructeur sans paramètres (obligatoire pour la sérialisation XML)
		public Machine()
		{
		}

		//constructeur avec l'adresse IP
		public Machine(string prmAdresseIp)
		{
			adresseIp = prmAdresseIp;
		}

		public string AdresseIp{
			get { return adresseIp; }
			set { adresseIp = value; }
		}

		public string NomHote{
			get { return nomHote; }
			set { nomHote = value; }
		}

		public string AdresseMac{
			get { return adresseMac; }
			set { adresseMac = value; }
		}

		public string Fabricant{
			get { return fabricant; }
			set { fabricant = value; }
		}

		public int Ttl{
			get { return ttl; }
			set { ttl = value; }
		}

		public string SystemeProbable{
			get { return systemeProbable; }
			set { systemeProbable = value; }
		}

		public int ConfianceSysteme{
			get { return confianceSysteme; }
			set { confianceSysteme = value; }
		}

		public EtatInfo EtatSysteme{
			get { return etatSysteme; }
			set { etatSysteme = value; }
		}

		public List<Service> Services{
			get { return services; }
			set { services = value; }
		}

		public int Score{
			get { return score; }
			set { score = value; }
		}

		public NiveauRisque Niveau{
			get { return niveau; }
			set { niveau = value; }
		}

		public List<string> DetailScore{
			get { return detailScore; }
			set { detailScore = value; }
		}

		public string Synthese{
			get { return synthese; }
			set { synthese = value; }
		}

		//ajouter un service ouvert
		public void ajouterService(Service s){
			services.Add(s);
		}

		//vrai si le port est ouvert sur la machine
		public bool portOuvert(int port){
			foreach(Service s in services)
				if (s.Port == port)
					return true;
			return false;
		}

		//nombre total de CVE sur la machine
		public int nombreCve(){
			int n = 0;
			foreach(Service s in services)
				n = n + s.Vulnerabilites.Count;
			return n;
		}

		//vrai si au moins un service cache sa version (le risque reste alors « inconnu » pour ce service)
		public bool aServiceMasque(){
			foreach(Service s in services)
				if (s.EtatVersion == EtatInfo.Masque)
					return true;
			return false;
		}

		//nom affiché : le nom d'hôte s'il existe, sinon l'adresse IP
		public string nomAffiche(){
			if (nomHote != "")
				return nomHote;
			else
				return adresseIp;
		}

		public override string ToString(){
			return adresseIp + " - " + systemeProbable + " - " + niveau.ToString();
		}
	}
}
