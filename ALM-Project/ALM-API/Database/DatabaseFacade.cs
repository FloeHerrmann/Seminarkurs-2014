﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsmgh.Alm.Model;

namespace Gsmgh.Alm.Database {
	public class DatabaseFacade {

		private IDatabaseConnector DatabaseConnector;

		public void OpenConnection () {
			this.DatabaseConnector.OpenConnection();
		}

		public void CloseConnection () {
			this.DatabaseConnector.CloseConnection();
		}

		public void SetDatabaseConnector( IDatabaseConnector DatabaseConnector ) {
			this.DatabaseConnector = DatabaseConnector;
		}

		public RootNode GetRootNode () {
			return this.DatabaseConnector.GetRootNode();
		}

		public FolderNode GetFolderNodeByID ( Int64 NodeID ) {
			return this.DatabaseConnector.GetFolderNodeByID( NodeID );
		}

		public DeviceNode GetDeviceNodeByID ( Int64 NodeID ) {
			return this.DatabaseConnector.GetDeviceNodeByID( NodeID );
		}

		public DatapointNode GetDatapointNodeByID ( Int64 NodeID ) {
			return this.DatabaseConnector.GetDatapointNodeByID( NodeID );
		}

		public Boolean InsertNode ( RootNode Node ) {
			return this.DatabaseConnector.InsertNode( Node );
		}

		public Boolean InsertNode ( FolderNode Node ) {
			return this.DatabaseConnector.InsertNode( Node );
		}

		public Boolean InsertNode ( DeviceNode Node ) {
			return this.DatabaseConnector.InsertNode( Node );
		}

		public Boolean InsertNode ( DatapointNode Node ) {
			return this.DatabaseConnector.InsertNode( Node );
		}

		public Boolean InsertNode ( DatapointValueNode Node ) {
			return this.DatabaseConnector.InsertNode( Node );
		}

		public Boolean UpdateNode ( RootNode Node ) {
			return this.DatabaseConnector.UpdateNode( Node );
		}
		public Boolean UpdateNode ( FolderNode Node ) {
			return this.DatabaseConnector.UpdateNode( Node );
		}
		public Boolean UpdateNode ( DeviceNode Node ) {
			return this.DatabaseConnector.UpdateNode( Node );
		}
		public Boolean UpdateNode ( DatapointNode Node ) {
			return this.DatabaseConnector.UpdateNode( Node );
		}

		public Boolean DeleteNodeByID ( Int64 NodeID ) {
			return this.DatabaseConnector.DeleteNodeByID( NodeID );
		}
		public Boolean DeleteNode ( RootNode Node ) {
			return this.DatabaseConnector.DeleteNodeByID( Node.GetID() );
		}
		public Boolean DeleteNode ( FolderNode Node ) {
			return this.DatabaseConnector.DeleteNodeByID( Node.GetID() );
		}
		public Boolean DeleteNode ( DeviceNode Node ) {
			return this.DatabaseConnector.DeleteNodeByID( Node.GetID() );
		}
		public Boolean DeleteNode ( DatapointNode Node ) {
			return this.DatabaseConnector.DeleteNodeByID( Node.GetID() );
		}

		/// <summary>
		///		Creates a dictionary that contains all subnodes for a specific parent id
		/// </summary>
		/// <returns>
		///		A dictionary that contains all subnodes for a specific parent ids
		/// </returns>
		public Dictionary<String , List<AbstractObjectNode>> GetObjectTreeSortedByParentID () {
			Dictionary<String , List<AbstractObjectNode>> parentIdDictionary = new Dictionary<string , List<AbstractObjectNode>>();
			List<AbstractObjectNode> abstractNodeList = this.DatabaseConnector.GetAllAbstractNodes();

			foreach( AbstractObjectNode currentNode in abstractNodeList ) {
				if( !parentIdDictionary.ContainsKey( currentNode.GetParentID().ToString() ) ) {
					parentIdDictionary.Add( currentNode.GetParentID().ToString() , new List<AbstractObjectNode>() );
				}
				parentIdDictionary[ currentNode.GetParentID().ToString() ].Add( currentNode );
			}

			return parentIdDictionary;
		}

		public List<DatapointNode> GetAllDatapointNodes () {
			return this.DatabaseConnector.GetAllDatapointNodes();
		}

		public List<DeviceNode> GetAllDeviceNodes () {
			return this.DatabaseConnector.GetAllDeviceNodes();
		}

		public List<DatapointNode> GetDatapointNodesByDeviceID ( Int64 DeviceID ) {
			return this.DatabaseConnector.GetDatapointNodesByDeviceID( DeviceID );
		}

		public List<DatapointValueNode> GetDatapointValuesByDatapointID ( Int64 DatapointID , DateTime From , DateTime To ) {
			return this.DatabaseConnector.GetDatapointValuesByDatapointID( DatapointID , From , To );
		}

		public Boolean DeleteAllObjectTreeData () {
			return this.DatabaseConnector.DeleteAllObjectTreeData();
		}
		public Boolean DeleteAllDatapointValuesData () {
			return this.DatabaseConnector.DeleteAllDatapointValuesData();
		}
	}
}
