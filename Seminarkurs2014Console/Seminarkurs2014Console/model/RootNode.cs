using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Seminarkurs2014Console.model {
	/// <summary>
	/// Model for the root node
	/// </summary>
	public class RootNode : AbstractObjectNode {

		/// <summary>
		/// Log4net logger instance for logging purposes
		/// </summary>
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		/// <summary>
		/// Number of the root node type [1]
		/// </summary>
		public static Int32 NODE_TYPE = 1;

		/// <summary>
		/// Constructor
		/// </summary>
		public RootNode () {
			logger.Trace( String.Format( "Creating a '{0}' instance" , MethodBase.GetCurrentMethod().DeclaringType.Name ) );
		}

		/// <summary>
		/// Gibt diese Implementierung vom Typ System.String zurück
		/// </summary>
		public override String ToString () {
			return String.Format( "RootNode({0}/{1}/{2}/{3})" ,
				this.GetID() ,
				this.GetParentID() ,
				this.GetName() ,
				this.GetDescription()
			);
		}

	}
}
