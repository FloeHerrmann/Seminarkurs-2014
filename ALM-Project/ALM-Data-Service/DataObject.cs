using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALM_Data_Service {
	public class DataObject {

		public String Loudness;
		public String CO2Concentration;
		public String Temperature;

		public Int32 GetCO2Concentration () {
			return Int32.Parse( CO2Concentration );
		}

		public Int32 GetLoudness () {
			return Int32.Parse( Loudness );
		}

		public Double GetTemperature () {
			return Double.Parse( Temperature.Replace( "." , "," ) );
		}

	}
}
