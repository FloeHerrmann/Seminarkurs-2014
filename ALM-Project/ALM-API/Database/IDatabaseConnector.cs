using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsmgh.Alm.Model;

namespace Gsmgh.Alm.Database {
	public interface IDatabaseConnector {

		void OpenConnection ();
		void CloseConnection ();

		RootNode GetRootNode ();
		FolderNode GetFolderNodeByID ( Int64 NodeID );
		DeviceNode GetDeviceNodeByID ( Int64 NodeID );
		DatapointNode GetDatapointNodeByID ( Int64 NodeID );

		Boolean InsertNode ( RootNode Node );
		Boolean InsertNode ( FolderNode Node );
		Boolean InsertNode ( DeviceNode Node );
		Boolean InsertNode ( DatapointNode Node );
		Boolean InsertNode ( DatapointValueNode Node );

		Boolean UpdateNode ( RootNode Node );
		Boolean UpdateNode ( FolderNode Node );
		Boolean UpdateNode ( DeviceNode Node );
		Boolean UpdateNode ( DatapointNode Node );

		Boolean DeleteNodeByID ( Int64 NodeID );
		Boolean DeleteNode ( RootNode Node );
		Boolean DeleteNode ( FolderNode Node );
		Boolean DeleteNode ( DeviceNode Node );
		Boolean DeleteNode ( DatapointNode Node );

		List<AbstractObjectNode> GetAllAbstractNodes ();
		List<DatapointNode> GetAllDatapointNodes ();
		List<DeviceNode> GetAllDeviceNodes ();
		List<DatapointNode> GetDatapointNodesByDeviceID ( Int64 DeviceID );
		List<DatapointValueNode> GetDatapointValuesByDatapointID ( Int64 DatapointID , DateTime From , DateTime To );

		Boolean DeleteAllObjectTreeData ();
		Boolean DeleteAllDatapointValuesData ();
		

	}
}
