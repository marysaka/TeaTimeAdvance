using System;

namespace TeaTimeAdvance.Cpu.Instruction.Definition
{
    [Flags]
    public enum CpuPSRFields : byte
    {
        Control = 1 << 0,
        Extension = 1 << 1,
        Status = 1 << 2,
        Flags = 1 << 3,

        Mask = 0xF
    }
}
