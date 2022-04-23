using System.Runtime.InteropServices;
using TeaTimeAdvance.Common.Memory;
using TeaTimeAdvance.Device.IO.LCD;

namespace TeaTimeAdvance.Device.IO
{
    [StructLayout(LayoutKind.Sequential /* , Size = 0x400 */)]
    public struct IORegisters
    {
        // LCD I/O Registers
        // TODO: Enums / Structs
        public LCDControl DISPCNT;
        public ushort GREENSWAP;
        public LCDStatus DISPSTAT;
        public ushort VCOUNT;
        public Array4<BackgroundControl> BGCNT;
        public Array4<BackgroundScrollingControl> BGOFS;
        public Array2<BackgroundAffineParameter> BGAP;
        public ushort WIN0H;
        public ushort WIN1H;
        public ushort WIN0V;
        public ushort WIN1V;
        public ushort WININ;
        public ushort WINOUT;
        public ushort MOSAIC;
        private ushort _unused4E;
        public ushort BLDCNT;
        public ushort BLDALPHA;
        public ushort BLDY;
        private Array10<byte> _unused56;
        // TODO: Sound Registers
        private Array20<uint> _reservedSoundRegisters;
        // TODO: DMA Transfer Channels
        private Array20<uint> _reservedDmaTransferChannels;
        // TODO: Timer Registers
        private Array8<uint> _reservedTimerRegisters;
        // TODO: Serial Communication (1)
        private Array4<uint> _reservedSerialCommunication1;
        // TODO: Keypad Input
        private Array4<byte> _reservedkeypadInput;
        // TODO: Serial Communication (2)
        private Array51<uint> _reservedSerialCommunication2;
        // TODO: Interrupt
        private Array4<byte> _reservedInterrupt;
        public WaitstateControl WAITCNT;
        private ushort _unused206;

        // TODO: More interupt, Power-Down Control...
    }
}
