using System.Linq;

namespace DioCli
{
    public class RS232Config
    {
        public static readonly int[] ValidBaudRates = { 110, 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 128000, 256000, 500000, 921600, 1000000, 1500000, 2000000, 3000000 };
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }

        public bool IsValid(out string invalidMessage)
        {
            if (!ValidBaudRates.Contains(BaudRate))
            {
                invalidMessage = "Invalid baud rate.";
                return false;
            }

            if (DataBits < 5 || DataBits > 8)
            {
                invalidMessage = "Invalid number of data bits (valid range is [5, 8]).";
                return false;
            }

            invalidMessage = string.Empty;
            return true;
        }
    }
}
