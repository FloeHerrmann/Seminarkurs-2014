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
using System.Windows.Forms.DataVisualization.Charting;

namespace ALM_Interface.UserControls {
	public partial class DatapointControl : UserControl {

		DatabaseFacade Database;
		Int64 DatapointID;

		public DatapointControl () {
			InitializeComponent();
			ChartArea area = new ChartArea( "Datapoint" );
			ChartDatapoint.ChartAreas.Add( area );
			ChartDatapoint.Dock = DockStyle.Fill;
		}

		public void LoadDatapointByID( Int64 nodeID , DatabaseFacade database ) {
			this.Database = database;
			this.Database.OpenConnection();
			DatapointNode datapoint = this.Database.GetDatapointNodeByID( nodeID );
			FolderNode parent = this.Database.GetFolderNodeByID( datapoint.GetParentID() );
			this.Database.CloseConnection();
			this.DatapointID = datapoint.GetID();
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
			DrawDatapointChart();
		}

		private void DrawDatapointChart () {

			ChartDatapoint.Series.Clear();

			Series datapointSeries = new Series();

			datapointSeries.ChartArea = "Datapoint";
			datapointSeries.ChartType = SeriesChartType.Line;
			datapointSeries.XValueType = ChartValueType.DateTime;

			DateTime From = DateTime.Now;
			TimeSpan FromTime = new TimeSpan(0, 0, 0);
			From = From.Date + FromTime;

			DateTime To = DateTime.Now;
			TimeSpan ToTime = new TimeSpan(23, 59, 59);
			To = To.Date + ToTime;

			this.Database.OpenConnection();
			List<DatapointValueNode> datapointValues = this.Database.GetDatapointValuesByDatapointID( this.DatapointID , From , To );
			this.Database.CloseConnection();

			Double maxValue = 1.0;
			foreach( DatapointValueNode valueNode in datapointValues ) {
				Double value = Double.Parse( valueNode.GetStringValue() );
				datapointSeries.Points.AddXY( valueNode.GetTimeStamp().ToOADate() , value );
				if( value > maxValue ) maxValue = value;
			}

			ChartDatapoint.Series.Add( datapointSeries );
			ChartDatapoint.ChartAreas[ 0 ].RecalculateAxesScale();
		}
	}
}
