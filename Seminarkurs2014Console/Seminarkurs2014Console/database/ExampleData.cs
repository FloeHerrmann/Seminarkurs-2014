using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seminarkurs2014Console.model;
using Seminarkurs2014Console.database;
using System.Net;

namespace Seminarkurs2014Console.database {
	public class ExampleData {

		List<AbstractObjectNode> nodes = new List<AbstractObjectNode>();

		public void InsertExampleData ( DatabaseFacade database ) {

			database.OpenConnection();

			RootNode rootNode = new RootNode();
			rootNode.SetName( "GSMGH" );
			rootNode.SetDescription( "Gewerbliche Schule Bad Mergentheim" );
			rootNode.SetLastUpdated( DateTime.Now );
			database.InsertNode( rootNode );
			nodes.Add( rootNode );

			FolderNode folderNode01 = new FolderNode();
			folderNode01.SetParentID( rootNode.GetID() );
			folderNode01.SetPath( rootNode.GetID().ToString() );
			folderNode01.SetName( "0. Erdgeschoss" );
			folderNode01.SetDescription( "Alle Sensoren im Erdgeschoss" );
			folderNode01.SetLastUpdated( DateTime.Now );
			database.InsertNode( folderNode01 );
			nodes.Add( folderNode01 );
			
			FolderNode folderNode02 = new FolderNode();
			folderNode02.SetParentID( rootNode.GetID() );
			folderNode02.SetPath( rootNode.GetID().ToString() );
			folderNode02.SetName( "1. Stock" );
			folderNode02.SetDescription( "Alle Sensoren im 1. Stock" );
			folderNode02.SetLastUpdated( DateTime.Now );
			database.InsertNode( folderNode02 );
			nodes.Add( folderNode02 );

			FolderNode folderNode03 = new FolderNode();
			folderNode03.SetParentID( rootNode.GetID() );
			folderNode03.SetPath( rootNode.GetID().ToString() );
			folderNode03.SetName( "2. Stock" );
			folderNode03.SetDescription( "Alle Sensoren im 2. Stock" );
			folderNode03.SetLastUpdated( DateTime.Now );
			database.InsertNode( folderNode03 );
			nodes.Add( folderNode03 );

			DeviceNode deviceNode01 = new DeviceNode();
			deviceNode01.SetParentID( folderNode01.GetID() );
			deviceNode01.SetPath( folderNode01.GetPath() + "/" + folderNode01.GetID() );
			deviceNode01.SetName( "Raum 140" );
			deviceNode01.SetDescription( "Sensor im Raum 140" );
			deviceNode01.SetLastUpdated( DateTime.Now );
			deviceNode01.SetIPAddress( new IPAddress( new Byte[]{ 192 , 168 , 5 , 12 } ) );
			deviceNode01.SetPort( 10002 );
			database.InsertNode( deviceNode01 );
			nodes.Add( deviceNode01 );

			database.CloseConnection();
		}

		public void DeleteExampleDate () {
		
		}
	}
}
