using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace RiskMap
{
	/// <summary>
	/// MODULE SCANNER (Ilyas) - étape 2 : tester une liste de ports TCP sur une machine.
	/// </summary>
	public class ScannerPorts
	{
		//délai maximum pour une tentative de connexion
		private int delaiMs;

		public ScannerPorts(int prmDelaiMs)
		{
			delaiMs = prmDelaiMs;
		}

		/// <summary>
		/// Renvoie un Service (Port + NomService) pour chaque port ouvert.
		/// </summary>
		public List<Service> scanner(string ip, List<int> ports){
			List<Service> ouverts = new List<Service>();

			// TODO (Ilyas) : pour chaque port, si this.portOuvert(ip, port)
			//   -> ouverts.Add(new Service(port, ScannerPorts.nomDuPort(port)));
			// Ensuite, tester plusieurs ports en parallèle (comme pour le ping).

			return ouverts;
		}

		/// <summary>
		/// Vrai si une connexion TCP réussit avant le délai.
		/// </summary>
		public bool portOuvert(string ip, int port){
			// TODO (Ilyas) : piste de départ
			// TcpClient client = new TcpClient();
			// IAsyncResult res = client.BeginConnect(ip, port, null, null);
			// bool ok = res.AsyncWaitHandle.WaitOne(delaiMs);
			// if (ok && client.Connected) { client.EndConnect(res); ... }
			// client.Close();
			return false;
		}

		/// <summary>
		/// Nom usuel d'un port connu (affichage et correspondance), "inconnu" sinon.
		/// </summary>
		public static string nomDuPort(int port){
			switch(port){
				case 21: return "ftp";
				case 22: return "ssh";
				case 23: return "telnet";
				case 25: return "smtp";
				case 53: return "dns";
				case 80: return "http";
				case 110: return "pop3";
				case 139: return "netbios-ssn";
				case 143: return "imap";
				case 443: return "https";
				case 445: return "microsoft-ds";
				case 3306: return "mysql";
				case 3389: return "ms-wbt-server";
				case 5900: return "vnc";
				case 8080: return "http-alt";
				case 9100: return "jetdirect";
				default: return "inconnu";
			}
		}
	}
}
