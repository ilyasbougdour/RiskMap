using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace RiskMap
{
	/// <summary>
	/// MODULE SCANNER (Ilyas) - étape 1 : trouver les machines actives par ping.
	/// </summary>
	public class DecouverteReseau
	{
		//délai d'attente d'une réponse au ping
		private int delaiMs;

		public DecouverteReseau(int prmDelaiMs)
		{
			delaiMs = prmDelaiMs;
		}

		/// <summary>
		/// Ping de prefixe + debut ... prefixe + fin, ex. ("192.168.56.", 1, 254).
		/// Renvoie une Machine par adresse qui répond, avec AdresseIp et Ttl remplis.
		/// </summary>
		public List<Machine> decouvrir(string prefixe, int debut, int fin){
			List<Machine> actives = new List<Machine>();

			// TODO (Ilyas) :
			// 1. Version simple d'abord (une adresse après l'autre) avec this.pinger(ip).
			// 2. Puis en parallèle pour aller plus vite : Parallel.For(debut, fin + 1, ...)
			//    (using System.Threading.Tasks) en protégeant la liste avec lock(actives) { ... }.
			// 3. Trier la liste par adresse à la fin (sinon l'ordre dépend des threads).

			return actives;
		}

		/// <summary>
		/// Ping d'une seule adresse. Renvoie la Machine si elle répond, null sinon.
		/// </summary>
		public Machine pinger(string ip){
			// TODO (Ilyas) : exemple de départ à compléter et tester
			// Ping p = new Ping();
			// PingReply rep = p.Send(ip, delaiMs);
			// if (rep.Status == IPStatus.Success) {
			//     Machine m = new Machine(ip);
			//     m.Ttl = rep.Options.Ttl;   // attention : rep.Options peut être null
			//     return m;
			// }
			return null;
		}

		/// <summary>
		/// Nom d'hôte de la machine (résolution DNS inverse), "" si inconnu.
		/// </summary>
		public static string nomHote(string ip){
			// TODO (Ilyas) : System.Net.Dns.GetHostEntry(ip).HostName dans un try / catch
			return "";
		}
	}
}
