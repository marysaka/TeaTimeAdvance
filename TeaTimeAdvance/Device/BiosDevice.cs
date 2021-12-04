using System;

namespace TeaTimeAdvance.Device
{
    public class BiosDevice : MemoryBackedDevice
    {
        public BiosDevice(ReadOnlySpan<byte> data) : base(data, 0x4000) { }

        public override void Write8(uint baseAddress, uint address, byte value)
        {
            // No operations
        }

        public override void Write16(uint baseAddress, uint address, ushort value)
        {
            // No operations
        }

        public override void Write32(uint baseAddress, uint address, uint value)
        {
            // No operations
        }
    }
}
