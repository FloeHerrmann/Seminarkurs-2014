using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Seminarkurs2014Console.database;

namespace Seminarkurs2014Console.model {
	/// <summary>
	/// Model for the folder node
	/// </summary>
	public class FolderNode : AbstractObjectNode {

		/// <summary>
		/// Log4net logger instance for logging purposes
		/// </summary>
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		/// <summary>
		/// Number of the folder node type [2]
		/// </summary>
		public static Int32 NODE_TYPE = 2;

		/// <summary>
		/// Constructor
		/// </summary>
		public FolderNode () {
			logger.Trace( String.Format( "Creating a '{0}' instance" , MethodBase.GetCurrentMethod().DeclaringType.Name ) );
			this.SetType( NODE_TYPE );
		}

		/// <summary>
		/// Constructor using a ObjectTreeRow for initialization
		/// </summary>
		public FolderNode ( ObjectTreeRow RootNodeRow ) {
			logger.Trace( String.Format( "Creating a '{0}' instance" , MethodBase.GetCurrentMethod().DeclaringType.Name ) );
			this.SetID( RootNodeRow.GetObjectID() );
			this.SetParentID( RootNodeRow.GetObjectParentID() );
			this.SetType( NODE_TYPE );
			this.SetPath( RootNodeRow.GetObjectPath() );
			this.SetName( RootNodeRow.GetObjectName() );
			this.SetDescription( RootNodeRow.GetObjectDescription() );
			this.SetLastUpdated( RootNodeRow.GetObjectLastUpdated() );
		}

		/// <summary>
		/// Gibt diese Implementierung vom Typ System.String zurück
		/// </summary>
		public override String ToString () {
			return String.Format( "FolderNode({0}/{1}/{2}/{3})" ,
				this.GetID() ,
				this.GetParentID() ,
				this.GetName() ,
				this.GetDescription()
			);
		}

	}
}
