using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gsmgh.Alm.Database;

namespace Gsmgh.Alm.Model {
	/// <summary>
	/// Model for a datapoint node
	/// </summary>
	public class DatapointNode : AbstractObjectNode {

		/// <summary>
		/// Number of the datapoint node type [4]
		/// </summary>
		public static Int32 NODE_TYPE = 4;

		/// <summary>
		/// Integer datapoint type
		/// </summary>
		public static Int32 TYPE_INTEGER = 0;

		/// <summary>
		/// Floating-Point datapoint type
		/// </summary>
		public static Int32 TYPE_FLOATING_POINT = 1;

		/// <summary>
		/// Boolean datapoint type
		/// </summary>
		public static Int32 TYPE_BOOL = 2;

		/// <summary>
		/// Text datapoint type
		/// </summary>
		public static Int32 TYPE_TEXT = 3;

		/// <summary>
		/// Type of the datapoint (Integer, Floating-Point, Bool or Text)
		/// </summary>
		private Int32 DatapointType;

		/// <summary>
		/// Unit of the datapoint
		/// </summary>
		private String Unit;

		/// <summary>
		/// Last value of the datapoint
		/// </summary>
		private String LastValue;

		/// <summary>
		/// Update of the last value of the datapoint
		/// </summary>
		private DateTime LastValueUpdate;

		/// <summary>
		/// Constructor
		/// </summary>
		public DatapointNode () {
			this.SetType( NODE_TYPE );
		}

		/// <summary>
		/// Constructor using a ObjectTreeRow for initialization
		/// </summary>
		public DatapointNode ( ObjectTreeRow RootNodeRow ) {
			this.SetID( RootNodeRow.GetObjectID() );
			this.SetParentID( RootNodeRow.GetObjectParentID() );
			this.SetType( NODE_TYPE );
			this.SetName( RootNodeRow.GetObjectName() );
			this.SetDescription( RootNodeRow.GetObjectDescription() );
			this.SetLastUpdated( RootNodeRow.GetObjectLastUpdated() );
			this.SetDatapointType( RootNodeRow.GetDatapointType() );
			this.SetUnit( RootNodeRow.GetDatapointUnit() );
			this.SetLastValue( RootNodeRow.GetDatapointLastValue() );
			this.SetLastValueUpdate( RootNodeRow.GetDatapointLastUpdated() );
		}

		/// <summary>
		/// Set the Type of the datapoint
		/// </summary>
		/// <param name="Type">
		/// Integer = 0
		/// Floating-Point = 1
		/// Bool = 2
		/// Text = 3
		/// </param>
		public void SetDatapointType ( Int32 DatapointType ) {
			this.DatapointType = DatapointType;		
		}

		/// <summary>
		/// Get the Type of the datapoint
		/// </summary>
		/// <returns>
		/// Type of the datapoint:
		/// Integer = 0
		/// Floating-Point = 1
		/// Bool = 2
		/// Text = 3
		/// </returns>
		public Int32 GetDatapointType () {
			return this.DatapointType;
		}

		/// <summary>
		/// Set the Unit of the datapoint
		/// </summary>
		/// <param name="Unit">String to describe the datapoint unit</param>
		public void SetUnit ( String Unit ) {
			this.Unit = Unit;
		}

		/// <summary>
		/// Get the Unit of the datapoint
		/// </summary>
		/// <returns>String that describes the datapoint unit</returns>
		public String GetUnit () {
			return this.Unit;
		}

		public void SetLastValue ( String Value ) {
			this.LastValue = Value;
		}
		public String GetLastValue () {
			return this.LastValue;
		}

		public void SetLastValueUpdate ( DateTime date ) {
			this.LastValueUpdate = date;
		}
		public DateTime GetLastValueUpdate () {
			return this.LastValueUpdate;
		}

		/// <summary>
		/// Gibt diese Implementierung vom Typ System.String zurück
		/// </summary>
		public override String ToString () {
			return String.Format( "DatapointNode({0}/{1}/{2}/{3})({4}/{5}/{6}/{7})" ,
				this.GetID() ,
				this.GetParentID() ,
				this.GetName() ,
				this.GetDescription(),
				this.GetType(),
				this.GetUnit(),
				this.GetLastValue(),
				this.GetLastValueUpdate()
			);
		}

	}
}
