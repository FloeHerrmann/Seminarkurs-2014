namespace ALM_Interface.UserControls {
	partial class DeviceControl {
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
			this.DeviceTabControl = new System.Windows.Forms.TabControl();
			this.DeviceTabControlOverview = new System.Windows.Forms.TabPage();
			this.DeviceTabControlAnalysis = new System.Windows.Forms.TabPage();
			this.DeviceTabControlSettings = new System.Windows.Forms.TabPage();
			this.DeviceTabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// DeviceTabControl
			// 
			this.DeviceTabControl.Controls.Add(this.DeviceTabControlOverview);
			this.DeviceTabControl.Controls.Add(this.DeviceTabControlAnalysis);
			this.DeviceTabControl.Controls.Add(this.DeviceTabControlSettings);
			this.DeviceTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DeviceTabControl.Location = new System.Drawing.Point(0, 0);
			this.DeviceTabControl.Name = "DeviceTabControl";
			this.DeviceTabControl.SelectedIndex = 0;
			this.DeviceTabControl.Size = new System.Drawing.Size(609, 440);
			this.DeviceTabControl.TabIndex = 0;
			// 
			// DeviceTabControlOverview
			// 
			this.DeviceTabControlOverview.Location = new System.Drawing.Point(4, 22);
			this.DeviceTabControlOverview.Name = "DeviceTabControlOverview";
			this.DeviceTabControlOverview.Padding = new System.Windows.Forms.Padding(3);
			this.DeviceTabControlOverview.Size = new System.Drawing.Size(601, 414);
			this.DeviceTabControlOverview.TabIndex = 0;
			this.DeviceTabControlOverview.Text = "Anlage - Übersicht";
			this.DeviceTabControlOverview.UseVisualStyleBackColor = true;
			// 
			// DeviceTabControlAnalysis
			// 
			this.DeviceTabControlAnalysis.Location = new System.Drawing.Point(4, 22);
			this.DeviceTabControlAnalysis.Name = "DeviceTabControlAnalysis";
			this.DeviceTabControlAnalysis.Padding = new System.Windows.Forms.Padding(3);
			this.DeviceTabControlAnalysis.Size = new System.Drawing.Size(601, 414);
			this.DeviceTabControlAnalysis.TabIndex = 1;
			this.DeviceTabControlAnalysis.Text = "Anlage - Analyse";
			this.DeviceTabControlAnalysis.UseVisualStyleBackColor = true;
			// 
			// DeviceTabControlSettings
			// 
			this.DeviceTabControlSettings.Location = new System.Drawing.Point(4, 22);
			this.DeviceTabControlSettings.Name = "DeviceTabControlSettings";
			this.DeviceTabControlSettings.Size = new System.Drawing.Size(601, 414);
			this.DeviceTabControlSettings.TabIndex = 2;
			this.DeviceTabControlSettings.Text = "Anlage - Eigenschaften";
			this.DeviceTabControlSettings.UseVisualStyleBackColor = true;
			// 
			// DeviceControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.DeviceTabControl);
			this.Name = "DeviceControl";
			this.Size = new System.Drawing.Size(609, 440);
			this.DeviceTabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl DeviceTabControl;
		private System.Windows.Forms.TabPage DeviceTabControlOverview;
		private System.Windows.Forms.TabPage DeviceTabControlAnalysis;
		private System.Windows.Forms.TabPage DeviceTabControlSettings;

	}
}
