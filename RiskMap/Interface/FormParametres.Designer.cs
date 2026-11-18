namespace RiskMap
{
	partial class FormParametres
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.CheckBox checkBoxUtiliserIa;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxUrlIa;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxModeleIa;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxCleIa;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxCleNvd;
		private System.Windows.Forms.Button buttonOk;
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
			this.checkBoxUtiliserIa = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxUrlIa = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxModeleIa = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxCleIa = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxCleNvd = new System.Windows.Forms.TextBox();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonAnnuler = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			// checkBoxUtiliserIa
			//
			this.checkBoxUtiliserIa.Location = new System.Drawing.Point(20, 15);
			this.checkBoxUtiliserIa.Name = "checkBoxUtiliserIa";
			this.checkBoxUtiliserIa.Size = new System.Drawing.Size(360, 24);
			this.checkBoxUtiliserIa.TabIndex = 0;
			this.checkBoxUtiliserIa.Text = "Utiliser l\'IA en ligne pour la synthèse (données anonymisées)";
			this.checkBoxUtiliserIa.UseVisualStyleBackColor = true;
			//
			// label1
			//
			this.label1.Location = new System.Drawing.Point(20, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(110, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Adresse de l\'API IA :";
			//
			// textBoxUrlIa
			//
			this.textBoxUrlIa.Location = new System.Drawing.Point(136, 47);
			this.textBoxUrlIa.Name = "textBoxUrlIa";
			this.textBoxUrlIa.Size = new System.Drawing.Size(300, 20);
			this.textBoxUrlIa.TabIndex = 2;
			//
			// label2
			//
			this.label2.Location = new System.Drawing.Point(20, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(110, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Modèle :";
			//
			// textBoxModeleIa
			//
			this.textBoxModeleIa.Location = new System.Drawing.Point(136, 77);
			this.textBoxModeleIa.Name = "textBoxModeleIa";
			this.textBoxModeleIa.Size = new System.Drawing.Size(300, 20);
			this.textBoxModeleIa.TabIndex = 4;
			//
			// label3
			//
			this.label3.Location = new System.Drawing.Point(20, 110);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(110, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "Clé API IA :";
			//
			// textBoxCleIa
			//
			this.textBoxCleIa.Location = new System.Drawing.Point(136, 107);
			this.textBoxCleIa.Name = "textBoxCleIa";
			this.textBoxCleIa.Size = new System.Drawing.Size(300, 20);
			this.textBoxCleIa.TabIndex = 6;
			this.textBoxCleIa.UseSystemPasswordChar = true;
			//
			// label4
			//
			this.label4.Location = new System.Drawing.Point(20, 140);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(110, 20);
			this.label4.TabIndex = 7;
			this.label4.Text = "Clé API NVD :";
			//
			// textBoxCleNvd
			//
			this.textBoxCleNvd.Location = new System.Drawing.Point(136, 137);
			this.textBoxCleNvd.Name = "textBoxCleNvd";
			this.textBoxCleNvd.Size = new System.Drawing.Size(300, 20);
			this.textBoxCleNvd.TabIndex = 8;
			this.textBoxCleNvd.UseSystemPasswordChar = true;
			//
			// buttonOk
			//
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Location = new System.Drawing.Point(255, 175);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(85, 25);
			this.buttonOk.TabIndex = 9;
			this.buttonOk.Text = "Enregistrer";
			this.buttonOk.UseVisualStyleBackColor = true;
			//
			// buttonAnnuler
			//
			this.buttonAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonAnnuler.Location = new System.Drawing.Point(351, 175);
			this.buttonAnnuler.Name = "buttonAnnuler";
			this.buttonAnnuler.Size = new System.Drawing.Size(85, 25);
			this.buttonAnnuler.TabIndex = 10;
			this.buttonAnnuler.Text = "Annuler";
			this.buttonAnnuler.UseVisualStyleBackColor = true;
			//
			// FormParametres
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(458, 215);
			this.Controls.Add(this.buttonAnnuler);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.textBoxCleNvd);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBoxCleIa);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBoxModeleIa);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxUrlIa);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkBoxUtiliserIa);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormParametres";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Paramètres";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}
