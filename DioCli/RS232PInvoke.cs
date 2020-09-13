using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace DioCli
{
    internal static class RS232PInvoke
    {
        private const string Kernel32DLL = "Kernel32.dll";

        //https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea
        //https://www.pinvoke.net/default.aspx/kernel32.CreateFile
        [DllImport(Kernel32DLL, CharSet = CharSet.Ansi, SetLastError=true)]
        internal static extern IntPtr CreateFileA(
             [MarshalAs(UnmanagedType.LPStr)] string filename,
             [MarshalAs(UnmanagedType.U4)] FileAccess access,
             [MarshalAs(UnmanagedType.U4)] FileShare share,
             IntPtr securityAttributes,
             [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
             [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
             IntPtr templateFile);

        //https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-setcommstate
        //https://www.pinvoke.net/default.aspx/kernel32.setcommstate
        [DllImport(Kernel32DLL)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetCommState(IntPtr hFile, [In] ref Dcb lpDCB);

        //https://docs.microsoft.com/en-us/windows/win32/api/handleapi/nf-handleapi-closehandle
        //https://www.pinvoke.net/default.aspx/kernel32.closehandle
        [DllImport(Kernel32DLL, SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        //https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-setcommtimeouts
        //https://www.pinvoke.net/default.aspx/kernel32.setcommtimeouts
        [DllImport(Kernel32DLL, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetCommTimeouts(IntPtr hFile, [In] ref COMMTIMEOUTS lpCommTimeouts);

        //https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-readfile
        //https://www.pinvoke.net/default.aspx/kernel32/readfile.html?diff=y
        [DllImport(Kernel32DLL, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ReadFile(IntPtr hFile, [Out] byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        //https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-writefile
        //http://pinvoke.net/default.aspx/kernel32/WriteFile.html?diff=y
        [DllImport(Kernel32DLL, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        //https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-purgecomm
        //https://www.pinvoke.net/default.aspx/kernel32.purgecomm
        [DllImport(Kernel32DLL, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PurgeComm(IntPtr hFile, uint dwFlags);
    }
}
