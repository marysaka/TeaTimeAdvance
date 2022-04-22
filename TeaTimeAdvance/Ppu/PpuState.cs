using System;
using TeaTimeAdvance.Common.Memory;
using TeaTimeAdvance.Device.IO.LCD;
using static TeaTimeAdvance.Ppu.PpuContext;

namespace TeaTimeAdvance.Ppu
{
    public class PpuState
    {
        public Array2<Point> ReferencePoints;

        public uint[] RawScreenBuffer { get; }

        public Span<uint> ScreenBuffer => RawScreenBuffer;

        public PpuState()
        {
            RawScreenBuffer = new uint[ScreenWidth * ScreenHeight];
        }

        internal void ReloadAffineRegisters(ReadOnlySpan<BackgroundAffineParameter> backgroundAffineParameters)
        {
            ReferencePoints[0] = backgroundAffineParameters[0].ReferencePoint;
            ReferencePoints[1] = backgroundAffineParameters[1].ReferencePoint;
        }
    }
}
