More of a server really. I should probably rename the output too.

### Compile the source with MONO:

mcs Program.cs

### Run the compiled exe under mono on Linux and OSX. Natively on Windows.

mono Program.exe [path to P file to serve] [com port name]

e.g:

* charlie$ mono Program.exe 3dmm.p /dev/ttl.usb01

* c:> program 3dmm.p COM13

