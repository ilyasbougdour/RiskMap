using System;
using System.IO;
using System.Net;
using System.Text;

namespace RiskMap
{
	/// <summary>
	/// INTERFACE (Aida) - export du rapport en page HTML (s'ouvre dans un navigateur, imprimable en PDF).
	/// Version de départ volontairement simple : à mettre en forme (CSS, couleurs, sommaire...).
	/// </summary>
	public class ExportRapport
	{
		public static void versHtml(ResultatScan r, string nomFichier){
			StreamWriter sw = new StreamWriter(nomFichier, false, Encoding.UTF8);
			sw.WriteLine("<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>Rapport RiskMap</title></head><body>");
			sw.WriteLine("<h1>Rapport RiskMap</h1>");
			sw.WriteLine("<p>Scan du " + r.DateScan.ToString() + " - plage " + html(r.PlageReseau) + "</p>");

			sw.WriteLine("<h2>Synthèse</h2>");
			if (r.SyntheseGlobale != ""){
				sw.WriteLine("<p><i>" + html(r.SourceSynthese) + "</i></p>");
				sw.WriteLine("<pre>" + html(r.SyntheseGlobale) + "</pre>");
			}
			else
				sw.WriteLine("<p>Pas de synthèse générée.</p>");

			sw.WriteLine("<h2>Machines</h2>");
			sw.WriteLine("<table border=\"1\" cellpadding=\"4\"><tr><th>Adresse</th><th>Nom</th><th>Système probable</th><th>Score</th><th>Niveau</th></tr>");
			foreach(Machine m in r.Machines)
				sw.WriteLine("<tr><td>" + m.AdresseIp + "</td><td>" + html(m.NomHote) + "</td><td>" + html(m.SystemeProbable)
					+ "</td><td>" + m.Score.ToString() + "</td><td>" + Textes.niveau(m.Niveau) + "</td></tr>");
			sw.WriteLine("</table>");

			foreach(Machine m in r.Machines){
				sw.WriteLine("<h3>" + html(m.nomAffiche()) + " (" + m.AdresseIp + ")</h3><ul>");
				foreach(Service s in m.Services){
					sw.WriteLine("<li>" + html(s.description()));
					foreach(Vulnerabilite v in s.Vulnerabilites)
						sw.WriteLine("<br>&nbsp;&nbsp;<a href=\"" + v.lien() + "\">" + v.IdCve + "</a> - CVSS " + v.ScoreCvss.ToString() + " (" + v.gravite() + ")");
					sw.WriteLine("</li>");
				}
				sw.WriteLine("</ul>");
			}
			sw.WriteLine("<p><small>Machines « potentiellement vulnérables » : une version ancienne peut déjà être corrigée par le système.</small></p>");
			sw.WriteLine("</body></html>");
			sw.Close();
		}

		//protège les caractères spéciaux (<, >, &...) des textes venus du réseau
		private static string html(string texte){
			return WebUtility.HtmlEncode(texte);
		}
	}
}
