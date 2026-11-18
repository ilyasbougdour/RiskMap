using System;
using System.Drawing;
using System.Windows.Forms;

namespace RiskMap
{
	/// <summary>
	/// INTERFACE (Aida) - boîte de dialogue (modale) : plage à scanner + confirmation d'autorisation.
	/// Le bouton Lancer reste grisé tant que la case d'autorisation n'est pas cochée.
	/// </summary>
	public partial class FormNouveauScan : Form
	{
		public FormNouveauScan()
		{
			InitializeComponent();
		}

		//préfixe réseau, ex. "192.168.56."
		public string getPrefixe(){
			string p = this.textBoxPrefixe.Text.Trim();
			if (p.EndsWith(".") == false)
				p = p + ".";
			return p;
		}

		public int getDebut(){
			return (int) Math.Min(this.numericUpDownDebut.Value, this.numericUpDownFin.Value);
		}

		public int getFin(){
			return (int) Math.Max(this.numericUpDownDebut.Value, this.numericUpDownFin.Value);
		}

		void CheckBoxAutorisationCheckedChanged(object sender, EventArgs e)
		{
			this.buttonLancer.Enabled = this.checkBoxAutorisation.Checked;
		}
	}
}
