using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DioCli
{
    public static class RS232
    {
        public static IntPtr OpenPort(int portNum, RS232Config config)
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

            // Return port handle for caller
            return hPort;
        }

        public static void ClosePort(IntPtr hPort)
        {
            if (!RS232PInvoke.CloseHandle(hPort))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        public static int Read(IntPtr hPort, byte[] buffer)
        {
            // Read bytes
            if (!RS232PInvoke.ReadFile(hPort, buffer, (uint)buffer.Length, out var n, IntPtr.Zero))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            return (int)n;
        }

        public static void Write(IntPtr hPort, byte[] buffer)
        {
            if (buffer == null) return;

            // Write bytes
            if(!RS232PInvoke.WriteFile(hPort, buffer, (uint)buffer.Length, out var _, IntPtr.Zero))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        public static void Purge(IntPtr hPort)
        {
            //flags for clearing rx and tx buffers
            //https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-purgecomm
            const uint PURGE_TXCLEAR = 0x0004;
            const uint PURGE_RXCLEAR = 0x0008;
            const uint flags = PURGE_TXCLEAR | PURGE_RXCLEAR;

            if (!RS232PInvoke.PurgeComm(hPort, flags))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }
    }
}
