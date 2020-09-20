namespace DioCli
{
    //https://www.pinvoke.net/default.aspx/Structures/COMMTIMEOUTS.html
    struct COMMTIMEOUTS
    {
        /// <summary>
        /// <para>Maximum time allowed to elapse between the arrival of two bytes on the communications line, in milliseconds. During a ReadFile operation, the time period begins when the first byte is received. If the interval between the arrival of any two bytes exceeds this amount, the ReadFile operation is completed and any buffered data is returned. A value of zero indicates that interval time-outs are not used.</para>
        /// <para>A value of MAXDWORD, combined with zero values for both the ReadTotalTimeoutConstant and ReadTotalTimeoutMultiplier members, specifies that the read operation is to return immediately with the bytes that have already been received, even if no bytes have been received.</para>
        /// </summary>
        public uint ReadIntervalTimeout;

        /// <summary>
        /// Multiplier used to calculate the total time-out period for read operations, in milliseconds. For each read operation, this value is multiplied by the requested number of bytes to be read.
        /// </summary>
        public uint ReadTotalTimeoutMultiplier;

        /// <summary>
        /// <para>Constant used to calculate the total time-out period for read operations, in milliseconds. For each read operation, this value is added to the product of the ReadTotalTimeoutMultiplier member and the requested number of bytes.</para>
        /// <para>A value of zero for both the ReadTotalTimeoutMultiplier and ReadTotalTimeoutConstant members indicates that total time-outs are not used for read operations.</para>
        /// </summary>
        public uint ReadTotalTimeoutConstant;

        /// <summary>
        /// Multiplier used to calculate the total time-out period for write operations, in milliseconds. For each write operation, this value is multiplied by the number of bytes to be written.
        /// </summary>
        public uint WriteTotalTimeoutMultiplier;

        /// <summary>
        /// <para>Constant used to calculate the total time-out period for write operations, in milliseconds. For each write operation, this value is added to the product of the WriteTotalTimeoutMultiplier member and the number of bytes to be written.</para>
        /// <para>A value of zero for both the WriteTotalTimeoutMultiplier and WriteTotalTimeoutConstant members indicates that total time-outs are not used for write operations.</para>
        /// </summary>
        public uint WriteTotalTimeoutConstant;
    }
}
