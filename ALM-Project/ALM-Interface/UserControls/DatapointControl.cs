using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Gsmgh.Alm.Database;
using Gsmgh.Alm.Model;

namespace ALM_Interface.UserControls {
	public partial class DatapointControl : UserControl {
		public DatapointControl () {
			InitializeComponent();
		}

		public void LoadDatapointByID( Int64 nodeID , DatabaseFacade database ) {
			database.OpenConnection();
				DatapointNode datapoint = database.GetDatapointNodeByID( nodeID );
				FolderNode parent = database.GetFolderNodeByID( datapoint.GetParentID() );
			database.CloseConnection();
			this.LabelDatapointName.Text = datapoint.GetName();
			this.LabelDatapointDescription.Text = datapoint.GetDescription();
			this.LabelDatapointID.Text = datapoint.GetID().ToString();
			this.LabelDatapointParent.Text = parent.GetName();
			if( datapoint.GetDatapointType() == DatapointNode.TYPE_BOOL ) this.LabelDatapointType.Text = "Bool (Wahr/Falsch)";
			if( datapoint.GetDatapointType() == DatapointNode.TYPE_FLOATING_POINT ) this.LabelDatapointType.Text = "Fließkommazahl";
			if( datapoint.GetDatapointType() == DatapointNode.TYPE_INTEGER ) this.LabelDatapointType.Text = "Ganzzahl";
			if( datapoint.GetDatapointType() == DatapointNode.TYPE_TEXT ) this.LabelDatapointType.Text = "Text";
			this.LabelDatapointUnit.Text = datapoint.GetUnit();
			this.LabelDatapointLastValue.Text = datapoint.GetLastValue() + " (" + datapoint.GetLastValueUpdate() + ")";
			this.LabelDatapointLastUpdate.Text = datapoint.GetLastUpdated().ToString();
		}
	}
}
