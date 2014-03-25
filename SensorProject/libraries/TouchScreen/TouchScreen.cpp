#include "pins_arduino.h"
#include "wiring_private.h"
#include <avr/pgmspace.h>
#include <Point.h>
#include <TouchScreen.h>
#include <SPI.h>

#define			SAMPLES					2
#define			COMPARE					2
#define			RXPLATE					300

const uchar		TS_FONT_SPACING			= 2;
const uchar		TS_PIXEL_BUFFER_SIZE	= 72;
const uchar		TS_TOUCH_PRESSURE		= 10;
const uint		TS_DISPLAY_WIDTH		= 240;
const uint		TS_DISPLAY_HEIGHT		= 320;

/*
 * Output Messages
 */
 /*
char MSG_OK[]							= "OK";
char MSG_FAILED[]						= "FAILED";
char MSG_ERROR[]						= "ERROR";
char MSG_FILE_NOT_FOUND[] 				= "FILE NOT FOUND";
char MSG_FILE_NOT_OPEN[] 				= "FILE NOT OPEN";
char MSG_BUFFER_TO_SMALL[] 				= "BUFFER TO SMALL";
char MSG_SEEK_ERROR[] 					= "SEEK ERROR";
char MSG_SECTION_NOT_FOUND[] 			= "SECTION NOT FOUND";
char MSG_KEY_NOT_FOUND[] 				= "KEY NOT FOUND";
char MSG_END_OF_FILE[] 					= "END OF FILE";
char MSG_UNKNOWN_ERROR[] 				= "UNKNOWN ERROR";
char MSG_UNKNOWN_ERROR_VALUE[] 			= "UNKNOWN ERROR VALUE";*/

/*
 * Function for POINT class
 */
Point::Point( void ) {
	_x = _y = 0;
}

Point::Point( int x0 , int y0 , int z0 ) {
	_x = x0;
	_y = y0;
	_z = z0;
}

bool Point::operator == ( Point p1 ) {
	return ( ( p1._x == _x ) && ( p1._y == _y ) && ( p1._z == _z ) );
}

bool Point::operator!=(Point p1) { 
	return ( ( p1._x != _x ) || ( p1._y != _y ) || ( p1._z != _z ) );
}

/*
 * Functions for TOUCHSCREEN class
 */

Point TouchScreen::getPoint( void ) {
	int x = 1;
	int y = 1;
	int z = 1;
	int samples[ SAMPLES ];
	uchar i, valid;

	uchar xp_port = digitalPinToPort( _xp );
	uchar yp_port = digitalPinToPort( _yp );
	uchar xm_port = digitalPinToPort( _xm );
	uchar ym_port = digitalPinToPort( _ym );

	uchar xp_pin = digitalPinToBitMask( _xp );
	uchar yp_pin = digitalPinToBitMask( _yp );
	uchar xm_pin = digitalPinToBitMask( _xm );
	uchar ym_pin = digitalPinToBitMask( _ym );

	valid = 1;
	pinMode( _yp , INPUT );
	pinMode( _ym , INPUT );

	*portOutputRegister( yp_port ) &= ~yp_pin;
	*portOutputRegister( ym_port ) &= ~ym_pin;

	pinMode( _xp , OUTPUT );
	pinMode( _xm , OUTPUT );

	*portOutputRegister( xp_port ) |= xp_pin;
	*portOutputRegister( xm_port ) &= ~xm_pin;

	for( i=0 ; i < SAMPLES; i++ ) {
		samples[i] = analogRead(_yp);
	}

	int icomp = samples[0] > samples[1] ? samples[0] - samples[1] : samples[1] - samples[0];
	if( icomp > COMPARE ) valid = 0;

	x = ( samples[0] + samples[1] );

	pinMode( _xp , INPUT );
	pinMode( _xm , INPUT );
	*portOutputRegister( xp_port ) &= ~xp_pin;

	pinMode( _yp , OUTPUT );
	*portOutputRegister( yp_port ) |= yp_pin;
	pinMode( _ym , OUTPUT );

	for( i=0 ; i < SAMPLES ; i++ ) {
		samples[i] = analogRead(_xm);
	}

	icomp = samples[0] > samples[1] ? samples[0] - samples[1] : samples[1] - samples[0];
	if( icomp > COMPARE ) valid = 0;

	y = (samples[0]+samples[0]);
	 
	pinMode(_xp, OUTPUT);
	 
	*portOutputRegister(xp_port) &= ~xp_pin;
	*portOutputRegister(ym_port) |=  ym_pin;
	*portOutputRegister(yp_port) &= ~yp_pin;
	
	pinMode(_yp, INPUT);

	int z1 = analogRead(_xm);
	int z2 = analogRead(_yp);
 
	float rtouch = 0;

	rtouch  = z2;
	rtouch /= z1;
	rtouch -= 1;
	rtouch *= (2046-x)/2;
	rtouch *= RXPLATE;
	rtouch /= 1024;
	z = rtouch;
	if (! valid) {
		z = 0;
	}

	return Point(x, y, z);
}

bool TouchScreen::isTouched( void ) {
	Point p = getPoint();
	if( p._z > TS_TOUCH_PRESSURE ) return true;
	return false;
}

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

void TouchScreen::backlightOn() {
	#if defined(__AVR_ATmega1280__) || defined(__AVR_ATmega2560__)
		DDRH |= 0x10;
		PORTH |=  0x10;
	#elif defined(__AVR_ATmega32U4__)
		DDRE |= 0x40;
		PORTE |=  0x40;
	#else
		DDRD |= 0x80;
		PORTD |=  0x80;
	#endif
}

void TouchScreen::backlightOff() {
	#if defined(__AVR_ATmega1280__) || defined(__AVR_ATmega2560__)
		DDRH |= 0x10;
		PORTH &=~ 0x10;
	#elif defined(__AVR_ATmega32U4__)
		DDRE |= 0x40;
		PORTE &=~ 0x40;
	#else
		DDRD |= 0x80;
		PORTD &=~ 0x80;
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

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

TouchScreen::TouchScreen( uchar xp , uchar xm , uchar yp , uchar ym ) {
	_yp = yp;
	_xm = xm;
	_ym = ym;
	_xp = xp;
}

void TouchScreen::init (void) {

	_lineHeight8px = 13;
	_lineHeight14px = 24;
	_lineHeight18px = 26;
	_lineHeight30px = 36;
	_displayState = 1;

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

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

void TouchScreen::clear( ) {
	fillScreen( );
	_currentLine = 0;
}

void TouchScreen::clear( uint color ) {
	fillScreen( 0 , 239 , 0 , 319 , color );
	_currentLine = 0;
}

void TouchScreen::clearContent( uint color ){
	setCol( _contentLeft , TS_DISPLAY_WIDTH - _contentRight );
	setPage( _contentTop , TS_DISPLAY_HEIGHT - _contentBottom );
	sendCMD( 0x2C );
	
	setDataCommandHigh();
	setSlaveSelectLow();
	ulong pixelAmount = ( (TS_DISPLAY_WIDTH - _contentRight) - _contentLeft ) * ( (TS_DISPLAY_HEIGHT - _contentBottom ) - _contentTop);
	for( uint i = 0 ; i < (pixelAmount/2) ; i++ ) {
		SPI.transfer( 255 );
		SPI.transfer( 255 );
		SPI.transfer( 255 );
		SPI.transfer( 255 );
	}
	setSlaveSelectHigh();
	_currentyPos = _contentTop;
}

void TouchScreen::clearBottom( uint color ){
	fillScreen( 0 , 240 , 290 , TS_DISPLAY_HEIGHT , color );
}

uint TouchScreen::newLine14px(){
	_currentyPos += _lineHeight14px;
	return _currentyPos;
}

uchar TouchScreen::getLineHeight14px(){
	return _lineHeight14px;
}
/*
uchar TouchScreen::getLineHeight18px(){
	return _lineHeight18px;
}
*/
uint TouchScreen::currentLine(){
	return _currentyPos;
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

void TouchScreen::setContentArea( uint top , uint right , uint bottom , uint left ) {
	_contentTop = top;
	_currentyPos = top;
	_contentRight = right;
	_contentBottom = bottom;
	_contentLeft = left;
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

void TouchScreen::drawTraingle( int poX1 , int poY1 , int poX2 , int poY2 , int poX3 , int poY3 , uint color ) {
	drawLine( poX1 , poY1 , poX2 , poY2 , color );
	drawLine( poX1 , poY1 , poX3 , poY3 , color );
	drawLine( poX2 , poY2 , poX3 , poY3 , color );
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

uchar TouchScreen::drawStringLeft14px( char *string , uint y , uint color , uint background ) {
	drawString14px( string , _contentLeft , y , color , background );
}

uchar TouchScreen::drawStringLeft14px( String message , uint y , uint color , uint background ){
	message.toCharArray( _displayBuffer , message.length() + 1 );
	drawString14px( _displayBuffer , _contentLeft , y , color , background );
}

uchar TouchScreen::drawStringCenter14px( char *string , uint y , uint color , uint background ) {
	uchar width = getWidth14px( string );
	drawString14px( string , ( TS_DISPLAY_WIDTH - width ) / 2 , y , color , background );
}

uchar TouchScreen::drawStringRight14px( char *string , uint y , uint color , uint background ){
	uchar width = getWidth14px( string );
	drawString14px( string , TS_DISPLAY_WIDTH - _contentLeft - width , y , color , background );
}

uchar TouchScreen::drawStringRight14px( String message , uint y , uint color , uint background ){
	message.toCharArray( _displayBuffer , message.length() + 1 );
	uchar width = getWidth14px( _displayBuffer );
	drawString14px( _displayBuffer , TS_DISPLAY_WIDTH - _contentLeft - width , y , color , background );
}

uchar TouchScreen::getWidth14px( char *string ){
	int totalWidth = 0;
	uchar width, widthTotal, xoffset;
	while( *string ) {
		uchar ASCII = *string++;
		if( (ASCII < 32) || (ASCII>127) ) ASCII = '?' - 32;
		else ASCII = ASCII - 32;
		width = pgm_read_byte( &font_helv_14[ ASCII ][ 0 ] );
		widthTotal = pgm_read_byte( &font_helv_14[ ASCII ][ 3 ] );
		xoffset = pgm_read_byte( &font_helv_14[ ASCII ][ 4 ] );

		totalWidth += xoffset + widthTotal;
	}
	totalWidth -= (widthTotal - width);
	return totalWidth;
}

uchar TouchScreen::drawString14px( char *string , uint x , uint y , uint color , uint background ) {
	//setSlaveSelectLow();
	while( *string ) {
		if( x < TS_DISPLAY_WIDTH ) {
			x += drawChar14px( *string++ , x , y , color , background );
		}
	}
	//setSlaveSelectHigh();
	return x;
}

uchar TouchScreen::drawChar14px( uchar ASCII , uint x , uint y , uint color , uint background ) {

	if( (ASCII < 32) || (ASCII>127) ) ASCII = '?' - 32;
	else ASCII = ASCII - 32;

	uchar charWidth = pgm_read_byte( &font_helv_14[ ASCII ][ 0 ] );
	uchar charHeight = pgm_read_byte( &font_helv_14[ ASCII ][ 1 ] );
	uchar charData = pgm_read_byte( &font_helv_14[ ASCII ][ 2 ] );
	uchar widthWithSpacing = pgm_read_byte( &font_helv_14[ ASCII ][ 3 ] );
	uchar xoffset = pgm_read_byte( &font_helv_14[ ASCII ][ 4 ] );
	uchar yoffset = pgm_read_byte( &font_helv_14[ ASCII ][ 5 ] );
	uchar bytesPerLine = charData / charHeight;

	if( xoffset == 254 ) { x -= 1; }
	else if( xoffset == 253 ) { x -= 2; }
	else if( xoffset == 252 ) { x -= 3; }
	else if( xoffset == 251 ) { x -= 4; }
	else { x += xoffset; }

	setCol( x , x + widthWithSpacing - 1 );
	/*
	sendCmdWithoutSelect( 0x2A );
	sendDataWithoutSelect( x );
	sendDataWithoutSelect( x + widthWithSpacing - 1 );
	*/
	
	setPage( y , y + _lineHeight30px - 1 );
	/*
	sendCmdWithoutSelect( 0x2B );
	sendDataWithoutSelect( y );
	sendDataWithoutSelect( y + _lineHeight14px - 1 );
	*/

	sendCMD( 0x2C );
	//sendCmdWithoutSelect( 0x2C );

	uchar currentByte;

	for( uint height = 0 ; height < charHeight ; height++ ) {
		uint tempWidth = charWidth;
		uint tempBits = charWidth;
		for( uint bytes = 0 ; bytes < bytesPerLine ; bytes++ ) {
			currentByte = pgm_read_byte( &font_helv_14[ ASCII ][ 6 + height * bytesPerLine + bytes ] );
			if( tempWidth > 8 ) {
				tempBits = 8;
				tempWidth -= 8;
			} else {
				tempBits = tempWidth;
			}
			for( uint w = 0 ; w < tempBits ; w++ ) {
				if( bitRead( currentByte , 7 - w ) ) {
					sendData( color );
					//sendDataWithoutSelect( color );
				} else {
					sendData( background );
					//sendDataWithoutSelect( background );
				}
			}
			if( (bytes+1) == bytesPerLine ) {
				for( uint width = 0 ; width < (widthWithSpacing - charWidth) ; width++ ) {
					sendData( background );
					//sendDataWithoutSelect( background );
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

void TouchScreen::sleep(){
	_displayState = 2;
	backlightOff();
	sendCMD(0x28);
}

void TouchScreen::wakeUp(){
	_displayState = 1;
	sendCMD(0x29);
	backlightOn();
}

uint TouchScreen::touchPressure(){
	return TS_TOUCH_PRESSURE;
}

bool TouchScreen::isOn() {
	if( _displayState == 1 ) return true;
	return false;
}

bool TouchScreen::isOff(){
	if( _displayState == 0 ) return true;
	return false;
}

bool TouchScreen::isSleeping(){
	if( _displayState == 2 ) return true;
	return false;
}

void TouchScreen::drawIcon( uint iconData[] , uint x , uint y , uint color , uint background ){
	setSlaveSelectLow();

	uint iconWidth = iconData[ 0 ];
	uint iconHeight = iconData[ 1 ];

	// setCol( x , x + width - 1 );
	sendCmdWithoutSelect( 0x2A );
	sendDataWithoutSelect( x );
	sendDataWithoutSelect( x + iconWidth - 1 );
	
	// setPage( y , y + height - 1 );
	sendCmdWithoutSelect( 0x2B );
	sendDataWithoutSelect( y );
	sendDataWithoutSelect( y + iconHeight - 1 );

	sendCmdWithoutSelect( 0x2C );

	for( uint height = 0 ; height < iconHeight ; height++ ) {
		uint currentByte = iconData[ 2 + height ];
		for( uint width = 0 ; width < iconWidth ; width++ ) {
			if( bitRead( currentByte , (iconWidth-1) - width ) ) {
				sendDataWithoutSelect( color );
			} else {
				sendDataWithoutSelect( background );
			}
		}
	}
	setSlaveSelectHigh();
}
