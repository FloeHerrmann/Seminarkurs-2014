# Seminarkurs-2014

Luft- und Lautstärkeüberwachung

## Datenbank

Die Datenbankschemas befinden sich in den Dateien

* `Datenbankschema-MySQL.sql` für MySQL Datenbanken
* `Datenbankschema-PostgreSQL.sql` für PostgreSQL Datenbanken

Zum aufsetzen der Datenbank müssen 2 Schritte ausgeführt werden

1. Ausführen des `CREATE DATABASE` Statements.
2. Ausführen der `CREATE TABLE` Statements. Hierbei ist darauf zu achten, das die Reihenfolge der Statements eingehalten wird.

Alternativ kann zum erstellen der Datenbank bzw. der Tabellen eine grafische Oberfläche verwendet werden.

## C# Anwendung

### Daten abfragen

Beispiel zum senden eines Kommandos und zum entfangen einer Antwort: https://gist.github.com/FloeHerrmann/b5b00f89f3a8ccb7b036

Um das Programm zum abfragen der Daten regelmäßig auszuführen sollte eine entsprechende Aufgabe erstellt werden:

`SCHTASKS /Create /SC MINUTE /MO 5 /SD 03/30/2014 /ST 10:00:00 /TN ALM-Data-Service /TR C:\Service\ALM-Data-Service.exe`

SCHTASKS Dokumentation: http://msdn.microsoft.com/en-us/library/windows/desktop/bb736357%28v=vs.85%29.aspx

## Arduino

Mit der neusten Version der Arduino Software wird nicht mehr die REST Schnittstelle des Arduino verwendent, sondern eigens definierte Kommandos: 

### Daten abfragen

* `C:Data:Get;` Liefert Informationen zum Sensor;
* `C:Data:Get;` Liefert die aktuellen Werte des Sensors
* `C:Data:Get:CO2Concentration;` Liefert die aktuelle CO2 Konzentration
* `C:Data:Get:Loudness;` Liefert die aktuelle Lautstärke
* `C:Data:Get:Temperature;` Liefert die aktuelle Temperatur

### Schwellenwert abfragen

* `C:Threshold:Get;` Liefert die Schwellwerte für die CO2 Konzentration und die Lautstärke
* `C:Threshold:Get:Loudness;` Liefert den Schwellwert für die Lautstärke
* `C:Threshold:Get:CO2Concentration;` Liefert den Schwellwert für die CO2 Konzentration

### Schwellenwert setzen

* `C:Threshold:Set:Loudness:{Value};` Setzt den Schwellwert für die Lautstärke auf den Wert {Value}
* `C:Threshold:Set:CO2Concentration:{Value};` Setzt den Schwellwert für die CO2 Konzentration auf den Wert {Value}

### Antwort verarbeiten

Die Antworten des Arduino werden dabei im JSON Format (http://www.json.org/json-de.html) zurückgeliefert. Die Antworten des Arduino können deshalb mit jedem REST-Client (vielleicht http://restsharp.org) oder jedem JSON Parser (z.B. http://james.newtonking.com/json) verarbeitet werden

### Verwendete Pins

* `D0` Hardware Serial
* `D1` Hardware Serial
* `D2` -
* `D3` -
* `D4` SD Card Chip Select
* `D5` TFT Display Chip Select
* `D6` TFT Data/Command Control Pin
* `D7` LED Output Pin
* `D8` UART CO2 Sensor RX
* `D9` UART CO2 Sensor TX
* `D10` SPI Chip Select
* `D11` SPI Data Pin
* `D12` SPI Data Pin
* `D13` SPI Serial Clock Pin
* `A0` Touch Screen Y- Input Pin
* `A1` Touch Screen X- Input Pin
* `A2` Touch Screen Y+ Input Pin
* `A3` Touch Screen X+ Input Pin
* `A4` Loudness Input Pin
* `A5` Temperature Input Pin

## Links

* Arduino Software: http://arduino.cc/en/Main/Software#toc3
* Arduino Yun: http://arduino.cc/en/Main/ArduinoBoardYun#.Uyc4INzSdak
* Arduino Yun - Getting Started: http://arduino.cc/en/Guide/ArduinoYun

* PostgreSQL Server: http://www.postgresql.org/download/
* PostgreSQL Connector: http://npgsql.projects.pgfoundry.org
* MySQL Server Installer WIN: http://dev.mysql.com/downloads/windows/installer/
* MySQL Server Installer OS X: http://dev.mysql.com/downloads/mysql/
* MySQL Connector: http://dev.mysql.com/downloads/connector/odbc/
