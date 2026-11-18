using System;
using System.Collections.Generic;

namespace RiskMap
{
	/// <summary>
	/// MODULE SCANNER (Ilyas) - point d'entrée du module : enchaîne les étapes 1 à 3.
	/// C'est la seule méthode que l'interface appelle.
	/// </summary>
	public class ScannerReseau
	{
		private Configuration config;

		public ScannerReseau(Configuration prmConfig)
		{
			config = prmConfig;
		}

		/// <summary>
		/// Scan complet de prefixe + debut ... prefixe + fin.
		/// Ne remplit PAS les CVE ni le score : c'est le rôle du module Analyse.
		/// </summary>
		public ResultatScan lancer(string prefixe, int debut, int fin){
			ResultatScan r = new ResultatScan();
			r.DateScan = DateTime.Now;
			r.PlageReseau = prefixe + debut.ToString() + "-" + fin.ToString();

			//1. découverte
			DecouverteReseau decouverte = new DecouverteReseau(config.DelaiPingMs);
			List<Machine> machines = decouverte.decouvrir(prefixe, debut, fin);

			ScannerPorts scanPorts = new ScannerPorts(config.DelaiPortMs);
			LecteurBanniere lecteur = new LecteurBanniere(config.DelaiPortMs);

			foreach(Machine m in machines){
				m.NomHote = DecouverteReseau.nomHote(m.AdresseIp);
				m.AdresseMac = DetecteurSysteme.adresseMac(m.AdresseIp);
				m.Fabricant = DetecteurSysteme.fabricant(m.AdresseMac);
				//2. ports ouverts
				m.Services = scanPorts.scanner(m.AdresseIp, config.Ports);
				//3. bannières puis système
				foreach(Service s in m.Services)
					lecteur.lire(m.AdresseIp, s);
				DetecteurSysteme.estimer(m);
				r.ajouterMachine(m);
			}
			return r;
		}
	}
}
