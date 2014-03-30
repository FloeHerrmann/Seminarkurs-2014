#include "SPI.h"
#include "Bridge.h"
#include "YunServer.h"
#include "YunClient.h"
#include "TouchScreen.h"
#include "SoftwareSerial.h"
#include "EEPROMex.h"

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
#define ChartStartY 30
#define TopBarHeight 20

// Time between two measurements/samples
#define SampleTime 5000

// Define Input Pins
#define LEDPin 7
#define CO2SensorRx 8
#define CO2SensorTx 9
#define TemperaturePin A5
#define LoudnessPin A4

// Define EEPROM Cells
#define CO2Cell 0
#define LoudnessCell 8

// Timestamp of the last measurement/sample
long LastSampleTimestamp;

// Threshold for maximum loudness
int TresholdLoudness;

// Threshold for minimum CO2 cocentration
int TresholdCO2Concentration;

// Helper for output on the display
int MaximumLoudnessBarHeight;
int MaximumCO2BarHeight;

// Current values
int CurrentCO2Concentration;
int CurrentLoudness;
float CurrentTemperature;

// Server for incoming connections
YunServer Server( 10001 );

// Object to access the display
TouchScreen Display = TouchScreen( );

// Object to access the CO2 sensor
SoftwareSerial CO2Sensor( CO2SensorRx , CO2SensorTx );

// Command sequence to read data from CO2 Sensor
byte CO2SensorRead[] = { 0xFE , 0X44 , 0X00 , 0X08 , 0X02 , 0X9F , 0X25 };

void setup() {

	// Open the serial connection to the CO2 sensor
	CO2Sensor.begin( 9600 );
	
	// Initialize and clear the display
	Display.init();
	Display.clear();
	
	// Flash LED on the arduino to indicate the programm is running
	pinMode( 13 , OUTPUT );
	digitalWrite( 13 , LOW );
	delay( 500 );
	for( int i = 0 ; i < 3 ; i++ ) {
		digitalWrite( 13 , HIGH );
		delay( 500 );
		digitalWrite( 13 , LOW );
		delay( 500 );
	}

	// Load maximum CO2 concentration treshold from EEPROM
	TresholdCO2Concentration = 1000;//EEPROM.readInt( CO2Cell );
	MaximumCO2BarHeight = ((float)TresholdCO2Concentration/MaximumTotalCO2Cocentration) * ChartBarHeight;

	// Load maximum loudness treshold from EEPROm
	TresholdLoudness = 500;//EEPROM.readInt( LoudnessCell );
	MaximumLoudnessBarHeight = ((float)TresholdLoudness/MaximumTotalLoudness) * ChartBarHeight;

	// Starting the Bridge
	Bridge.begin();
	
	// Flash LED on the arduino to indicate the bridge is running
	digitalWrite( 13 , HIGH );
	delay( 1000 );
	digitalWrite( 13 , LOW );
	delay( 1000 );

	// Starting the YunServer
	Server.noListenOnLocalhost();
	Server.begin();
	
	// Set LED on the arduino to indicate the server is running
	digitalWrite( 13 , HIGH );

	// Draw Header
	//Display.fillRectangle( 0 , TopBarHeight , FillColorBlue );
	//Display.drawStringCenter8px( "Air-Loudness-Monitoring" , 5 , FontColorWhite , FillColorBlue );
	
	// Frame for the CO2 Concentration bar
	Display.drawRectangle( 20 , ChartStartY , ChartBarWidth , ChartBarHeight , FontColorBlack );
	//Display.drawStringCenter8px( "Air" , 20 , 110 , 280 , FontColorBlack , FillColorWhite );

	// Frame for the loudness bar
	Display.drawRectangle( 130 , ChartStartY , ChartBarWidth , ChartBarHeight , FontColorBlack );
	//Display.drawStringCenter8px( "Loudness" , 130 , 220 , 280 , FontColorBlack , FillColorWhite );

	// Get current Values
	CurrentCO2Concentration = getCO2Concentration();
	CurrentLoudness = getLoudness();
	CurrentTemperature = getTemperature();

	// Update the display
	updateDisplay();

	// Update timestamp for the last measurement
	LastSampleTimestamp = millis();
}

void loop() {

	// Check if there is a client
	YunClient client = Server.accept();

	// Handle client request
	if( client ) {
		// Let some time pass to receive all data
		delay( 50 );
		//client.setTimeout( 5000 );
		// Receive the command
		String command = client.readString();
		// Trim the command
		command.trim();
		// Process the command
		processCommand( command , client );
		// Stop client connection
		client.stop();
	}
	
	// Check time between the now() and the last measurement
	if( millis() - LastSampleTimestamp > SampleTime ){

		// Get current values
		CurrentCO2Concentration = getCO2Concentration();
		CurrentLoudness = getLoudness();
		CurrentTemperature = getTemperature();

		// Turn the LED on > Normally the window would be opened
		if( CurrentCO2Concentration > TresholdCO2Concentration ) {
			setLEDState( true );
		} else {
			setLEDState( false );
		}

		// Update the display
		updateDisplay( );

		// Update the timestamp for the last measurement
		LastSampleTimestamp = millis();
	}
}

// Update the display
void updateDisplay( ) {

	int StartPosY = ChartStartY + ChartBarMargin;
	int InnerBarHeight = ChartBarHeight - ChartBarMargin - ChartBarMargin;
	int InnerBarWidth = ChartBarWidth - ChartBarMargin - ChartBarMargin;

	float CO2Percentage = (float)CurrentCO2Concentration / MaximumTotalCO2Cocentration;
	int CO2BarHeight = InnerBarHeight * CO2Percentage;
	int CO2Offset = InnerBarHeight - CO2BarHeight;

	int StartPosX = 20 + ChartBarMargin;
	Display.fillRectangle( StartPosX , StartPosY , InnerBarWidth , CO2Offset , FillColorWhite );
	if( CurrentCO2Concentration > TresholdCO2Concentration ) {
		int Difference = InnerBarHeight - CO2Offset - MaximumCO2BarHeight;
		Display.fillRectangle( StartPosX , StartPosY + CO2Offset , InnerBarWidth , Difference , FillColorRed );
		Display.fillRectangle( StartPosX , StartPosY + CO2Offset + Difference , InnerBarWidth , MaximumCO2BarHeight , FillColorDarkGreen );
	} else {
		Display.fillRectangle( StartPosX , StartPosY + CO2Offset , InnerBarWidth , CO2BarHeight , FillColorDarkGreen );
	}
	/*
	String CO2Value = "   ";
	CO2Value += CurrentCO2Concentration;
	CO2Value += "ppm   ";
	Display.drawStringCenter8px( CO2Value , 20 , 110 , 250 , FontColorBlack , FillColorDarkGreen );
	*/

	float LoudnessPercentage = (float)CurrentLoudness / MaximumTotalLoudness;
	int LoudnessBarHeight = InnerBarHeight * LoudnessPercentage;
	int LoudnessOffset = InnerBarHeight - LoudnessBarHeight;

	StartPosX = 130 + ChartBarMargin;
	Display.fillRectangle( StartPosX , StartPosY , InnerBarWidth , LoudnessOffset , FillColorWhite );
	if( CurrentLoudness > TresholdLoudness ) {
		int Difference = InnerBarHeight - LoudnessOffset - MaximumLoudnessBarHeight;
		Display.fillRectangle( StartPosX , StartPosY + LoudnessOffset , InnerBarWidth , Difference , FillColorRed );
		Display.fillRectangle( StartPosX , StartPosY + LoudnessOffset + Difference , InnerBarWidth , MaximumLoudnessBarHeight , FillColorDarkGreen );
	} else {
		Display.fillRectangle( StartPosX , StartPosY + LoudnessOffset , InnerBarWidth , LoudnessBarHeight , FillColorDarkGreen );
	}

	/*
	String LoudnessValue = "   ";
	LoudnessValue += CurrentLoudness;
	LoudnessValue += "db   ";
	Display.drawStringCenter8px( CO2Value , 130 , 220 , 250 , FontColorBlack , FillColorDarkGreen );
	*/
}

// Processing the received command
void processCommand( String command , YunClient client ) {
	if( command == "C:Data:Get;" ) {
		processDataCommand( true , true , true , client );
	} else if( command == "C:Data:Get:CO2Concentration;" ) {
		processDataCommand( true , false , false , client );
	} else if( command == "C:Data:Get:Loudness;" ) {
		processDataCommand( false , true , false , client );
	} else if( command == "C:Data:Get:Temperature;" ) {
		processDataCommand( false , false , true , client );
	} else if( command == "C:Threshold:Get;" ) {
		processGetTresholdCommand( true , true , client );
	} else if( command == "C:Threshold:Get:Loudness;" ) {
		processGetTresholdCommand( false , true , client );
	} else if( command == "C:Threshold:Get:CO2Concentration;" ) {
		processGetTresholdCommand( true , false , client );
	} else if( command.indexOf( "C:Threshold:Set:Loudness:" ) != -1 ) {
		// Command > C:Treshold:Set:Loudness:80;
		int StartIndex = command.lastIndexOf( ":" );
		int EndIndex = command.lastIndexOf( ";" );
		String value = command.substring( StartIndex + 1 , EndIndex );
		processSetTresholdCommand( false , true , value , client );
	} else if( command.indexOf( "C:Threshold:Set:CO2Concentration:" ) != -1 ) {
		// Command > C:Treshold:Set:CO2Concentration:2000;
		int StartIndex = command.lastIndexOf( ":" );
		int EndIndex = command.lastIndexOf( ";" );
		String value = command.substring( StartIndex + 1 , EndIndex );
		processSetTresholdCommand( true , false , value , client );
	} else {
		String Response = "{\"State\":\"Unknown Command\"}";
		client.println( Response );
	}
}

// Processing the command to get data
void processDataCommand( bool CO2 , bool Loudness , bool Temperature , YunClient client ) {
	String dataString = "{";
	if ( CO2 == true) {
		dataString += "\"CO2Concentration\":\"";
		dataString += CurrentCO2Concentration;
		dataString += "\"";
		if( Loudness == true || Temperature == true ) dataString += ",";
	}
	if( Loudness == true ) {
		dataString += "\"Loudness\":\"";
		dataString += CurrentLoudness;
		dataString += "\"";
		if( Temperature == true ) dataString += ",";
	}
	if( Temperature == true ){
		dataString += "\"Temperature\":\"";
		dataString += CurrentTemperature;
		dataString += "\"";
	}
	dataString += "}";
	client.println( dataString );
}

// Processing the command to get a treshold
void processGetTresholdCommand( bool CO2 , bool Loudness , YunClient client ) {
	String dataString = "{";
	if( CO2 == true ) {
		dataString += "\"CO2Treshold\":\"";
		dataString += TresholdCO2Concentration;
		dataString += "\"";
		if( Loudness == true ) dataString += ",";
	}
	if( Loudness == true ) {
		dataString += "\"LoudnessTreshold\":\"";
		dataString += CurrentLoudness;
		dataString += "\"";
	}
	dataString += "}";
	client.println( dataString );
}

// Processing the command to set a treshold
void processSetTresholdCommand( bool CO2 , bool Loudness , String value , YunClient client ) {
	if( CO2 == true ) {
	 	TresholdCO2Concentration = value.toInt();
	 	MaximumCO2BarHeight = ((float)TresholdCO2Concentration/MaximumTotalCO2Cocentration) * ChartBarHeight;
	 	EEPROM.updateInt( CO2Cell , TresholdCO2Concentration );
	}
	if( Loudness == true ) {
		TresholdLoudness = value.toInt();
		MaximumLoudnessBarHeight = ((float)TresholdLoudness/MaximumTotalLoudness) * ChartBarHeight;
		EEPROM.updateInt( LoudnessCell , TresholdLoudness );
	}
	processGetTresholdCommand( CO2 , Loudness , client );
}

// Get the current temperature
float getTemperature(){
	int temp = analogRead( TemperaturePin );
	float resistance = (float)( 1023 - temp) * 10000/temp;
	float temperature = 1 / ( log( resistance / 10000 ) / 3975 + 1 / 298.15 ) - 273.15;
	return temperature;
}

// Turn the LED on or off
void setLEDState( bool state ) {
	if( state == true ) digitalWrite( LEDPin , HIGH );
	else if( state == false ) digitalWrite( LEDPin , LOW ); 
}

// Get the current loudness
int getLoudness(){
	return analogRead( LoudnessPin );
}

// Get the current CO2 Concentration
int getCO2Concentration(){

	int RetryCounter = 0;
	while( CO2Sensor.available() == 0 ) {
		CO2Sensor.write( CO2SensorRead , 7 );
		if( RetryCounter++ > 20 ) return -800;
		delay( 50 );
	}

	RetryCounter = 0;
	while( CO2Sensor.available() < 7 ) {
		delay( 5 );
		if( RetryCounter++ > 40 ) break;
	}

	if( CO2Sensor.available() == 7 ) {
		byte Response[] = { 0 , 0 , 0 , 0 , 0 , 0 , 0 };
		for( int i = 0 ; i < 7 ; i++ ) {
			Response[ i ] = CO2Sensor.read();
		}
		int HighByte = Response[3];
		int LowByte = Response[4];
		unsigned long CO2Value = HighByte * 256 + LowByte;
		if( CO2Value > MaximumTotalCO2Cocentration ) return (int)MaximumTotalCO2Cocentration;
		return CO2Value;
	} else {
		long CO2Value = -900;
		while( CO2Sensor.available() > 0 ) {
			CO2Value--;
			CO2Sensor.read();
		}
	}
}
