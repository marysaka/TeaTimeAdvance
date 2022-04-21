using System;
using System.Drawing;
using TeaTimeAdvance.Common.Memory;
using TeaTimeAdvance.Device.IO.LCD;
using static TeaTimeAdvance.Ppu.PpuContext;
using Point = TeaTimeAdvance.Device.IO.LCD.Point;

namespace TeaTimeAdvance.Ppu
{
    public class PpuState
    {
        public Array2<Point> ReferencePoints;

        public Color[] RawScreenBuffer { get; }

        public Span<Color> ScreenBuffer => RawScreenBuffer;

        public PpuState()
        {
            RawScreenBuffer = new Color[ScreenWidth * ScreenHeight];
        }

        internal void ReloadAffineRegisters(ReadOnlySpan<BackgroundAffineParameter> backgroundAffineParameters)
        {
            ReferencePoints[0] = backgroundAffineParameters[0].ReferencePoint;
            ReferencePoints[1] = backgroundAffineParameters[1].ReferencePoint;
        }
    }
}
