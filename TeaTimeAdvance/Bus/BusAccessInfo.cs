using System;

namespace TeaTimeAdvance.Bus
{
    [Flags]
    public enum BusAccessInfo : byte
    {
        Memory8 = 1,
        Memory16,
        Memory32,

        MemoryMask = 0b10,

        Read = 1 << 2,
        Write = 1 << 3,
    }
}
