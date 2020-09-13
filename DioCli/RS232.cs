using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DioCli
{
    public static class RS232
    {
        private readonly static Dictionary<int, IntPtr> _ports = new Dictionary<int, IntPtr>();

        public static void OpenPort(int portNum, RS232Config config)
        {
            // Validate inputs
            if (portNum < 0 || portNum > 255)
            {
                throw new ArgumentException("Invalid port number (valid range is [0, 255]).", nameof(portNum));
            }

            if (!config.IsValid(out var msg))
            {
                throw new ArgumentException(msg, nameof(config));
            }

            // Open the port
            var fileName = "\\\\.\\COM" + portNum;
            var hPort = RS232PInvoke.CreateFileA(
                fileName,                               // File name for com port
                FileAccess.ReadWrite,                   // Read/Write access for (rx/tx)
                FileShare.None,                         // no share
                IntPtr.Zero,                            // no security
                FileMode.Open,                          // open existing
                FileAttributes.Normal,                  // normal file
                IntPtr.Zero);                           // no template file
            if (hPort.ToInt32() == -1)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            // Configure the port
            var dcb = new Dcb
            {
                BaudRate = (uint)config.BaudRate,
                ByteSize = (byte)config.DataBits,
                Parity = config.Parity,
                StopBits = config.StopBits,
            };
            if (!RS232PInvoke.SetCommState(hPort, ref dcb))
            {
                RS232PInvoke.CloseHandle(hPort);
                throw new Exception("Could not configure COM port settings.");
            }

            // Configure the port time-outs
            var timeouts = new COMMTIMEOUTS
            {
                // Read operations return immediately 
                ReadIntervalTimeout = 0xFFFF_FFFF,  // MAXDWORD
                ReadTotalTimeoutMultiplier = 0,
                ReadTotalTimeoutConstant = 0,

                // Timeouts not used for write operations
                WriteTotalTimeoutMultiplier = 0,
                WriteTotalTimeoutConstant = 0,
            };
            if (!RS232PInvoke.SetCommTimeouts(hPort, ref timeouts))
            {
                RS232PInvoke.CloseHandle(hPort);
                throw new Exception("Could not configure COM port timeouts.");
            }

            // Keep track of the open port
            _ports.Add(portNum, hPort);
        }

        public static void ClosePort(int portNum)
        {
            if (!_ports.Remove(portNum, out var hPort)) return;
            RS232PInvoke.CloseHandle(hPort);
        }

        public static int Read(int portNum, byte[] buffer)
        {
            // Return 0 if the port isn't found
            if (!_ports.TryGetValue(portNum, out var hPort))
            {
                return 0;
            }

            // Read bytes
            RS232PInvoke.ReadFile(hPort, buffer, (uint)buffer.Length, out var n, IntPtr.Zero);
            return (int)n;
        }

        public static void Write(int portNum, byte[] buffer)
        {
            if (buffer == null) return;

            if (!_ports.TryGetValue(portNum, out var hPort))
            {
                throw new ArgumentException($"Port {portNum} not opened.", nameof(portNum));
            }

            // Write bytes
            if(!RS232PInvoke.WriteFile(hPort, buffer, (uint)buffer.Length, out var _, IntPtr.Zero))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }
    }
}
