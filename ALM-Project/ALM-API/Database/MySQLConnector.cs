using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Gsmgh.Alm.Model;

namespace Gsmgh.Alm.Database {
	public class MySQLConnector : IDatabaseConnector {

		/// <summary>
		///		Handles the database connection
		/// </summary>
		private MySqlConnection Connection;

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="ConnectionString">
		///		Connection string (See MySQL Documentation)
		///		Example: 'SERVER=localhost;DATABASE=tempDatabase;UID=root;PASSWORD=root;'
		/// </param>
		public MySQLConnector ( String ConnectionString ) {
			this.Connection = new MySqlConnection( ConnectionString );
		}

		/// <summary>
		///		Open a connection to the database
		/// </summary>
		public void OpenConnection () {
			// Open the database connection
			this.Connection.Open();
		}
		
		/// <summary>
		///		Close the connection to the datbase
		/// </summary>
		public void CloseConnection () {
			// Close the datbase connection
			this.Connection.Close();
		}

		/// <summary>
		///		Get the root node from the database
		/// </summary>
		/// <returns>
		///		The RootNode Instance or
		///		NULL if no root node is present in the datbase
		/// </returns>
		public RootNode GetRootNode () {

			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Reader to read the database answer
			MySqlDataReader Reader;

			// There can obly be one!!! root node so we can get the root node by the object type
			// SELECT *					> Get all columns of the table
			// FROM object_tree			> Target table object_tree
			// WHERE object_type = {0}	> Get all rows where the column object_type matches the given id
			// LIMIT 1					> Because the id is unique we can limit it to 1 row
			Command.CommandText = "SELECT * FROM object_tree WHERE object_type = 1 LIMIT 1";
			Command.Connection = this.Connection;

			// Represents a row (tuple) of the object_tree table
			ObjectTreeRow RootNodeRow = null;

			// Execute the command and read the answer
			Reader = Command.ExecuteReader();

			// Read the answer
			while( Reader.Read() ) {
				string row = "";
				for( int i = 0 ; i < ( Reader.FieldCount - 1 ) ; i++ )
					row += Reader.GetValue( i ).ToString() + ",";
				RootNodeRow = new ObjectTreeRow( row );
			}
			Reader.Close();

			// When there is no answer from the database (e.g. there is no node root node) return null
			if( RootNodeRow == null ) return null;

			// Return the found root node as an RootNode Instance
			return new RootNode( RootNodeRow );
		}

		/// <summary>
		///		A row of the object_tree table
		/// </summary>
		/// <param name="NodeID">
		///		ID of the row in the database
		/// </param>
		/// <returns>
		///		A ObjectTreeRow instance that represents the database row or
		///		NULL if the row is not present in the datbase
		/// </returns>
		private ObjectTreeRow GetNodeByID ( Int64 NodeID ) {

			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Reader to read the database answer
			MySqlDataReader Reader;

			// Get the row (tuple) from the datbase with a specific id
			// SELECT *					> Get all columns of the table
			// FROM object_tree			> Target table object_tree
			// WHERE object_id = {0}	> Get all rows where the column object_id matches the given id
			// LIMIT 1					> Because the id is unique we can limit it to 1 row
			Command.CommandText = String.Format( "SELECT * FROM object_tree WHERE object_id = {0} LIMIT 1" , NodeID );
			Command.Connection = this.Connection;

			// Represents a row (tuple) of the object_tree table
			ObjectTreeRow NodeRow = null;

			// Execute the command and read the answer
			Reader = Command.ExecuteReader();

			// Read the answer
			while( Reader.Read() ) {
				string row = "";
				for( int i = 0 ; i < ( Reader.FieldCount ) ; i++ )
					row += Reader.GetValue( i ).ToString() + ",";
				NodeRow = new ObjectTreeRow( row.Substring( 0 , row.Length - 1) );
			}
			Reader.Close();

			// When there is no answer from the database (e.g. there is no row with the specific id) return null
			if( NodeRow == null ) return null;

			// Return the object that represents a row (tuple) of the object_tree table
			// Important: It is no Node Instance, that mean you have to create the corresponding Node Instance in a further step
			return NodeRow;
		}

		/// <summary>
		///		Get a Folder from the database by its ID
		/// </summary>
		/// <returns>
		///		The FolderNode instance or
		///		NULL if no folder node is present in the datbase
		/// </returns>
		public FolderNode GetFolderNodeByID ( Int64 NodeID ) {
			// Read the row with from the databse with the given ID
			ObjectTreeRow NodeRow = this.GetNodeByID( NodeID );

			// If there is no such row in the database return null
			if( NodeRow == null ) return null;

			// If there is such a row in the datbase create and return a FolderNode Instance
			return new FolderNode( NodeRow );
		}

		/// <summary>
		///		Get a Device from the database by its ID
		/// </summary>
		/// <returns>
		///		The DeviceNode Instance or
		///		NULL if no device node is present in the datbase
		/// </returns>
		public DeviceNode GetDeviceNodeByID ( Int64 NodeID ) {
			// Read the row with from the databse with the given ID
			ObjectTreeRow NodeRow = this.GetNodeByID( NodeID );

			// If there is no such row in the database return null
			if( NodeRow == null ) return null;

			// If there is such a row in the datbase create and return a DeviceNode Instance
			return new DeviceNode( NodeRow );
		}

		/// <summary>
		///		Get a Datapoint from the database by its ID
		/// </summary>
		/// <returns>
		///		The DatapointNode instance or
		///		NULL if no datapoint node is present in the datbase
		/// </returns>
		public DatapointNode GetDatapointNodeByID ( Int64 NodeID ) {
			// Read the row with from the databse with the given ID
			ObjectTreeRow NodeRow = this.GetNodeByID( NodeID );

			// If there is no such row in the database return null
			if( NodeRow == null ) return null;

			// If there is such a row in the datbase create and return a DatapointNode Instance
			return new DatapointNode( NodeRow );
		}

		/// <summary>
		///		Insert a RootNode into the database
		/// </summary>
		/// <param name="Node">
		///		RootNode that should be insert
		/// </param>
		/// <returns>
		///		true, if a row was inserted
		///		false, if no row was inserted
		/// </returns>
		public Boolean InsertNode ( RootNode Node ) {
			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Update a row in the databse with the given node id
			// UPDATE						> Update rows of a table
			// SET	[colum name] = [value],	> Set a column to the given value
			//		[colum name] = [value]	> Set a column to the given value
			// WHERE object_id = {0}		> Update all rows where the column object_id matches the given id
			Command.CommandText = String.Format(
				"INSERT INTO object_tree ( object_type , object_name , object_description , object_last_updated ) Values( {0} , '{1}' , '{2}' , '{3}' )" ,
				RootNode.NODE_TYPE , Node.GetName() , Node.GetDescription() , Node.GetLastUpdated().ToString( "yyyy-MM-dd HH:mm:ss" )
			);
			Command.Connection = this.Connection;

			// Execute the command and get the number of rows that are affected by the command
			Int32 AffectedRows = Command.ExecuteNonQuery();

			// Set the ID of the node to the ID that was provided by the database
			Node.SetID( Command.LastInsertedId );
				
			// If no rows where affected return false
			if( AffectedRows == 0 ) return false;

			// Row successfully insert
			return true;
		}

		/// <summary>
		///		Insert a FolderNode into the database
		/// </summary>
		/// <param name="Node">
		///		FolderNode that should be insert
		/// </param>
		/// <returns>
		///		true, if a row was inserted
		///		false, if no row was inserted
		/// </returns>
		public Boolean InsertNode ( FolderNode Node ) {
			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Update a row in the databse with the given node id
			// UPDATE						> Update rows of a table
			// SET	[colum name] = [value],	> Set a column to the given value
			//		[colum name] = [value]	> Set a column to the given value
			// WHERE object_id = {0}		> Update all rows where the column object_id matches the given id
			Command.CommandText = String.Format(
					"INSERT INTO object_tree ( object_parent_id , object_type , object_name , object_description , object_last_updated ) Values( {0} , {1} , '{2}' , '{3}' , '{4}' )" ,
					Node.GetParentID() , FolderNode.NODE_TYPE , Node.GetName() , Node.GetDescription() , Node.GetLastUpdated().ToString( "yyyy-MM-dd HH:mm:ss" )
			);
			Command.Connection = this.Connection;

			// Execute the command and get the number of rows that are affected by the command
			Int32 AffectedRows = Command.ExecuteNonQuery();

			// Set the ID of the node to the ID that was provided by the database
			Node.SetID( Command.LastInsertedId );
				
			// If no rows where affected return false
			if( AffectedRows == 0 ) return false;

			// Row successfully insert
			return true;
		}

		/// <summary>
		///		Insert a DeviceNode into the database
		/// </summary>
		/// <param name="Node">
		///		DeviceNode that should be insert
		/// </param>
		/// <returns>
		///		true, if a row was inserted
		///		false, if no row was inserted
		/// </returns>
		public Boolean InsertNode ( DeviceNode Node ) {
			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Update a row in the databse with the given node id
			// UPDATE						> Update rows of a table
			// SET	[colum name] = [value],	> Set a column to the given value
			//		[colum name] = [value]	> Set a column to the given value
			// WHERE object_id = {0}		> Update all rows where the column object_id matches the given id
			Command.CommandText = String.Format(
				"INSERT INTO object_tree ( object_parent_id , object_type , object_name , object_description , object_last_updated , sensor_ip_address , sensor_port ) Values( {0} , {1} , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' )" ,
				Node.GetParentID() , DeviceNode.NODE_TYPE , Node.GetName() , Node.GetDescription() , Node.GetLastUpdated().ToString( "yyyy-MM-dd HH:mm:ss" ) , Node.GetIPAddress().ToString() , Node.GetPort()
			);
			Command.Connection = this.Connection;

			// Execute the command and get the number of rows that are affected by the command
			Int32 AffectedRows = Command.ExecuteNonQuery();

			// Set the ID of the node to the ID that was provided by the database
			Node.SetID( Command.LastInsertedId );
				
			// If no rows where affected return false
			if( AffectedRows == 0 ) return false;

			// Row successfully insert
			return true;
		}

		/// <summary>
		///		Insert a DatapointNode into the database
		/// </summary>
		/// <param name="Node">
		///		DatapointNode that should be insert
		/// </param>
		/// <returns>
		///		true, if a row was inserted
		///		false, if no row was inserted
		/// </returns>
		public Boolean InsertNode ( DatapointNode Node ) {
			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Update a row in the databse with the given node id
			// UPDATE						> Update rows of a table
			// SET	[colum name] = [value],	> Set a column to the given value
			//		[colum name] = [value]	> Set a column to the given value
			// WHERE object_id = {0}		> Update all rows where the column object_id matches the given id
			Command.CommandText = String.Format(
				"INSERT INTO object_tree ( object_parent_id , object_type ,  object_name , object_description , object_last_updated , datapoint_type , datapoint_unit , datapoint_last_value , datapoint_last_updated ) Values( {0} , {1} , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' , '{7}' , '{8}' )" ,
				Node.GetParentID() , DatapointNode.NODE_TYPE , Node.GetName() , Node.GetDescription() , Node.GetLastUpdated().ToString( "yyyy-MM-dd HH:mm:ss" ) , Node.GetDatapointType().ToString() , Node.GetUnit() , Node.GetLastValue() , Node.GetLastValueUpdate().ToString( "yyyy-MM-dd HH:mm:ss" )
			);
			Command.Connection = this.Connection;

			// Execute the command and get the number of rows that are affected by the command
			Int32 AffectedRows = Command.ExecuteNonQuery();

			// Set the ID of the node to the ID that was provided by the database
			Node.SetID( Command.LastInsertedId );

			// If no rows where affected return false
			if( AffectedRows == 0 ) return false;

			// Row successfully insert
			return true;
		}

		/// <summary>
		///		Update a RootNode in the database
		/// </summary>
		/// <param name="Node">
		///		RootNode that should be updated
		/// </param>
		/// <returns>
		///		true, if the node is updated
		///		false, if no node is updated
		/// </returns>
		public Boolean UpdateNode ( RootNode Node ) {
			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Update a row in the databse with the given node id
			// UPDATE						> Update rows of a table
			// SET	[colum name] = [value],	> Set a column to the given value
			//		[colum name] = [value]	> Set a column to the given value
			// WHERE object_id = {0}		> Update all rows where the column object_id matches the given id
			Command.CommandText = String.Format(
				"UPDATE object_tree SET object_parent_id={0}, object_name='{1}', object_description='{2}', object_last_updated='{3}' WHERE object_id={4}" ,
				Node.GetParentID() , Node.GetName() , Node.GetDescription() , DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ) , Node.GetID()
			);
			Command.Connection = this.Connection;

			// Execute the command and get the number of rows that are affected by the command
			Int32 AffectedRows = Command.ExecuteNonQuery();

			// If no rows where affected return false
			if( AffectedRows == 0 ) return false;

			// Row successfully updated
			return true;
		}

		/// <summary>
		///		Update a FolderNode in the database
		/// </summary>
		/// <param name="Node">
		///		FolderNode that should be updated
		/// </param>
		/// <returns>
		///		true, if the node is updated
		///		false, if no node is updated
		/// </returns>
		public Boolean UpdateNode ( FolderNode Node ){
			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Update a row in the databse with the given node id
			// UPDATE						> Update rows of a table
			// SET	[colum name] = [value],	> Set a column to the given value
			//		[colum name] = [value]	> Set a column to the given value
			// WHERE object_id = {0}		> Update all rows where the column object_id matches the given id
			Command.CommandText = String.Format(
				"UPDATE object_tree SET object_parent_id={0}, object_name='{1}', object_description='{2}', object_last_updated='{3}' WHERE object_id={4}" ,
				Node.GetParentID() , Node.GetName() , Node.GetDescription() , DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ) , Node.GetID()
			);
			Command.Connection = this.Connection;

			// Execute the command and get the number of rows that are affected by the command
			Int32 AffectedRows = Command.ExecuteNonQuery();

			// If no rows where affected return false
			if( AffectedRows == 0 ) return false;

			// Row successfully updated
			return true;
		}

		/// <summary>
		///		Update a DeviceNode in the database
		/// </summary>
		/// <param name="Node">
		///		DeviceNode that should be updated
		/// </param>
		/// <returns>
		///		true, if the node is updated
		///		false, if no node is updated
		/// </returns>
		public Boolean UpdateNode ( DeviceNode Node ) {
			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Update a row in the databse with the given node id
			// UPDATE						> Update rows of a table
			// SET	[colum name] = [value],	> Set a column to the given value
			//		[colum name] = [value]	> Set a column to the given value
			// WHERE object_id = {0}		> Update all rows where the column object_id matches the given id
			Command.CommandText = String.Format(
				"UPDATE object_tree SET object_parent_id={0}, object_name='{1}', object_description='{2}', object_last_updated='{3}', sensor_ip_address='{4}', sensor_port={5} WHERE object_id={6}" ,
				Node.GetParentID() , Node.GetName() , Node.GetDescription() , DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ) , Node.GetIPAddress().ToString() , Node.GetPort() , Node.GetID()
			);
			Command.Connection = this.Connection;

			// Execute the command and get the number of rows that are affected by the command
			Int32 AffectedRows = Command.ExecuteNonQuery();

			// If no rows where affected return false
			if( AffectedRows == 0 ) return false;

			// Row successfully updated
			return true;
		}

		/// <summary>
		///		Update a DatapointNode in the database
		/// </summary>
		/// <param name="Node">
		///		DatapointNode that should be updated
		/// </param>
		/// <returns>
		///		true, if the node is updated
		///		false, if no node is updated
		/// </returns>
		public Boolean UpdateNode ( DatapointNode Node ) {
			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Update a row in the databse with the given node id
			// UPDATE						> Update rows of a table
			// SET	[colum name] = [value],	> Set a column to the given value
			//		[colum name] = [value]	> Set a column to the given value
			// WHERE object_id = {0}		> Update all rows where the column object_id matches the given id
			Command.CommandText = String.Format(
				"UPDATE object_tree SET object_parent_id={0}, object_name='{1}', object_description='{2}', object_last_updated='{3}', datapoint_type={4}, datapoint_unit='{5}', datapoint_last_value='{6}', datapoint_last_updated='{7}' WHERE object_id={8}" ,
				Node.GetParentID() , Node.GetName() , Node.GetDescription() , DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ) , Node.GetDatapointType() , Node.GetUnit() , Node.GetLastValue() , Node.GetLastValueUpdate().ToString( "yyyy-MM-dd HH:mm:ss" ) , Node.GetID() 
			);
			Command.Connection = this.Connection;

			// Execute the command and get the number of rows that are affected by the command
			Int32 AffectedRows = Command.ExecuteNonQuery();

			// If no rows where affected return false
			if( AffectedRows == 0 ) return false;

			// Row successfully updated
			return true;
		}

		/// <summary>
		///		Remove a node from the database
		/// </summary>
		/// <param name="NodeID">
		///		ID of the node (row) that should be deleted
		/// </param>
		/// <returns>
		///		false, if the node could not be deleted
		///		true, if the node is deleted
		/// </returns>
		public Boolean DeleteNodeByID ( Int64 NodeID ) {
			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Delete a row in the databse with the given node id
			// DELETE					> Delete rows from a table
			// FROM object_tree			> Target table object_tree
			// WHERE object_id = {0}	> Delete all rows where the column object_id matches the given id
			Command.CommandText = String.Format( "DELETE FROM object_tree WHERE object_id = {0}" , NodeID );
			Command.Connection = this.Connection;

			// Execute the command and get the number of rows that are affected by the command
			Int32 AffectedRows = Command.ExecuteNonQuery();

			// If no rows where affected return false
			if( AffectedRows == 0 ) return false;

			// Row successfully deleted
			return true;
		}

		/// <summary>
		///		Remove a RootNode from the database
		/// </summary>
		/// <param name="Node">
		///		The RootNode that should be removed from database
		///	</param>
		/// <returns>
		///		false, if the node could not be deleted
		///		true, if the node is deleted
		/// </returns>
		public Boolean DeleteNode ( RootNode Node ) {
			return this.DeleteNodeByID( Node.GetID() );
		}

		/// <summary>
		///		Remove a FolderNode from the database
		/// </summary>
		/// <param name="Node">
		///		The FolderNode that should be removed from database
		///	</param>
		/// <returns>
		///		false, if the node could not be deleted
		///		true, if the node is deleted
		/// </returns>
		public Boolean DeleteNode ( FolderNode Node ) {
			return this.DeleteNodeByID( Node.GetID() );
		}

		/// <summary>
		///		Remove a DeviceNode from the database
		/// </summary>
		/// <param name="Node">
		///		The DeviceNode that should be removed from database
		///	</param>
		/// <returns>
		///		false, if the node could not be deleted
		///		true, if the node is deleted
		/// </returns>
		public Boolean DeleteNode ( DeviceNode Node ) {
			return this.DeleteNodeByID( Node.GetID() );
		}

		/// <summary>
		///		Remove a DatapointNode from the database
		/// </summary>
		/// <param name="Node">
		///		The DatapointNode that should be removed from database
		///	</param>
		/// <returns>
		///		false, if the node could not be deleted
		///		true, if the node is deleted
		/// </returns>
		public Boolean DeleteNode ( DatapointNode Node ) {
			return this.DeleteNodeByID( Node.GetID() );
		}

		public List<AbstractObjectNode> GetAllAbstractNodes () {
			// A list of all nodes in the database
			List<AbstractObjectNode> nodeList = new List<AbstractObjectNode>();

			// Command that will be send to the databse
			MySqlCommand Command = this.Connection.CreateCommand();

			// Reader to read the database answer
			MySqlDataReader Reader;

			// Get the row (tuple) from the datbase with a specific id
			// SELECT *					> Get all columns of the table
			// FROM object_tree			> Target table object_tree
			// WHERE object_id = {0}	> Get all rows where the column object_id matches the given id
			// LIMIT 1					> Because the id is unique we can limit it to 1 row
			Command.CommandText = "SELECT * FROM object_tree ORDER BY object_name";
			Command.Connection = this.Connection;

			// Represents a row (tuple) of the object_tree table
			ObjectTreeRow NodeRow = null;

			// Execute the command and read the answer
			Reader = Command.ExecuteReader();

			// Read the answer
			while( Reader.Read() ) {
				string row = "";
				for( int i = 0 ; i < ( Reader.FieldCount - 1 ) ; i++ )
					row += Reader.GetValue( i ).ToString() + ",";
				// RootNode = AbstracObjectNode
				nodeList.Add( new RootNode( new ObjectTreeRow( row ) ) );
			}
			Reader.Close();

			// Return the list with all nodes
			return nodeList;
		}

	}
}
