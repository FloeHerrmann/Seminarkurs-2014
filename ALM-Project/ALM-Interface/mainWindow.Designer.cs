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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainWindow));
			this.NodeTreeView = new System.Windows.Forms.TreeView();
			this.MainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.LabelNavigation = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.ContentPanel = new System.Windows.Forms.Panel();
			this.MainLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// NodeTreeView
			// 
			this.NodeTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.NodeTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NodeTreeView.FullRowSelect = true;
			this.NodeTreeView.Location = new System.Drawing.Point(6, 29);
			this.NodeTreeView.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
			this.NodeTreeView.Name = "NodeTreeView";
			this.NodeTreeView.ShowNodeToolTips = true;
			this.NodeTreeView.ShowRootLines = false;
			this.NodeTreeView.Size = new System.Drawing.Size(180, 467);
			this.NodeTreeView.TabIndex = 0;
			this.NodeTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.NodeTreeViewAfterCollapse);
			this.NodeTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.NodeTreeViewAfterExpand);
			this.NodeTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeTreeViewNodeMouseClick);
			// 
			// MainLayoutPanel
			// 
			this.MainLayoutPanel.BackColor = System.Drawing.SystemColors.Window;
			this.MainLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.MainLayoutPanel.ColumnCount = 2;
			this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.MainLayoutPanel.Controls.Add(this.NodeTreeView, 0, 1);
			this.MainLayoutPanel.Controls.Add(this.LabelNavigation, 0, 0);
			this.MainLayoutPanel.Controls.Add(this.label1, 1, 0);
			this.MainLayoutPanel.Controls.Add(this.ContentPanel, 1, 1);
			this.MainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.MainLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MainLayoutPanel.Name = "MainLayoutPanel";
			this.MainLayoutPanel.RowCount = 2;
			this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MainLayoutPanel.Size = new System.Drawing.Size(653, 502);
			this.MainLayoutPanel.TabIndex = 1;
			// 
			// LabelNavigation
			// 
			this.LabelNavigation.AutoSize = true;
			this.LabelNavigation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
			this.LabelNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LabelNavigation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LabelNavigation.Location = new System.Drawing.Point(1, 1);
			this.LabelNavigation.Margin = new System.Windows.Forms.Padding(0);
			this.LabelNavigation.Name = "LabelNavigation";
			this.LabelNavigation.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.LabelNavigation.Size = new System.Drawing.Size(185, 22);
			this.LabelNavigation.TabIndex = 1;
			this.LabelNavigation.Text = "Navigation";
			this.LabelNavigation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(187, 1);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.label1.Size = new System.Drawing.Size(465, 22);
			this.label1.TabIndex = 2;
			this.label1.Text = "Details";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ContentPanel
			// 
			this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ContentPanel.Location = new System.Drawing.Point(190, 27);
			this.ContentPanel.Name = "ContentPanel";
			this.ContentPanel.Size = new System.Drawing.Size(459, 471);
			this.ContentPanel.TabIndex = 3;
			// 
			// mainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(653, 502);
			this.Controls.Add(this.MainLayoutPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "mainWindow";
			this.Text = "Luft- & Lautstärkeüberwachung";
			this.MainLayoutPanel.ResumeLayout(false);
			this.MainLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView NodeTreeView;
		private System.Windows.Forms.TableLayoutPanel MainLayoutPanel;
		private System.Windows.Forms.Label LabelNavigation;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel ContentPanel;

	}
}

