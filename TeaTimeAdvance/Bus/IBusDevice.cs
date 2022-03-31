using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Bus
{
    public interface IBusDevice
    {
        uint MappedSize { get; }

        void UpdateScheduler(BusContext context, uint address, BusAccessType accessType, BusAccessInfo accessInfo);

        byte Read8(uint baseAddress, uint address);

        ushort Read16(uint baseAddress, uint address);

        uint Read32(uint baseAddress, uint address);

        void Write8(uint baseAddress, uint address, byte value);

        void Write16(uint baseAddress, uint address, ushort value);

        void Write32(uint baseAddress, uint address, uint value);
    }
}
