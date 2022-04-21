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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color ToColor()
        {
            return ConvertRGB555(R, G, B);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ConvertRGB555(byte r, byte g, byte b)
        {
            r <<= 3;
            g <<= 3;
            b <<= 3;

            return Color.FromArgb(r, g, b);
        }
    }
}
