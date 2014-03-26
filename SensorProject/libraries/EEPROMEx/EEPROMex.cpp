#include "EEPROMex.h"

EEPROMClassEx::EEPROMClassEx(){
}

uint8_t EEPROMClassEx::read(int address) {
	return eeprom_read_byte( (unsigned char *) address );
}

uint16_t EEPROMClassEx::readInt(int address) {
	return eeprom_read_word( ( uint16_t * ) address );
}

bool EEPROMClassEx::write(int address, uint8_t value) {
	eeprom_write_byte((unsigned char *) address, value);
	return true;
}

bool EEPROMClassEx::writeInt(int address, uint16_t value) {
	eeprom_write_word( ( uint16_t * ) address , value );
	return true;
}

bool EEPROMClassEx::updateInt(int address, uint16_t value) {
	return( updateBlock<uint16_t>( address , value ) != 0 );
}

EEPROMClassEx EEPROM;
