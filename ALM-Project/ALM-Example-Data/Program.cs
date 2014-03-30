using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsmgh.Alm.Database;
using Gsmgh.Alm.Model;
using System.Net;

namespace ALM_Example_Data {
	class Program {

		static DatabaseFacade Database;
		static Dictionary<String , List<AbstractObjectNode>> ParentIdDictionary;

		static void Main ( string[] args ) {

			Database = new DatabaseFacade();
			Database.SetDatabaseConnector(
				new MySQLConnector( "SERVER=localhost;DATABASE=seminarkurs2014;UID=root;PASSWORD=root;" )
			);

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine( "ALM Example Data Application" );

			while( true ) {

				Console.WriteLine();
				Console.WriteLine( "- - - - - - - - - - - - " );
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine( "1. Insert Example Data" );
				Console.WriteLine( "2. Delet All Data" );
				Console.WriteLine( "3. Get All Data" );
				Console.WriteLine( "4. Exit" );
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine();
				Console.Write( "Input: " );

				String Input = Console.ReadLine();

				if( Input.Equals( "1" ) ) {

					Boolean invalidInput = true;
					Int32 numberOfDays = 0;
					Int32 minutesBetweenValues = 5;

					while( invalidInput ) {
						try {
							Console.WriteLine();
							Console.Write( "For how many days values should be created? [1-31] " );
							String numberOfDaysString = Console.ReadLine();
							numberOfDays = Int32.Parse( numberOfDaysString );
							if( numberOfDays < 1 || numberOfDays > 31 ) {
								ConsoleWriteError( "Value must between 1 and 31" );
								invalidInput = true;
							} else {
								Console.Write( "How many Minutes between 2 values? [1-15] " );
								String minutesBetweenValuesString = Console.ReadLine();
								minutesBetweenValues = Int32.Parse( minutesBetweenValuesString );
								if( minutesBetweenValues < 1 || minutesBetweenValues > 15 ) {
									ConsoleWriteError( "Value must between 1 and 15" );
									invalidInput = true;
								} else {
									invalidInput = false;
								}
							}
						} catch( Exception ex ) {
							ConsoleWriteError( "> Incorrect input: " + ex.Message );
						}
					}

					Console.WriteLine( "> Insert example data into the database" );
					if( OpenConnection() ) {
						InsertExampleData();
						InsertExampleDatapointValues( numberOfDays , minutesBetweenValues );
						CloseConnection();
					}
					
				} else if( Input.Equals( "2" ) ) {
					
					Console.WriteLine();
					Console.WriteLine( "> Delet all data from the database" );
					if( OpenConnection() ) {
						DeleteAllData();
						CloseConnection();
					}

				} else if( Input.Equals( "3" ) ) {
					
					Console.WriteLine();
					Console.WriteLine( "> Get all data from the database" );
					if( OpenConnection() ) {
						GetAllData();
						CloseConnection();
					}

				} else if( Input.Equals( "4" ) ) {
					return;
				} else {
					Console.WriteLine();
					ConsoleWriteError( "Unknown Command!!" );
				}
			}
		}

		static Boolean OpenConnection( ) {
			try {
				Console.Write( "> Open connection..." );
				Database.OpenConnection();
				ConsoleWriteSuccess( "OK" );
				return true;
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
				return false;
			}
		}

		static Boolean CloseConnection () {
			try {
				Console.Write( "> Close connection..." );
				Database.CloseConnection();
				ConsoleWriteSuccess( "OK" );
				return true;
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
				return false;
			}
		}

		static void InsertExampleData () {
			Int64 rootNodeID = InsertExampleRootNode();

			Int64 folderNodeID = InsertExampleFolderNode( rootNodeID );
			Int64 deviceNodeID = InsertExampleDeviceNode( folderNodeID );
			InsertExampleDatapointNodes( deviceNodeID );

			folderNodeID = InsertExampleFolderNode( folderNodeID );
			deviceNodeID = InsertExampleDeviceNode( folderNodeID );
			InsertExampleDatapointNodes( deviceNodeID );

			folderNodeID = InsertExampleFolderNode( rootNodeID );
			deviceNodeID = InsertExampleDeviceNode( folderNodeID );
			InsertExampleDatapointNodes( deviceNodeID );
		}


		static void InsertExampleDatapointValues ( Int32 NumberOfDays , Int32 MinutesBetweenValues ) {
			List<DatapointNode> datapointNodes = Database.GetAllDatapointNodes();
			Console.Write( String.Format( "> Insert values for {0} datapoins, {1} days in a {2} minutes period..." , datapointNodes.Count , NumberOfDays , MinutesBetweenValues ) );
			try {
				Random random = new Random();
				const long NSPerSecond = 10000000;
				Int64 TimeTicks = 60 * 60 * 24 * NSPerSecond;
				TimeTicks *= NumberOfDays;
				TimeTicks += random.Next( 1 , 59 ) * NSPerSecond;
				DateTime EndTime = DateTime.Now;
				DateTime StartTime = new DateTime( EndTime.Ticks - TimeTicks );

				DatapointValueNode valueNode = new DatapointValueNode();
				foreach( DatapointNode datapointNode in datapointNodes ) {
					DateTime TempTime = StartTime;
					valueNode.SetDatapointID( datapointNode.GetID() );
					valueNode.SetType( datapointNode.GetDatapointType() );
					while( TempTime.Ticks < EndTime.Ticks ) {
						valueNode.SetTimeStamp( TempTime );
						if( datapointNode.GetDatapointType() == DatapointNode.TYPE_FLOATING_POINT ) {
							valueNode.SetDecimalValue( random.NextDouble() );
							valueNode.SetStringValue( valueNode.GetDecimalValue().ToString().Replace( "," , "." ) );
						} else if( datapointNode.GetDatapointType() == DatapointNode.TYPE_INTEGER ) {
							valueNode.SetIntegerValue( random.Next( 30 , 130 ) );
							valueNode.SetStringValue( valueNode.GetIntegerValue().ToString() );
						}
						Database.InsertNode( valueNode );
						TempTime = TempTime.AddMinutes( MinutesBetweenValues );
					}
					datapointNode.SetLastValue( valueNode.GetStringValue() );
					datapointNode.SetLastValueUpdate( valueNode.GetTimeStamp() );
					Database.UpdateNode( datapointNode );
				}
				ConsoleWriteSuccess( "OK" );
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
			}
		}

		static Int64 InsertExampleRootNode () {
			try {
				Console.Write( "> Insert root node..." );
				RootNode rootNode = new RootNode();
				rootNode.SetName( "GSMGH" );
				rootNode.SetDescription( "Gewerbliche Schule Bad Mergentheim" );
				rootNode.SetLastUpdated( DateTime.Now );
				Boolean inserted = Database.InsertNode( rootNode );
				if( inserted ) ConsoleWriteSuccess( rootNode.ToString() );
				else ConsoleWriteError( "Could not insert root node" );
				return rootNode.GetID();
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
				RootNode rootNode = Database.GetRootNode();
				if( rootNode == null ) throw new Exception( "There is no root node in the database" );
				return rootNode.GetID();
			}
		}
		static Int64 InsertExampleFolderNode ( Int64 ParentID ) {
			try {
				Console.Write( "> Insert folder node..." );
				Random random = new Random();
				String Name = random.Next( 1 , 20 ).ToString() + ". Ordner";
				FolderNode folderNode = new FolderNode();
				folderNode.SetName( Name );
				folderNode.SetParentID( ParentID );
				folderNode.SetDescription( "Sensoren im " + Name );
				folderNode.SetLastUpdated( DateTime.Now );
				Boolean inserted = Database.InsertNode( folderNode );
				if( inserted ) ConsoleWriteSuccess( folderNode.ToString() );
				else ConsoleWriteError( "Could not insert folder node" );
				return folderNode.GetID();
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
				return 0;
			}
		}
		static Int64 InsertExampleDeviceNode ( Int64 ParentID ) {
			try {
				Console.Write( "> Insert device node..." );
				Random random = new Random();
				String Name = "Sensor " + random.Next( 1 , 500 ).ToString();
				DeviceNode deviceNode = new DeviceNode();
				deviceNode.SetName( Name );
				deviceNode.SetParentID( ParentID );
				deviceNode.SetDescription( Name );
				deviceNode.SetLastUpdated( DateTime.Now );
				deviceNode.SetIPAddress( new IPAddress( new Byte[] { 192 , 168 , 2 , 116 } ) );
				deviceNode.SetPort( 10001 );
				deviceNode.SetCO2Threshold( 1000 );
				deviceNode.SetLoudnessThreshold( 80 );
				Boolean inserted = Database.InsertNode( deviceNode );
				if( inserted ) ConsoleWriteSuccess( deviceNode.ToString() );
				else ConsoleWriteError( "Could not device node" );
				return deviceNode.GetID();
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
				return 0;
			}
		}
		static void InsertExampleDatapointNodes ( Int64 ParentID ) {
			try {
				Console.Write( "> Insert datapoint node..." );
				Random random = new Random();
				DatapointNode datapointNode = new DatapointNode();
				datapointNode.SetName( "Sauerstoff" );
				datapointNode.SetParentID( ParentID );
				datapointNode.SetDescription( "CO2Concentration" );
				datapointNode.SetLastUpdated( DateTime.Now );
				datapointNode.SetDatapointType( DatapointNode.TYPE_INTEGER );
				datapointNode.SetUnit( "PPM" );
				datapointNode.SetLastValue( random.Next( 300 , 3000 ).ToString() );
				datapointNode.SetLastValueUpdate( DateTime.Now );
				Boolean inserted = Database.InsertNode( datapointNode );
				if( inserted ) ConsoleWriteSuccess( datapointNode.ToString() );
				else ConsoleWriteError( "Could not insert datapoint node" );
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
			}

			try {
				Console.Write( "> Insert datapoint node..." );
				Random random = new Random();
				DatapointNode datapointNode = new DatapointNode();
				datapointNode.SetName( "Lautstärke" );
				datapointNode.SetParentID( ParentID );
				datapointNode.SetDescription( "Loudness" );
				datapointNode.SetLastUpdated( DateTime.Now );
				datapointNode.SetDatapointType( DatapointNode.TYPE_INTEGER );
				datapointNode.SetUnit( "DB" );
				datapointNode.SetLastValue( random.Next( 20 , 130 ).ToString() );
				datapointNode.SetLastValueUpdate( DateTime.Now );
				Boolean inserted = Database.InsertNode( datapointNode );
				if( inserted ) ConsoleWriteSuccess( datapointNode.ToString() );
				else ConsoleWriteError( "Could not insert root node" );
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
			}

			try {
				Console.Write( "> Insert datapoint node..." );
				Random random = new Random();
				DatapointNode datapointNode = new DatapointNode();
				datapointNode.SetName( "Temperatur" );
				datapointNode.SetParentID( ParentID );
				datapointNode.SetDescription( "Temperature" );
				datapointNode.SetLastUpdated( DateTime.Now );
				datapointNode.SetDatapointType( DatapointNode.TYPE_FLOATING_POINT );
				datapointNode.SetUnit( "°C" );
				datapointNode.SetLastValue( random.NextDouble().ToString( "0.00" ) );
				datapointNode.SetLastValueUpdate( DateTime.Now );
				Boolean inserted = Database.InsertNode( datapointNode );
				if( inserted ) ConsoleWriteSuccess( datapointNode.ToString() );
				else ConsoleWriteError( "Could not insert root node" );
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
			}
		}

		static void DeleteAllData () {
			try {
				Console.Write( "> Delete all data from 'datapoint_values'..." );
				Boolean deleted = Database.DeleteAllDatapointValuesData();
				if( deleted ) ConsoleWriteSuccess( "OK" );
				else ConsoleWriteError( "Could not delete data" );

				Console.Write( "> Delete all data from 'object_tree'..." );
				deleted = Database.DeleteAllObjectTreeData();
				if( deleted ) ConsoleWriteSuccess( "OK" );
				else ConsoleWriteError( "Could not delete data" );
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
			}
		}

		static void GetAllData () {
			try {
				ParentIdDictionary = Database.GetObjectTreeSortedByParentID();
				RootNode rootNode = Database.GetRootNode();
				ConsoleWriteError( "> " + rootNode.ToString() );
				GetSubnodes( 0 , "" );
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
			}
		}

		static void GetSubnodes ( Int64 ParentID , String Indent) {
			try {
				if( ParentIdDictionary.ContainsKey( ParentID.ToString() ) ) {
					List<AbstractObjectNode> subNodes = ParentIdDictionary[ ParentID.ToString() ];
					foreach( AbstractObjectNode currentSubNode in subNodes ) {

						if( currentSubNode.GetType() == FolderNode.NODE_TYPE ) {
							FolderNode Node = Database.GetFolderNodeByID( currentSubNode.GetID() );
							Console.ForegroundColor = ConsoleColor.Magenta;
							Console.WriteLine( "> " + Indent + Node.ToString() );
							Console.ForegroundColor = ConsoleColor.White;
						}
						if( currentSubNode.GetType() == DeviceNode.NODE_TYPE ) {
							DeviceNode Node = Database.GetDeviceNodeByID( currentSubNode.GetID() );
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine( "> " + Indent + Node.ToString() );
							Console.ForegroundColor = ConsoleColor.White;
						}
						if( currentSubNode.GetType() == DatapointNode.NODE_TYPE ) {
							DatapointNode Node = Database.GetDatapointNodeByID( currentSubNode.GetID() );
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.WriteLine( "> " + Indent + Node.ToString() );
							Console.ForegroundColor = ConsoleColor.White;
						}
						GetSubnodes( currentSubNode.GetID() , Indent + "   " );
					}
				}
			} catch( Exception ex ) {
				ConsoleWriteError( ex.Message );
			}
		}

		static void ConsoleWriteError ( String Message ) {
			ConsoleColor colorBackup = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine( Message );
			Console.ForegroundColor = colorBackup;
		}
		static void ConsoleWriteSuccess ( String Message ) {
			ConsoleColor colorBackup = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine( Message );
			Console.ForegroundColor = colorBackup;
		}
	}
}
