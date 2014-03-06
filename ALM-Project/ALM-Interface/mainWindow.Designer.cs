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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.LabelNavigation = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// nodeTreeView
			// 
			this.nodeTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.nodeTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeTreeView.FullRowSelect = true;
			this.nodeTreeView.Location = new System.Drawing.Point(6, 29);
			this.nodeTreeView.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
			this.nodeTreeView.Name = "nodeTreeView";
			this.nodeTreeView.ShowNodeToolTips = true;
			this.nodeTreeView.ShowRootLines = false;
			this.nodeTreeView.Size = new System.Drawing.Size(180, 467);
			this.nodeTreeView.TabIndex = 0;
			this.nodeTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.nodeTreeViewAfterCollapse);
			this.nodeTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.nodeTreeViewAfterExpand);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Window;
			this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.nodeTreeView, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.LabelNavigation, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(653, 502);
			this.tableLayoutPanel1.TabIndex = 1;
			this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
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
			// mainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(653, 502);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "mainWindow";
			this.Text = "Form1";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView nodeTreeView;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label LabelNavigation;
		private System.Windows.Forms.Label label1;

	}
}

