using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gsmgh.Alm.Database;

namespace Gsmgh.Alm.Model {
	/// <summary>
	/// Model for device node
	/// </summary>
	public class DeviceNode : AbstractObjectNode {

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
		/// Maximum Threshold for CO2 Concentration
		/// </summary>
		private Int32 CO2Threshold;

		/// <summary>
		/// Maximum Threshold for Loudness
		/// </summary>
		private Int32 LoudnessThreshold;

		/// <summary>
		/// Last connection to the device
		/// </summary>
		private DateTime LastConnection;

		/// <summary>
		/// Constructor
		/// </summary>
		public DeviceNode () {
			this.SetType( NODE_TYPE );
		}

		/// <summary>
		/// Constructor using a ObjectTreeRow for initialization
		/// </summary>
		public DeviceNode ( ObjectTreeRow DeviceNodeRow ) {
			this.SetID( DeviceNodeRow.GetObjectID() );
			this.SetParentID( DeviceNodeRow.GetObjectParentID() );
			this.SetType( NODE_TYPE );
			this.SetName( DeviceNodeRow.GetObjectName() );
			this.SetDescription( DeviceNodeRow.GetObjectDescription() );
			this.SetLastUpdated( DeviceNodeRow.GetObjectLastUpdated() );
			this.SetCO2Threshold( DeviceNodeRow.GetCO2Threshold() );
			this.SetLoudnessThreshold( DeviceNodeRow.GetLoudnessThreshold() );
			this.SetIPAddress( DeviceNodeRow.GetDeviceIPAddress() );
			this.SetPort( DeviceNodeRow.GetDevicePort() );
			this.SetLastConnection( DeviceNodeRow.GetDeviceLastConnection() );
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
		/// Set the CO2 threshold
		/// </summary>
		public void SetCO2Threshold ( Int32 CO2Threshold ) {
			this.CO2Threshold = CO2Threshold;
		}

		/// <summary>
		/// Get the CO2 threshold
		/// </summary>
		public Int32 GetCO2Threshold () {
			return this.CO2Threshold;
		}

		/// <summary>
		/// Set the loudness threshold
		/// </summary>
		public void SetLoudnessThreshold ( Int32 LoudnessThreshold ) {
			this.LoudnessThreshold = LoudnessThreshold;
		}

		/// <summary>
		/// Get the loudness threshold
		/// </summary>
		public Int32 GetLoudnessThreshold () {
			return this.LoudnessThreshold;
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
