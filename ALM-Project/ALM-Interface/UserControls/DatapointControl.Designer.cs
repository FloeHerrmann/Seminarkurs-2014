namespace ALM_Interface.UserControls {
	partial class DatapointControl {
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
			this.DatapointTabControl = new System.Windows.Forms.TabControl();
			this.DatapointTabControlOverview = new System.Windows.Forms.TabPage();
			this.DatapointTabControlSettings = new System.Windows.Forms.TabPage();
			this.DatapointTabControlAnalysis = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.LabelDatapointID = new System.Windows.Forms.Label();
			this.LabelDatapointParent = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.LabelDatapointType = new System.Windows.Forms.Label();
			this.LabelDatapointUnit = new System.Windows.Forms.Label();
			this.LabelDatapointLastUpdate = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.LabelDatapointDescription = new System.Windows.Forms.Label();
			this.LabelDatapointName = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.LabelDatapointLastValue = new System.Windows.Forms.Label();
			this.DatapointTabControl.SuspendLayout();
			this.DatapointTabControlOverview.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// DatapointTabControl
			// 
			this.DatapointTabControl.Controls.Add(this.DatapointTabControlOverview);
			this.DatapointTabControl.Controls.Add(this.DatapointTabControlAnalysis);
			this.DatapointTabControl.Controls.Add(this.DatapointTabControlSettings);
			this.DatapointTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DatapointTabControl.Location = new System.Drawing.Point(0, 0);
			this.DatapointTabControl.Name = "DatapointTabControl";
			this.DatapointTabControl.SelectedIndex = 0;
			this.DatapointTabControl.Size = new System.Drawing.Size(393, 386);
			this.DatapointTabControl.TabIndex = 0;
			// 
			// DatapointTabControlOverview
			// 
			this.DatapointTabControlOverview.Controls.Add(this.tableLayoutPanel1);
			this.DatapointTabControlOverview.Location = new System.Drawing.Point(4, 22);
			this.DatapointTabControlOverview.Name = "DatapointTabControlOverview";
			this.DatapointTabControlOverview.Padding = new System.Windows.Forms.Padding(3);
			this.DatapointTabControlOverview.Size = new System.Drawing.Size(385, 360);
			this.DatapointTabControlOverview.TabIndex = 0;
			this.DatapointTabControlOverview.Text = "Datenpunkt - Übersicht";
			this.DatapointTabControlOverview.UseVisualStyleBackColor = true;
			// 
			// DatapointTabControlSettings
			// 
			this.DatapointTabControlSettings.Location = new System.Drawing.Point(4, 22);
			this.DatapointTabControlSettings.Name = "DatapointTabControlSettings";
			this.DatapointTabControlSettings.Padding = new System.Windows.Forms.Padding(3);
			this.DatapointTabControlSettings.Size = new System.Drawing.Size(385, 360);
			this.DatapointTabControlSettings.TabIndex = 1;
			this.DatapointTabControlSettings.Text = "Datenpunk - Eigenschaften";
			this.DatapointTabControlSettings.UseVisualStyleBackColor = true;
			// 
			// DatapointTabControlAnalysis
			// 
			this.DatapointTabControlAnalysis.Location = new System.Drawing.Point(4, 22);
			this.DatapointTabControlAnalysis.Name = "DatapointTabControlAnalysis";
			this.DatapointTabControlAnalysis.Size = new System.Drawing.Size(385, 360);
			this.DatapointTabControlAnalysis.TabIndex = 2;
			this.DatapointTabControlAnalysis.Text = "Datenpunk - Analyse";
			this.DatapointTabControlAnalysis.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
			this.tableLayoutPanel1.Controls.Add(this.LabelDatapointLastValue, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.LabelDatapointID, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.LabelDatapointParent, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.LabelDatapointType, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.LabelDatapointUnit, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.LabelDatapointLastUpdate, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 8;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(379, 354);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(0, 60);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.label1.Size = new System.Drawing.Size(113, 30);
			this.label1.TabIndex = 0;
			this.label1.Text = "Datenpunkt ID";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(0, 90);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.label2.Size = new System.Drawing.Size(113, 30);
			this.label2.TabIndex = 1;
			this.label2.Text = "Übergordnetes Objekt";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LabelDatapointID
			// 
			this.LabelDatapointID.AutoSize = true;
			this.LabelDatapointID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
			this.LabelDatapointID.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LabelDatapointID.Location = new System.Drawing.Point(113, 60);
			this.LabelDatapointID.Margin = new System.Windows.Forms.Padding(0);
			this.LabelDatapointID.Name = "LabelDatapointID";
			this.LabelDatapointID.Size = new System.Drawing.Size(266, 30);
			this.LabelDatapointID.TabIndex = 2;
			this.LabelDatapointID.Text = "-";
			this.LabelDatapointID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LabelDatapointParent
			// 
			this.LabelDatapointParent.AutoSize = true;
			this.LabelDatapointParent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LabelDatapointParent.Location = new System.Drawing.Point(113, 90);
			this.LabelDatapointParent.Margin = new System.Windows.Forms.Padding(0);
			this.LabelDatapointParent.Name = "LabelDatapointParent";
			this.LabelDatapointParent.Size = new System.Drawing.Size(266, 30);
			this.LabelDatapointParent.TabIndex = 3;
			this.LabelDatapointParent.Text = "-";
			this.LabelDatapointParent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(0, 120);
			this.label4.Margin = new System.Windows.Forms.Padding(0);
			this.label4.Name = "label4";
			this.label4.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.label4.Size = new System.Drawing.Size(113, 30);
			this.label4.TabIndex = 4;
			this.label4.Text = "Datentyp";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point(0, 150);
			this.label5.Margin = new System.Windows.Forms.Padding(0);
			this.label5.Name = "label5";
			this.label5.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.label5.Size = new System.Drawing.Size(113, 30);
			this.label5.TabIndex = 5;
			this.label5.Text = "Einheit";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point(0, 180);
			this.label6.Margin = new System.Windows.Forms.Padding(0);
			this.label6.Name = "label6";
			this.label6.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.label6.Size = new System.Drawing.Size(113, 30);
			this.label6.TabIndex = 6;
			this.label6.Text = "Zuletzt geändert";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LabelDatapointType
			// 
			this.LabelDatapointType.AutoSize = true;
			this.LabelDatapointType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
			this.LabelDatapointType.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LabelDatapointType.Location = new System.Drawing.Point(113, 120);
			this.LabelDatapointType.Margin = new System.Windows.Forms.Padding(0);
			this.LabelDatapointType.Name = "LabelDatapointType";
			this.LabelDatapointType.Size = new System.Drawing.Size(266, 30);
			this.LabelDatapointType.TabIndex = 7;
			this.LabelDatapointType.Text = "-";
			this.LabelDatapointType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LabelDatapointUnit
			// 
			this.LabelDatapointUnit.AutoSize = true;
			this.LabelDatapointUnit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LabelDatapointUnit.Location = new System.Drawing.Point(113, 150);
			this.LabelDatapointUnit.Margin = new System.Windows.Forms.Padding(0);
			this.LabelDatapointUnit.Name = "LabelDatapointUnit";
			this.LabelDatapointUnit.Size = new System.Drawing.Size(266, 30);
			this.LabelDatapointUnit.TabIndex = 8;
			this.LabelDatapointUnit.Text = "-";
			this.LabelDatapointUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LabelDatapointLastUpdate
			// 
			this.LabelDatapointLastUpdate.AutoSize = true;
			this.LabelDatapointLastUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
			this.LabelDatapointLastUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LabelDatapointLastUpdate.Location = new System.Drawing.Point(113, 180);
			this.LabelDatapointLastUpdate.Margin = new System.Windows.Forms.Padding(0);
			this.LabelDatapointLastUpdate.Name = "LabelDatapointLastUpdate";
			this.LabelDatapointLastUpdate.Size = new System.Drawing.Size(266, 30);
			this.LabelDatapointLastUpdate.TabIndex = 9;
			this.LabelDatapointLastUpdate.Text = "-";
			this.LabelDatapointLastUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.LabelDatapointDescription, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.LabelDatapointName, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(379, 60);
			this.tableLayoutPanel2.TabIndex = 10;
			// 
			// LabelDatapointDescription
			// 
			this.LabelDatapointDescription.AutoSize = true;
			this.LabelDatapointDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LabelDatapointDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LabelDatapointDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(111)))), ((int)(((byte)(111)))));
			this.LabelDatapointDescription.Location = new System.Drawing.Point(0, 30);
			this.LabelDatapointDescription.Margin = new System.Windows.Forms.Padding(0);
			this.LabelDatapointDescription.Name = "LabelDatapointDescription";
			this.LabelDatapointDescription.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.LabelDatapointDescription.Size = new System.Drawing.Size(379, 30);
			this.LabelDatapointDescription.TabIndex = 0;
			this.LabelDatapointDescription.Text = "-";
			this.LabelDatapointDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LabelDatapointName
			// 
			this.LabelDatapointName.AutoSize = true;
			this.LabelDatapointName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LabelDatapointName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LabelDatapointName.Location = new System.Drawing.Point(0, 0);
			this.LabelDatapointName.Margin = new System.Windows.Forms.Padding(0);
			this.LabelDatapointName.Name = "LabelDatapointName";
			this.LabelDatapointName.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
			this.LabelDatapointName.Size = new System.Drawing.Size(379, 30);
			this.LabelDatapointName.TabIndex = 1;
			this.LabelDatapointName.Text = "-";
			this.LabelDatapointName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.SystemColors.Window;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(0, 210);
			this.label3.Margin = new System.Windows.Forms.Padding(0);
			this.label3.Name = "label3";
			this.label3.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.label3.Size = new System.Drawing.Size(113, 30);
			this.label3.TabIndex = 11;
			this.label3.Text = "Letzter Wert";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LabelDatapointLastValue
			// 
			this.LabelDatapointLastValue.AutoSize = true;
			this.LabelDatapointLastValue.BackColor = System.Drawing.SystemColors.Window;
			this.LabelDatapointLastValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LabelDatapointLastValue.Location = new System.Drawing.Point(113, 210);
			this.LabelDatapointLastValue.Margin = new System.Windows.Forms.Padding(0);
			this.LabelDatapointLastValue.Name = "LabelDatapointLastValue";
			this.LabelDatapointLastValue.Size = new System.Drawing.Size(266, 30);
			this.LabelDatapointLastValue.TabIndex = 12;
			this.LabelDatapointLastValue.Text = "-";
			this.LabelDatapointLastValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DatapointControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.DatapointTabControl);
			this.Name = "DatapointControl";
			this.Size = new System.Drawing.Size(393, 386);
			this.DatapointTabControl.ResumeLayout(false);
			this.DatapointTabControlOverview.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl DatapointTabControl;
		private System.Windows.Forms.TabPage DatapointTabControlOverview;
		private System.Windows.Forms.TabPage DatapointTabControlAnalysis;
		private System.Windows.Forms.TabPage DatapointTabControlSettings;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label LabelDatapointID;
		private System.Windows.Forms.Label LabelDatapointParent;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label LabelDatapointType;
		private System.Windows.Forms.Label LabelDatapointUnit;
		private System.Windows.Forms.Label LabelDatapointLastUpdate;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label LabelDatapointDescription;
		private System.Windows.Forms.Label LabelDatapointName;
		private System.Windows.Forms.Label LabelDatapointLastValue;
		private System.Windows.Forms.Label label3;

	}
}
