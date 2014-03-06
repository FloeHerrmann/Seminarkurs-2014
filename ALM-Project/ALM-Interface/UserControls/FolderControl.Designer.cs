namespace ALM_Interface.UserControls {
	partial class FolderControl {
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose ( bool disposing ) {
			if( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Vom Komponenten-Designer generierter Code

		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent () {
			this.FolderTabControl = new System.Windows.Forms.TabControl();
			this.FolderTabControlOverview = new System.Windows.Forms.TabPage();
			this.FolderTabControlSettings = new System.Windows.Forms.TabPage();
			this.FolderTabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// FolderTabControl
			// 
			this.FolderTabControl.Controls.Add(this.FolderTabControlOverview);
			this.FolderTabControl.Controls.Add(this.FolderTabControlSettings);
			this.FolderTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FolderTabControl.Location = new System.Drawing.Point(0, 0);
			this.FolderTabControl.Name = "FolderTabControl";
			this.FolderTabControl.SelectedIndex = 0;
			this.FolderTabControl.Size = new System.Drawing.Size(433, 345);
			this.FolderTabControl.TabIndex = 0;
			// 
			// FolderTabControlOverview
			// 
			this.FolderTabControlOverview.Location = new System.Drawing.Point(4, 22);
			this.FolderTabControlOverview.Name = "FolderTabControlOverview";
			this.FolderTabControlOverview.Padding = new System.Windows.Forms.Padding(3);
			this.FolderTabControlOverview.Size = new System.Drawing.Size(425, 319);
			this.FolderTabControlOverview.TabIndex = 0;
			this.FolderTabControlOverview.Text = "Ordner - Übersicht";
			this.FolderTabControlOverview.UseVisualStyleBackColor = true;
			// 
			// FolderTabControlSettings
			// 
			this.FolderTabControlSettings.Location = new System.Drawing.Point(4, 22);
			this.FolderTabControlSettings.Name = "FolderTabControlSettings";
			this.FolderTabControlSettings.Padding = new System.Windows.Forms.Padding(3);
			this.FolderTabControlSettings.Size = new System.Drawing.Size(425, 319);
			this.FolderTabControlSettings.TabIndex = 1;
			this.FolderTabControlSettings.Text = "Ordner - Eigenschaften";
			this.FolderTabControlSettings.UseVisualStyleBackColor = true;
			// 
			// FolderControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.FolderTabControl);
			this.Name = "FolderControl";
			this.Size = new System.Drawing.Size(433, 345);
			this.FolderTabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl FolderTabControl;
		private System.Windows.Forms.TabPage FolderTabControlOverview;
		private System.Windows.Forms.TabPage FolderTabControlSettings;

	}
}
