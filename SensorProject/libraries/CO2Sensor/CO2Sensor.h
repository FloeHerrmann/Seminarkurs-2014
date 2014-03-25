#if ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

#ifndef CO2Sensor_h
	#define CO2Sensor_h

	#include <SoftwareSerial.h> 	//Virtual Serial library

	class CO2Sensor {
		public:
			CO2Sensor( uint8_t Rx , uint8_t Tx );
			double getCO2( char format );
			double getTemp( char unit );
			double getRH();
			bool _K33;
			bool _ASCII;
			int cmdInit();
		private:
			SoftwareSerial* _Serial;
			void chkSensorType();
			void chkASCII();
			void chkK33();
			int sendRequest( int reqType , int respSize , int respInd );
			long getResp( int size , int strt );	
			void wait( int ms );
	};
#endif