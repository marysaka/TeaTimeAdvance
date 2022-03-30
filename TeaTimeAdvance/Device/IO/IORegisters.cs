using System.Runtime.InteropServices;

namespace TeaTimeAdvance.Device.IO
{
    [StructLayout(LayoutKind.Sequential/* , Size = 0x400 */)]
    public struct IORegisters
    {
        // LCD I/O Registers
        // TODO: Enums
        public ushort DISPCNT;
        public ushort GREENSWAP;
        public ushort DISPSTAT;
        public ushort VCOUNT;
        public ushort BG0CNT;
        public ushort BG1CNT;
        public ushort BG2CNT;
        public ushort BG3CNT;
        public ushort BG0HOFS;
        public ushort BG0VOFS;
        public ushort BG1HOFS;
        public ushort BG1VOFS;
        public ushort BG2HOFS;
        public ushort BG2VOFS;
        public ushort BG3HOFS;
        public ushort BG3VOFS;
        public ushort BG2PA;
        public ushort BG2PB;
        public ushort BG2PC;
        public ushort BG2PD;
        public uint BG2X;
        public uint BG2Y;
        public ushort BG3PA;
        public ushort BG3PB;
        public ushort BG3PC;
        public ushort BG3PD;
        public uint BG3X;
        public uint BG3Y;
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
