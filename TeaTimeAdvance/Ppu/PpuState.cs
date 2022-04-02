using System;
using System.Drawing;

using static TeaTimeAdvance.Ppu.PpuContext;

namespace TeaTimeAdvance.Ppu
{
    public class PpuState
    {
        public Color[] RawScreenBuffer { get; }

        public Span<Color> ScreenBuffer => RawScreenBuffer;

        public PpuState()
        {
            RawScreenBuffer = new Color[ScreenWidth * ScreenHeight];
        }
    }
}
