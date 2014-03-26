#ifndef EEPROMEX_h
#define EEPROMEX_h

#include <Arduino.h> 
#include <inttypes.h>
#include <avr/eeprom.h>

class EEPROMClassEx {
	  
  public:
	EEPROMClassEx();
		
    uint8_t read(int);	
    uint16_t readInt(int);
	bool write(int, uint8_t);
	bool writeInt(int, uint16_t);
	bool updateInt(int, uint16_t);
	
	template <class T> int updateBlock( int address , const T& value ) {
		int writeCount=0;
		const byte* bytePointer = (const byte*)(const void*)&value;
		for (unsigned int i = 0; i < (unsigned int)sizeof(value); i++) {
			if (read(address)!=*bytePointer) {
				write(address, *bytePointer);
				writeCount++;		
			}
			address++;
			bytePointer++;
		}
		return writeCount;
	}
};

extern EEPROMClassEx EEPROM;

#endif

