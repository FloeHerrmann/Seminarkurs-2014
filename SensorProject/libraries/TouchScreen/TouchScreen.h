#ifndef TouchScreen_H

	#define TouchScreen_H

	#define uchar uint8_t
	#define uint uint16_t
	#define ulong uint32_t

 	#include <Arduino.h>x
	#include <avr/pgmspace.h>
	#include <SPI.h>
	#include <SD.h>

	#define TS_RED				0xf800
	#define TS_GREEN			0x07e0
	#define TS_GREEN_DARK		0xA7e0
	#define TS_BLUE				0x001f
	#define TS_BLACK			0x0000
	#define TS_YELLOW			0xffe0
	#define TS_WHITE			0xffff
	#define TS_CYAN				0x07ff
	#define TS_BRIGHT_RED		0xf810
	#define TS_GRAY1			0x8410
	#define TS_GRAY2			0x4208

	#define TS_MIN_X			0
	#define TS_MIN_Y			0
	#define TS_MAX_X			239
	#define TS_MAX_Y			319

	#define TS_Y_POSITIVE 		A2	// Must be an analog pin
	#define TS_Y_NEGATIVE 		54	// Can be a digital pin
	#define TS_X_POSITIVE 		57	// Can be a digital pin
	#define TS_X_NEGATIVE 		A1	// Must be an analog pin

	#define TS_MINX 			116 * 2
	#define TS_MAXX 			890 * 2
	#define TS_MINY 			83 * 2
	#define TS_MAXY 			913 * 2

	/*
	 * Fonts
	 */
	extern uchar font_helv_08[][24];
	extern uchar font_helv_14[][47];
	extern uchar font_fub_30[][160];

	class Point {
		public:
			int _x , _y , _z;

		public:
			Point( void );
			Point( int x , int y , int z );
			bool operator == ( Point );
			bool operator != ( Point );
	};

	class TouchScreen {
		private:
			uchar _yp , _ym , _xm , _xp;
			uchar _currentLine;
			uchar _lineHeight;
			uchar _lineHeight8px;
			uchar _lineHeight14px;
			uchar _lineHeight18px;
			uchar _lineHeight30px;
			uchar _contentTop;
			uint _currentyPos;
			uchar _contentRight;
			uchar _contentBottom;
			uchar _contentLeft;
			uchar _displayState;
			char _displayBuffer[100];

		public:
			static const uchar TS_MSG_OK 						= 0;
			static const uchar TS_MSG_FAILED 					= 1;
			static const uchar TS_MSG_ERROR 					= 2;
			static const uchar TS_MSG_FILE_NOT_FOUND 			= 3;
			static const uchar TS_MSG_FILE_NOT_OPEN 			= 4;
			static const uchar TS_MSG_BUFFER_TO_SMALL 			= 5;
			static const uchar TS_MSG_SEEK_ERROR 				= 6;
			static const uchar TS_MSG_SECTION_NOT_FOUND 		= 7;
			static const uchar TS_MSG_KEY_NOT_FOUND 			= 8;
			static const uchar TS_MSG_END_OF_FILE 				= 9;
			static const uchar TS_MSG_UNKNOWN_ERROR 			= 10;
			static const uchar TS_MSG_UNKNOWN_ERROR_VALUE 		= 11;

			void setSlaveSelectHigh();
			void setSlaveSelectLow();
			void setDataCommandHigh();
			void setDataCommandLow();

			TouchScreen( uchar xp , uchar xm , uchar yp , uchar ym );
			void init( void );

			void clear();
			void clear( uint color );
			void clearContent( uint color );
			void clearBottom( uint color );

			void backlightOn();
			void backlightOff();

			void setCol( uint StartCol , uint EndCol );
			void setColWithoutSelect( uint StartCol , uint EndCol );
			void setPage( uint StartPage , uint EndPage );
			void setPageWithoutSelect( uint StartPage , uint EndPage );
			void setXY( uint poX , uint poY );
			void setPixel( uint poX , uint poY , uint color );
			void setContentArea( uint top , uint right , uint bottom , uint left );
			void sendCMD( uchar index );
			void sendCmdWithoutSelect( uchar index );
			void writePackage( uint *data , uchar howmany );
			void writeData( uchar data );
			void sendData( uint data );
			void sendDataWithoutSelect( uint data );
			uchar readRegister( uchar Addr , uchar xParameter );
			
			uint currentLine();

			void fillScreen(uint XL,uint XR,uint YU,uint YD,uint color);
			void fillScreen( void );
			void fillRectangle( uint start , uint height , uint color );
			void fillRectangle( uint poX, uint poY, uint length, uint width, uint color);
			void fillCircle(int poX, int poY, int r,uint color);

			void drawLine(uint x0,uint y0,uint x1,uint y1,uint color);
			void drawVerticalLine(uint poX, uint poY,uint length,uint color);
			void drawHorizontalLine(uint poX, uint poY,uint length,uint color);
			void drawRectangle(uint poX, uint poY, uint length,uint width,uint color);
			void drawCircle(int poX, int poY, int r,uint color);
			void drawTraingle(int poX1, int poY1, int poX2, int poY2, int poX3, int poY3, uint color);

			bool isTouched(void);
			Point getPoint();
			void reverseString( char str[] );
			void sleep();
			void wakeUp();
			uint touchPressure();

			bool isOn();
			bool isOff();
			bool isSleeping();

			/*
			 * 14px Font
			 */
			uint newLine14px();
			uchar getLineHeight14px();
			uchar drawChar14px( uchar ASCII , uint x , uint y , uint color , uint background );
			uchar drawString14px( char *string , uint x , uint y , uint color , uint background );
			uchar drawStringLeft14px( char *string , uint y , uint color , uint background );
			uchar drawStringLeft14px( String message , uint y , uint color , uint background );
			uchar drawStringCenter14px( char *string , uint y , uint color , uint background );
			uchar drawStringRight14px( char *string , uint y , uint color , uint background );
			uchar drawStringRight14px( String message , uint y , uint color , uint background );
			uchar getWidth14px( char *string );
			
			void drawIcon( uint iconData[] , uint x , uint y , uint color , uint background );
	};

#endif