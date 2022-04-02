namespace TeaTimeAdvance.Device.IO.LCD
{
    public record struct BackgroundControl
    {
        private ushort _rawValue;

        public BackgroundControl(ushort rawValue)
        {
            _rawValue = rawValue;
        }

        public byte Priority => (byte)(_rawValue & 3);
        public byte CharacterBaseBlock => (byte)((_rawValue >> 2) & 3);
        public bool Mosaic => ((_rawValue >> 6) & 1) != 0;
        public byte PaletteType => (byte)((_rawValue >> 7) & 1);
        public byte ScreenBaseBlock => (byte)((_rawValue >> 8) & 0x1f);
        public bool DisplayAreaOverflow => ((_rawValue >> 13) & 1) != 0;
        public byte ScreenSize => (byte)((_rawValue >> 14) & 3);
    }
}
