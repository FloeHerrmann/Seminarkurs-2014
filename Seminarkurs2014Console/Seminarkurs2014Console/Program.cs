using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seminarkurs2014Console.database;
using Seminarkurs2014Console.model;
using System.Net;

[assembly: log4net.Config.XmlConfigurator( ConfigFile = "Log4Net.config" , Watch = true )]

namespace Seminarkurs2014Console {
	class Program {

		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		static void Main ( string[] args ) {

			logger.Info( "- - - - - - - - - - - - - - - - " );
			logger.Info( "Seminarkurs 2014 Konsolenanwendung" );

			DatabaseFacade database = new DatabaseFacade();
			database.SetDatabaseConnector( new MySQLConnector( "SERVER=localhost;DATABASE=seminarkurs2014;UID=root;PASSWORD=root;" ) );

			// Save all nodes that are inserted
			List<AbstractObjectNode> nodes = new List<AbstractObjectNode>();

			// Open a connection to the datbase
			database.OpenConnection();

			// Create a root node and save it
			RootNode rootNode = new RootNode();
			rootNode.SetName( "GSMGH" );
			rootNode.SetDescription( "Gewerbliche Schule Bad Mergentheim" );
			rootNode.SetLastUpdated( DateTime.Now );

			database.InsertNode( rootNode );
			nodes.Add( rootNode );

			// Create a folder node and save it
			FolderNode folderNode01 = new FolderNode();
			folderNode01.SetParentID( rootNode.GetID() );
			folderNode01.SetPath( rootNode.GetID().ToString() );
			folderNode01.SetName( "0. Erdgeschoss" );
			folderNode01.SetDescription( "Alle Sensoren im Erdgeschoss" );
			folderNode01.SetLastUpdated( DateTime.Now );

			database.InsertNode( folderNode01 );
			nodes.Add( folderNode01 );

			// Creat a folder node and save it
			FolderNode folderNode02 = new FolderNode();
			folderNode02.SetParentID( rootNode.GetID() );
			folderNode02.SetPath( rootNode.GetID().ToString() );
			folderNode02.SetName( "1. Stock" );
			folderNode02.SetDescription( "Alle Sensoren im 1. Stock" );
			folderNode02.SetLastUpdated( DateTime.Now );

			database.InsertNode( folderNode02 );
			nodes.Add( folderNode02 );

			// Create a device node and save it
			DeviceNode deviceNode01 = new DeviceNode();
			deviceNode01.SetParentID( folderNode01.GetID() );
			deviceNode01.SetPath( folderNode01.GetPath() + "/" + folderNode01.GetID() );
			deviceNode01.SetName( "Raum 140" );
			deviceNode01.SetDescription( "Sensor im Raum 140" );
			deviceNode01.SetLastUpdated( DateTime.Now );
			deviceNode01.SetIPAddress( new IPAddress( new Byte[] { 192 , 168 , 5 , 12 } ) );
			deviceNode01.SetPort( 10002 );

			database.InsertNode( deviceNode01 );
			nodes.Add( deviceNode01 );

			// Create a datapoint node and save it
			DatapointNode datapointNode01 = new DatapointNode();
			datapointNode01.SetParentID( deviceNode01.GetParentID() );
			datapointNode01.SetPath( deviceNode01.GetPath() + "/" + deviceNode01.GetID() );
			datapointNode01.SetName( "Sauerstoff" );
			datapointNode01.SetDescription( "CO2 gehalt in ppm" );
			datapointNode01.SetLastUpdated( DateTime.Now );
			datapointNode01.SetDatapointType( DatapointNode.TYPE_FLOATING_POINT );
			datapointNode01.SetUnit( "ppm" );

			database.InsertNode( datapointNode01 );
			nodes.Add( datapointNode01 );

			// Create a datapoint node and save it
			DatapointNode datapointNode02 = new DatapointNode();
			datapointNode02.SetParentID( deviceNode01.GetParentID() );
			datapointNode02.SetPath( deviceNode01.GetPath() + "/" + deviceNode01.GetID() );
			datapointNode02.SetName( "Lautstärke" );
			datapointNode02.SetDescription( "Lautstärke in db" );
			datapointNode02.SetLastUpdated( DateTime.Now );
			datapointNode02.SetDatapointType( DatapointNode.TYPE_INTEGER );
			datapointNode02.SetUnit( "db" );

			database.InsertNode( datapointNode02 );
			nodes.Add( datapointNode02 );

			DeviceNode deviceNodeSelect = database.GetDeviceNodeByID( deviceNode01.GetID() );

			rootNode.SetName( rootNode.GetName() + "(UPDATED)" );
			database.UpdateNode( rootNode );

			folderNode01.SetName( folderNode01.GetName() + "(UPDATED)" );
			database.UpdateNode( folderNode01 );

			deviceNode01.SetName( deviceNode01.GetName() + "(UPDATED)" );
			database.UpdateNode( deviceNode01 );

			datapointNode01.SetName( datapointNode01.GetName() + "(UPDATED)" );
			database.UpdateNode( datapointNode01 );

			foreach( AbstractObjectNode currentNode in nodes ){
				database.DeleteNodeByID( currentNode.GetID() );
			}

			database.CloseConnection();

			Console.ReadKey();

		}
	}
}
