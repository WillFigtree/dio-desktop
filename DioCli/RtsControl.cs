//http://www.pinvoke.net/default.aspx/Enums/RtsControl.html

namespace DioCli
{
    public enum RtsControl : int
    {
        /// <summary>
        /// Disables the RTS line when the device is opened and leaves it disabled.
        /// </summary>
        Disable = 0,

        /// <summary>
        /// Enables the RTS line when the device is opened and leaves it on.
        /// </summary>
        Enable = 1,

        /// <summary>
        /// Enables RTS handshaking. The driver raises the RTS line when the "type-ahead" (input) buffer
        /// is less than one-half full and lowers the RTS line when the buffer is more than
        /// three-quarters full. If handshaking is enabled, it is an error for the application to
        /// adjust the line by using the EscapeCommFunction function.
        /// </summary>
        Handshake = 2,

        /// <summary>
        /// Specifies that the RTS line will be high if bytes are available for transmission. After
        /// all buffered bytes have been sent, the RTS line will be low.
        /// </summary>
        Toggle = 3
    }
}
