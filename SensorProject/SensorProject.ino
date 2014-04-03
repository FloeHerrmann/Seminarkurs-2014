#include "Bridge.h"
#include "YunServer.h"
#include "YunClient.h"
#include "SoftwareSerial.h"
#include "avr/eeprom.h"
#include "avr/wdt.h"
#include "SPI.h"

#define uchar uint8_t
#define uint uint16_t
#define ulong uint32_t

#define SLAVE_SELECT_LOW  {DDRC |= 0x40;PORTC &=~ 0x40;}
#define SLAVE_SELECT_HIGH {DDRC |= 0x40;PORTC |=  0x40;}
#define DATA_COMMAND_LOW  {DDRD |= 0x80;PORTD &=~ 0x80;}
#define DATA_COMMAND_HIGH {DDRD |= 0x80;PORTD |=  0x80;}

#define SCREEN_MIN_X 0
#define SCREEN_MAX_X 239
#define SCREEN_MIN_Y 0
#define SCREEN_MAX_Y 319

uchar numbers[10][16] = {
	{8,10, 60,126,102,195,195,195,195,195,195,195,102,126,60},
	{5,7,  24,248,248,24,24,24,24,24,24,24,24,24,24},
	{8,10, 60,254,195,3,7,14,28,56,112,224,192,255,255},
	{8,10, 62,127,195,195,6,28,30,7,3,195,199,126,60},
	{8,10, 6,14,30,54,102,198,198,255,255,6,6,6,6,6},
	{8,10, 254,254,192,192,252,254,199,3,3,195,199,254,124},
	{8,10, 60,127,99,192,192,220,254,195,195,195,227,126,60},
	{8,10, 255,255,3,6,12,12,24,24,48,48,96,96,96},
	{8,10, 60,126,231,195,195,102,126,231,195,195,231,126,60},
	{8,10, 60,126,199,195,195,195,127,59,3,3,198,254,124},
};

uchar Chars[10][16] = {
	{8,10, 192,192,192,192,192,192,192,192,192,192,192,255,255}, // L
	{8,10, 0,0,0,195,195,195,195,195,195,195,199,255,123}, // u
	{6,8, 28,60,48,48,252,252,48,48,48,48,48,48,48}, // f
	{6,8, 48,48,48,252,252,48,48,48,48,48,48,60,28}, // t
	{8,10,  0,0,0,123,255,199,195,195,195,195,199,255,123}, //a
	{7,9, 0,0,0,60,126,198,192,252,62,6,198,252,120}, // s
	{8,10,  195,195,0,123,255,199,195,195,195,195,199,255,123}, //ä
	{5,7, 0,0,0,216,216,224,192,192,192,192,192,192,192}, // r
	{8,10, 192,192,192,198,204,216,240,248,216,204,206,198,199}, // k
	{8,8, 0,0,0,60,126,195,195,255,192,192,227,127,60}, // e
};

// Define Colors
#define ColorBlack 0x0000
#define ColorWhite 0xFFFF
#define ColorLightRed 0xE5B5
#define ColorRed 0xD9A2
#define ColorBlue 0x0339
#define ColorDarkGray 0x20E4
#define ColorLightGray 0x8410
#define ColorDarkGreen 0x858E
#define ColorGreen 0x07E0
#define ColorLightPurple 0xB415
#define ColorCyan 0x6679
#define ColorPurple 0x826F

#define MaximumTotalLoudness 1023.0
#define MaximumTotalCO2Cocentration 3000.0

// Data for display
#define ChartBarMargin 10
#define ChartBarWidth (240 - 3*ChartBarMargin)/2
#define ChartBarHeight (320 - ChartBarMargin - 40)

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
// TouchScreen Display = TouchScreen( );

// Object to access the CO2 sensor
SoftwareSerial CO2Sensor( CO2SensorRx , CO2SensorTx );

// Command sequence to read data from CO2 Sensor
const byte CO2SensorRead[] = { 0xFE , 0X44 , 0X00 , 0X08 , 0X02 , 0X9F , 0X25 };

char DisplayBuffer[12];

const char FirmwareVersion[] = "1.2";
const char FirmwareDate[] = "31.03.2014 10:01";

void setup() {

	// Open the serial connection to the CO2 sensor
	CO2Sensor.begin( 9600 );

	// Initialize and clear the display
	Init();
	Clear();
	DrawRectangle( 48 , 168 , 144 , 14 , ColorBlue );
	drawProgressBar( 6 );
	delay( 500 );

	// Load maximum CO2 concentration treshold from EEPROM
	TresholdCO2Concentration = EepromReadInt( CO2Cell );
	// When no threshold is defined, set default
	if( TresholdCO2Concentration == -1 ) TresholdCO2Concentration = 1500;
	MaximumCO2BarHeight = (TresholdCO2Concentration/MaximumTotalCO2Cocentration) * ChartBarHeight;
	drawProgressBar( 20 );

	// Load maximum loudness treshold from EEPROm
	TresholdLoudness = EepromReadInt( LoudnessCell );
	// When no threshold is defined, set default
	if( TresholdLoudness == -1 ) TresholdLoudness = 800;
	MaximumLoudnessBarHeight = (TresholdLoudness/MaximumTotalLoudness) * ChartBarHeight;
	drawProgressBar( 34 );

	// Starting the Bridge
	Bridge.begin();
	drawProgressBar( 62 );

	// Starting the YunServer
	Server.noListenOnLocalhost();
	Server.begin();
	drawProgressBar( 90 );

	// Enable Watchdog
	WatchdogEnable();

	pinMode( 13 , OUTPUT );
	digitalWrite( 13 , HIGH );

	// Get current Values
	CurrentCO2Concentration = getCO2Concentration();
	CurrentLoudness = getLoudness();
	CurrentTemperature = getTemperature();
	drawProgressBar( 100 );
	WatchdogReset();

	delay( 1000 );
	Clear();
	
	// Frame for the CO2 Concentration bar
	DrawRectangle( ChartBarMargin , ChartBarMargin , ChartBarWidth , ChartBarHeight , ColorBlack );
	// Display.drawStringCenter8px( "Air" , 20 , 110 , 280 , ColorBlack , ColorWhite );

	// Frame for the loudness bar
	DrawRectangle( ChartBarWidth + (2*ChartBarMargin) , ChartBarMargin , ChartBarWidth , ChartBarHeight , ColorBlack );
	// Display.drawStringCenter8px( "Loudness" , 130 , 220 , 280 , ColorBlack , ColorWhite );

	int PosX = ChartBarMargin + (( ChartBarWidth - 35 )/2);
	PosX += DrawCharFromArray( Chars[0] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[1] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[2] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[3] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	
	PosX = ( 2 * ChartBarMargin + ChartBarWidth ) + (( ChartBarWidth - 90 )/2);
	PosX += DrawCharFromArray( Chars[0] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[4] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[1] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[3] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[5] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[3] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[6] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[7] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[8] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[9] , PosX , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );

	// Update the display
	updateDisplay();

	// Update timestamp for the last measurement
	LastSampleTimestamp = millis();
}

void loop() {

	// Reset Watchdog
	WatchdogReset();

	// Check if there is a client
	YunClient client = Server.accept();

	// Handle client request
	if( client ) {
		// Let some time pass to receive all data
		delay( 50 );
		// Connection will be closed after the timeout
		client.setTimeout( 2000 );
		// Receive the command
		String command = client.readString();
		// Trim the command
		command.trim();
		// Process the command
		processCommand( command , client );
		// Reset Watchdog
		WatchdogReset();
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

	int StartPosY = ChartBarMargin + 1;
	int StartPosX = ChartBarMargin + 1;

	int InnerBarHeight = ChartBarHeight - 2;
	int InnerBarWidth = ChartBarWidth - 2;

	int CO2BarHeight = InnerBarHeight * ( CurrentCO2Concentration / MaximumTotalCO2Cocentration );
	int CO2Offset = InnerBarHeight - CO2BarHeight;

	FillRectangle( StartPosX , StartPosY , InnerBarWidth , CO2Offset , ColorWhite );
	if( CurrentCO2Concentration > TresholdCO2Concentration ) {
		int Difference = InnerBarHeight - CO2Offset - MaximumCO2BarHeight;
		FillRectangle( StartPosX , StartPosY + CO2Offset , InnerBarWidth , Difference , ColorRed );
		FillRectangle( StartPosX , StartPosY + CO2Offset + Difference , InnerBarWidth , MaximumCO2BarHeight , ColorGreen );
	} else {
		FillRectangle( StartPosX , StartPosY + CO2Offset , InnerBarWidth , CO2BarHeight , ColorGreen );
	}

	String CO2Value = "";
	CO2Value += CurrentCO2Concentration;
	DrawStringMessageCenter( CO2Value , StartPosX , StartPosX + InnerBarWidth , StartPosY + InnerBarHeight - 24 , ColorBlack , ColorGreen );

	int LoudnessBarHeight = InnerBarHeight * ( CurrentLoudness / MaximumTotalLoudness );
	int LoudnessOffset = InnerBarHeight - LoudnessBarHeight;

	StartPosX = ChartBarWidth + (2*ChartBarMargin) + 1;
	FillRectangle( StartPosX , StartPosY , InnerBarWidth , LoudnessOffset , ColorWhite );
	if( CurrentLoudness > TresholdLoudness ) {
		int Difference = InnerBarHeight - LoudnessOffset - MaximumLoudnessBarHeight;
		FillRectangle( StartPosX , StartPosY + LoudnessOffset , InnerBarWidth , Difference , ColorRed );
		FillRectangle( StartPosX , StartPosY + LoudnessOffset + Difference , InnerBarWidth , MaximumLoudnessBarHeight , ColorGreen );
	} else {
		FillRectangle( StartPosX , StartPosY + LoudnessOffset , InnerBarWidth , LoudnessBarHeight , ColorGreen );
	}

	String LoudnessValue = "";
	LoudnessValue += CurrentLoudness;
	DrawStringMessageCenter( LoudnessValue , StartPosX , StartPosX + InnerBarWidth , StartPosY + InnerBarHeight - 24 , ColorBlack , ColorGreen );
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
	} else if( command.indexOf( "C:Device:Reset;") != -1 ) {
		client.println( "{\"State\":\"OK\"}" );
		client.flush();
		client.stop();
		delay( 10 * 1000 );
	} else if( command.indexOf( "C:Device:Info;") != -1 ) {
		processInfoCommand( client );
	} else {
		client.println( "{\"State\":\"Unknown Command\"}" );
		client.flush();
		client.stop();
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
	client.flush();
	client.stop();
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
		dataString += TresholdLoudness;
		dataString += "\"";
	}
	dataString += "}" ;
	client.println( dataString );
	client.flush();
	client.stop();
}

// Processing the command to set a treshold
void processSetTresholdCommand( bool CO2 , bool Loudness , String value , YunClient client ) {
	if( CO2 == true ) {
	 	TresholdCO2Concentration = value.toInt();
	 	MaximumCO2BarHeight = ((float)TresholdCO2Concentration/MaximumTotalCO2Cocentration) * ChartBarHeight;
	 	EepromWriteInt( CO2Cell , TresholdCO2Concentration );
	}
	if( Loudness == true ) {
		TresholdLoudness = value.toInt();
		MaximumLoudnessBarHeight = ((float)TresholdLoudness/MaximumTotalLoudness) * ChartBarHeight;
		EepromWriteInt( LoudnessCell , TresholdLoudness );
	}
	processGetTresholdCommand( CO2 , Loudness , client );
}

void processInfoCommand( YunClient client ){
	String dataString = "{\"FirmwareVersion\":\"";
	dataString += FirmwareVersion;
	dataString += "\",\"FirmwareDate\":\"";
	dataString += FirmwareDate;
	dataString += "\"}";
	client.println( dataString );
	client.flush();
	client.stop();
}


void drawProgressBar( unsigned char percentage ) {
	FillRectangle( 50 , 170 , int( float(percentage) * 1.4 ) , 10 , ColorBlue );
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

void EepromWriteInt( int address , int value ) {
	int currentValue = EepromReadInt( value );
	if( value != currentValue ) {
		byte lowByte = ((value >> 0) & 0xFF);
		byte highByte = ((value >> 8) & 0xFF);
		eeprom_write_byte((unsigned char *) address, lowByte);
		eeprom_write_byte((unsigned char *) address, highByte);
	}
}

unsigned int EepromReadInt( int address ) {
	byte lowByte = eeprom_read_byte((unsigned char *) address);
	byte highByte = eeprom_read_byte((unsigned char *) address);
	return ((lowByte << 0) & 0xFF) + ((highByte << 8) & 0xFF00);
}

void WatchdogEnable() {
	wdt_enable( 9 );
}

void WatchdogDisable( ) {
	wdt_reset();
	wdt_disable( );
}

void WatchdogReset( ) {
	wdt_reset( );
}

void SendCommand( uchar Command ){
	DATA_COMMAND_LOW;
	SLAVE_SELECT_LOW;
	SPI.transfer( Command );
	SLAVE_SELECT_HIGH;
}

void SendData( uchar Data ){
	DATA_COMMAND_HIGH;
	SLAVE_SELECT_LOW;
	SPI.transfer( Data );
	SLAVE_SELECT_HIGH;
}

void SendDataInt( uint Data ){
	uchar DataHigh = Data >> 8;
	uchar DataLow = Data & 0xFF;
	DATA_COMMAND_HIGH;
	SLAVE_SELECT_LOW;
	SPI.transfer( DataHigh );
	SPI.transfer( DataLow );
	SLAVE_SELECT_HIGH;
}

void SetColumn( uint StartColumn , uint EndColumn ){
	SendCommand( 0x2A );
	SendDataInt( StartColumn );
	SendDataInt( EndColumn );
}

void SetPage( uint StartPage , uint EndPage ){
	SendCommand( 0x2B );
	SendDataInt( StartPage );
	SendDataInt( EndPage );
}

void Init( ){
    SPI.begin();
    
    SLAVE_SELECT_HIGH
    DATA_COMMAND_HIGH;

    delay(500);
    SendCommand(0x01);
    delay(200);

    SendCommand(0xCF);
    SendData(0x00);
    SendData(0x8B);
    SendData(0X30);

    SendCommand(0xED);
    SendData(0x67);
    SendData(0x03);
    SendData(0X12);
    SendData(0X81);

    SendCommand(0xE8);
    SendData(0x85);
    SendData(0x10);
    SendData(0x7A);

    SendCommand(0xCB);
    SendData(0x39);
    SendData(0x2C);
    SendData(0x00);
    SendData(0x34);
    SendData(0x02);

    SendCommand(0xF7);
    SendData(0x20);

    SendCommand(0xEA);
    SendData(0x00);
    SendData(0x00);

    // Power Control
    SendCommand(0xC0);
    SendData(0x1B);   

    // Power Control
    SendCommand(0xC1);
    SendData(0x10);   

    // VCM Control
    SendCommand(0xC5);
    SendData(0x3F);
    SendData(0x3C);

    // VCM Control 2
    SendCommand(0xC7);
    SendData(0XB7);

    // Memory Access Control
    SendCommand(0x36);
    SendData(0x08);

    SendCommand(0x3A);
    SendData(0x55);

    SendCommand(0xB1);
    SendData(0x00);
    SendData(0x1B);

    // Display Function Control
    SendCommand(0xB6);
    SendData(0x0A);
    SendData(0xA2);

    // 3Gamma Function Disable
    SendCommand(0xF2);
    SendData(0x00);

    // Gamma Curve Selected
    SendCommand(0x26);
    SendData(0x01);

    // Set Gamma
    SendCommand(0xE0);
    SendData(0x0F);
    SendData(0x2A);
    SendData(0x28);
    SendData(0x08);
    SendData(0x0E);
    SendData(0x08);
    SendData(0x54);
    SendData(0XA9);
    SendData(0x43);
    SendData(0x0A);
    SendData(0x0F);
    SendData(0x00);
    SendData(0x00);
    SendData(0x00);
    SendData(0x00);

    // Set Gamma
    SendCommand(0XE1);
    SendData(0x00);
    SendData(0x15);
    SendData(0x17);
    SendData(0x07);
    SendData(0x11);
    SendData(0x06);
    SendData(0x2B);
    SendData(0x56);
    SendData(0x3C);
    SendData(0x05);
    SendData(0x10);
    SendData(0x0F);
    SendData(0x3F);
    SendData(0x3F);
    SendData(0x0F);

    // Exit Sleep
    SendCommand(0x11);
    delay(120);
    // Display On
    SendCommand(0x29);
}

void Clear(){
	SetColumn( SCREEN_MIN_X , SCREEN_MAX_X );
	SetPage( SCREEN_MIN_Y , SCREEN_MAX_Y );
	SendCommand( 0x2C );

	DATA_COMMAND_HIGH;
	SLAVE_SELECT_LOW;
	for( uint i = 0 ; i < 38400 ; i++ ) {
		SPI.transfer( 255 );
		SPI.transfer( 255 );
		SPI.transfer( 255 );
		SPI.transfer( 255 );
	}
	SLAVE_SELECT_HIGH;
}

void DrawHorizontalLine( uint poX , uint poY , uint length , uint color ) {
	SetColumn( poX , poX + length );
	SetPage( poY , poY );
	SendCommand( 0x2c );
	DATA_COMMAND_HIGH;
	SLAVE_SELECT_LOW;
	for( int i = 0 ; i < length ; i++ ) {
		uchar DataHigh = color >> 8;
		uchar DataLow = color & 0xFF;
		SPI.transfer( DataHigh );
		SPI.transfer( DataLow );
	}
	SLAVE_SELECT_HIGH;
}

void DrawVerticalLine( uint poX , uint poY , uint length , uint color ) {
	SetColumn( poX , poX );
	SetPage( poY , poY + length );
	SendCommand( 0x2c );
	DATA_COMMAND_HIGH;
	SLAVE_SELECT_LOW;
	for( int i = 0 ; i < length ; i++ ) {
		uchar DataHigh = color >> 8;
		uchar DataLow = color & 0xFF;
		SPI.transfer( DataHigh );
		SPI.transfer( DataLow );
	}
	SLAVE_SELECT_HIGH;
}

void DrawRectangle( uint poX , uint poY , uint length , uint width , uint color ) {
	DrawHorizontalLine( poX , poY , length , color );
	DrawHorizontalLine( poX , poY + width , length + 1, color );
	DrawVerticalLine( poX , poY , width , color );
	DrawVerticalLine( poX + length , poY , width , color );
}

void FillRectangle( uint x , uint y , uint width , uint height , uint fillColor ){

	SetColumn( x , x + width );
	SetPage( y , y + height );
	SendCommand( 0x2C );

	uchar HighColor = fillColor >> 8;
	uchar LowColor = fillColor & 0xff;
	ulong Pixels = ( width + 1 ) * ( height + 1 );

	DATA_COMMAND_HIGH;
	SLAVE_SELECT_LOW;

	for( ulong i =0 ; i < Pixels ; i++ ) {
		SPI.transfer( HighColor );
		SPI.transfer( LowColor );
		SPI.transfer( HighColor );
		SPI.transfer( LowColor );
	}

	SLAVE_SELECT_HIGH;
}

uchar DrawChar( uchar ASCII , uint x , uint y , uint color , uint background ) {

	ASCII = ASCII - 48;

	uchar CharWidth = numbers[ ASCII ][ 0 ];
	uchar WidthWithSpacing = numbers[ ASCII ][ 1 ];

	SetColumn( x , x + WidthWithSpacing - 1 );
	SetPage( y , y + 13 );
	SendCommand( 0x2C );

	for( byte lines = 0 ; lines < 13 ; lines++ ) {
		uchar currentByte = numbers[ ASCII ][ 2 + lines ];
		for( byte width = 0 ; width < CharWidth ; width++ ) {
			if( bitRead( currentByte , 7 - width ) ) {
				SendDataInt( color );
			} else {
				SendDataInt( background );
			}
		}
		for( byte width = 0 ; width < (WidthWithSpacing - CharWidth) ; width++ ) {
			SendDataInt( background );
		}
	}

	return WidthWithSpacing;
}

uchar DrawCharFromArray( uchar Char[] , uint x , uint y , uint color , uint background ) {

	uchar CharWidth = Char[ 0 ];
	uchar WidthWithSpacing = Char[ 1 ];

	SetColumn( x , x + WidthWithSpacing - 1 );
	SetPage( y , y + 13 );
	SendCommand( 0x2C );

	for( byte lines = 0 ; lines < 13 ; lines++ ) {
		uchar currentByte = Char[ 2 + lines ];
		for( byte width = 0 ; width < CharWidth ; width++ ) {
			if( bitRead( currentByte , 7 - width ) ) {
				SendDataInt( color );
			} else {
				SendDataInt( background );
			}
		}
		for( byte width = 0 ; width < (WidthWithSpacing - CharWidth) ; width++ ) {
			SendDataInt( background );
		}
	}
	return WidthWithSpacing;
}

void DrawString( char *string , uint x , uint y , uint color , uint background ) {
	while( *string ) {
		x += DrawChar( *string++ , x , y , color , background );
	}
}

void DrawStringMessage( String message , uint x , uint y , uint color , uint background ){
	message.toCharArray( DisplayBuffer , message.length() + 1 );
	DrawString( DisplayBuffer , x , y , color , background );
}

uchar DrawStringMessageCenter( String message , uint xStart , uint xEnd , uint y , uint color , uint background ){
	message.toCharArray( DisplayBuffer , message.length() + 1 );
	uchar width = GetWidth( DisplayBuffer );
	DrawString( DisplayBuffer , xStart + 1 + ( (xEnd - xStart) - width ) / 2 , y , color , background );
	return ( xStart + ( (xEnd - xStart) - width ) / 2 ) + width;
}

uchar GetWidth( char *string ){
	int totalWidth = 0;
	uchar width, widthTotal, xoffset;
	while( *string ) {
		uchar ASCII = *string++;
		ASCII = ASCII - 48;
		width = numbers[ ASCII ][ 0 ];
		widthTotal = numbers[ ASCII ][ 1 ];
		totalWidth += widthTotal;
	}
	totalWidth -= (widthTotal - width);
	return totalWidth;
}