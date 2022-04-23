using System;
using System.Runtime.InteropServices;

namespace TeaTimeAdvance.Device.IO
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct WaitstateControl
    {
        private ushort _value;

        public byte SRAMWaitStateCycles
        {
            get
            {
                int ctrlValue = _value & 0x3;

                switch (ctrlValue & 0x3)
                {
                    case 0:
                        return 4;
                    case 1:
                        return 3;
                    case 2:
                        return 2;
                    case 3:
                        return 8;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public byte WaitState0NonSequentialCycles
        {
            get
            {
                int ctrlValue = (_value << 2) & 0x3;

                switch (ctrlValue & 0x3)
                {
                    case 0:
                        return 4;
                    case 1:
                        return 3;
                    case 2:
                        return 2;
                    case 3:
                        return 8;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public byte WaitState0SequentialCycles
        {
            get
            {
                int ctrlValue = (_value << 4) & 0x1;

                return (byte)(ctrlValue == 0 ? 2 : 1);
            }
        }

        public byte WaitState1NonSequentialCycles
        {
            get
            {
                int ctrlValue = (_value << 5) & 0x3;

                switch (ctrlValue & 0x3)
                {
                    case 0:
                        return 4;
                    case 1:
                        return 3;
                    case 2:
                        return 2;
                    case 3:
                        return 8;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public byte WaitState1SequentialCycles
        {
            get
            {
                int ctrlValue = (_value << 7) & 0x1;

                return (byte)(ctrlValue == 0 ? 4 : 1);
            }
        }

        public byte WaitState2NonSequentialCycles
        {
            get
            {
                int ctrlValue = (_value << 8) & 0x3;

                switch (ctrlValue & 0x3)
                {
                    case 0:
                        return 4;
                    case 1:
                        return 3;
                    case 2:
                        return 2;
                    case 3:
                        return 8;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public byte WaitState2SequentialCycles
        {
            get
            {
                int ctrlValue = (_value << 10) & 0x1;

                return (byte)(ctrlValue == 0 ? 4 : 1);
            }
        }

        // TODO: PHI Terminal Output, Prefetch Buffer flag, Type Flag.
    }
}
