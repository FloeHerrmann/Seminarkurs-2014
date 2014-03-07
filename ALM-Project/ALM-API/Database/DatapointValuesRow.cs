using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gsmgh.Alm.Database {
	public class DatapointValuesRow {

		/// <summary>
		///		Column names of the object_tree table
		/// </summary>
		String[] ValueKeys = { 
			"TIME_STAMP" , "DATAPOINT_ID" , "DATATYPE" ,  "INT_VALUE" , "DECIMAL_VALUE" , "STRING_VALUE"
		};

		/// <summary>
		///		Dictionary with column name as key and a string value
		/// </summary>
		public Dictionary<String , String> ObjectData = new Dictionary<String , String>();

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="ObjectRow">
		///		A ',' delimted string with all column values of the object_tree table
		/// </param>
		public DatapointValuesRow ( String ObjectRow ) {
			String[] ObjectValues = ObjectRow.Split( '/' );
			if( ObjectValues.Length >= ValueKeys.Length ) {
				for( Int32 index = 0 ; index < ObjectValues.Length ; index++ ) {
					this.ObjectData.Add(
						ValueKeys[ index ] ,
						ObjectValues[ index ]
					);
				}
			} else {
				throw new Exception( "Amount of values != Amount of keys" );
			}
		}

		public Int64 GetDatapointID () {
			try {
				return Int64.Parse( ObjectData[ "DATAPOINT_ID" ] );
			} catch( Exception ) {
				return 0L;
			}
		}

		public DateTime GetTimeStamp () {
			return DateTime.ParseExact( ObjectData[ "TIME_STAMP" ] , "dd.MM.yyyy HH:mm:ss" , null );
		}

		public Int32 GetDataType () {
			try {
				return Int32.Parse( ObjectData[ "DATATYPE" ] );
			} catch( Exception ) {
				return 0;
			}
		}
		public Int32 GetIntegerValue () {
			try {
				return Int32.Parse( ObjectData[ "INT_VALUE" ] );
			} catch( Exception ) {
				return 0;
			}
		}
		public Double GetDecimalValue () {
			try {
				return Double.Parse( ObjectData[ "DECIMAL_VALUE" ] );
			} catch( Exception ) {
				return 0.0;
			}
		}

		public String GetStringValue () {
			return ObjectData[ "STRING_VALUE" ];
		}
	}
}
