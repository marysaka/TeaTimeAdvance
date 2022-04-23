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

        public void ReloadAffineRegister(int index, ref BackgroundAffineParameter backgroundAffineParameter)
        {
            ReferencePoints[index] = backgroundAffineParameter.ReferencePoint;
        }

        public void UpdateAffineRegisters(ReadOnlySpan<BackgroundAffineParameter> backgroundAffineParameters)
        {
            for (int i = 0; i < ReferencePoints.Length; i++)
            {
                ReferencePoints[i].X += backgroundAffineParameters[i].PB;
                ReferencePoints[i].Y += backgroundAffineParameters[i].PD;
            }
        }
    }
}
