using System;

namespace TeaTimeAdvance.Cpu.State
{
    [Flags]
    public enum CurrentProgramStatusRegister : uint
    {
        M0 = 1 << 0,
        M1 = 1 << 0,
        M2 = 1 << 2,
        M3 = 1 << 3,
        M4 = 1 << 4,
        Thumb = 1 << 5,
        MaskFIQ = 1 << 6,
        MaskIRQ = 1 << 7,
        Overflow = 1 << 28,
        Carry = 1 << 29,
        Zero = 1 << 30,
        Negative = 1U << 31,
    }
}
