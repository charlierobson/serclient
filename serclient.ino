void setup() {
  // put your setup code here, to run once:
  Serial.begin(38400);
}


void loop() {
  static byte block = 0;
  static int fileLen;
  
  // clear flush the serial input buffer
  while(Serial.available()) Serial.read(); 

  // request length
  Serial.write('I');
  while(Serial.available() < 2);
  fileLen = Serial.read()
  fileLen += 256 * Serial.read();

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
    unsigned short rxdsum = Serial.read();
    rxdsum += 256 * Serial.read();

    if (calcsum == rxdsum) {
      // move on
      fileLen -= 256;
      ++block;
    }
    // else re-request the last block
  }
}

