namespace RiskMap
{
	partial class FormNouveauScan
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxPrefixe;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numericUpDownDebut;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numericUpDownFin;
		private System.Windows.Forms.CheckBox checkBoxAutorisation;
		private System.Windows.Forms.Button buttonLancer;
		private System.Windows.Forms.Button buttonAnnuler;

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
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxPrefixe = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDownDebut = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.numericUpDownFin = new System.Windows.Forms.NumericUpDown();
			this.checkBoxAutorisation = new System.Windows.Forms.CheckBox();
			this.buttonLancer = new System.Windows.Forms.Button();
			this.buttonAnnuler = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDebut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownFin)).BeginInit();
			this.SuspendLayout();
			//
			// label1
			//
			this.label1.Location = new System.Drawing.Point(20, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Préfixe réseau :";
			//
			// textBoxPrefixe
			//
			this.textBoxPrefixe.Location = new System.Drawing.Point(126, 20);
			this.textBoxPrefixe.Name = "textBoxPrefixe";
			this.textBoxPrefixe.Size = new System.Drawing.Size(140, 20);
			this.textBoxPrefixe.TabIndex = 1;
			this.textBoxPrefixe.Text = "192.168.56.";
			//
			// label2
			//
			this.label2.Location = new System.Drawing.Point(20, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "Adresses de :";
			//
			// numericUpDownDebut
			//
			this.numericUpDownDebut.Location = new System.Drawing.Point(126, 56);
			this.numericUpDownDebut.Maximum = new decimal(new int[] {
			254,
			0,
			0,
			0});
			this.numericUpDownDebut.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.numericUpDownDebut.Name = "numericUpDownDebut";
			this.numericUpDownDebut.Size = new System.Drawing.Size(55, 20);
			this.numericUpDownDebut.TabIndex = 3;
			this.numericUpDownDebut.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			//
			// label3
			//
			this.label3.Location = new System.Drawing.Point(187, 58);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(20, 20);
			this.label3.TabIndex = 4;
			this.label3.Text = "à";
			//
			// numericUpDownFin
			//
			this.numericUpDownFin.Location = new System.Drawing.Point(211, 56);
			this.numericUpDownFin.Maximum = new decimal(new int[] {
			254,
			0,
			0,
			0});
			this.numericUpDownFin.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.numericUpDownFin.Name = "numericUpDownFin";
			this.numericUpDownFin.Size = new System.Drawing.Size(55, 20);
			this.numericUpDownFin.TabIndex = 5;
			this.numericUpDownFin.Value = new decimal(new int[] {
			254,
			0,
			0,
			0});
			//
			// checkBoxAutorisation
			//
			this.checkBoxAutorisation.Location = new System.Drawing.Point(23, 92);
			this.checkBoxAutorisation.Name = "checkBoxAutorisation";
			this.checkBoxAutorisation.Size = new System.Drawing.Size(330, 36);
			this.checkBoxAutorisation.TabIndex = 6;
			this.checkBoxAutorisation.Text = "Je confirme que ce réseau m\'appartient ou que j\'ai l\'autorisation de le scanner.";
			this.checkBoxAutorisation.UseVisualStyleBackColor = true;
			this.checkBoxAutorisation.CheckedChanged += new System.EventHandler(this.CheckBoxAutorisationCheckedChanged);
			//
			// buttonLancer
			//
			this.buttonLancer.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonLancer.Enabled = false;
			this.buttonLancer.Location = new System.Drawing.Point(172, 140);
			this.buttonLancer.Name = "buttonLancer";
			this.buttonLancer.Size = new System.Drawing.Size(85, 25);
			this.buttonLancer.TabIndex = 7;
			this.buttonLancer.Text = "Lancer";
			this.buttonLancer.UseVisualStyleBackColor = true;
			//
			// buttonAnnuler
			//
			this.buttonAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonAnnuler.Location = new System.Drawing.Point(268, 140);
			this.buttonAnnuler.Name = "buttonAnnuler";
			this.buttonAnnuler.Size = new System.Drawing.Size(85, 25);
			this.buttonAnnuler.TabIndex = 8;
			this.buttonAnnuler.Text = "Annuler";
			this.buttonAnnuler.UseVisualStyleBackColor = true;
			//
			// FormNouveauScan
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(374, 181);
			this.Controls.Add(this.buttonAnnuler);
			this.Controls.Add(this.buttonLancer);
			this.Controls.Add(this.checkBoxAutorisation);
			this.Controls.Add(this.numericUpDownFin);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.numericUpDownDebut);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxPrefixe);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormNouveauScan";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Nouveau scan";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDebut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownFin)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}
