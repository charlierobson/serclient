void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
}

static byte block = 0;
static int fileLen;

void loop() {
  // clear flush the serial input buffer
  while(Serial.available()) Serial.read(); 

  // request length
  Serial.write('I');
  while(Serial.available() < 2);
  fileLen = Serial.read() + 256 * Serial.read();

  while(fileLen > 0)
  {
    // tell host to TX
    Serial.write('T');
    Serial.write(block);

    byte blen = 0;
    unsigned short calcsum = 0;

    if (fileLen < 256) blen = fileLen;
    Serial.write(blen);

    // receive bacon ~~~
    for(int i = 0; i < blen; ++i) {
      while(!Serial.available());
      byte b = Serial.read();
      // do something with b
      calcsum += b;
    }

    // bacon received, get checksum
    while(Serial.available() < 2);
    unsigned short rxdsum = Serial.read() + 256 * Serial.read();

    // problem?
    if (calcsum == rxdsum) {
      fileLen -= 256;
      ++block;
 
      // flush
      while(Serial.available()) Serial.read(); 
    }
  }
}

