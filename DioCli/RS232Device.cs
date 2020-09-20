using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DioCli
{
    class RS232Device : IDevice, IDisposable
    {
        static readonly int BlinkInterval = 200;    // the interval between LED blinks
        static readonly int PollInterval = 10;      // rx polling interval (milliseconds)
        static readonly int RxBufferSize = 1024;    // rx buffer size (bytes)

        private IntPtr _hPort;
        private bool _disposedValue;
        private int _portNum;

        public RS232Device(int portNum)
        {
            _portNum = portNum;
            _hPort = IntPtr.Zero;
        }

        public override string ToString()
        {
            return $"COM{_portNum}";
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                if (_hPort != IntPtr.Zero)
                {
                    RS232.ClosePort(_hPort);
                    _hPort = IntPtr.Zero;
                }
                
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~RS232Device()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public void Open()
        {
            _hPort = RS232.OpenPort(_portNum, new RS232Config
            {
                BaudRate = 115200,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
            });

            RS232.Purge(_hPort);
        }

        public void Close()
        {
            if (_hPort != IntPtr.Zero)
            {
                RS232.ClosePort(_hPort);
                _hPort = IntPtr.Zero;
            }
        }

        public async Task RunAsync(CancellationToken ct)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

            var rxTask = RecieveLoopAsync(cts.Token);
            var txTask = TransmitLoopAsync(cts.Token);

            // Cancel all tasks if one stops (exceptions aren't rethrown by WhenAny)
            await Task.WhenAny(rxTask, txTask).ConfigureAwait(false);
            cts.Cancel();

            // Wait for all tasks to stop before returning control and/or throwing exceptions
            await Task.WhenAll(rxTask, txTask).ConfigureAwait(false);
        }

        private async Task TransmitLoopAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            // transmit led on/off commands 
            while (true)
            {
                // Send LED ON command [1] with 0 delimiter
                Console.WriteLine("tx: LED ON");
                RS232.Write(_hPort, new byte[] { 1, 0 });
                await Task.Delay(BlinkInterval / 2, ct).ConfigureAwait(false);

                // Send LED OFF command [2] with 0 delimiter
                Console.WriteLine("tx: LED OFF");
                RS232.Write(_hPort, new byte[] { 2, 0 });
                await Task.Delay(BlinkInterval / 2, ct).ConfigureAwait(false);
            }
        }

        private async Task RecieveLoopAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var rxBuffer = new byte[RxBufferSize];
            while (true)
            {
                await Task.Delay(PollInterval, ct).ConfigureAwait(false);

                var n = RS232.Read(_hPort, rxBuffer);
                if (n == 0) continue;

                var s = Encoding.ASCII.GetString(rxBuffer, 0, n);
                Console.WriteLine("rx: " + s);
            }
        }
    }
}
