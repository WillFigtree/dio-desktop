using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DioCli
{
    class Program
    {
        static readonly int port = 5;               // the COM port number to use
        static async Task Main(string[] args)
        {
            Console.WriteLine("====Dio CLI====");

            // Open the DIO serial port
            var device = new RS232Device(port);

            var cts = new CancellationTokenSource(10000);    // Cancel after 10 seconds
            var run = true;
            while (run)
            {
                if (TryOpenDevice(device))
                {
                    // Run DIO communications
                    try
                    {
                        await device.RunAsync(cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        // Stop running on cancellation
                        run = false;
                    }
                    catch (System.Runtime.InteropServices.COMException) { Console.WriteLine("COM exception! Restarting"); }
                }
            }


            // Shut down
            device.Close();
            Console.WriteLine("Dio complete");
        }

        private static bool TryOpenDevice(IDevice device)
        {
            Console.WriteLine($"Opening {device}");

            try
            {
                device.Open();
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Error: Invalid port options.");
                return false;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: Could not find {device}.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Could not open {device}. {ex.Message}");
                return false;
            }
            
            return true;
        }
    }
}
