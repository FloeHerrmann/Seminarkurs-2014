#include "pins_arduino.h"
#include "wiring_private.h"
#include "avr/pgmspace.h"
#include "TouchScreen.h"
#include "SPI.h"

void TouchScreen::setSlaveSelectHigh(){
	#if defined(__AVR_ATmega1280__) || defined(__AVR_ATmega2560__)
		DDRE |= 0x08;
		PORTE |=  0x08;
	#elif defined(__AVR_ATmega32U4__)
		DDRC |= 0x40;
		PORTC |=  0x40;
	#else
		DDRD |= 0x20;
		PORTD |=  0x20;
	#endif
}

void TouchScreen::setSlaveSelectLow(){
	#if defined(__AVR_ATmega1280__) || defined(__AVR_ATmega2560__)
		DDRE |= 0x08;
		PORTE &=~ 0x08;
	#elif defined(__AVR_ATmega32U4__)
		DDRC |= 0x40;
		PORTC &=~ 0x40;
	#else
		DDRD |= 0x20;
		PORTD &=~ 0x20;
	#endif
}

void TouchScreen::setDataCommandHigh(){
	#if defined(__AVR_ATmega1280__) || defined(__AVR_ATmega2560__)
		DDRH |= 0x08;
		PORTH |=  0x08;
	#elif defined(__AVR_ATmega32U4__)
		DDRD |= 0x80;
		PORTD |=  0x80;
	#else
		DDRD |= 0x40;
		PORTD |=  0x40;
	#endif
}

void TouchScreen::setDataCommandLow(){
	#if defined(__AVR_ATmega1280__) || defined(__AVR_ATmega2560__)
		DDRH |= 0x08;
		PORTH &=~ 0x08;
	#elif defined(__AVR_ATmega32U4__)
		DDRD |= 0x80;
		PORTD &=~ 0x80;
	#else
		DDRD |= 0x40;
		PORTD &=~ 0x40;
	#endif
}

void TouchScreen::sendCMD( uchar index ) {
	setDataCommandLow();
	setSlaveSelectLow();
	SPI.transfer(index);
	setSlaveSelectHigh();
}

void TouchScreen::writeData( uchar data ) {
	setDataCommandHigh();
	setSlaveSelectLow();
	SPI.transfer( data );
	setSlaveSelectHigh();
}

void TouchScreen::sendData( uint data ) {
	uchar data1 = data >> 8;
	uchar data2 = data & 0xff;
	setDataCommandHigh();
	setSlaveSelectLow();
	SPI.transfer( data1 );
	SPI.transfer( data2 );
	setSlaveSelectHigh();
}

uchar TouchScreen::readRegister( uchar Addr , uchar xParameter ) {
	uchar data = 0;
	sendCMD( 0xd9 );
	writeData( 0x10 + xParameter );
	setDataCommandLow();
	setSlaveSelectLow();
	SPI.transfer( Addr );
	setDataCommandHigh();
	data = SPI.transfer( 0 );
	setSlaveSelectHigh();
	return data;
}

TouchScreen::TouchScreen( void ) {
}

void TouchScreen::init (void) {

	SPI.begin();
	setSlaveSelectHigh();
	setDataCommandHigh();

	delay( 200 );
	sendCMD( 0x01 );
	delay( 200 );

	sendCMD( 0xCF );
	writeData( 0x00 );
	writeData( 0x8B );
	writeData( 0X30 );

	sendCMD( 0xED );
	writeData( 0x67 );
	writeData( 0x03 );
	writeData( 0X12 );
	writeData( 0X81 );

	sendCMD( 0xE8 );
	writeData( 0x85 );
	writeData( 0x10 );
	writeData( 0x7A );

	sendCMD( 0xCB );
	writeData( 0x39 );
	writeData( 0x2C );
	writeData( 0x00 );
	writeData( 0x34 );
	writeData( 0x02 );

	sendCMD( 0xF7 );
	writeData( 0x20 );

	sendCMD( 0xEA );
	writeData( 0x00 );
	writeData( 0x00 );

	sendCMD( 0xC0 );
	writeData( 0x1B );

	sendCMD( 0xC1 );
	writeData( 0x10 );

	sendCMD( 0xC5 );
	writeData( 0x3F );
	writeData( 0x3C );

	sendCMD( 0xC7 );
	writeData( 0XB7 );

	sendCMD( 0x36 );
	writeData( 0x08 );

	sendCMD( 0x3A );
	writeData( 0x55 );

	sendCMD( 0xB1 );
	writeData( 0x00 );
	//writeData( 0x1B );
	writeData( 0x13 );

	sendCMD( 0xB6 );
	writeData( 0x0A );
	writeData( 0xA2 );


	sendCMD( 0xF2 );
	writeData( 0x00 );

	sendCMD( 0x26 );
	writeData( 0x01 );

	sendCMD( 0xE0 );
	writeData( 0x0F );
	writeData( 0x2A );
	writeData( 0x28 );
	writeData( 0x08 );
	writeData( 0x0E );
	writeData( 0x08 );
	writeData( 0x54 );
	writeData( 0XA9 );
	writeData( 0x43 );
	writeData( 0x0A );
	writeData( 0x0F );
	writeData( 0x00 );
	writeData( 0x00 );
	writeData( 0x00 );
	writeData( 0x00 );

	sendCMD( 0XE1 );
	writeData( 0x00 );
	writeData( 0x15 );
	writeData( 0x17 );
	writeData( 0x07 );
	writeData( 0x11 );
	writeData( 0x06 );
	writeData( 0x2B );
	writeData( 0x56 );
	writeData( 0x3C );
	writeData( 0x05 );
	writeData( 0x10 );
	writeData( 0x0F );
	writeData( 0x3F );
	writeData( 0x3F );
	writeData( 0x0F );

	sendCMD( 0x11 );
	delay( 120 );
	sendCMD( 0x29 );
	clear();
}

void TouchScreen::clear( ) {
	fillScreen( );
}

void TouchScreen::clear( uint color ) {
	fillScreen( TS_MIN_X , TS_MAX_X , TS_MIN_Y , TS_MAX_Y , color );
}

void TouchScreen::setCol( uint StartCol , uint EndCol ) {
	sendCMD( 0x2A );
	sendData( StartCol );
	sendData( EndCol );
}

void TouchScreen::setPage( uint StartPage , uint EndPage ) {
	sendCMD( 0x2B );
	sendData( StartPage );
	sendData( EndPage );
}

void TouchScreen::setXY( uint poX , uint poY ){
	setCol( poX , poX );
	setPage( poY , poY );
	sendCMD( 0x2c );
}

void TouchScreen::setPixel( uint poX , uint poY , uint color ) {
	setXY( poX , poY );
	sendData( color );
}

void TouchScreen::fillScreen(void) {
	setCol( 0 , 239 );
	setPage( 0 , 319 );
	sendCMD( 0x2c );
	
	setDataCommandHigh();
	setSlaveSelectLow();
	for( uint i = 0 ; i < 38400 ; i++ ) {
		SPI.transfer( 255 );
		SPI.transfer( 255 );
		SPI.transfer( 255 );
		SPI.transfer( 255 );
	}
	setSlaveSelectHigh();
}

void TouchScreen::sendCmdWithoutSelect( uchar index ) {
	setDataCommandLow();
	SPI.transfer( index );
}

void TouchScreen::sendDataWithoutSelect( uint data ) {
	uchar data1 = data >> 8;
	uchar data2 = data & 0xff;
	setDataCommandHigh();
	SPI.transfer( data1 );
	SPI.transfer( data2 );
}

void TouchScreen::setColWithoutSelect( uint StartCol , uint EndCol ) {
	sendCmdWithoutSelect( 0x2A );
	sendDataWithoutSelect( StartCol );
	sendDataWithoutSelect( EndCol );
}

void TouchScreen::setPageWithoutSelect( uint StartPage , uint EndPage ) {
	sendCmdWithoutSelect( 0x2B );
	sendDataWithoutSelect( StartPage );
	sendDataWithoutSelect( EndPage );
}

void TouchScreen::fillRectangle( uint start , uint height , uint color ) {
	setSlaveSelectLow();

	setColWithoutSelect( 0 , 239 );
	setPageWithoutSelect( start , start+height );
	sendCmdWithoutSelect( 0x2c );

	uchar Hcolor = color >> 8;
	uchar Lcolor = color & 0xff;
	ulong pixels = (240 * (height+1) ) / 2;
	// sendCMD( 0x2C )
	sendCmdWithoutSelect( 0x2C );
	setDataCommandHigh();
	for( ulong i = 0 ; i < pixels ; i++ ) {
		SPI.transfer( Hcolor );
		SPI.transfer( Lcolor );
		SPI.transfer( Hcolor );
		SPI.transfer( Lcolor );
	}
	setSlaveSelectHigh();
}

void TouchScreen::fillScreen( uint XL , uint XR , uint YU , uint YD , uint color ) {
	ulong XY = 0;

	if( XL > XR ) {
		XL = XL ^ XR;
		XR = XL ^ XR;
		XL = XL ^ XR;
	}

	if( YU > YD ) {
		YU = YU ^ YD;
		YD = YU ^ YD;
		YU = YU ^ YD;
	}

	XL = constrain( XL , TS_MIN_X , TS_MAX_X );
	XR = constrain( XR , TS_MIN_X , TS_MAX_X );
	YU = constrain( YU , TS_MIN_Y , TS_MAX_Y );
	YD = constrain( YD , TS_MIN_Y , TS_MAX_Y );

	XY = ( XR - XL + 1 );
	XY = XY * ( YD - YU + 1 );

	setSlaveSelectLow();

	setColWithoutSelect( XL , XR );
	setPageWithoutSelect( YU , YD );
	sendCmdWithoutSelect( 0x2c );

	setDataCommandHigh();

	uchar Hcolor = color >> 8;
	uchar Lcolor = color & 0xff;
	for( ulong i =0 ; i < XY ; i++ ) {
		SPI.transfer( Hcolor );
		SPI.transfer( Lcolor );
	}

	setSlaveSelectHigh();
}

void TouchScreen::fillRectangle( uint poX , uint poY , uint length , uint width , uint color ) {
	fillScreen( poX , poX + length , poY , poY + width , color );
}

void  TouchScreen::drawHorizontalLine( uint poX , uint poY , uint length , uint color ) {
	setSlaveSelectLow();
	setColWithoutSelect( poX , poX + length );
	setPageWithoutSelect( poY , poY );
	sendCmdWithoutSelect( 0x2c );
	for( int i = 0 ; i < length ; i++ ) sendDataWithoutSelect( color );
	setSlaveSelectHigh();
}

void TouchScreen::drawVerticalLine( uint poX , uint poY , uint length , uint color ) {
	setSlaveSelectLow();
	setColWithoutSelect( poX , poX );
	setPageWithoutSelect( poY , poY + length );
	sendCmdWithoutSelect( 0x2c );
	for( int i = 0 ; i < length ; i++ ) sendData( color );
	setSlaveSelectHigh();
}

void TouchScreen::drawLine( uint x0 , uint y0 , uint x1 , uint y1 , uint color ) {
	int x = x1 - x0;
	int y = y1 - y0;
	int dx = abs( x ), sx = x0 < x1 ? 1 : -1;
	int dy = -abs( y ), sy = y0 < y1 ? 1 : -1;
	int err = dx + dy , e2;
	for (;;){
		setPixel( x0 , y0 , color );
		e2 = 2 * err;
		if( e2 >= dy ) {
			if( x0 == x1 ) break;
			err += dy; x0 += sx;
		}
		if( e2 <= dx ) {
			if( y0 == y1 ) break;
			err += dx; y0 += sy;
		}
	}
}

void TouchScreen::drawRectangle( uint poX , uint poY , uint length , uint width , uint color ) {
	drawHorizontalLine( poX , poY , length , color );
	drawHorizontalLine( poX , poY + width , length + 1, color );
	drawVerticalLine( poX , poY , width , color );
	drawVerticalLine( poX + length , poY , width , color );
}

void TouchScreen::drawCircle( int poX , int poY , int r , uint color ) {
	int x = -r, y = 0, err = 2-2*r, e2;
	do {
		setPixel(poX-x, poY+y,color);
		setPixel(poX+x, poY+y,color);
		setPixel(poX+x, poY-y,color);
		setPixel(poX-x, poY-y,color);
		e2 = err;
		if (e2 <= y) {
			err += ++y*2+1;
			if (-x == y && e2 <= x) e2 = 0;
		}
		if (e2 > x) err += ++x*2+1;
	} while (x <= 0);
}

void TouchScreen::fillCircle( int poX , int poY , int r , uint color ) {
	int x = -r, y = 0, err = 2 - 2 * r, e2;
	do {
		drawVerticalLine( poX - x , poY - y , 2 * y , color );
		drawVerticalLine( poX + x , poY - y , 2 * y , color );
		e2 = err;
		if( e2 <= y ) {
			err += ++y * 2 + 1;
			if( -x == y && e2 <= x ) e2 = 0;
		}
		if( e2 > x ) err += ++x * 2 + 1;
	} while( x <= 0 );
}

void TouchScreen::drawTriangle( int poX1 , int poY1 , int poX2 , int poY2 , int poX3 , int poY3 , uint color ) {
	drawLine( poX1 , poY1 , poX2 , poY2 , color );
	drawLine( poX1 , poY1 , poX3 , poY3 , color );
	drawLine( poX2 , poY2 , poX3 , poY3 , color );
}

uchar TouchScreen::drawChar8px( uchar ASCII , uint x , uint y , uint color , uint background ) {

	if( (ASCII < 32) || (ASCII>127) ) ASCII = '?' - 32;
	else ASCII = ASCII - 32;

	uchar charWidth = pgm_read_byte( &font_helv_08[ ASCII ][ 0 ] );
	uchar charHeight = pgm_read_byte( &font_helv_08[ ASCII ][ 1 ] );
	uchar charData = pgm_read_byte( &font_helv_08[ ASCII ][ 2 ] );
	uchar widthWithSpacing = pgm_read_byte( &font_helv_08[ ASCII ][ 3 ] );
	uchar xoffset = pgm_read_byte( &font_helv_08[ ASCII ][ 4 ] );
	uchar yoffset = pgm_read_byte( &font_helv_08[ ASCII ][ 5 ] );
	uchar bytesPerLine = charData / charHeight;

	if( xoffset == 254 ) { x -= 1; }
	else if( xoffset == 253 ) { x -= 2; }
	else if( xoffset == 252 ) { x -= 3; }
	else if( xoffset == 251 ) { x -= 4; }
	else { x += xoffset; }

	setCol( x , x + widthWithSpacing - 1 );
	setPage( y , y + 13 );
	sendCMD( 0x2C );

	uchar currentByte;

	for( uint height = 0 ; height < charHeight ; height++ ) {
		uint tempWidth = charWidth;
		uint tempBits = charWidth;
		for( uint bytes = 0 ; bytes < bytesPerLine ; bytes++ ) {
			currentByte = pgm_read_byte( &font_helv_08[ ASCII ][ 6 + height * bytesPerLine + bytes ] );
			if( tempWidth > 8 ) {
				tempBits = 8;
				tempWidth -= 8;
			} else {
				tempBits = tempWidth;
			}
			for( uint w = 0 ; w < tempBits ; w++ ) {
				if( bitRead( currentByte , 7 - w ) ) {
					sendData( color );
				} else {
					sendData( background );
				}
			}
			if( (bytes+1) == bytesPerLine ) {
				for( uint width = 0 ; width < (widthWithSpacing - charWidth) ; width++ ) {
					sendData( background );
				}
			}
		}
	}

	if( xoffset == 254 ) return widthWithSpacing - 1;
	if( xoffset == 253 ) return widthWithSpacing - 2;
	if( xoffset == 252 ) return widthWithSpacing - 3;
	if( xoffset == 251 ) return widthWithSpacing - 4;
	else return widthWithSpacing + xoffset;
}

uchar TouchScreen::drawChar8pxNoSpacing( uchar ASCII , uint x , uint y , uint color , uint background ) {

	if( (ASCII < 32) || (ASCII>127) ) ASCII = '?' - 32;
	else ASCII = ASCII - 32;

	uchar charWidth = pgm_read_byte( &font_helv_08[ ASCII ][ 0 ] );
	uchar charHeight = pgm_read_byte( &font_helv_08[ ASCII ][ 1 ] );
	uchar charData = pgm_read_byte( &font_helv_08[ ASCII ][ 2 ] );
	uchar widthWithSpacing = pgm_read_byte( &font_helv_08[ ASCII ][ 3 ] );
	uchar xoffset = pgm_read_byte( &font_helv_08[ ASCII ][ 4 ] );
	uchar yoffset = pgm_read_byte( &font_helv_08[ ASCII ][ 5 ] );
	uchar bytesPerLine = charData / charHeight;

	if( xoffset == 254 ) { x -= 1; }
	else if( xoffset == 253 ) { x -= 2; }
	else if( xoffset == 252 ) { x -= 3; }
	else if( xoffset == 251 ) { x -= 4; }
	else { x += xoffset; }

	if( yoffset == 254 ) { y -= 1; }
	else if( yoffset == 253 ) { y -= 2; }
	else if( yoffset == 252 ) { y -= 3; }
	else if( yoffset == 251 ) { y -= 4; }
	else { y += yoffset; }

	// setCol( x , x + width - 1 );
	sendCmdWithoutSelect( 0x2A );
	sendDataWithoutSelect( x );
	sendDataWithoutSelect( x + charWidth - 1 );

	// setPage( y , y + height - 1 );
	sendCmdWithoutSelect( 0x2B );
	sendDataWithoutSelect( y );
	sendDataWithoutSelect( y + charHeight - 1 );

	sendCmdWithoutSelect( 0x2C );

	uchar currentByte;
	uchar currentByte02;
	uchar currentBit;

	for( uint height = 0 ; height < charHeight ; height++ ) {
		uint tempWidth = charWidth;
		uint tempBits = charWidth;
		for( uint bytes = 0 ; bytes < bytesPerLine ; bytes++ ) {
			currentByte = pgm_read_byte( &font_helv_08[ ASCII ][ 6 + height * bytesPerLine + bytes ] );
			if( tempWidth > 8 ) {
				tempBits = 8;
				tempWidth -= 8;
			} else {
				tempBits = tempWidth;
			}
			for( uint w = 0 ; w < tempBits ; w++ ) {
				if( bitRead( currentByte , 7 - w ) ) {
					sendDataWithoutSelect( color );
				} else {
					sendDataWithoutSelect( background );
				}
            }
    	}
    }

	if( xoffset == 254 ) return widthWithSpacing - 1;
	if( xoffset == 253 ) return widthWithSpacing - 2;
	if( xoffset == 252 ) return widthWithSpacing - 3;
	if( xoffset == 251 ) return widthWithSpacing - 4;
	else return widthWithSpacing + xoffset;
}

uchar TouchScreen::drawString8px( char *string , uint x , uint y , uint color , uint background ) {
	while( *string ) {
		if( x < TS_DISPLAY_WIDTH ) {
			x += drawChar8px( *string++ , x , y , color , background );
		}
	}
	return x;
}

uchar TouchScreen::drawString8px( String message , uint x , uint y , uint color , uint background ){
	message.toCharArray( _displayBuffer , message.length() + 1 );
	drawString8px( _displayBuffer , x , y , color , background );
}

uchar TouchScreen::drawStringCenter8px( char *string , uint y , uint color , uint background ){
	uchar width = getWidth8px( string );
	drawString8px( string , ( TS_DISPLAY_WIDTH - width ) / 2 , y , color , background );
}

uchar TouchScreen::drawStringCenter8px( String message , uint xStart , uint xEnd , uint y , uint color , uint background ){
	message.toCharArray( _displayBuffer , message.length() + 1 );
	drawStringCenter8px( _displayBuffer , xStart , xEnd , y , color , background );
}

uchar TouchScreen::drawStringCenter8px( char *string , uint xStart , uint xEnd , uint y , uint color , uint background ){
	uchar width = getWidth8px( string );
	drawString8px( string , xStart + ( (xEnd - xStart) - width ) / 2 , y , color , background );
}

uchar TouchScreen::drawStringCenter8px( String message , uint y , uint color , uint background ){
	message.toCharArray( _displayBuffer , message.length() + 1 );
	uchar width = getWidth8px( _displayBuffer );
	drawString8px( _displayBuffer , ( TS_DISPLAY_WIDTH - width ) / 2 , y , color , background );
}

uchar TouchScreen::getWidth8px( char *string ){
	int totalWidth = 0;
	uchar width, widthTotal, xoffset;
	while( *string ) {
		uchar ASCII = *string++;
		if( (ASCII < 32) || (ASCII>127) ) ASCII = '?' - 32;
		else ASCII = ASCII - 32;
		width = pgm_read_byte( &font_helv_08[ ASCII ][ 0 ] );
		widthTotal = pgm_read_byte( &font_helv_08[ ASCII ][ 3 ] );
		xoffset = pgm_read_byte( &font_helv_08[ ASCII ][ 4 ] );

		totalWidth += xoffset + widthTotal;
	}
	totalWidth -= (widthTotal - width);
	return totalWidth;
}

void TouchScreen::sleep(){
	sendCMD( 0x28 );
}

void TouchScreen::wakeUp(){
	sendCMD( 0x29 );
}