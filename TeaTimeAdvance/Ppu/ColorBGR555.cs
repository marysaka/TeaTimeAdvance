using System.Buffers.Binary;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TeaTimeAdvance.Ppu
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public record struct ColorBGR555
    {
        public ushort Value;

        public byte R => (byte)(Value & 0x1F);
        public byte G => (byte)((Value >> 5) & 0x1F);
        public byte B => (byte)((Value >> 10) & 0x1F);

        public ColorBGR555(byte r, byte g, byte b)
        {
            Value = (ushort)((b & 0x1F) << 10 | (g & 0x1F) << 5 | (r & 0x1F) << 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ToRGBA8888()
        {
            const int BitPerPixelRGBA8888 = 8;
            const int BitPerPixelRGB555 = 5;
            const int BitPerPixelDiff = BitPerPixelRGBA8888 - BitPerPixelRGB555;

            return (uint)(R << (24 + BitPerPixelDiff) | G << (16 + BitPerPixelDiff) | B << (8 + BitPerPixelDiff) | 0xFF << 0);
        }
    }
}
