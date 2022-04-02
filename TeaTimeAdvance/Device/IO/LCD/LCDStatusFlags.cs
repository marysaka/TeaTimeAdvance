using System;

namespace TeaTimeAdvance.Device.IO.LCD
{
    [Flags]
    public enum LCDStatusFlags : byte
    {
        VerticalBlank = 1 << 0,
        HorizontalBlank = 1 << 1,
        VerticalCount = 1 << 2,
        VerticalBlankIRQ = 1 << 3,
        HorizontalBlankIRQ = 1 << 4,
        VerticalCountIRQ = 1 << 5,
    }
}
