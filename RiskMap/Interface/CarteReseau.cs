using System;
using System.Drawing;
using System.Windows.Forms;

namespace RiskMap
{
	/// <summary>
	/// INTERFACE (Aida) - carte du réseau.
	/// Version de départ : une case colorée par machine (couleur = niveau de risque), détail au clic.
	/// À améliorer : regrouper par sous-réseau, dessiner la passerelle au centre, légende, etc.
	/// </summary>
	public class CarteReseau
	{
		//dessine une case par machine dans le panneau ; clic = gestionnaire appelé au clic sur une case
		public static void dessiner(FlowLayoutPanel panneau, ResultatScan r, EventHandler clic){
			panneau.Controls.Clear();
			foreach(Machine m in r.Machines){
				Button b = new Button();
				b.Width = 170;
				b.Height = 90;
				b.FlatStyle = FlatStyle.Flat;
				b.BackColor = CarteReseau.couleur(m.Niveau);
				string texte = "";
				if (m.NomHote != "")
					texte = m.NomHote + Environment.NewLine;
				texte = texte + m.AdresseIp + Environment.NewLine
					+ m.SystemeProbable + Environment.NewLine
					+ "Score " + m.Score.ToString() + " - " + Textes.niveau(m.Niveau);
				b.Text = texte;
				//la machine est rangée dans le bouton pour la retrouver au clic
				b.Tag = m;
				b.Click += clic;
				panneau.Controls.Add(b);
			}
		}

		//couleur associée à un niveau de risque
		public static Color couleur(NiveauRisque niveau){
			switch(niveau){
				case NiveauRisque.Critique: return Color.FromArgb(235, 110, 100);
				case NiveauRisque.Eleve: return Color.FromArgb(245, 170, 90);
				case NiveauRisque.Moyen: return Color.FromArgb(245, 220, 110);
				default: return Color.FromArgb(150, 210, 140);
			}
		}
	}
}
