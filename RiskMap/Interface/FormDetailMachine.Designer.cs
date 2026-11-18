namespace RiskMap
{
	partial class FormDetailMachine
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label labelTitre;
		private System.Windows.Forms.Label labelSysteme;
		private System.Windows.Forms.Label labelScore;
		private System.Windows.Forms.DataGridView dataGridViewServices;
		private System.Windows.Forms.DataGridViewTextBoxColumn colPort;
		private System.Windows.Forms.DataGridViewTextBoxColumn colService;
		private System.Windows.Forms.DataGridViewTextBoxColumn colVersion;
		private System.Windows.Forms.DataGridViewTextBoxColumn colEtat;
		private System.Windows.Forms.DataGridViewTextBoxColumn colCve;
		private System.Windows.Forms.TextBox textBoxDetail;

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
			this.labelTitre = new System.Windows.Forms.Label();
			this.labelSysteme = new System.Windows.Forms.Label();
			this.labelScore = new System.Windows.Forms.Label();
			this.dataGridViewServices = new System.Windows.Forms.DataGridView();
			this.colPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colService = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colEtat = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colCve = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.textBoxDetail = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewServices)).BeginInit();
			this.SuspendLayout();
			//
			// labelTitre
			//
			this.labelTitre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.labelTitre.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
			this.labelTitre.Location = new System.Drawing.Point(12, 9);
			this.labelTitre.Name = "labelTitre";
			this.labelTitre.Padding = new System.Windows.Forms.Padding(4);
			this.labelTitre.Size = new System.Drawing.Size(660, 30);
			this.labelTitre.TabIndex = 0;
			this.labelTitre.Text = "Machine";
			//
			// labelSysteme
			//
			this.labelSysteme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.labelSysteme.Location = new System.Drawing.Point(12, 46);
			this.labelSysteme.Name = "labelSysteme";
			this.labelSysteme.Size = new System.Drawing.Size(660, 18);
			this.labelSysteme.TabIndex = 1;
			this.labelSysteme.Text = "Système probable :";
			//
			// labelScore
			//
			this.labelScore.Location = new System.Drawing.Point(12, 66);
			this.labelScore.Name = "labelScore";
			this.labelScore.Size = new System.Drawing.Size(400, 18);
			this.labelScore.TabIndex = 2;
			this.labelScore.Text = "Score :";
			//
			// dataGridViewServices
			//
			this.dataGridViewServices.AllowUserToAddRows = false;
			this.dataGridViewServices.AllowUserToDeleteRows = false;
			this.dataGridViewServices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridViewServices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridViewServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewServices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.colPort,
			this.colService,
			this.colVersion,
			this.colEtat,
			this.colCve});
			this.dataGridViewServices.Location = new System.Drawing.Point(12, 92);
			this.dataGridViewServices.Name = "dataGridViewServices";
			this.dataGridViewServices.ReadOnly = true;
			this.dataGridViewServices.RowHeadersVisible = false;
			this.dataGridViewServices.Size = new System.Drawing.Size(660, 150);
			this.dataGridViewServices.TabIndex = 3;
			//
			// colPort
			//
			this.colPort.FillWeight = 40F;
			this.colPort.HeaderText = "Port";
			this.colPort.Name = "colPort";
			this.colPort.ReadOnly = true;
			//
			// colService
			//
			this.colService.FillWeight = 70F;
			this.colService.HeaderText = "Service";
			this.colService.Name = "colService";
			this.colService.ReadOnly = true;
			//
			// colVersion
			//
			this.colVersion.HeaderText = "Logiciel / version";
			this.colVersion.Name = "colVersion";
			this.colVersion.ReadOnly = true;
			//
			// colEtat
			//
			this.colEtat.FillWeight = 70F;
			this.colEtat.HeaderText = "État";
			this.colEtat.Name = "colEtat";
			this.colEtat.ReadOnly = true;
			//
			// colCve
			//
			this.colCve.FillWeight = 160F;
			this.colCve.HeaderText = "CVE";
			this.colCve.Name = "colCve";
			this.colCve.ReadOnly = true;
			//
			// textBoxDetail
			//
			this.textBoxDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxDetail.Location = new System.Drawing.Point(12, 252);
			this.textBoxDetail.Multiline = true;
			this.textBoxDetail.Name = "textBoxDetail";
			this.textBoxDetail.ReadOnly = true;
			this.textBoxDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxDetail.Size = new System.Drawing.Size(660, 197);
			this.textBoxDetail.TabIndex = 4;
			//
			// FormDetailMachine
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(684, 461);
			this.Controls.Add(this.textBoxDetail);
			this.Controls.Add(this.dataGridViewServices);
			this.Controls.Add(this.labelScore);
			this.Controls.Add(this.labelSysteme);
			this.Controls.Add(this.labelTitre);
			this.Name = "FormDetailMachine";
			this.Text = "Détail de la machine";
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewServices)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}
