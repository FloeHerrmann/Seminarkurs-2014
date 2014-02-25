using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Seminarkurs2014Console.database {
	public class ObjectTreeRow {

		String[] ValueKeys = { 
			"OBJECT_ID" , "OBJECT_PARENT_ID" , "OBJECT_TYPE" , "OBJECT_PATH" , "OBJECT_NAME" , "OBJECT_DESCRIPTION" , "OBJECT_LAST_UPDATED" ,
			"SENSOR_IP_ADDRESS" , "SENSOR_PORT" , "SENSOR_LAST_CONNECTION" ,
			"DATAPOINT_TYPE" , "DATAPOINT_UNIT" , "DATAPOINT_CALCULATION"
		};

		public Dictionary<String , String> ObjectData = new Dictionary<String , String>();

		public ObjectTreeRow ( String ObjectRow ) {
			String[] ObjectValues = ObjectRow.Split( ',' );
			if( ObjectValues.Length >= ValueKeys.Length ) {
				for( Int32 index = 0 ; index < ObjectValues.Length ; index++ ) {
					this.ObjectData.Add(
						ValueKeys[ index ] ,
						ObjectValues[ index ]
					);
				}
			} else {
				throw new Exception( "Anzahl der Werte != Anzahl der Schlüssel" );
			}
		}

		public Int64 GetObjectID () {
			try {
				return Int64.Parse( ObjectData[ "OBJECT_ID" ] );
			} catch( Exception ) {
				return 0L;
			} 
		}

		public Int64 GetObjectParentID () {
			try {
				return Int64.Parse( ObjectData[ "OBJECT_PARENT_ID" ] );
			} catch( Exception ) {
				return 0L;
			}
		}

		public Int32 GetObjectType () {
			try {
				return Int32.Parse( ObjectData[ "OBJECT_TYPE" ] );
			} catch( Exception ) {
				return 0;
			}
		}

		public String GetObjectPath () {
			return ObjectData[ "OBJECT_PATH" ];
		}

		public String GetObjectName () {
			return ObjectData[ "OBJECT_NAME" ];
		}

		public String GetObjectDescription () {
			return ObjectData[ "OBJECT_DESCRIPTION" ];
		}

		public DateTime GetObjectLastUpdated () {
			return DateTime.Parse( ObjectData[ "OBJECT_LAST_UPDATED" ] );
		}

		public IPAddress GetDeviceIPAddress () {
			return IPAddress.Parse( ObjectData[ "SENSOR_IP_ADDRESS" ] );
		}

		public Int32 GetDevicePort () {
			try {
				return Int32.Parse( ObjectData[ "SENSOR_PORT" ] );
			} catch( Exception ) {
				return 0;
			}
		}

		public DateTime GetDeviceLastConnection () {
			return DateTime.Parse( ObjectData[ "SENSOR_LAST_CONNECTION" ] );
		}

		public Int32 GetDatapointType () {
			try {
				return Int32.Parse( ObjectData[ "DATAPOINT_TYPE" ] );
			} catch( Exception ) {
				return 0;
			}
		}

		public String GetDatapointUnit () {
			return ObjectData[ "DATAPOINT_UNIT" ];
		}

		public String GetDatapointCalculation () {
			return ObjectData[ "DATAPOINT_CALCULATION" ];
		}

	}
}
