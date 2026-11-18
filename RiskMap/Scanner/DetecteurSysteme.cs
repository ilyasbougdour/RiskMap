using System;

namespace RiskMap
{
	/// <summary>
	/// MODULE SCANNER (Ilyas) - étape 3b : estimer le système d'exploitation en croisant les indices.
	/// Le système n'est jamais annoncé directement : c'est une estimation avec un niveau de confiance.
	/// </summary>
	public class DetecteurSysteme
	{
		/// <summary>
		/// Remplit SystemeProbable, ConfianceSysteme (0-100) et EtatSysteme de la machine.
		/// À appeler APRÈS le scan des ports et la lecture des bannières.
		/// </summary>
		public static void estimer(Machine m){
			// TODO (Ilyas) : idée de départ, on additionne des indices
			//  - TTL ~64 -> Linux/macOS ; ~128 -> Windows ; 255 -> équipement réseau
			//  - bannière contenant "Ubuntu", "Debian", "Windows", "Microsoft-IIS"...
			//  - ports 445 + 3389 ouverts ensemble -> Windows très probable
			//  - fabricant (MAC) : imprimante, routeur, téléphone
			// Plus les indices sont d'accord, plus la confiance monte.
			// Aucun indice -> EtatSysteme = NonDetermine.
			m.EtatSysteme = EtatInfo.NonDetermine;
		}

		/// <summary>
		/// Adresse MAC d'une machine du même sous-réseau, "" si inconnue.
		/// </summary>
		public static string adresseMac(string ip){
			// TODO (Ilyas) : lancer "arp -a" (System.Diagnostics.Process, RedirectStandardOutput)
			// après le ping, puis chercher la ligne de l'IP.
			return "";
		}

		/// <summary>
		/// Fabricant à partir des 3 premiers octets de la MAC (petite table à faire à la main), "" si inconnu.
		/// </summary>
		public static string fabricant(string mac){
			// TODO (Ilyas) : ex. "08:00:27" -> "Oracle VirtualBox", "00:15:5D" -> "Microsoft (Hyper-V)"
			return "";
		}
	}
}
