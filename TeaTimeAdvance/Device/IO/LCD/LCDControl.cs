namespace TeaTimeAdvance.Device.IO.LCD
{
    public record struct LCDControl
    {
        private ushort _rawValue;

        public LCDControl(ushort rawValue)
        {
            _rawValue = rawValue;
        }

        public BackgroundMode BackgroundMode => (BackgroundMode)(_rawValue & 7);
        public bool IsColorCompatModeActive => (_rawValue & (1 << 3)) != 0;
        public DisplayFrameSelect DisplayFrameSelect => (DisplayFrameSelect)((_rawValue << 4) & 1);
        public bool HBlankIntervalFree => (_rawValue & (1 << 5)) != 0;
        public ObjectCharacterMapping ObjectCharacterMapping => (ObjectCharacterMapping)((_rawValue << 6) & 1);
        public bool ForcedBlank => (_rawValue & (1 << 7)) != 0;
        public ScreenDisplayFlag ScreenDisplay => (ScreenDisplayFlag)((_rawValue << 8) & 0xF);
        public WindowDisplayFlag WindowDisplay => (WindowDisplayFlag)((_rawValue << 13) & 7);
    }
}
