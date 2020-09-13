using System;
using System.IO;
using System.Threading;

namespace DioCli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====Dio CLI====");

            // Open a port
            var port = 5;
            try
            {
                RS232.OpenPort(port, new RS232Config
                {
                    BaudRate = 115200,
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                });
            }
            catch(ArgumentException)
            {
                Console.WriteLine("Error: Invalid port options.");
                return;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: Could not find port COM{port}.");
                return;
            }

            // Read for a while
            var rxBuffer = new byte[1024];
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(10);
                var n = RS232.ReadPort(port, rxBuffer);
                var s = System.Text.Encoding.ASCII.GetString(rxBuffer, 0, n);
                Console.Write(s);
            }

            // Close the port
            RS232.ClosePort(port);
        }
    }
}
