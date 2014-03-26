#ifndef TouchScreen_H

	#define TouchScreen_H

	#define uchar uint8_t
	#define uint uint16_t
	#define ulong uint32_t

 	#include "Arduino.h"
	#include "avr/pgmspace.h"
	#include "SPI.h"

	#define TS_MIN_X 0
	#define TS_MIN_Y 0
	#define TS_MAX_X 239
	#define TS_MAX_Y 319

	#define TS_Y_POSITIVE A2
	#define TS_Y_NEGATIVE A0
	#define TS_X_POSITIVE A3
	#define TS_X_NEGATIVE A1

	#define TS_MINX 116 * 2
	#define TS_MAXX 890 * 2
	#define TS_MINY 83 * 2
	#define TS_MAXY 913 * 2

	#define	SAMPLES 2
	#define	COMPARE	2
	#define	RXPLATE	300

	#define TS_FONT_SPACING 2
	#define TS_PIXEL_BUFFER_SIZE 72
	#define TS_DISPLAY_WIDTH 240
	#define TS_DISPLAY_HEIGHT 320

	/*
	 * Fonts
	 */
	extern uchar font_helv_08[][12];

	class TouchScreen {
		private:
			char _displayBuffer[30];

		public:
			void setSlaveSelectHigh();
			void setSlaveSelectLow();
			void setDataCommandHigh();
			void setDataCommandLow();

			TouchScreen( void );
			void init( void );

			void clear();
			void clear( uint color );

			void setCol( uint StartCol , uint EndCol );
			void setColWithoutSelect( uint StartCol , uint EndCol );
			void setPage( uint StartPage , uint EndPage );
			void setPageWithoutSelect( uint StartPage , uint EndPage );
			void setXY( uint poX , uint poY );
			void setPixel( uint poX , uint poY , uint color );
			void sendCMD( uchar index );
			void sendCmdWithoutSelect( uchar index );
			void writePackage( uint *data , uchar howmany );
			void writeData( uchar data );
			void sendData( uint data );
			void sendDataWithoutSelect( uint data );
			uchar readRegister( uchar Addr , uchar xParameter );

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
			void drawTriangle(int poX1, int poY1, int poX2, int poY2, int poX3, int poY3, uint color);

			void reverseString( char str[] );
			void sleep();
			void wakeUp();

			/*
			 * 8px Font
			 */
			uint newLine8px();
			uchar getLineHeight8px();
			uchar drawChar8px( uchar ASCII , uint x , uint y , uint color , uint background );
			uchar drawChar8pxNoSpacing( uchar ASCII , uint x , uint y , uint color , uint background );
			uchar drawString8px( char *string , uint x , uint y , uint color , uint background );
			uchar drawString8px( String message , uint x , uint y , uint color , uint background );
			uchar drawStringCenter8px( char *string , uint y , uint color , uint background );
			uchar drawStringCenter8px( String message , uint xStart , uint xEnd , uint y , uint color , uint background );
			uchar drawStringCenter8px( char *string , uint xStart , uint xEnd , uint y , uint color , uint background );
			uchar drawStringCenter8px( String message , uint y , uint color , uint background );
			uchar getWidth8px( char *string );
	};

#endif