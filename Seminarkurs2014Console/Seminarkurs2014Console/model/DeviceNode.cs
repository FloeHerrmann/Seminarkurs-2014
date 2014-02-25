using Seminarkurs2014Console.database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Seminarkurs2014Console.model {
	/// <summary>
	/// Model for device node
	/// </summary>
	public class DeviceNode : AbstractObjectNode {

		/// <summary>
		/// Log4net logger instance for logging purposes
		/// </summary>
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		/// <summary>
		/// Number of the datapoint node type [3]
		/// </summary>
		public static Int32 NODE_TYPE = 3;

		/// <summary>
		/// IP Address of the device
		/// </summary>
		private IPAddress IpAddress;

		/// <summary>
		/// Port for incoming connections
		/// </summary>
		private Int32 Port;

		/// <summary>
		/// Last connection to the device
		/// </summary>
		private DateTime LastConnection;

		/// <summary>
		/// Constructor
		/// </summary>
		public DeviceNode () {
			logger.Trace( String.Format( "Creating a '{0}' instance" , MethodBase.GetCurrentMethod().DeclaringType.Name ) );
			this.SetType( NODE_TYPE );
		}

		/// <summary>
		/// Constructor using a ObjectTreeRow for initialization
		/// </summary>
		public DeviceNode ( ObjectTreeRow RootNodeRow ) {
			logger.Trace( String.Format( "Creating a '{0}' instance" , MethodBase.GetCurrentMethod().DeclaringType.Name ) );
			this.SetID( RootNodeRow.GetObjectID() );
			this.SetParentID( RootNodeRow.GetObjectParentID() );
			this.SetType( NODE_TYPE );
			this.SetPath( RootNodeRow.GetObjectPath() );
			this.SetName( RootNodeRow.GetObjectName() );
			this.SetDescription( RootNodeRow.GetObjectDescription() );
			this.SetLastUpdated( RootNodeRow.GetObjectLastUpdated() );
			this.SetIPAddress( RootNodeRow.GetDeviceIPAddress() );
			this.SetPort( RootNodeRow.GetDevicePort() );
			this.SetLastConnection( RootNodeRow.GetDeviceLastConnection() );
		}

		/// <summary>
		/// Set the IP Address for the device
		/// </summary>
		public void SetIPAddress ( IPAddress IpAddress ) {
			this.IpAddress = IpAddress;		
		}

		/// <summary>
		/// Get the IP Address of the device
		/// </summary>
		public IPAddress GetIPAddress ( ) {
			return this.IpAddress;
		}

		/// <summary>
		/// Set the Port for incoming connections of the device
		/// </summary>
		public void SetPort ( Int32 Port ) {
			this.Port = Port;
		}

		/// <summary>
		/// Get the Port for incoming connections of the device
		/// </summary>
		public Int32 GetPort () {
			return this.Port;
		}

		/// <summary>
		/// Set the date of the last connection to the device
		/// </summary>
		public void SetLastConnection ( DateTime LastConnection ) {
			this.LastConnection = LastConnection;
		}

		/// <summary>
		/// Get the date of the last connection to the device
		/// </summary>
		public DateTime GetLastConnection () {
			return this.LastConnection;
		}

		/// <summary>
		/// Gibt diese Implementierung vom Typ System.String zurück
		/// </summary>
		public override String ToString () {
			return String.Format( "DeviceNode({0}/{1}/{2}/{3})({4}:{5})" ,
				this.GetID() ,
				this.GetParentID() ,
				this.GetName() ,
				this.GetDescription(),
				this.GetIPAddress(),
				this.GetPort()
			);
		}

	}
}
