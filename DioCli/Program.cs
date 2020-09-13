using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DioCli
{
    class Program
    {
        static readonly int BlinkInterval = 200;    // the interval between LED blinks
        static readonly int PollInterval = 10;      // rx polling interval (milliseconds)
        static readonly int RxBufferSize = 1024;    // rx buffer size (bytes)
        static async Task Main(string[] args)
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

            var cts = new CancellationTokenSource(5000);
            var rxTask = RecieveLoopAsync(port, cts.Token);
            var txTask = TransmitLoopAsync(port, cts.Token);

            try
            {
                await Task.WhenAll(rxTask, txTask);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured during rx/tx: {ex.Message}");
            }
            finally
            {
                // Close the port
                RS232.ClosePort(port);
            }


            //// Read for a while
            //var rxBuffer = new byte[1024];
            //for (int i = 0; i < 1000; i++)
            //{
            //    Thread.Sleep(10);
            //    var n = RS232.Read(port, rxBuffer);
            //    var s = System.Text.Encoding.ASCII.GetString(rxBuffer, 0, n);
            //    Console.Write(s);
            //}

            //// Close the port
            //RS232.ClosePort(port);

            Console.WriteLine("DIO complete.");
        }

        static async Task TransmitLoopAsync(int portNum, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            byte[] bytes;

            // transmit led on/off commands 
            while(true)
            {
                bytes = System.Text.Encoding.ASCII.GetBytes("LED ON");
                RS232.Write(portNum, bytes);
                await Task.Delay(BlinkInterval / 2, ct).ConfigureAwait(false);

                bytes = System.Text.Encoding.ASCII.GetBytes("LED OFF");
                RS232.Write(portNum, bytes);
                await Task.Delay(BlinkInterval / 2, ct).ConfigureAwait(false);
            }
        }

        static async Task RecieveLoopAsync(int portNum, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var rxBuffer = new byte[RxBufferSize];
            while(true)
            {
                await Task.Delay(PollInterval, ct).ConfigureAwait(false);

                var n = RS232.Read(portNum, rxBuffer);
                var s = System.Text.Encoding.ASCII.GetString(rxBuffer, 0, n);

                Console.Write(s);
            }
        }
    }
}
