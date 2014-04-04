using Gsmgh.Alm.Database;
using Gsmgh.Alm.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using log4net;
using System.Net.NetworkInformation;

namespace ALM_Data_Service {
	class Program {

		static DatabaseFacade Database;
		private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		static void Main ( string[] args ) {
			log4net.Config.XmlConfigurator.Configure();

			Database = new DatabaseFacade();
			Database.SetDatabaseConnector(
				new MySQLConnector( "SERVER=localhost;DATABASE=seminarkurs2014;UID=root;PASSWORD=root;" )
			);

			Logger.Info( String.Format( "Getting Data for all active devices" ) );
			if( OpenConnection() ) {
				Logger.Debug( "Database Connection Established" );
				List<DeviceNode> deviceNodes = Database.GetAllDeviceNodes();
				Logger.Debug( String.Format( "Found {0} Device Nodes" , deviceNodes.Count ) );
				foreach( DeviceNode deviceNode in deviceNodes ) {
					List<DatapointNode> datapointNodes = Database.GetDatapointNodesByDeviceID( deviceNode.GetID() );
					if( datapointNodes.Count > 0 ) {
						// Buffer for incoming data
						char[] readBuffer = new char[ 150 ];

						// Store the device answer
						String deviceResponse = null;

						if( PingHost( deviceNode.GetIPAddress().ToString() ) ) {
							try {
								Logger.Info( String.Format( "Get Data For Device '{0}' With IP '{1}' And Port '{2}'" , deviceNode.GetName() , deviceNode.GetIPAddress() , deviceNode.GetPort() ) );

								// Create a tcp client and open the connection
								TcpClient tcpClient = new TcpClient( deviceNode.GetIPAddress().ToString() , deviceNode.GetPort() );
								tcpClient.ReceiveTimeout = 10000;
								tcpClient.SendTimeout = 2000;

								// Getting the network stream
								NetworkStream networkStream = tcpClient.GetStream();
								// Create a writer to write on the network stream
								StreamWriter streamWriter = new StreamWriter( networkStream );
								streamWriter.AutoFlush = true;
								// Create a reader to read from the network stream
								StreamReader streamReader = new StreamReader( networkStream );

								// Send a command to the device
								streamWriter.Write( "C:Data:Get;" );

								// Read answer from the device
								deviceResponse = streamReader.ReadLine();

								streamWriter.Close();
								tcpClient.Close();

								DataObject dataObject = JsonConvert.DeserializeObject<DataObject>( deviceResponse );
								DatapointValueNode valueNode = new DatapointValueNode();
								foreach( DatapointNode datapointNode in datapointNodes ) {
									valueNode.SetDatapointID( datapointNode.GetID() );
									valueNode.SetType( datapointNode.GetDatapointType() );
									valueNode.SetTimeStamp( DateTime.Now );
									if( datapointNode.GetDescription().Equals( "Loudness" ) ) {
										valueNode.SetIntegerValue( dataObject.GetLoudness() );
										valueNode.SetDecimalValue( 0.0 );
										valueNode.SetStringValue( dataObject.GetLoudness().ToString() );
										Logger.Debug( "Loudness = " + valueNode.GetIntegerValue() );
									} else if( datapointNode.GetDescription().Equals( "CO2Concentration" ) ) {
										valueNode.SetIntegerValue( dataObject.GetCO2Concentration() );
										valueNode.SetDecimalValue( 0.0 );
										valueNode.SetStringValue( dataObject.GetCO2Concentration().ToString() );
										Logger.Debug( "CO2 Concentration = " + valueNode.GetIntegerValue() );
									} else if( datapointNode.GetDescription().Equals( "Temperature" ) ) {
										valueNode.SetIntegerValue( 0 );
										valueNode.SetDecimalValue( dataObject.GetTemperature() );
										valueNode.SetStringValue( dataObject.GetTemperature().ToString().Replace( "," , "." ) );
										Logger.Debug( "Temperature = " + valueNode.GetDecimalValue() );
									}
									Database.InsertNode( valueNode );
									datapointNode.SetLastValue( valueNode.GetStringValue() );
									datapointNode.SetLastValueUpdate( valueNode.GetTimeStamp() );
									Database.UpdateNode( datapointNode );
								}
							} catch( Exception ex ) {
								Logger.Error( ex.Message , ex );
							}
						} else {
							Logger.Warn( String.Format( "Device '{0}'({1}) Could Not Be Pinged > No Need To Establish A Connection" , deviceNode.GetName() , deviceNode.GetID() ) );
						}
					} else {
						Logger.Warn( String.Format( "Device '{0}'({1}) Has No Datapoints > No Need To Establish A Connection" , deviceNode.GetName() , deviceNode.GetID() ) );
					}
				}
				CloseConnection();
			} else {
				Logger.Warn( "Database Connection Could Not Be Established" );
			}
		}

		static Boolean OpenConnection () {
			try {
				Database.OpenConnection();
				return true;
			} catch( Exception ex ) {
				Logger.Error( ex.Message , ex );
				return false;
			}
		}

		static Boolean CloseConnection () {
			try {
				Database.CloseConnection();
				return true;
			} catch( Exception ex ) {
				Logger.Error( ex.Message , ex );
				return false;
			}
		}

		static Boolean PingHost ( String NameOrIP ) {
			try {
				Ping pingRequest = new Ping();
				PingReply reply = pingRequest.Send( NameOrIP );
				return ( reply.Status == IPStatus.Success );
			} catch( PingException ex ) {
				Logger.Error( ex.Message , ex );
				return false;
			}
		}
	}
}
