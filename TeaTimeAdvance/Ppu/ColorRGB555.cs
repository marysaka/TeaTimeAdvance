using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TeaTimeAdvance.Ppu
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public record struct ColorRGB555
    {
        private ushort _value;

        public byte R => (byte)(_value & 0x1F);
        public byte G => (byte)((_value >> 5) & 0x1F);
        public byte B => (byte)((_value >> 10) & 0x1F);

        public ColorRGB555(byte r, byte g, byte b)
        {
            _value = (ushort)((b & 0x1F) << 10 | (g & 0x1F) << 5 | (r & 0x1F) << 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ToRGBA8888()
        {
            return (uint)(byte.MaxValue << 24 | B << 16 | B << 8 | R << 0);
        }
    }
}
