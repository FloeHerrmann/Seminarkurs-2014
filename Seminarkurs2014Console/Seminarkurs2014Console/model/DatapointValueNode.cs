using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Seminarkurs2014Console.model {
	/// <summary>
	/// Model for a datapoint node
	/// </summary>
	public class DatapointValueNode {

		/// <summary>
		/// Log4net logger instance for logging purposes
		/// </summary>
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		/// <summary>
		/// Number of the datapoint node type [5]
		/// </summary>
		public static Int32 NODE_TYPE = 5;

		/// <summary>
		/// Timestamp of the datapoint value
		/// </summary>
		private DateTime TimeStamp;

		/// <summary>
		/// ID of the corresponding datapoint
		/// </summary>
		private Int64 DatapointID;

		/// <summary>
		/// Unit of the datapoint
		/// </summary>
		private Int32 Type;

		/// <summary>
		/// Interger value
		/// </summary>
		private Int32 IntegerValue;

		/// <summary>
		/// Interger value
		/// </summary>
		private Double DecimalValue;

		/// <summary>
		/// Interger value
		/// </summary>
		private String StringValue;

		/// <summary>
		/// Constructor
		/// </summary>
		public DatapointValueNode () {
			logger.Trace( String.Format( "Creating a '{0}' instance" , MethodBase.GetCurrentMethod().DeclaringType.Name ) );
		}

		/// <summary>
		/// Set the Timestamp of the datapoint value
		/// </summary>
		/// <param name="Unit">String to describe the datapoint unit</param>
		public void SetTimeStamp ( DateTime TimeStamp ) {
			this.TimeStamp = TimeStamp;
		}

		/// <summary>
		/// Timestamp of the datapoint value
		/// </summary>
		/// <returns>String that describes the datapoint unit</returns>
		public DateTime GetTimeStamp () {
			return this.TimeStamp;
		}

		/// <summary>
		/// Set the ID of the corresponding datapoint
		/// </summary>
		public void GetDatapointID ( Int64 DatapointID ) {
			this.DatapointID = DatapointID;
		}

		/// <summary>
		/// Get the ID of the corresponding datapoint
		/// </summary>
		public Int64 GetDatapointID () {
			return this.DatapointID;
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
		public void SetType ( Int32 Type ) {
			this.Type = Type;		
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
		public Int32 GetType () {
			return this.Type;
		}

		/// <summary>
		/// Set the Unit of the datapoint
		/// </summary>
		/// <param name="Unit">String to describe the datapoint unit</param>
		public void SetIntegerValue ( Int32 Value ) {
			this.IntegerValue = Value;
		}

		/// <summary>
		/// Get the Unit of the datapoint
		/// </summary>
		/// <returns>String that describes the datapoint unit</returns>
		public Int32 GetIntegerValue () {
			return this.IntegerValue;
		}

		/// <summary>
		/// Set the calculation that is applied to incoming values
		/// </summary>
		/// <param name="Calculation">Calculation formula</param>
		public void SetDecimalValue ( Double Value ) {
			this.DecimalValue = Value;
		}

		/// <summary>
		/// Get the calculation that is applied to incoming values
		/// </summary>
		/// <returns>Calculation formula</returns>
		public Double GetDecimalValue () {
			return this.DecimalValue;
		}

		/// <summary>
		/// Set the calculation that is applied to incoming values
		/// </summary>
		/// <param name="Calculation">Calculation formula</param>
		public void SetStringValue ( String Value ) {
			this.StringValue = Value;
		}

		/// <summary>
		/// Get the calculation that is applied to incoming values
		/// </summary>
		/// <returns>Calculation formula</returns>
		public String GetStringValue () {
			return this.StringValue;
		}

		/// <summary>
		/// Gibt diese Implementierung vom Typ System.String zurück
		/// </summary>
		public override String ToString () {
			return String.Format( "DatapointValueNode({0}/{1}/{2}/{3})" ,
				this.GetDatapointID() ,
				this.GetType() ,
				this.GetTimeStamp() ,
				this.GetStringValue()
			);
		}

	}
}
