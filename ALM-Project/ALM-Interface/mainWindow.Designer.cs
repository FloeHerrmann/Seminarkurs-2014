namespace ALM_Interface {
	partial class mainWindow {
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

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent () {
			this.nodeTreeView = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// nodeTreeView
			// 
			this.nodeTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeTreeView.Location = new System.Drawing.Point(0, 0);
			this.nodeTreeView.Name = "nodeTreeView";
			this.nodeTreeView.Size = new System.Drawing.Size(270, 522);
			this.nodeTreeView.TabIndex = 0;
			// 
			// mainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(270, 522);
			this.Controls.Add(this.nodeTreeView);
			this.Name = "mainWindow";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView nodeTreeView;

	}
}

