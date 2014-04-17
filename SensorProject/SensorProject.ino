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
	{8,10, 60,126,102,195,195,195,195,195,195,195,102,126,60}, // 0
	{5,7,  24,248,248,24,24,24,24,24,24,24,24,24,24}, // 1
	{8,10, 60,254,195,3,7,14,28,56,112,224,192,255,255}, // 2
	{8,10, 62,127,195,195,6,28,30,7,3,195,199,126,60}, // 3
	{8,10, 6,14,30,54,102,198,198,255,255,6,6,6,6,6}, // 4
	{8,10, 254,254,192,192,252,254,199,3,3,195,199,254,124}, // 5
	{8,10, 60,127,99,192,192,220,254,195,195,195,227,126,60}, // 6
	{8,10, 255,255,3,6,12,12,24,24,48,48,96,96,96}, // 7
	{8,10, 60,126,231,195,195,102,126,231,195,195,231,126,60}, // 8
	{8,10, 60,126,199,195,195,195,127,59,3,3,198,254,124}, // 9
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

#define MaximumTotalLoudness 1023
#define MaximumTotalCO2Cocentration 5000

// Data for display

#define BarMarginX 10
#define BarMarginY 40
#define BarHeight 240
#define BarWidth 105

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

// Object to access the CO2 sensor
SoftwareSerial CO2Sensor( CO2SensorRx , CO2SensorTx );

// Command sequence to read data from CO2 Sensor
const byte CO2SensorRead[] = { 0xFE , 0X44 , 0X00 , 0X08 , 0X02 , 0X9F , 0X25 };

char DisplayBuffer[12];

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
	MaximumCO2BarHeight = ((float)TresholdCO2Concentration/(float)MaximumTotalCO2Cocentration) * BarHeight;
	drawProgressBar( 20 );

	// Load maximum loudness treshold from EEPROm
	TresholdLoudness = EepromReadInt( LoudnessCell );
	// When no threshold is defined, set default
	if( TresholdLoudness == -1 ) TresholdLoudness = 200;
	MaximumLoudnessBarHeight = ((float)TresholdLoudness/(float)MaximumTotalLoudness) * BarHeight;
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

	// "Luft" Heading
	int PosX = BarMarginX + (( BarWidth - 35 )/2);
	PosX += DrawCharFromArray( Chars[0] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[1] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[2] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[3] , PosX , 16 , ColorBlack , ColorWhite );

	// "Lautstärke" Heading
	PosX = ( 2 * BarMarginX + BarWidth ) + (( BarWidth - 90 )/2);
	PosX += DrawCharFromArray( Chars[0] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[4] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[1] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[3] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[5] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[3] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[6] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[7] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[8] , PosX , 16 , ColorBlack , ColorWhite );
	PosX += DrawCharFromArray( Chars[9] , PosX , 16 , ColorBlack , ColorWhite );

	// Frame for the CO2 Concentration bar
	DrawRectangle( BarMarginX , BarMarginY , BarWidth , BarHeight , ColorBlack );

	// Frame for the loudness bar
	DrawRectangle( 2 * BarMarginX + BarWidth , BarMarginY , BarWidth , BarHeight , ColorBlack );

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
		delay( 100 );
		// Connection will be closed after the timeout
		// client.setTimeout( 5000 );
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
	// CO2 Concentration
	drawBar( BarMarginX + 1 , BarMarginY + 1 , MaximumCO2BarHeight , TresholdCO2Concentration , CurrentCO2Concentration , MaximumTotalCO2Cocentration );

	// Loudness
	drawBar( 2 * BarMarginX + BarWidth + 1 , BarMarginY + 1 , MaximumLoudnessBarHeight , TresholdLoudness , CurrentLoudness , MaximumTotalLoudness );
}

void drawBar( uint x , uint y , uint MaxBarHeight , uint Threshold , uint Value , uint MaxValue ) {
	WatchdogReset();
	// Loudness
	uint CurrentValue = Value;
	// If current loudness is negativ, set to 0
	if( CurrentValue < 0 ) CurrentValue = 0;
	// Calculate the height of the Bar that matches the current loudness
	uint CurrentBarHeight = (BarHeight - 2) * ( (float)CurrentValue/(float)MaxValue );
	// Check if current loudness is higher than the defined treshold
	if( CurrentValue > Threshold ) {
		FillRectangle( x , y , (BarWidth - 2) , (BarHeight - 2) - CurrentBarHeight , ColorWhite );
		FillRectangle( x , y + ((BarHeight - 2) - CurrentBarHeight) , (BarWidth - 2) , CurrentBarHeight - MaxBarHeight , ColorRed );
		FillRectangle( x , y + ((BarHeight - 2) - MaxBarHeight) + 1 , (BarWidth - 2) , MaxBarHeight - 1, ColorGreen );
	} else {
		FillRectangle( x , y , (BarWidth - 2) , (BarHeight - 2) - MaxBarHeight - 1 , ColorWhite );
		FillRectangle( x , y + ((BarHeight - 2) - MaxBarHeight) + 1 , (BarWidth - 2) , MaxBarHeight - CurrentBarHeight , ColorWhite );
		FillRectangle( x , y + ((BarHeight - 2) - CurrentBarHeight) , (BarWidth - 2) , CurrentBarHeight , ColorGreen );
	}
	// Show the current treshold as a horizontal line
	DrawHorizontalLine( x , y + ((BarHeight - 2) - MaxBarHeight) , BarWidth - 1 , ColorRed );
	// Show current loudness
	String StringValue = "";
	StringValue += Value;
	FillRectangle( x , SCREEN_MAX_Y - 24 , x + BarWidth - 1 , 20 , ColorWhite );
	DrawStringMessageCenter( StringValue , x , x + (BarWidth - 2) , SCREEN_MAX_Y - 24 , ColorBlack , ColorWhite );
}

// Processing the received command
void processCommand( String command , YunClient client ) {
	WatchdogReset();
	if( command.equals( "C:Data:Get;" ) ) {
		processDataCommand( true , true , true , client );
	} else if( command.equals( "C:Data:Get:CO2Concentration;" ) ) {
		processDataCommand( true , false , false , client );
	} else if( command.equals( "C:Data:Get:Loudness;" ) ) {
		processDataCommand( false , true , false , client );
	} else if( command.equals( "C:Data:Get:Temperature;" ) ) {
		processDataCommand( false , false , true , client );
	} else if( command.equals( "C:Device:Reset;" ) ) {
		client.println( "{\"State\":\"OK\"}" );
		client.flush();
		client.stop();
		delay( 10 * 1000 );
	} else if( command.equals( "C:Device:Info;" ) ) {
		client.println( "{\"FirmwareVersion\":\"1.3\",\"FirmwareDate\":\"04.04.2014 13:34\"}" );
		client.flush();
		client.stop();
	} else if( command.equals( "C:Threshold:Get;" ) ) {
		processGetTresholdCommand( true , true , client );
	} else if( command.equals( "C:Threshold:Get:Loudness;" ) ) {
		processGetTresholdCommand( false , true , client );
	} else if( command.equals( "C:Threshold:Get:CO2Concentration;" ) ) {
		processGetTresholdCommand( true , false , client );
	} else if( command.indexOf( "C:Threshold:Set:Loudness:" ) != -1 ) {
		processSetTresholdCommand( false , true , command.substring( command.lastIndexOf( ":" ) + 1 , command.lastIndexOf( ";" ) ) , client );
		updateDisplay();
	} else if( command.indexOf( "C:Threshold:Set:CO2Concentration:" ) != -1 ) {
		processSetTresholdCommand( true , false , command.substring( command.lastIndexOf( ":" ) + 1 , command.lastIndexOf( ";" ) ) , client );
		updateDisplay();
	} else {
		client.println( "{\"State\":\"Unknown Command\"}" );
	}
}

// Processing the command to get data
void processDataCommand( bool CO2 , bool Loudness , bool Temperature , YunClient client ) {
	WatchdogReset();
	client.print( "{" );
	if ( CO2 == true) {
		client.print( "\"CO2Concentration\":\"" );
		client.print( CurrentCO2Concentration );
		client.print( "\"" );
		if( Loudness == true || Temperature == true ) client.print( "," );
	}
	if( Loudness == true ) {
		client.print( "\"Loudness\":\"" );
		client.print( CurrentLoudness );
		client.print( "\"" );
		if( Temperature == true ) client.print( "," );
	}
	if( Temperature == true ){
		client.print( "\"Temperature\":\"" );
		client.print( CurrentTemperature );
		client.print( "\"" );
	}
	client.println( "}" );
	client.flush();
	client.stop();
}

// Processing the command to get a treshold
void processGetTresholdCommand( bool CO2 , bool Loudness , YunClient client ) {
	WatchdogReset();
	client.print( "{" );
	if( CO2 == true ) {
		client.print( "\"CO2Treshold\":\"" );
		client.print( TresholdCO2Concentration );
		client.print( "\"" );
		if( Loudness == true ) client.print( "," );
	} else if( Loudness == true ) {
		client.print( "\"LoudnessTreshold\":\"" );
		client.print( TresholdLoudness );
		client.print( "\"" );
	}
	client.println( "}" );
	client.flush();
	client.stop();
}

// Processing the command to set a treshold
void processSetTresholdCommand( bool CO2 , bool Loudness , String value , YunClient client ) {
	WatchdogReset();
	int intValue = value.toInt();
	client.print( "{" );
	if( CO2 == true ) {
	 	TresholdCO2Concentration = intValue;
	 	MaximumCO2BarHeight = ((float)TresholdCO2Concentration/(float)MaximumTotalCO2Cocentration) * BarHeight;
	 	EepromWriteInt( CO2Cell , TresholdCO2Concentration );
	 	client.print( "\"CO2Treshold\":\"" );
		client.print( TresholdCO2Concentration );
		client.print( "\"" );
	} else if( Loudness == true ) {
		TresholdLoudness = intValue;
		MaximumLoudnessBarHeight = ((float)TresholdLoudness/(float)MaximumTotalLoudness) * BarHeight;
		EepromWriteInt( LoudnessCell , TresholdLoudness );
		client.print( "\"LoudnessTreshold\":\"" );
		client.print( TresholdLoudness );
		client.print( "\"" );
	}
	client.println( "}" );
	client.flush();
	client.stop();
}

void drawProgressBar( unsigned char percentage ) {
	WatchdogReset();
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
int getCO2Concentration(  ){
	int RetryCounter = 0;
	while( CO2Sensor.available() == 0 ) {
		CO2Sensor.write( CO2SensorRead , 7 );
		if( RetryCounter++ > 20 ) return CurrentCO2Concentration;
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
		while( CO2Sensor.available() > 0 ) {
			CO2Sensor.read();
		}
		return CurrentCO2Concentration;
	}
}

void EepromWriteInt( int address , int value ) {
	int currentValue = EepromReadInt( value );
	if( value != currentValue ) {
		byte lowByte = ((value >> 0) & 0xFF);
		byte highByte = ((value >> 8) & 0xFF);
		eeprom_write_byte((unsigned char *) address , lowByte );
		eeprom_write_byte((unsigned char *) address + 1, highByte );
	}
}

unsigned int EepromReadInt( int address ) {
	byte lowByte = eeprom_read_byte( (unsigned char *) address );
	byte highByte = eeprom_read_byte( (unsigned char *) address + 1 );
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
	uchar ASCII;
	while( *string ) {
		ASCII = *string++;
		ASCII -= 48;
		x += DrawCharFromArray( numbers[ASCII] , x , y , color , background );
	}
}

void DrawStringMessageCenter( String message , uint xStart , uint xEnd , uint y , uint color , uint background ){
	message.toCharArray( DisplayBuffer , message.length() + 1 );
	uchar width = GetWidth( DisplayBuffer );
	DrawString( DisplayBuffer , xStart + 1 + ( (xEnd - xStart) - width ) / 2 , y , color , background );
}

uchar GetWidth( char *string ){
	uchar totalWidth = 0;
	while( *string ) {
		uchar ASCII = *string++;
		ASCII = ASCII - 48;
		totalWidth += numbers[ ASCII ][ 1 ];
	}
	totalWidth -= 2;
	return totalWidth;
}