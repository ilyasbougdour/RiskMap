namespace RiskMap
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ouvrirToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem enregistrerToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exempleToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exporterToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem scanToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nouveauScanToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem analyseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem analyserToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem syntheseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem outilsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem parametresToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView dataGridViewMachines;
		private System.Windows.Forms.DataGridViewTextBoxColumn colIp;
		private System.Windows.Forms.DataGridViewTextBoxColumn colNom;
		private System.Windows.Forms.DataGridViewTextBoxColumn colSysteme;
		private System.Windows.Forms.DataGridViewTextBoxColumn colServices;
		private System.Windows.Forms.DataGridViewTextBoxColumn colCve;
		private System.Windows.Forms.DataGridViewTextBoxColumn colScore;
		private System.Windows.Forms.DataGridViewTextBoxColumn colNiveau;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCarte;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel labelStatut;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialogRapport;

		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ouvrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enregistrerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exempleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exporterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.scanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nouveauScanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.analyseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.analyserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.syntheseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.outilsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.parametresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.dataGridViewMachines = new System.Windows.Forms.DataGridView();
			this.colIp = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colNom = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colSysteme = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colServices = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colCve = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colNiveau = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.flowLayoutPanelCarte = new System.Windows.Forms.FlowLayoutPanel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.labelStatut = new System.Windows.Forms.ToolStripStatusLabel();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.saveFileDialogRapport = new System.Windows.Forms.SaveFileDialog();
			this.menuStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewMachines)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			//
			// menuStrip1
			//
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.fichierToolStripMenuItem,
			this.scanToolStripMenuItem,
			this.analyseToolStripMenuItem,
			this.outilsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(984, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			//
			// fichierToolStripMenuItem
			//
			this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.ouvrirToolStripMenuItem,
			this.enregistrerToolStripMenuItem,
			this.toolStripSeparator1,
			this.exempleToolStripMenuItem,
			this.toolStripSeparator2,
			this.exporterToolStripMenuItem,
			this.toolStripSeparator3,
			this.quitterToolStripMenuItem});
			this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
			this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.fichierToolStripMenuItem.Text = "Fichier";
			//
			// ouvrirToolStripMenuItem
			//
			this.ouvrirToolStripMenuItem.Name = "ouvrirToolStripMenuItem";
			this.ouvrirToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
			this.ouvrirToolStripMenuItem.Text = "Ouvrir un résultat...";
			this.ouvrirToolStripMenuItem.Click += new System.EventHandler(this.OuvrirToolStripMenuItemClick);
			//
			// enregistrerToolStripMenuItem
			//
			this.enregistrerToolStripMenuItem.Name = "enregistrerToolStripMenuItem";
			this.enregistrerToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
			this.enregistrerToolStripMenuItem.Text = "Enregistrer le résultat...";
			this.enregistrerToolStripMenuItem.Click += new System.EventHandler(this.EnregistrerToolStripMenuItemClick);
			//
			// toolStripSeparator1
			//
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(247, 6);
			//
			// exempleToolStripMenuItem
			//
			this.exempleToolStripMenuItem.Name = "exempleToolStripMenuItem";
			this.exempleToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
			this.exempleToolStripMenuItem.Text = "Charger les données d\'exemple";
			this.exempleToolStripMenuItem.Click += new System.EventHandler(this.ExempleToolStripMenuItemClick);
			//
			// toolStripSeparator2
			//
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(247, 6);
			//
			// exporterToolStripMenuItem
			//
			this.exporterToolStripMenuItem.Name = "exporterToolStripMenuItem";
			this.exporterToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
			this.exporterToolStripMenuItem.Text = "Exporter le rapport...";
			this.exporterToolStripMenuItem.Click += new System.EventHandler(this.ExporterToolStripMenuItemClick);
			//
			// toolStripSeparator3
			//
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(247, 6);
			//
			// quitterToolStripMenuItem
			//
			this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
			this.quitterToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
			this.quitterToolStripMenuItem.Text = "Quitter";
			this.quitterToolStripMenuItem.Click += new System.EventHandler(this.QuitterToolStripMenuItemClick);
			//
			// scanToolStripMenuItem
			//
			this.scanToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.nouveauScanToolStripMenuItem});
			this.scanToolStripMenuItem.Name = "scanToolStripMenuItem";
			this.scanToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.scanToolStripMenuItem.Text = "Scan";
			//
			// nouveauScanToolStripMenuItem
			//
			this.nouveauScanToolStripMenuItem.Name = "nouveauScanToolStripMenuItem";
			this.nouveauScanToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.nouveauScanToolStripMenuItem.Text = "Nouveau scan...";
			this.nouveauScanToolStripMenuItem.Click += new System.EventHandler(this.NouveauScanToolStripMenuItemClick);
			//
			// analyseToolStripMenuItem
			//
			this.analyseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.analyserToolStripMenuItem,
			this.syntheseToolStripMenuItem});
			this.analyseToolStripMenuItem.Name = "analyseToolStripMenuItem";
			this.analyseToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
			this.analyseToolStripMenuItem.Text = "Analyse";
			//
			// analyserToolStripMenuItem
			//
			this.analyserToolStripMenuItem.Name = "analyserToolStripMenuItem";
			this.analyserToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
			this.analyserToolStripMenuItem.Text = "Rechercher les CVE et calculer le risque";
			this.analyserToolStripMenuItem.Click += new System.EventHandler(this.AnalyserToolStripMenuItemClick);
			//
			// syntheseToolStripMenuItem
			//
			this.syntheseToolStripMenuItem.Name = "syntheseToolStripMenuItem";
			this.syntheseToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
			this.syntheseToolStripMenuItem.Text = "Générer la synthèse...";
			this.syntheseToolStripMenuItem.Click += new System.EventHandler(this.SyntheseToolStripMenuItemClick);
			//
			// outilsToolStripMenuItem
			//
			this.outilsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.parametresToolStripMenuItem});
			this.outilsToolStripMenuItem.Name = "outilsToolStripMenuItem";
			this.outilsToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
			this.outilsToolStripMenuItem.Text = "Outils";
			//
			// parametresToolStripMenuItem
			//
			this.parametresToolStripMenuItem.Name = "parametresToolStripMenuItem";
			this.parametresToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.parametresToolStripMenuItem.Text = "Paramètres...";
			this.parametresToolStripMenuItem.Click += new System.EventHandler(this.ParametresToolStripMenuItemClick);
			//
			// splitContainer1
			//
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			//
			// splitContainer1.Panel1
			//
			this.splitContainer1.Panel1.Controls.Add(this.dataGridViewMachines);
			//
			// splitContainer1.Panel2
			//
			this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanelCarte);
			this.splitContainer1.Size = new System.Drawing.Size(984, 515);
			this.splitContainer1.SplitterDistance = 220;
			this.splitContainer1.TabIndex = 1;
			//
			// dataGridViewMachines
			//
			this.dataGridViewMachines.AllowUserToAddRows = false;
			this.dataGridViewMachines.AllowUserToDeleteRows = false;
			this.dataGridViewMachines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridViewMachines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewMachines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.colIp,
			this.colNom,
			this.colSysteme,
			this.colServices,
			this.colCve,
			this.colScore,
			this.colNiveau});
			this.dataGridViewMachines.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridViewMachines.Location = new System.Drawing.Point(0, 0);
			this.dataGridViewMachines.MultiSelect = false;
			this.dataGridViewMachines.Name = "dataGridViewMachines";
			this.dataGridViewMachines.ReadOnly = true;
			this.dataGridViewMachines.RowHeadersVisible = false;
			this.dataGridViewMachines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridViewMachines.Size = new System.Drawing.Size(984, 220);
			this.dataGridViewMachines.TabIndex = 0;
			this.dataGridViewMachines.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewMachinesCellDoubleClick);
			//
			// colIp
			//
			this.colIp.HeaderText = "Adresse IP";
			this.colIp.Name = "colIp";
			this.colIp.ReadOnly = true;
			//
			// colNom
			//
			this.colNom.HeaderText = "Nom";
			this.colNom.Name = "colNom";
			this.colNom.ReadOnly = true;
			//
			// colSysteme
			//
			this.colSysteme.HeaderText = "Système probable";
			this.colSysteme.Name = "colSysteme";
			this.colSysteme.ReadOnly = true;
			//
			// colServices
			//
			this.colServices.HeaderText = "Services";
			this.colServices.Name = "colServices";
			this.colServices.ReadOnly = true;
			//
			// colCve
			//
			this.colCve.HeaderText = "CVE";
			this.colCve.Name = "colCve";
			this.colCve.ReadOnly = true;
			//
			// colScore
			//
			this.colScore.HeaderText = "Score";
			this.colScore.Name = "colScore";
			this.colScore.ReadOnly = true;
			//
			// colNiveau
			//
			this.colNiveau.HeaderText = "Niveau";
			this.colNiveau.Name = "colNiveau";
			this.colNiveau.ReadOnly = true;
			//
			// flowLayoutPanelCarte
			//
			this.flowLayoutPanelCarte.AutoScroll = true;
			this.flowLayoutPanelCarte.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanelCarte.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanelCarte.Name = "flowLayoutPanelCarte";
			this.flowLayoutPanelCarte.Padding = new System.Windows.Forms.Padding(8);
			this.flowLayoutPanelCarte.Size = new System.Drawing.Size(984, 291);
			this.flowLayoutPanelCarte.TabIndex = 0;
			//
			// statusStrip1
			//
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.labelStatut});
			this.statusStrip1.Location = new System.Drawing.Point(0, 539);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(984, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			//
			// labelStatut
			//
			this.labelStatut.Name = "labelStatut";
			this.labelStatut.Size = new System.Drawing.Size(40, 17);
			this.labelStatut.Text = "Prêt";
			//
			// openFileDialog
			//
			this.openFileDialog.Filter = "Résultat RiskMap (*.xml)|*.xml";
			this.openFileDialog.Title = "Ouvrir un résultat";
			//
			// saveFileDialog
			//
			this.saveFileDialog.Filter = "Résultat RiskMap (*.xml)|*.xml";
			this.saveFileDialog.Title = "Enregistrer le résultat";
			//
			// saveFileDialogRapport
			//
			this.saveFileDialogRapport.Filter = "Page web (*.html)|*.html";
			this.saveFileDialogRapport.Title = "Exporter le rapport";
			//
			// MainForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(984, 561);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "RiskMap";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewMachines)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}
