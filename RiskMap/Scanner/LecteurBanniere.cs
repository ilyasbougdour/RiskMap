using System;
using System.Net.Sockets;

namespace RiskMap
{
	/// <summary>
	/// MODULE SCANNER (Ilyas) - étape 3a : lire ce que chaque service annonce (bannière)
	/// et en déduire produit + version + état (Identifié / Masqué / Non déterminé).
	/// </summary>
	public class LecteurBanniere
	{
		private int delaiMs;

		public LecteurBanniere(int prmDelaiMs)
		{
			delaiMs = prmDelaiMs;
		}

		/// <summary>
		/// Remplit Banniere, Produit, Version et EtatVersion du service.
		/// En cas d'erreur ou de délai dépassé : EtatVersion = NonDetermine (ne jamais planter).
		/// </summary>
		public void lire(string ip, Service s){
			// TODO (Ilyas) :
			// - ssh, ftp, smtp, pop3, imap : le service parle en premier -> lire la 1re ligne reçue
			//   (TcpClient + NetworkStream avec ReadTimeout = delaiMs).
			// - http / http-alt : envoyer "HEAD / HTTP/1.0\r\n\r\n" puis chercher la ligne "Server: ..."
			// - autres ports : laisser NonDetermine.
			// Ensuite appeler LecteurBanniere.analyser(s).
			s.EtatVersion = EtatInfo.NonDetermine;
		}

		/// <summary>
		/// Découpe s.Banniere en Produit + Version. Exemples à gérer :
		///   "SSH-2.0-OpenSSH_8.9p1 Ubuntu-3"     -> OpenSSH / 8.9p1         -> Identifie
		///   "220 (vsFTPd 2.3.4)"                 -> vsftpd / 2.3.4          -> Identifie
		///   "Server: Apache/2.2.8 (Ubuntu)"      -> Apache httpd / 2.2.8    -> Identifie
		///   "Server: nginx"                      -> nginx / ""              -> Masque
		///   ""                                   -> ""                      -> NonDetermine
		/// Les noms de produits doivent être ceux de Donnees/produits.xml (voir avec Abdelali).
		/// </summary>
		public static void analyser(Service s){
			// TODO (Ilyas) : String.Split, IndexOf, Substring suffisent.
		}
	}
}
