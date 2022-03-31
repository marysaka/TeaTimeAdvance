using System;

namespace TeaTimeAdvance.Device.IO.LCD
{
    [Flags]
    public enum ScreenDisplayFlag : byte
    {
        None,
        Background0 = 1 << 0,
        Background1 = 1 << 1,
        Background2 = 1 << 2,
        Background3 = 1 << 3,
        BackgroundObject = 1 << 4,
    }
}
