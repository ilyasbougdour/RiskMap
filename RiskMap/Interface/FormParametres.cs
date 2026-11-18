using System;
using System.Drawing;
using System.Windows.Forms;

namespace RiskMap
{
	/// <summary>
	/// INTERFACE (Aida) - boîte de dialogue (modale) des paramètres, enregistrés dans config.xml.
	/// Les clés sont masquées à l'écran (UseSystemPasswordChar).
	/// </summary>
	public partial class FormParametres : Form
	{
		public FormParametres()
		{
			InitializeComponent();
		}

		//remplit les champs à partir de la configuration
		public void setConfiguration(Configuration c){
			this.checkBoxUtiliserIa.Checked = c.UtiliserIa;
			this.textBoxUrlIa.Text = c.UrlApiIa;
			this.textBoxModeleIa.Text = c.ModeleIa;
			this.textBoxCleIa.Text = c.CleApiIa;
			this.textBoxCleNvd.Text = c.CleApiNvd;
		}

		//recopie les champs dans la configuration
		public void lireConfiguration(Configuration c){
			c.UtiliserIa = this.checkBoxUtiliserIa.Checked;
			c.UrlApiIa = this.textBoxUrlIa.Text.Trim();
			c.ModeleIa = this.textBoxModeleIa.Text.Trim();
			c.CleApiIa = this.textBoxCleIa.Text.Trim();
			c.CleApiNvd = this.textBoxCleNvd.Text.Trim();
		}
	}
}
