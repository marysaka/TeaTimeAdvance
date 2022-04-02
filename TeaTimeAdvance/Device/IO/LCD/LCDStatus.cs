namespace TeaTimeAdvance.Device.IO.LCD
{
    public record struct LCDStatus
    {
        public LCDStatusFlags Flags;
        public ushort VerticalCount;
    }
}
