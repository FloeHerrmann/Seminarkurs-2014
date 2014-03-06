using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gsmgh.Alm.Database;

namespace Gsmgh.Alm.Model {
	/// <summary>
	/// Model for the folder node
	/// </summary>
	public class FolderNode : AbstractObjectNode {

		/// <summary>
		/// Number of the folder node type [2]
		/// </summary>
		public static Int32 NODE_TYPE = 2;

		/// <summary>
		/// Constructor
		/// </summary>
		public FolderNode () {
			this.SetType( NODE_TYPE );
		}

		/// <summary>
		/// Constructor using a ObjectTreeRow for initialization
		/// </summary>
		public FolderNode ( ObjectTreeRow RootNodeRow ) {
			this.SetID( RootNodeRow.GetObjectID() );
			this.SetParentID( RootNodeRow.GetObjectParentID() );
			this.SetType( NODE_TYPE );
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
