#include "Bridge.h"
#include "YunServer.h"
#include "YunClient.h"
#include "EEPROM.h"

#if defined(__SAM3X8E__)
		#undef __FlashStringHelper::F(string_literal)
		#define F(string_literal) string_literal
#endif

YunServer server;

int maxLoudness;
int minAirQuality;

void setup() {
	maxLoudness = EEPROM.read( 0 );
	minAirQuality = EEPROM.read( 1 );

	// Starting the Bridge
	pinMode(13,OUTPUT);
	digitalWrite(13, LOW);
	Bridge.begin();
	digitalWrite(13, HIGH);

	// Starting the YunServer
	server.listenOnLocalhost();
	server.begin();

	randomSeed( analogRead(0) );
}

void loop() {
	YunClient client = server.accept();
	if( client ) {
		process( client );
		client.stop();
	}
	delay(50);
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
