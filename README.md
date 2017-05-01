More of a server really.

Use with LOAD "$" on ZXpand+ or the 'TROLL' release firmware for ZXpand classic.

---

### 1. If necessary, compile the source with MONO:

mcs zxsvr.cs

The supplied pre-compiled .net assembly should work out of the box. I think. Possibly.

### Run the compiled exe under mono on Linux and OSX. Natively on Windows.

Supply the following parameters:
* path to P file to serve
* com port name

e.g:

* charlie$ mono zxsvr.exe 3dmm.p /dev/ttl.usb01whatever

* c:\charlie> zxsvr 3dmm.p COM13

---

Included is the magnificent UWOL and an Arduino sketch that I used for initial development. It's since proved useful for debugging.
