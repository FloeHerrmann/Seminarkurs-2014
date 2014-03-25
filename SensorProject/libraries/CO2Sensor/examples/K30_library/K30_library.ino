#include "kSeries.h"

CO2Sensor Sensor( 12 , 13 );

void setup() {
	Serial.begin( 9600 );
	Serial.println( "CO2 Sensor Test" );
}
void loop() {
	double co2 = K_30.getCO2( 'p' );
	Serial.print( "Co2 ppm = " );
	Serial.println( co2 );
	delay( 2000 );
}