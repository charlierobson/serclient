using System;
using System.IO;
using System.IO.Ports;

class Program
{
    private enum XmitState
    {
        Busy,
        Done,
        Error
    }

    private static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No P file specified.");
                return;
            }

            var serialPortString = "COM1";

            var platform = (int) Environment.OSVersion.Platform;
            if ((platform == 4) || (platform == 6) || (platform == 128))
            {
                // linux based platform
                serialPortString = "/dev/ttyS0";
            }

            if (args.Length == 2)
            {
                serialPortString = args[1];
            }

            Console.WriteLine($"Using serial port '{serialPortString}'");

            var serial = new SerialPort(serialPortString, 115200) {ReadTimeout = 400};
            serial.Open();

            var pBytes = File.ReadAllBytes(args[0]);

            Console.WriteLine($"{pBytes.Length} bytes read.\nServer running.");

            var state = XmitState.Busy;
            while (state == XmitState.Busy)
            {
                try
                {
                    var b = (byte) serial.ReadByte();
                    Console.Write($"\n {(char)b}");

                    if (b == 'I')
                    {
                        // client requests info block {len:2}
                        Console.Write($" -> {pBytes.Length}");

                        serial.Write(new[] { (byte)(pBytes.Length & 0xff), (byte)(pBytes.Length >> 8) }, 0, 2);
                    }
                    else if (b == 'T')
                    {
                        // client requests transmission of block X
                        var blockNum = serial.ReadByte();
                        var blockLen = serial.ReadByte();
                        if (blockLen == 0) blockLen = 256;

                        Console.Write($" -> {blockNum,3}, {blockLen}");

                        SendBlock(serial, pBytes, blockNum*256, blockLen);
                    }
                    else if (b == 'X')
                    {
                        Console.Write($" -> OK");
                        state = XmitState.Done;
                    }
                    else
                    {
                        Console.Write(" !Error!");
                        state = XmitState.Error;
                    }
                }
                catch (TimeoutException)
                {
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    private static void SendBlock(SerialPort serial, byte[] pBytes, int offset, int blockSize)
    {
        var sum = 0;

        for (var i = 0; i < blockSize; ++i)
        {
            var b = pBytes[offset + i];
            serial.Write(pBytes, offset + i, 1);
            sum += b;
        }

        serial.Write(new byte[]{ (byte)(sum & 0xff), (byte)((sum >> 8) & 0xff) }, 0, 2);

        Console.Write($"  CS: ${sum:X4}");
    }
}
