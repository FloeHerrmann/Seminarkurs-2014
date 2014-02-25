using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Seminarkurs2014Console.model;

namespace Seminarkurs2014Console.database {
	public class MySQLConnector : IDatabaseConnector {

		/// <summary>
		/// Log4net logger instance for logging purposes
		/// </summary>
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private MySqlConnection Connection;

		public MySQLConnector ( String ConnectionString ) {
			this.Connection = new MySqlConnection( ConnectionString );
		}

		public void OpenConnection () {
			try {
				this.Connection.Open();
			} catch( Exception ex ) {
				logger.Error( ex.Message , ex );
			}
		}

		public void CloseConnection () {
			try {
				this.Connection.Close();
			} catch( Exception ex ) {
				logger.Error( ex.Message , ex );
			}
		}

		private ObjectTreeRow GetNodeByID( Int64 NodeID ) {
			try {
				MySqlCommand Command = this.Connection.CreateCommand();
				MySqlDataReader Reader;

				Command.CommandText = String.Format( "SELECT * FROM object_tree WHERE object_id = {0} LIMIT 1" , NodeID );
				Command.Connection = this.Connection;

				ObjectTreeRow NodeRow = null;
				Reader = Command.ExecuteReader();
				while( Reader.Read() ) {
					string row = "";
					for( int i = 0 ; i < ( Reader.FieldCount - 1 ) ; i++ )
						row += Reader.GetValue( i ).ToString() + ",";
					NodeRow = new ObjectTreeRow( row );
				}

				if( NodeRow == null ) return null;
				return NodeRow;
			} catch( Exception ex ) {
				logger.Error( ex.Message , ex );
				return null;
			}
		}

		public RootNode GetRootNode () {
			try {
				MySqlCommand command = this.Connection.CreateCommand();
				MySqlDataReader Reader;

				command.CommandText = "SELECT * FROM object_tree WHERE object_type = 1 LIMIT 1";

				ObjectTreeRow RootNodeRow = null;
				Reader = command.ExecuteReader();
				while( Reader.Read() ) {
					string row = "";
					for( int i = 0 ; i < ( Reader.FieldCount - 1 ) ; i++ )
						row += Reader.GetValue( i ).ToString() + ",";
					RootNodeRow = new ObjectTreeRow( row );
				}

				if( RootNodeRow == null ) return null;
				return new RootNode( RootNodeRow );
			} catch( Exception ex ) { 
				logger.Error( ex.Message , ex );
				return null;
			}
		}

		public FolderNode GetFolderNodeByID ( Int64 NodeID ) {
			try {
				ObjectTreeRow NodeRow = this.GetNodeByID( NodeID );
				if( NodeRow == null ) return null;
				return new FolderNode( NodeRow );
			} catch( Exception ex ) {
				logger.Error( ex.Message , ex );
				return null;
			}
		}

		public DeviceNode GetDeviceNodeByID ( Int64 NodeID ) {
			try {
				ObjectTreeRow NodeRow = this.GetNodeByID( NodeID );
				if( NodeRow == null ) return null;
				return new DeviceNode( NodeRow );
			} catch( Exception ex ) {
				logger.Error( ex.Message , ex );
				return null;
			}
		}

		public DatapointNode GetDatapointNodeByID ( Int64 NodeID ) {
			try {
				ObjectTreeRow NodeRow = this.GetNodeByID( NodeID );
				if( NodeRow == null ) return null;
				return new DatapointNode( NodeRow );
			} catch( Exception ex ) {
				logger.Error( ex.Message , ex );
				return null;
			}
		}

		public Boolean InsertNode ( RootNode Node ) {
			try {
				logger.Info( String.Format( "Insert: '{0}'" , Node.ToString() ) );
				String InsertQuery = String.Format(
					"INSERT INTO object_tree (" +
					"object_type , object_name , object_description , object_last_updated" +
					") Values( {0} , '{1}' , '{2}' , '{3}' )" ,
					RootNode.NODE_TYPE ,
					Node.GetName() ,
					Node.GetDescription() ,
					Node.GetLastUpdated().ToString( "yyyy-MM-dd HH:mm:ss" )
				);
				logger.Info( String.Format( "Query: '{0}'" , InsertQuery ) );
				MySqlCommand Command = new MySqlCommand( InsertQuery );
				Command.Connection = this.Connection;
				Int32 AffectedRows = Command.ExecuteNonQuery();
				Node.SetID( Command.LastInsertedId );
				if( AffectedRows == 0 ) return false;
				return true;
			} catch( Exception ex ) {
				logger.Error( ex.Message , ex );
				return false;
			}
		}

		public Boolean InsertNode ( FolderNode Node ) {
			try {
				logger.Info( String.Format( "Insert: '{0}'" , Node.ToString() ) );
				String InsertQuery = String.Format(
					"INSERT INTO object_tree (" +
					"object_parent_id , object_type , object_path , object_name , object_description , object_last_updated"+
					") Values( {0} , {1} , '{2}' , '{3}' , '{4}' , '{5}' )",
					Node.GetParentID(),
					FolderNode.NODE_TYPE,
					Node.GetPath(),
					Node.GetName(),
					Node.GetDescription(),
					Node.GetLastUpdated().ToString("yyyy-MM-dd HH:mm:ss")
				);
				logger.Info( String.Format( "Query: '{0}'" , InsertQuery ) );
				MySqlCommand Command = new MySqlCommand( InsertQuery );
				Command.Connection = this.Connection;
				Int32 AffectedRows = Command.ExecuteNonQuery();
				Node.SetID( Command.LastInsertedId );
				if( AffectedRows == 0 ) return false;
				return true;
			} catch( Exception ex ) {
				logger.Error( ex.Message , ex );
				return false;
			}
		}

		public Boolean InsertNode ( DeviceNode Node ) {
			try {
				logger.Info( String.Format( "Insert: '{0}'" , Node.ToString() ) );
				String InsertQuery = String.Format(
					"INSERT INTO object_tree (" +
					"object_parent_id , object_type , object_path , object_name , object_description , object_last_updated , sensor_ip_address , sensor_port" +
					") Values( {0} , {1} , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' , {7} )" ,
					Node.GetParentID() ,
					DeviceNode.NODE_TYPE ,
					Node.GetPath() ,
					Node.GetName() ,
					Node.GetDescription() ,
					Node.GetLastUpdated().ToString( "yyyy-MM-dd HH:mm:ss" ),
					Node.GetIPAddress().ToString(),
					Node.GetPort()
				);
				logger.Info( String.Format( "Query: '{0}'" , InsertQuery ) );
				MySqlCommand Command = new MySqlCommand( InsertQuery );
				Command.Connection = this.Connection;
				Int32 AffectedRows = Command.ExecuteNonQuery();
				Node.SetID( Command.LastInsertedId );
				if( AffectedRows == 0 ) return false;
				return true;
			} catch( Exception ex ) {
				logger.Error( ex.Message , ex );
				return false;
			}
		}

		public Boolean InsertNode ( DatapointNode Node ) {
			try {
				logger.Info( String.Format( "Insert: '{0}'" , Node.ToString() ) );
				String InsertQuery = String.Format(
					"INSERT INTO object_tree (" +
					"object_parent_id , object_type , object_path , object_name , object_description , object_last_updated , datapoint_type , datapoint_unit , datapoint_calculation" +
					") Values( {0} , {1} , '{2}' , '{3}' , '{4}' , '{5}' , {6} , '{7}' , '{8}' )" ,
					Node.GetParentID() ,
					DatapointNode.NODE_TYPE ,
					Node.GetPath() ,
					Node.GetName() ,
					Node.GetDescription() ,
					Node.GetLastUpdated().ToString( "yyyy-MM-dd HH:mm:ss" ) ,
					Node.GetDatapointType().ToString() ,
					Node.GetUnit(),
					Node.GetCalculation()
				);
				logger.Info( String.Format( "Query: '{0}'" , InsertQuery ) );
				MySqlCommand Command = new MySqlCommand( InsertQuery );
				Command.Connection = this.Connection;
				Int32 AffectedRows = Command.ExecuteNonQuery();
				Node.SetID( Command.LastInsertedId );
				if( AffectedRows == 0 ) return false;
				return true;
			} catch( Exception ex ) {
				logger.Error( ex.Message , ex );
				return false;
			}
		}
	}
}
