using System.Runtime.InteropServices;
using TeaTimeAdvance.Common.Memory;
using TeaTimeAdvance.Device.IO.LCD;

namespace TeaTimeAdvance.Device.IO
{
    [StructLayout(LayoutKind.Sequential/* , Size = 0x400 */)]
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
        private uint _unused56;

        // TODO: Sound Registers
        // TODO: DMA Transfer Channels
        // TODO: Timer Registers
        // TODO: Serial Communication (1)
        // TODO: Keypad Input
        // TODO: Serial Communication (2)
        // TODO: Interrupt, Waitstate, and Power-Down Control
    }
}
