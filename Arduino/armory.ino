// Some setup code borrowed from Arduino SimpleWebServerWiFi example by Tom Igoe
#include <SPI.h>
#include <WiFiNINA.h>
#include "wifi_creds.h"

char ssid[] = NETWORK_SSID;
char pass[] = NETWORK_PASS;

const int shoot = A0;
const int sword = A1;
const int shield = A2;
bool button = false;

int swordThresh = 200;
int shieldThresh = 200;

int status = WL_IDLE_STATUS;
WiFiServer server(80);

void setup() {
  Serial.begin(9600);
  pinMode(LED_BUILTIN, OUTPUT);      // set the LED pin mode
  pinMode(shoot, INPUT);
  pinMode(sword, INPUT);
  pinMode(shield, INPUT);

  Serial.println("Starting");
  // check for the WiFi module:
  if (WiFi.status() == WL_NO_MODULE) {
    Serial.println("Communication with WiFi module failed!");
    // don't continue
    while (true);
  }

  String fv = WiFi.firmwareVersion();
  if (fv < WIFI_FIRMWARE_LATEST_VERSION) {
    Serial.println("Please upgrade the firmware");
  }

  // attempt to connect to WiFi network:
  while (status != WL_CONNECTED) {
    Serial.print("Attempting to connect to Network named: ");
    Serial.println(ssid);                   // print the network name (SSID);

    // Connect to WPA/WPA2 network. Change this line if using open or WEP network:
    status = WiFi.begin(ssid, pass);
    // wait 10 seconds for connection:
    delay(10000);
  }
  server.begin();                           // start the web server on port 80
  printWifiStatus();                        // you're connected now, so print out the status
}


void loop() {
  WiFiClient client = server.available();   // listen for incoming clients

  if (client) {                             // if you get a client,
    Serial.println("new client");           // print a message out the serial port
    while (client.connected()) {            // loop while the client's connected
      digitalWrite(LED_BUILTIN, HIGH);
      
      if (analogRead(sword) < swordThresh) {
        Serial.println("sword");
        client.println("sword");
      } else if (analogRead(shield) < shieldThresh) {
        Serial.println("shield");
        client.println("shield");
      } else {
        Serial.println("crossbow");
        client.println("crossbow");
      }

    //Mason fixed this from here
      if (analogRead(shoot) == 1023 && !button) {
        Serial.println("shoot");
        client.println("shoot");
        button = true;
      }
      
      if(analogRead(shoot)==0){
        button = false; 
      }
      //to here (:
      
      if (client.available()) {             // if there's bytes to read from the client,
        char c = client.read();             // read a byte, then
        Serial.write(c);                    // print it out on the serial monitor
      }
      
    }
    // close the connection:
    client.stop();
    digitalWrite(LED_BUILTIN, LOW);
    Serial.println("client disconnected");
  }
}

void printWifiStatus() {
  // print the SSID of the network you're attached to:
  Serial.print("SSID: ");
  Serial.println(WiFi.SSID());

  // print your board's IP address:
  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);

  // print the received signal strength:
  long rssi = WiFi.RSSI();
  Serial.print("signal strength (RSSI):");
  Serial.print(rssi);
  Serial.println(" dBm");
  // print where to go in a browser:
  Serial.print("To see this page in action, open a browser to http://");
  Serial.println(ip);
}
