#include <WatchdogTimer.h>

void WatchdogTimer::Enable( int watchdogTime ) {
	wdt_enable( watchdogTime );
}

void WatchdogTimer::Disable( ) {
	wdt_reset();
	wdt_disable( );
}

void WatchdogTimer::Reset( ) {
	wdt_reset( );
}