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

Beispiel zum senden eines Kommandos und zum entfangen einer Antwort: https://gist.github.com/FloeHerrmann/b5b00f89f3a8ccb7b036
In unserem Fall müsste anstatt des Kommandos `C:GetData;` ein HTTP Request gesendet werden:

```
HTTP {URL} HTTP/1.1
Host: {IP-Adresse}
Connection: close
```
```
HTTP /arduino/data HTTP/1.1
Host: 192.168.5.134
Connection: close
```

## Arduino

Verwendete URLs für das Abfragen von Werten und setzen von Grenzen

* `http://{IP-Adresse}/arduino/info` Liefert die Informationen zum Sensor, z.B. MAC, IP, Software-Version, etc.
* `http://{IP-Adresse}/arduino/data` Liefert die aktuellen Werte der Sensoren
* `http://{IP-Adresse}/arduino/data/loudness` Liefert den aktuellen Lautstärke-Wert
* `http://{IP-Adresse}/arduino/data/airquality` Liefert den aktuellen CO2-Wert
* `http://{IP-Adresse}/arduino/airquality/` Liefert die aktuell gesetzte untere Grenze für den CO2-Wert zurück
* `http://{IP-Adresse}/arduino/airquality/minimum/{Wert}` Setzt die untere Grenze für den CO2-Wert auf einen {Wert}
* `http://{IP-Adresse}/arduino/loudness` Liefter die aktuell gesetzte obere Grenze für die Läutstärke zurück
* `http://{IP-Adresse}/arduino/loudness/maximum/{Wert}` Setzt die obere Grenze für die Lautstärke auf einen {Wert}

Die Antworten des Arduino werden dabei im JSON Format (http://www.json.org/json-de.html) zurückgeliefert. Zum Beispiel liefert die URL `http://{IP-Adresse}/arduino/data` die Antwort `{"airQuality":"465","loudness":"70"}` zurück. Die Antworten des Arduino können deshalb mit jedem REST-Client (vielleicht http://restsharp.org) oder jedem JSON Parser (z.B. http://james.newtonking.com/json) verarbeitet werden

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
