using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace DioCli
{
    //https://www.pinvoke.net/default.aspx/Structures/DCB.html
    [StructLayout(LayoutKind.Sequential)]
    internal struct Dcb
    {
        internal uint DCBLength;
        internal uint BaudRate;
        private BitVector32 Flags;

        private ushort wReserved;        // not currently used
        internal ushort XonLim;           // transmit XON threshold
        internal ushort XoffLim;          // transmit XOFF threshold             

        internal byte ByteSize;
        internal Parity Parity;
        internal StopBits StopBits;

        internal sbyte XonChar;          // Tx and Rx XON character
        internal sbyte XoffChar;         // Tx and Rx XOFF character
        internal sbyte ErrorChar;        // error replacement character
        internal sbyte EofChar;          // end of input character
        internal sbyte EvtChar;          // received event character
        private ushort wReserved1;       // reserved; do not use     

        private static readonly int fBinary;
        private static readonly int fParity;
        private static readonly int fOutxCtsFlow;
        private static readonly int fOutxDsrFlow;
        private static readonly BitVector32.Section fDtrControl;
        private static readonly int fDsrSensitivity;
        private static readonly int fTXContinueOnXoff;
        private static readonly int fOutX;
        private static readonly int fInX;
        private static readonly int fErrorChar;
        private static readonly int fNull;
        private static readonly BitVector32.Section fRtsControl;
        private static readonly int fAbortOnError;

        static Dcb()
        {
            // Create Boolean Mask
            int previousMask;
            fBinary = BitVector32.CreateMask();
            fParity = BitVector32.CreateMask(fBinary);
            fOutxCtsFlow = BitVector32.CreateMask(fParity);
            fOutxDsrFlow = BitVector32.CreateMask(fOutxCtsFlow);
            previousMask = BitVector32.CreateMask(fOutxDsrFlow);
            previousMask = BitVector32.CreateMask(previousMask);
            fDsrSensitivity = BitVector32.CreateMask(previousMask);
            fTXContinueOnXoff = BitVector32.CreateMask(fDsrSensitivity);
            fOutX = BitVector32.CreateMask(fTXContinueOnXoff);
            fInX = BitVector32.CreateMask(fOutX);
            fErrorChar = BitVector32.CreateMask(fInX);
            fNull = BitVector32.CreateMask(fErrorChar);
            previousMask = BitVector32.CreateMask(fNull);
            previousMask = BitVector32.CreateMask(previousMask);
            fAbortOnError = BitVector32.CreateMask(previousMask);

            // Create section Mask
            BitVector32.Section previousSection;
            previousSection = BitVector32.CreateSection(1);
            previousSection = BitVector32.CreateSection(1, previousSection);
            previousSection = BitVector32.CreateSection(1, previousSection);
            previousSection = BitVector32.CreateSection(1, previousSection);
            fDtrControl = BitVector32.CreateSection(2, previousSection);
            previousSection = BitVector32.CreateSection(1, fDtrControl);
            previousSection = BitVector32.CreateSection(1, previousSection);
            previousSection = BitVector32.CreateSection(1, previousSection);
            previousSection = BitVector32.CreateSection(1, previousSection);
            previousSection = BitVector32.CreateSection(1, previousSection);
            previousSection = BitVector32.CreateSection(1, previousSection);
            fRtsControl = BitVector32.CreateSection(3, previousSection);
            previousSection = BitVector32.CreateSection(1, fRtsControl);
        }

        public bool Binary
        {
            get { return Flags[fBinary]; }
            set { Flags[fBinary] = value; }
        }

        public bool CheckParity
        {
            get { return Flags[fParity]; }
            set { Flags[fParity] = value; }
        }

        public bool OutxCtsFlow
        {
            get { return Flags[fOutxCtsFlow]; }
            set { Flags[fOutxCtsFlow] = value; }
        }

        public bool OutxDsrFlow
        {
            get { return Flags[fOutxDsrFlow]; }
            set { Flags[fOutxDsrFlow] = value; }
        }

        public DtrControl DtrControl
        {
            get { return (DtrControl)Flags[fDtrControl]; }
            set { Flags[fDtrControl] = (int)value; }
        }

        public bool DsrSensitivity
        {
            get { return Flags[fDsrSensitivity]; }
            set { Flags[fDsrSensitivity] = value; }
        }

        public bool TxContinueOnXoff
        {
            get { return Flags[fTXContinueOnXoff]; }
            set { Flags[fTXContinueOnXoff] = value; }
        }

        public bool OutX
        {
            get { return Flags[fOutX]; }
            set { Flags[fOutX] = value; }
        }

        public bool InX
        {
            get { return Flags[fInX]; }
            set { Flags[fInX] = value; }
        }

        public bool ReplaceErrorChar
        {
            get { return Flags[fErrorChar]; }
            set { Flags[fErrorChar] = value; }
        }

        public bool Null
        {
            get { return Flags[fNull]; }
            set { Flags[fNull] = value; }
        }

        public RtsControl RtsControl
        {
            get { return (RtsControl)Flags[fRtsControl]; }
            set { Flags[fRtsControl] = (int)value; }
        }

        public bool AbortOnError
        {
            get { return Flags[fAbortOnError]; }
            set { Flags[fAbortOnError] = value; }
        }
    }
}
