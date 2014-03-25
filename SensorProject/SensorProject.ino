#include "SPI.h"
#include "Bridge.h"
#include "YunServer.h"
#include "YunClient.h"
#include "TouchScreen.h"
#include "CO2Sensor.h"

// Define Colors

#define FontColorBlack 0x0000
#define FontColorWhite 0xFFFF
#define FontColorLightRed 0xE5B5
#define FontColorRed 0xD9A2
#define FontColorBlue 0x0339
#define FontColorDarkGray 0x20E4
#define FontColorLightGray 0x8410
#define FontColorDarkGreen 0x07E0
#define FontColorGreen 0x858E
#define FontColorCyan 0x6679
#define FontColorLightPurple 0xB41
#define FontColorPurple 0x826

#define FillColorBlack 0x0000
#define FillColorWhite 0xFFFF
#define FillColorLightRed 0xE5B5
#define FillColorRed 0xD9A2
#define FillColorBlue 0x0339
#define FillColorDarkGray 0x20E4
#define FillColorLightGray 0x8410
#define FillColorDarkGreen 0x07E0
#define FillColorGreen 0x858E
#define FillColorCyan 0x6679
#define FillColorLightPurple 0xB415
#define FillColorPurple 0x826F

#define MaximumTotalLoudness 1023.0
#define MaximumTotalCO2Cocentration 3000.0

// Data for display
#define ChartBarHeight 240
#define ChartBarWidth 90
#define ChartBarMargin 2
#define ChartStartY 20

// Time between two measurements/samples
#define SampleTime 5000

// Define Input Pins
#define LEDPin 7
#define TemperaturePin A5
#define LoudnessPin A4
#define CO2SensorRx 8
#define CO2SensorTx 9

// Timestamp of the last measurement/sample
long LastSampleTimestamp;

// Server for incoming connections
YunServer Server;

// Threshold for maximum loudness
int MaximumLoudness;
int MaximumLoudnessBarHeight;

// Threshold for minimum CO2 cocentration
int MaximumCO2Concentration;
int MaximumCO2BarHeight;

// Object to access the display
TouchScreen Display = TouchScreen( A0 , A1 , A2 , A3 );

// Object to access the CO2 sensor
CO2Sensor co2Sensor( CO2SensorRx , CO2SensorTx );

void setup() {

	pinMode( 13 , OUTPUT );
	
	// Initialize and clear the display
	Display.init();
	Display.clear();
	
	digitalWrite( 13 , LOW );
	delay( 1000 );
	for( int i = 0 ; i < 3 ; i++ ) {
		digitalWrite( 13 , HIGH );
		delay( 1000 );
		digitalWrite( 13 , LOW );
		delay( 1000 );
	}

	// Load maximum CO2 concentration treshold from EEPROM
	MaximumCO2Concentration = 1500;
	MaximumCO2BarHeight = ((float)MaximumCO2Concentration/MaximumTotalCO2Cocentration) * ChartBarHeight;

	// Load maximum loudness treshold from EEPROm
	MaximumLoudness = 800;
	MaximumLoudnessBarHeight = ((float)MaximumLoudness/MaximumTotalLoudness) * ChartBarHeight;

	// Starting the Bridge
	Bridge.begin();
	
	digitalWrite( 13 , HIGH );
	delay( 1000 );
	digitalWrite( 13 , LOW );
	delay( 1000 );

	// Starting the YunServer
	Server.noListenOnLocalhost();
	Server.begin();
	
	digitalWrite( 13 , HIGH );
	delay( 1000 );
	digitalWrite( 13 , LOW );
	delay( 1000 );

	Display.drawRectangle( 20 , ChartStartY , ChartBarWidth , ChartBarHeight , FontColorBlack );
	Display.drawRectangle( 130 , ChartStartY , ChartBarWidth , ChartBarHeight , FontColorBlack );

	updateDisplay( getLoudness() , getCO2Concentration() );

	LastSampleTimestamp = millis();
}

void loop() {
	YunClient client = Server.accept();
	if( client ) {
		processRequest( client );
		client.stop();
	}
	
	if( millis() - LastSampleTimestamp > SampleTime ){
		updateDisplay( getLoudness() , getCO2Concentration() );
		LastSampleTimestamp = millis();
	}

	if( getCO2Concentration() > MaximumCO2Concentration ) {
		setLEDState( true );
	} else {
		setLEDState( false );
	}
}

void updateDisplay( int Loudness , int CO2Concentration ) {

	float CO2Percentage = (float)CO2Concentration / MaximumTotalCO2Cocentration;
	int CO2BarHeight = ChartBarHeight * CO2Percentage;
	int CO2Offset = ChartBarHeight - CO2BarHeight;

	int StartPosX = 20 + ChartBarMargin;
	int StartPosY = ChartStartY + ChartBarMargin;
	int TempWidth = ChartBarWidth - ChartBarMargin - ChartBarMargin;
	Display.fillRectangle( StartPosX , StartPosY , TempWidth , CO2Offset , FillColorWhite );
	if( CO2Concentration > MaximumCO2Concentration ) {
		int Difference = CO2BarHeight - MaximumCO2BarHeight;
		Display.fillRectangle( StartPosX , StartPosY + CO2Offset , TempWidth , Difference , FillColorRed );
		Display.fillRectangle( StartPosX , StartPosY + CO2Offset + Difference , TempWidth , MaximumCO2BarHeight , FillColorDarkGreen );
	} else {
		Display.fillRectangle( StartPosX , ChartStartY + CO2Offset , TempWidth , CO2BarHeight , FillColorDarkGreen );
	}

	float LoudnessPercentage = (float)Loudness / MaximumTotalLoudness;
	int LoudnessBarHeight = ChartBarHeight * LoudnessPercentage;
	int LoudnessOffset = ChartBarHeight - LoudnessBarHeight;

	StartPosX = 130 + ChartBarMargin;
	Display.fillRectangle( StartPosX , StartPosY , TempWidth , LoudnessOffset , FillColorWhite );
	if( Loudness > MaximumLoudness ) {
		int Difference = LoudnessBarHeight - MaximumLoudnessBarHeight;
		Display.fillRectangle( StartPosX , StartPosY + LoudnessOffset , TempWidth , Difference , FillColorRed );
		Display.fillRectangle( StartPosX , StartPosY + LoudnessOffset + Difference , TempWidth , MaximumLoudnessBarHeight , FillColorDarkGreen );
	} else {
		Display.fillRectangle( StartPosX , ChartStartY + LoudnessOffset , TempWidth , LoudnessBarHeight , FillColorDarkGreen );
	}
}

void processRequest(YunClient client) {
	String command = client.readStringUntil('/');
	command.trim();
	if (command == "data") {
		messageDataCommand( client );
	} else if( command == "loudness" ) {
		messageLoudnessCommand( client );
	} else if( command = "airquality" ) {
		messageAirQualityCommand( client );
	}
}

void messageDataCommand( YunClient client ) {
	String message = client.readStringUntil('/');
	message.trim();
	client.println( F( "Status: 200" ) );
 	client.println( F( "Content-type: application/json" ) );
 	client.println();
	if( message == "loudness" ) {
		client.print( F( "{\"loudness\":\"") );
		client.print( random( 30 , 130 ) );
		client.println( F( "\"}") );
	}
	if ( message == "airquality" ) {
		client.print( F( "{\"airQuality\":\"0.") );
		client.print( random( 0 , 100 ) );
		client.println( F( "\"}") );
	}
	if ( message != "loudness" && message != "airquality" ){
		client.print( F("{\"airQuality\":\"0.") );
		client.print( random( 0 , 100 ) );
		client.print( F( "\",\"loudness\":\"") );
		client.print( random( 30 , 130 ) );
		client.println( F( "\"}") );
	}
}

void messageLoudnessCommand( YunClient client ) {
	String message = client.readStringUntil('/');
	message.trim();
	client.println( F( "Status: 200" ) );
 	client.println( F( "Content-type: application/json" ) );
 	client.println();
	if( message == "set" ) {
		String value = client.readStringUntil( '/' );
	} else if ( message == "get") {
		client.print( F( "{\"maxLoudness\":\"") );
		client.print( MaximumLoudness );
		client.println( F( "\"}") );
	}
}

void messageAirQualityCommand( YunClient client ) {
	String message = client.readStringUntil('/');
	message.trim();
	client.println( F( "Status: 200" ) );
 	client.println( F( "Content-type: application/json" ) );
 	client.println();
	if( message == "set" ) {
		String value = client.readStringUntil( '/' );
	} else if ( message == "get") {
		client.print( F( "{\"maxCO2Concentration\":\"") );
		client.print( MaximumCO2Concentration );
		client.println( F( "\"}") );
	}
}

float getTemperature(){
	int temp = analogRead( TemperaturePin );
	float resistance = (float)( 1023 - temp) * 10000/temp;
	float temperature = 1 / ( log( resistance / 10000 ) / 3975 + 1 / 298.15 ) - 273.15;
	return temperature;
}

void setLEDState( bool state ) {
	if( state == true ) digitalWrite( LEDPin , HIGH );
	else if( state == false ) digitalWrite( LEDPin , LOW ); 
}

int getLoudness(){
	return analogRead( LoudnessPin );
}

int getCO2Concentration(){
	double co2 = co2Sensor.getCO2( 'p' );
	return (int)co2;
}
