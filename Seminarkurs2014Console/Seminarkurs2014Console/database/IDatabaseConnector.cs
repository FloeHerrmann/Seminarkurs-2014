using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seminarkurs2014Console.model;

namespace Seminarkurs2014Console.database {
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

		Boolean UpdateNode ( RootNode Node );
		Boolean UpdateNode ( FolderNode Node );
		Boolean UpdateNode ( DeviceNode Node );
		Boolean UpdateNode ( DatapointNode Node );

		Boolean DeleteNodeByID ( Int64 NodeID );
		Boolean DeleteNode ( RootNode Node );
		Boolean DeleteNode ( FolderNode Node );
		Boolean DeleteNode ( DeviceNode Node );
		Boolean DeleteNode ( DatapointNode Node );

	}
}
