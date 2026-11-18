using System;

namespace RiskMap
{
	/// <summary>
	/// Jeu de données d'exemple pour avancer sans attendre le scanner (carte, tableau, rapport, IA).
	/// Les machines imitent un réseau de laboratoire (dont une machine type Metasploitable).
	/// Les CVE sont réelles mais choisies à la main : les scores servent juste d'exemple.
	/// Le moteur de risque n'est PAS appliqué ici : Score et Niveau sont calculés ensuite.
	/// </summary>
	public class DonneesFictives
	{
		public static ResultatScan creer(){
			ResultatScan r = new ResultatScan();
			r.PlageReseau = "192.168.56.1-254 (données d'exemple)";

			//--- machine de labo volontairement vulnérable ---
			Machine m1 = new Machine("192.168.56.101");
			m1.NomHote = "metasploitable";
			m1.AdresseMac = "08:00:27:4A:1B:2C";
			m1.Fabricant = "Oracle VirtualBox";
			m1.Ttl = 64;
			m1.SystemeProbable = "Linux (Ubuntu)";
			m1.ConfianceSysteme = 85;
			m1.EtatSysteme = EtatInfo.Identifie;
			Service ftp = service(21, "ftp", "220 (vsFTPd 2.3.4)", "vsftpd", "2.3.4", EtatInfo.Identifie);
			ftp.Vulnerabilites.Add(new Vulnerabilite("CVE-2011-2523", 9.8, "vsftpd 2.3.4 downloaded between 20110630 and 20110703 contains a backdoor which opens a shell on port 6200/tcp."));
			m1.ajouterService(ftp);
			m1.ajouterService(service(22, "ssh", "SSH-2.0-OpenSSH_4.7p1 Debian-8ubuntu1", "OpenSSH", "4.7p1", EtatInfo.Identifie));
			m1.ajouterService(service(23, "telnet", "", "", "", EtatInfo.NonDetermine));
			Service web = service(80, "http", "Server: Apache/2.2.8 (Ubuntu) DAV/2", "Apache httpd", "2.2.8", EtatInfo.Identifie);
			web.Vulnerabilites.Add(new Vulnerabilite("CVE-2011-3192", 7.8, "The byterange filter in the Apache HTTP Server 1.3.x, 2.0.x through 2.0.64, and 2.2.x through 2.2.19 allows remote attackers to cause a denial of service."));
			m1.ajouterService(web);
			r.ajouterMachine(m1);

			//--- poste Windows ---
			Machine m2 = new Machine("192.168.56.110");
			m2.NomHote = "PC-COMPTA";
			m2.AdresseMac = "00:15:5D:01:02:03";
			m2.Fabricant = "Microsoft (Hyper-V)";
			m2.Ttl = 128;
			m2.SystemeProbable = "Windows";
			m2.ConfianceSysteme = 90;
			m2.EtatSysteme = EtatInfo.Identifie;
			Service smb = service(445, "microsoft-ds", "", "", "", EtatInfo.NonDetermine);
			smb.Vulnerabilites.Add(new Vulnerabilite("CVE-2017-0144", 8.8, "The SMBv1 server in Microsoft Windows allows remote attackers to execute arbitrary code via crafted packets (EternalBlue)."));
			m2.ajouterService(smb);
			m2.ajouterService(service(3389, "ms-wbt-server", "", "", "", EtatInfo.NonDetermine));
			m2.ajouterService(service(80, "http", "Server: Microsoft-IIS/10.0", "Microsoft IIS", "10.0", EtatInfo.Identifie));
			r.ajouterMachine(m2);

			//--- imprimante réseau qui cache sa version ---
			Machine m3 = new Machine("192.168.56.120");
			m3.AdresseMac = "3C:2A:F4:10:20:30";
			m3.Fabricant = "Brother";
			m3.Ttl = 255;
			m3.SystemeProbable = "Équipement réseau (imprimante)";
			m3.ConfianceSysteme = 60;
			m3.EtatSysteme = EtatInfo.Identifie;
			m3.ajouterService(service(80, "http", "Server: debut", "", "", EtatInfo.Masque));
			m3.ajouterService(service(9100, "jetdirect", "", "", "", EtatInfo.NonDetermine));
			r.ajouterMachine(m3);

			//--- serveur Linux à jour ---
			Machine m4 = new Machine("192.168.56.130");
			m4.NomHote = "srv-web";
			m4.AdresseMac = "08:00:27:AA:BB:CC";
			m4.Fabricant = "Oracle VirtualBox";
			m4.Ttl = 64;
			m4.SystemeProbable = "Linux (Ubuntu)";
			m4.ConfianceSysteme = 80;
			m4.EtatSysteme = EtatInfo.Identifie;
			m4.ajouterService(service(22, "ssh", "SSH-2.0-OpenSSH_9.6p1 Ubuntu-3ubuntu13", "OpenSSH", "9.6p1", EtatInfo.Identifie));
			m4.ajouterService(service(443, "https", "Server: nginx", "nginx", "", EtatInfo.Masque));
			r.ajouterMachine(m4);

			return r;
		}

		//création rapide d'un service
		private static Service service(int port, string nom, string banniere, string produit, string version, EtatInfo etat){
			Service s = new Service(port, nom);
			s.Banniere = banniere;
			s.Produit = produit;
			s.Version = version;
			s.EtatVersion = etat;
			return s;
		}
	}
}
