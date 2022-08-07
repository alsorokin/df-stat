using System;
using System.Runtime.InteropServices;

namespace WpfClient
{
    public static class MemoryReader
    {
        [DllImport("Kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, ref UInt32 lpNumberOfBytesRead);

        public static byte[] Read(IntPtr handle, IntPtr address, UInt32 size, ref UInt32 bytes)
        {
            byte[] buffer = new byte[size];
            ReadProcessMemory(handle, address, buffer, size, ref bytes);
            return buffer;
        }
    }
}
