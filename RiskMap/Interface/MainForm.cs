using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RiskMap
{
	/// <summary>
	/// INTERFACE (Aida) - fenêtre principale : tableau des machines en haut, carte en bas.
	/// Elle ne fait aucun calcul : elle appelle ScannerReseau, ServiceAnalyse et ISummaryProvider.
	/// </summary>
	public partial class MainForm : Form
	{
		//résultat affiché (null tant qu'on n'a rien chargé)
		private ResultatScan resultat = null;
		//paramètres (config.xml)
		private Configuration config;

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			config = Configuration.charger();
			this.afficher(null);
		}

		//affiche un résultat dans le tableau et sur la carte
		private void afficher(ResultatScan r){
			resultat = r;
			this.dataGridViewMachines.Rows.Clear();
			this.flowLayoutPanelCarte.Controls.Clear();
			if (r == null){
				this.labelStatut.Text = "Aucun résultat : Fichier > Charger les données d'exemple, ou Scan > Nouveau scan.";
				return;
			}
			foreach(Machine m in r.Machines){
				int i = this.dataGridViewMachines.Rows.Add(m.AdresseIp, m.NomHote, m.SystemeProbable,
					m.Services.Count, m.nombreCve(), m.Score, Textes.niveau(m.Niveau));
				//la machine est rangée dans la ligne : reste juste même si l'utilisateur trie le tableau
				this.dataGridViewMachines.Rows[i].Tag = m;
				this.dataGridViewMachines.Rows[i].DefaultCellStyle.BackColor = CarteReseau.couleur(m.Niveau);
			}
			CarteReseau.dessiner(this.flowLayoutPanelCarte, r, new EventHandler(this.CaseMachineClick));
			this.labelStatut.Text = r.Machines.Count.ToString() + " machine(s) - " + r.PlageReseau
				+ " - scan du " + r.DateScan.ToString();
		}

		//fenêtre de détail (non modale) d'une machine
		private void ouvrirDetail(Machine m){
			FormDetailMachine frm = new FormDetailMachine();
			frm.setMachine(m);
			frm.Show();
		}

		//vrai s'il y a un résultat, message sinon
		private bool resultatPresent(){
			if (resultat == null){
				MessageBox.Show("Chargez d'abord un résultat ou lancez un scan.", "RiskMap");
				return false;
			}
			return true;
		}

		//--- carte et tableau ---

		void CaseMachineClick(object sender, EventArgs e)
		{
			Button b = (Button) sender;
			this.ouvrirDetail((Machine) b.Tag);
		}

		void DataGridViewMachinesCellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
				this.ouvrirDetail((Machine) this.dataGridViewMachines.Rows[e.RowIndex].Tag);
		}

		//--- menu Fichier ---

		void OuvrirToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (this.openFileDialog.ShowDialog() == DialogResult.OK){
				try
				{
					this.afficher(ResultatScan.createFromXMLFile(this.openFileDialog.FileName));
				}
				catch
				{
					MessageBox.Show("Ce fichier n'est pas un résultat RiskMap valide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		void EnregistrerToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (this.resultatPresent() && this.saveFileDialog.ShowDialog() == DialogResult.OK)
				resultat.saveToXml(this.saveFileDialog.FileName);
		}

		void ExempleToolStripMenuItemClick(object sender, EventArgs e)
		{
			ResultatScan r = DonneesFictives.creer();
			MoteurRisque moteur = new MoteurRisque(ServiceAnalyse.chargerGrille());
			moteur.evaluer(r);
			this.afficher(r);
		}

		void ExporterToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (this.resultatPresent() && this.saveFileDialogRapport.ShowDialog() == DialogResult.OK){
				ExportRapport.versHtml(resultat, this.saveFileDialogRapport.FileName);
				this.labelStatut.Text = "Rapport exporté : " + this.saveFileDialogRapport.FileName;
			}
		}

		void QuitterToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Close();
		}

		//--- menu Scan ---

		void NouveauScanToolStripMenuItemClick(object sender, EventArgs e)
		{
			FormNouveauScan dlg = new FormNouveauScan();
			if (dlg.ShowDialog() == DialogResult.OK){
				// TODO (Aida) : lancer le scan dans un BackgroundWorker pour que la fenêtre ne se fige pas,
				// avec une barre de progression (le scan d'un /24 peut prendre du temps).
				this.Cursor = Cursors.WaitCursor;
				try
				{
					ScannerReseau scanner = new ScannerReseau(config);
					this.afficher(scanner.lancer(dlg.getPrefixe(), dlg.getDebut(), dlg.getFin()));
				}
				catch (Exception ex)
				{
					MessageBox.Show("Erreur pendant le scan : " + ex.Message, "Scan", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				this.Cursor = Cursors.Default;
			}
		}

		//--- menu Analyse ---

		void AnalyserToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (this.resultatPresent() == false)
				return;
			this.Cursor = Cursors.WaitCursor;
			ServiceAnalyse analyse = new ServiceAnalyse(config);
			int echecs = analyse.analyser(resultat);
			this.Cursor = Cursors.Default;
			this.afficher(resultat);
			if (echecs > 0)
				MessageBox.Show(echecs.ToString() + " recherche(s) de CVE impossible(s) (connexion ou quota NVD).", "Analyse", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		void SyntheseToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (this.resultatPresent() == false)
				return;

			//choix du fournisseur : IA en ligne seulement si elle est configurée ET acceptée
			ISummaryProvider fournisseur = new SyntheseSansIA();
			if (config.iaDisponible()){
				DialogResult rep = MessageBox.Show(
					"Les résultats vont être envoyés à un service d'IA externe (" + config.UrlApiIa + ")."
					+ Environment.NewLine + "Ils sont anonymisés avant l'envoi : ni adresse IP, ni nom de machine, ni adresse MAC."
					+ Environment.NewLine + Environment.NewLine + "Continuer ? (Non = synthèse automatique sans IA)",
					"Synthèse par IA", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (rep == DialogResult.Yes)
					fournisseur = new ApiSummaryProvider(config);
			}

			this.Cursor = Cursors.WaitCursor;
			try
			{
				fournisseur.genererSynthese(resultat);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Synthèse par IA impossible : " + ex.Message + Environment.NewLine
					+ "La synthèse automatique (sans IA) est utilisée à la place.", "Synthèse", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				new SyntheseSansIA().genererSynthese(resultat);
			}
			this.Cursor = Cursors.Default;

			// TODO (Aida) : une vraie fenêtre d'affichage de la synthèse (texte long, copiable)
			MessageBox.Show(resultat.SyntheseGlobale, resultat.SourceSynthese);
		}

		//--- menu Outils ---

		void ParametresToolStripMenuItemClick(object sender, EventArgs e)
		{
			FormParametres dlg = new FormParametres();
			dlg.setConfiguration(config);
			if (dlg.ShowDialog() == DialogResult.OK){
				dlg.lireConfiguration(config);
				config.sauver();
			}
		}
	}
}
