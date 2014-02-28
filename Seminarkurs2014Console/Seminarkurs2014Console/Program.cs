using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Gsmgh.Alm.Database;
using Gsmgh.Alm.Model;

namespace Seminarkurs2014Console {
	class Program {

		static void Main ( string[] args ) {

			Console.WriteLine( "- - - - - - - - - - - - - - - - " );
			Console.WriteLine( "Seminarkurs 2014 Konsolenanwendung" );

			// Create a DatabaseFacade instance
			DatabaseFacade database = new DatabaseFacade();

			// Tell the DatabseFacade to use the MySQLConnector
			database.SetDatabaseConnector(
				new MySQLConnector( "SERVER=localhost;DATABASE=seminarkurs2014;UID=root;PASSWORD=root;" )
			);

			// Open a connection to the datbase
			Console.Write( "Open a connection to the database..." );
			database.OpenConnection();
			Console.WriteLine( "OK" );

			// - - - - - - - - - - - - - - - - - I N S E R T - - - - - - - - - - - - - - - - -
			Console.WriteLine( "- - - - - - - - - - - - - - - - - I N S E R T - - - - - - - - - - - - - - - - -" );

			// Create a RootNode instance
			RootNode rootNode = new RootNode();
			rootNode.SetName( "GSMGH" );
			rootNode.SetDescription( "Gewerbliche Schule Bad Mergentheim" );
			rootNode.SetLastUpdated( DateTime.Now );

			// Save the RootNode instance
			Console.Write( String.Format( "Save {0}..." , rootNode.ToString() ) );
			Boolean inserted = database.InsertNode( rootNode );
			if( inserted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Create a FolderNode instance
			FolderNode folderNode01 = new FolderNode();
			folderNode01.SetParentID( rootNode.GetID() );
			folderNode01.SetPath( rootNode.GetID().ToString() );
			folderNode01.SetName( "0. Erdgeschoss" );
			folderNode01.SetDescription( "Alle Sensoren im Erdgeschoss" );
			folderNode01.SetLastUpdated( DateTime.Now );

			// Save the FolderNode instance
			Console.Write( String.Format( "Save {0}..." , folderNode01.ToString() ) );
			inserted = database.InsertNode( folderNode01 );
			if( inserted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Creat a FolderNode instance
			FolderNode folderNode02 = new FolderNode();
			folderNode02.SetParentID( rootNode.GetID() );
			folderNode02.SetPath( rootNode.GetID().ToString() );
			folderNode02.SetName( "1. Stock" );
			folderNode02.SetDescription( "Alle Sensoren im 1. Stock" );
			folderNode02.SetLastUpdated( DateTime.Now );

			// Save the FolderNode instance
			Console.Write( String.Format( "Save {0}..." , folderNode02.ToString() ) );
			inserted = database.InsertNode( folderNode02 );
			if( inserted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Create a DeviceNode instance
			DeviceNode deviceNode01 = new DeviceNode();
			deviceNode01.SetParentID( folderNode01.GetID() );
			deviceNode01.SetPath( folderNode01.GetPath() + "/" + folderNode01.GetID() );
			deviceNode01.SetName( "Raum 140" );
			deviceNode01.SetDescription( "Sensor im Raum 140" );
			deviceNode01.SetLastUpdated( DateTime.Now );
			deviceNode01.SetIPAddress( new IPAddress( new Byte[] { 192 , 168 , 5 , 12 } ) );
			deviceNode01.SetPort( 10002 );

			// Save the DeviceNode instance
			Console.Write( String.Format( "Save {0}..." , deviceNode01.ToString() ) );
			inserted = database.InsertNode( deviceNode01 );
			if( inserted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Create a DatapointNode instance
			DatapointNode datapointNode01 = new DatapointNode();
			datapointNode01.SetParentID( deviceNode01.GetParentID() );
			datapointNode01.SetPath( deviceNode01.GetPath() + "/" + datapointNode01.GetID() );
			datapointNode01.SetName( "Sauerstoff" );
			datapointNode01.SetDescription( "CO2 gehalt in ppm" );
			datapointNode01.SetLastUpdated( DateTime.Now );
			datapointNode01.SetDatapointType( DatapointNode.TYPE_FLOATING_POINT );
			datapointNode01.SetUnit( "ppm" );

			// Save the DatapointNode instance
			Console.Write( String.Format( "Save {0}..." , rootNode.ToString() ) );
			inserted = database.InsertNode( datapointNode01 );
			if( inserted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Create a DatapointNode instance
			DatapointNode datapointNode02 = new DatapointNode();
			datapointNode02.SetParentID( deviceNode01.GetParentID() );
			datapointNode02.SetPath( deviceNode01.GetPath() + "/" + deviceNode01.GetID() );
			datapointNode02.SetName( "Lautstärke" );
			datapointNode02.SetDescription( "Lautstärke in db" );
			datapointNode02.SetLastUpdated( DateTime.Now );
			datapointNode02.SetDatapointType( DatapointNode.TYPE_INTEGER );
			datapointNode02.SetUnit( "db" );

			// Save the DatapointNode instance
			Console.Write( String.Format( "Save {0}..." , datapointNode02.ToString() ) );
			inserted = database.InsertNode( datapointNode02 );
			if( inserted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// - - - - - - - - - - - - - - - - - S E L E C T - - - - - - - - - - - - - - - - -
			Console.WriteLine( "- - - - - - - - - - - - - - - - - S E L E C T - - - - - - - - - - - - - - - - -" );

			// Get RootNode from database
			Console.Write( "Get RootNode..." );
			RootNode rootNodeSelect = database.GetRootNode();
			if( rootNodeSelect != null ) Console.WriteLine( String.Format( "OK: {0}" , rootNodeSelect.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get FolderNode from database
			Console.Write( String.Format( "Get FolderNode with ID '{0}'..." , folderNode01.GetID() ) );
			FolderNode folderNode01Select = database.GetFolderNodeByID( folderNode01.GetID() );
			if( folderNode01Select != null ) Console.WriteLine( String.Format( "OK: {0}" , folderNode01Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get FolderNode from database
			Console.Write( String.Format( "Get FolderNode with ID '{0}'..." , folderNode02.GetID() ) );
			FolderNode folderNode02Select = database.GetFolderNodeByID( folderNode02.GetID() );
			if( folderNode02Select != null ) Console.WriteLine( String.Format( "OK: {0}" , folderNode02Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get DeviceNode from database
			Console.Write( String.Format( "Get DeviceNode with ID '{0}'..." , deviceNode01.GetID() ) );
			DeviceNode deviceNode01Select = database.GetDeviceNodeByID( deviceNode01.GetID() );
			if( deviceNode01Select != null ) Console.WriteLine( String.Format( "OK: {0}" , deviceNode01Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get DatapointNode from database
			Console.Write( String.Format( "Get DatapointNode with ID '{0}'..." , datapointNode01.GetID() ) );
			DatapointNode datapointNode01Select = database.GetDatapointNodeByID( datapointNode01.GetID() );
			if( datapointNode01Select != null ) Console.WriteLine( String.Format( "OK: {0}" , datapointNode01Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get DatapointNode from database
			Console.Write( String.Format( "Get DatapointNode with ID '{0}'..." , datapointNode02.GetID() ) );
			DatapointNode datapointNode02Select = database.GetDatapointNodeByID( datapointNode02.GetID() );
			if( datapointNode02Select != null ) Console.WriteLine( String.Format( "OK: {0}" , datapointNode02Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// - - - - - - - - - - - - - - - - - U P D A T E - - - - - - - - - - - - - - - - -
			Console.WriteLine( "- - - - - - - - - - - - - - - - - U P D A T E - - - - - - - - - - - - - - - - -" );

			// Update RootNode
			Console.Write( String.Format( "Update RootNode with ID '{0}'..." , rootNode.GetID() ) );
			rootNode.SetName( rootNode.GetName() + " [UPDATED]" );
			Boolean updated = database.UpdateNode( rootNode );
			if( updated ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Update FolderNode
			Console.Write( String.Format( "Update FolderNode with ID '{0}'..." , folderNode01.GetID() ) );
			folderNode01.SetName( folderNode01.GetName() + " [UPDATED]" );
			updated = database.UpdateNode( folderNode01 );
			if( updated ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Update FolderNode
			Console.Write( String.Format( "Update FolderNode with ID '{0}'..." , folderNode02.GetID() ) );
			folderNode02.SetName( folderNode02.GetName() + " [UPDATED]" );
			updated = database.UpdateNode( folderNode02 );
			if( updated ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Update DeviceNode
			Console.Write( String.Format( "Update DeviceNode with ID '{0}'..." , deviceNode01.GetID() ) );
			deviceNode01.SetName( deviceNode01.GetName() + " [UPDATED]" );
			updated = database.UpdateNode( deviceNode01 );
			if( updated ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Update DatapointNode
			Console.Write( String.Format( "Update DatapointNode with ID '{0}'..." , datapointNode01.GetID() ) );
			datapointNode01.SetName( datapointNode01.GetName() + " [UPDATED]" );
			updated = database.UpdateNode( datapointNode01 );
			if( updated ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Update DatapointNode
			Console.Write( String.Format( "Update DatapointNode with ID '{0}'..." , datapointNode02.GetID() ) );
			datapointNode02.SetName( datapointNode02.GetName() + " [UPDATED]" );
			updated = database.UpdateNode( datapointNode02 );
			if( updated ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// - - - - - - - - - - - - - - - - - S E L E C T - - - - - - - - - - - - - - - - -
			Console.WriteLine( "- - - - - - - - - - - - - - - - - S E L E C T - - - - - - - - - - - - - - - - -" );

			// Get RootNode from database
			Console.Write( "Get RootNode..." );
			rootNodeSelect = database.GetRootNode();
			if( rootNodeSelect != null ) Console.WriteLine( String.Format( "OK: {0}" , rootNodeSelect.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get FolderNode from database
			Console.Write( String.Format( "Get FolderNode with ID '{0}'..." , folderNode01.GetID() ) );
			folderNode01Select = database.GetFolderNodeByID( folderNode01.GetID() );
			if( folderNode01Select != null ) Console.WriteLine( String.Format( "OK: {0}" , folderNode01Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get FolderNode from database
			Console.Write( String.Format( "Get FolderNode with ID '{0}'..." , folderNode02.GetID() ) );
			folderNode02Select = database.GetFolderNodeByID( folderNode02.GetID() );
			if( folderNode02Select != null ) Console.WriteLine( String.Format( "OK: {0}" , folderNode02Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get DeviceNode from database
			Console.Write( String.Format( "Get DeviceNode with ID '{0}'..." , deviceNode01.GetID() ) );
			deviceNode01Select = database.GetDeviceNodeByID( deviceNode01.GetID() );
			if( deviceNode01Select != null ) Console.WriteLine( String.Format( "OK: {0}" , deviceNode01Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get DatapointNode from database
			Console.Write( String.Format( "Get DatapointNode with ID '{0}'..." , datapointNode01.GetID() ) );
			datapointNode01Select = database.GetDatapointNodeByID( datapointNode01.GetID() );
			if( datapointNode01Select != null ) Console.WriteLine( String.Format( "OK: {0}" , datapointNode01Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get DatapointNode from database
			Console.Write( String.Format( "Get DatapointNode with ID '{0}'..." , datapointNode02.GetID() ) );
			datapointNode02Select = database.GetDatapointNodeByID( datapointNode02.GetID() );
			if( datapointNode02Select != null ) Console.WriteLine( String.Format( "OK: {0}" , datapointNode02Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// - - - - - - - - - - - - - - - - - D E L E T E - - - - - - - - - - - - - - - - -
			Console.WriteLine( "- - - - - - - - - - - - - - - - - D E L E T E - - - - - - - - - - - - - - - - -" );

			// Update RootNode
			Console.Write( String.Format( "Delete RootNode with ID '{0}'..." , rootNode.GetID() ) );
			Boolean deleted = database.DeleteNode( rootNode );
			if( deleted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Update FolderNode
			Console.Write( String.Format( "Delete FolderNode with ID '{0}'..." , folderNode01.GetID() ) );
			deleted = database.DeleteNode( folderNode01 );
			if( deleted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Update FolderNode
			Console.Write( String.Format( "Delete FolderNode with ID '{0}'..." , folderNode02.GetID() ) );
			deleted = database.DeleteNode( folderNode02 );
			if( deleted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Update DeviceNode
			Console.Write( String.Format( "Delete FolderNode with ID '{0}'..." , deviceNode01.GetID() ) );
			deleted = database.DeleteNode( deviceNode01 );
			if( deleted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Update DatapointNode
			Console.Write( String.Format( "Delete FolderNode with ID '{0}'..." , datapointNode01.GetID() ) );
			deleted = database.DeleteNode( datapointNode01 );
			if( deleted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// Update DatapointNode
			Console.Write( String.Format( "Delete FolderNode with ID '{0}'..." , datapointNode02.GetID() ) );
			deleted = database.DeleteNode( datapointNode02 );
			if( deleted ) Console.WriteLine( "OK" );
			else Console.WriteLine( "FAILED" );

			// - - - - - - - - - - - - - - - - - S E L E C T - - - - - - - - - - - - - - - - -
			Console.WriteLine( "- - - - - - - - - - - - - - - - - S E L E C T - - - - - - - - - - - - - - - - -" );

			// Get RootNode from database
			Console.Write( "Get RootNode..." );
			rootNodeSelect = database.GetRootNode();
			if( rootNodeSelect != null ) Console.WriteLine( String.Format( "OK: {0}" , rootNodeSelect.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get FolderNode from database
			Console.Write( String.Format( "Get FolderNode with ID '{0}'..." , folderNode01.GetID() ) );
			folderNode01Select = database.GetFolderNodeByID( folderNode01.GetID() );
			if( folderNode01Select != null ) Console.WriteLine( String.Format( "OK: {0}" , folderNode01Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get FolderNode from database
			Console.Write( String.Format( "Get FolderNode with ID '{0}'..." , folderNode02.GetID() ) );
			folderNode02Select = database.GetFolderNodeByID( folderNode02.GetID() );
			if( folderNode02Select != null ) Console.WriteLine( String.Format( "OK: {0}" , folderNode02Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get DeviceNode from database
			Console.Write( String.Format( "Get DeviceNode with ID '{0}'..." , deviceNode01.GetID() ) );
			deviceNode01Select = database.GetDeviceNodeByID( deviceNode01.GetID() );
			if( deviceNode01Select != null ) Console.WriteLine( String.Format( "OK: {0}" , deviceNode01Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get DatapointNode from database
			Console.Write( String.Format( "Get DatapointNode with ID '{0}'..." , datapointNode01.GetID() ) );
			datapointNode01Select = database.GetDatapointNodeByID( datapointNode01.GetID() );
			if( datapointNode01Select != null ) Console.WriteLine( String.Format( "OK: {0}" , datapointNode01Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Get DatapointNode from database
			Console.Write( String.Format( "Get DatapointNode with ID '{0}'..." , datapointNode02.GetID() ) );
			datapointNode02Select = database.GetDatapointNodeByID( datapointNode02.GetID() );
			if( datapointNode02Select != null ) Console.WriteLine( String.Format( "OK: {0}" , datapointNode02Select.ToString() ) );
			else Console.WriteLine( "FAILED" );

			// Close connection to the datbase
			Console.Write( "Close the connection to the database..." );
			database.CloseConnection();
			Console.WriteLine( "OK" );

			Console.ReadKey();

		}
	}
}
