using System;
using System.Drawing;
using System.Windows.Forms;

namespace RiskMap
{
	/// <summary>
	/// INTERFACE (Aida) - fenêtre d'affichage (non modale) du détail d'une machine.
	/// </summary>
	public partial class FormDetailMachine : Form
	{
		public FormDetailMachine()
		{
			InitializeComponent();
		}

		//remplit la fenêtre avec les informations de la machine
		public void setMachine(Machine m){
			this.Text = "Détail - " + m.nomAffiche();
			this.labelTitre.Text = m.nomAffiche() + "  (" + m.AdresseIp + ")";
			this.labelTitre.BackColor = CarteReseau.couleur(m.Niveau);
			this.labelSysteme.Text = "Système probable : " + m.SystemeProbable
				+ " - confiance " + m.ConfianceSysteme.ToString() + " % (" + Textes.etat(m.EtatSysteme) + ")"
				+ "   MAC : " + m.AdresseMac + " " + m.Fabricant;
			this.labelScore.Text = "Score : " + m.Score.ToString() + " - risque " + Textes.niveau(m.Niveau).ToLower();

			this.dataGridViewServices.Rows.Clear();
			foreach(Service s in m.Services){
				string cve = "";
				foreach(Vulnerabilite v in s.Vulnerabilites)
					cve = cve + v.ToString() + "  ";
				this.dataGridViewServices.Rows.Add(s.Port, s.NomService, s.Produit + " " + s.Version, Textes.etat(s.EtatVersion), cve);
			}

			string texte = "Calcul du score :" + Environment.NewLine;
			foreach(string ligne in m.DetailScore)
				texte = texte + "  " + ligne + Environment.NewLine;
			if (m.Synthese != "")
				texte = texte + Environment.NewLine + "Synthèse :" + Environment.NewLine + m.Synthese;
			this.textBoxDetail.Text = texte;
		}
	}
}
