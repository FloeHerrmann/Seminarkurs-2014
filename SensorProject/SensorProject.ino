#include "Bridge.h"
#include "YunServer.h"
#include "YunClient.h"
#include "EEPROM.h"
#include "SPI.h"
#include "TouchScreen.h"

#if defined(__SAM3X8E__)
		#undef __FlashStringHelper::F(string_literal)
		#define F(string_literal) string_literal
#endif

const uint FontColorBlack = 0x0000; // #000000
const uint FontColorWhite = 0xFFFF; // #FFFFFF
const uint FontColorLightRed = 0xE5B5;
const uint FontColorRed = 0xD9A2; // #DA3610
const uint FontColorBlue = 0x0339; // #0066CC
const uint FontColorDarkGray = 0x20E4;
const uint FontColorLightGray = 0x8410;
const uint FontColorDarkGreen = 0x07E0;
const uint FontColorGreen = 0x858E;
const uint FontColorCyan = 0x6679;
const uint FontColorLightPurple = 0xB415; // B380A9
const uint FontColorPurple = 0x826F; // 814E78

const uint FillColorBlack = 0x0000; // #000000
const uint FillColorWhite = 0xFFFF; // #FFFFFF
const uint FillColorLightRed = 0xE5B5;
const uint FillColorRed = 0xD9A2; // #DA3610
const uint FillColorBlue = 0x0339; // #0066CC
const uint FillColorDarkGray = 0x20E4;
const uint FillColorLightGray = 0x8410;
const uint FillColorDarkGreen = 0x07E0;
const uint FillColorGreen = 0x858E;
const uint FillColorCyan = 0x6679;
const uint FillColorLightPurple = 0xB415; // B380A9
const uint FillColorPurple = 0x826F; // 814E78

YunServer server;

int currentLoudness;
int lastLoudness;
int currentAirQuality;
int lastAirQuality;

int maxLoudness;
int minAirQuality;

int chartBarHeight = 240;
int chartBarWidth = 90;
int chartBarMargin = 2;

// Alle 10 Sekunden eine Messung durchführen
long sampleTime = 10 * 1000;
long lastSample;

TouchScreen Display = TouchScreen( A0 , A1 , A2 , A3 );

void setup() {
	Display.init();
	Display.clear();

	maxLoudness = EEPROM.read( 0 );
	minAirQuality = EEPROM.read( 1 );

	// Starting the Bridge
	Display.drawString14px( "Starting Bridge..." , 5 , 10 , FontColorBlack , FontColorWhite );
	Bridge.begin();
	Display.drawString14px( "Starting Bridge...OK" , 5 , 10 , FontColorBlack , FontColorWhite );

	// Starting the YunServer
	Display.drawString14px( "Starting Server..." , 5 , 30 , FontColorBlack , FontColorWhite );
	server.listenOnLocalhost();
	server.begin();
	Display.drawString14px( "Starting Server...OK" , 5 , 30 , FontColorBlack , FontColorWhite );

	Display.clear();
	Display.fillRectangle( 0 , 0 , 240 , 30 , FillColorBlue );
	Display.drawStringCenter14px( "Air-Loudness-Monitoring" , 5 , FontColorWhite, FillColorBlue );

	Display.drawRectangle( 20 - chartBarMargin , 50 - chartBarMargin , chartBarWidth + 2 * chartBarMargin , chartBarHeight + 2 * chartBarMargin , FontColorBlack );
	Display.drawRectangle( 130 - chartBarMargin , 50 - chartBarMargin , chartBarWidth + 2 * chartBarMargin , chartBarHeight + 2 * chartBarMargin , FontColorBlack );

	Display.drawString14px( "Air" , 164 , 294 , FontColorBlack , FillColorWhite );
	Display.drawString14px( "Loudness" , 26 , 294 , FontColorBlack , FillColorWhite );

	randomSeed( analogRead(0) );
	
	currentLoudness = random( 30 , 130 );
	currentAirQuality = random( 0 , 2000 );
	lastLoudness = 0;
	lastAirQuality = 0;
	displayMeasurement();

	// millis() = Vergangene Millisekunden seit dem Start
	lastSample = millis();
}

void loop() {
	YunClient client = server.accept();
	if( client ) {
		process( client );
		client.stop();
	}
	
	// millis() - lastSample = Zeit seit der letzten Messung
	if( millis() - lastSample > sampleTime ){
		currentLoudness = random( 30 , 130 );
		currentAirQuality = random( 0 , 2000 );
		displayMeasurement();
		lastSample = millis();
	}
}

void displayMeasurement(){
	float loudnessPercentage = currentLoudness / 130.0;
	int loudnessHeight = chartBarHeight * loudnessPercentage;
	int loudnessOffset = chartBarHeight - loudnessHeight;

	float airQualityPercentage = currentAirQuality / 2000.0;
	int airQualityHeight = chartBarHeight * airQualityPercentage;
	int airQualityOffset = chartBarHeight - airQualityHeight;

	if( lastLoudness < currentLoudness ) {
		Display.fillRectangle(
			20, 
			50 + loudnessOffset , 
			chartBarWidth,
			chartBarHeight - loudnessOffset,
			FontColorDarkGreen
		);
	} else {
		Display.fillRectangle(
			20, 
			50, 
			chartBarWidth,
			loudnessOffset,
			FontColorWhite
		);
	}

	if( lastAirQuality < currentAirQuality ) {
		Display.fillRectangle(
			130, 
			50 + airQualityOffset , 
			chartBarWidth,
			chartBarHeight - airQualityOffset,
			FontColorDarkGreen
		);
	} else {
		Display.fillRectangle(
			130, 
			50, 
			chartBarWidth,
			airQualityOffset,
			FontColorWhite
		);
	}
	lastLoudness = currentLoudness;
	lastAirQuality = currentAirQuality;
}

void process(YunClient client) {
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
	client.println("Status: 200");
 	client.println("Content-type: application/json");
 	client.println();
	if( message == "loudness" ) {
		Console.println( "LOUDNESS" );
		client.print( F( "{\"loudness\":\"") );
		client.print( random( 30 , 130 ) );
		client.println( F( "\"}") );
	}
	if ( message == "airquality" ) {
		Console.println( "AIR QUALITY" );
		client.print( F( "{\"airQuality\":\"0.") );
		client.print( random( 0 , 100 ) );
		client.println( F( "\"}") );
	}
	if ( message != "loudness" && message != "airquality" ){
		Console.println( "LOUDNESS AND AIR QUALITY" );
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
	client.println("Status: 200");
 	client.println("Content-type: application/json");
 	client.println();
	if( message == "set" ) {
		String value = client.readStringUntil( '/' );
	} else if ( message == "get") {
		client.print( F( "{\"maxLoudness\":\"") );
		client.print( maxLoudness );
		client.println( F( "\"}") );
	}
}

void messageAirQualityCommand( YunClient client ) {
	String message = client.readStringUntil('/');
	message.trim();
	client.println("Status: 200");
 	client.println("Content-type: application/json");
 	client.println();
	if( message == "set" ) {
		String value = client.readStringUntil( '/' );
	} else if ( message == "get") {
		client.print( F( "{\"minAirQuality\":\"") );
		client.print( minAirQuality );
		client.println( F( "\"}") );
	}
}
