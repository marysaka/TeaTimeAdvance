using System;

namespace TeaTimeAdvance.Device.IO.LCD
{
    [Flags]
    public enum WindowDisplayFlag : byte
    {
        None,
        Window0 = 1 << 0,
        Window1 = 1 << 1,
        WindowObject = 1 << 2,
    }
}
