#ifndef WATCHDOG_H
	#define WATCHDOG_H

	extern "C" { 
		#include <avr/wdt.h>
	}

	// watchdog times that can be used
	#define WATCHDOG_15MS 0
	#define WATCHDOG_30MS 1
	#define WATCHDOG_60MS 2
	#define WATCHDOG_120MS 3
	#define WATCHDOG_250MS 4
	#define WATCHDOG_500MS 5
	#define WATCHDOG_1S 6
	#define WATCHDOG_2S 7
	#define WATCHDOG_4S 8
	#define WATCHDOG_8S 9

	class WatchdogTimer {

		public:

		    /*
			 * Enable watchdog with given time, must be called as first.
			 */
			void Enable( int watchdogTime );
			
		    /*
			 * Disable watchdog
			 */
		    void Disable( );

			/*
			 * Reset watchdog. Will be initialized with the {watchdogTime} again
			 */
			void Reset( );
			
	};

#endif




