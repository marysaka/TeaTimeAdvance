using System;
using TeaTimeAdvance.Bus;

namespace TeaTimeAdvance.Device
{
    public class RomDevice : MemoryBackedDevice
    {
        private byte[] _romWaitBase;
        private byte[] _romWait32;

        //public override uint MappedSize => 0x6000000;

        // TODO: Handles weird out of range behaviours.
        // TODO: Handle prefetch correctly and wait
        public RomDevice(byte[] data, byte[] romWaitBase, byte[] romWait32) : base(data)
        {
            _romWaitBase = romWaitBase;
            _romWait32 = romWait32;
        }

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

        public override void UpdateScheduler(BusContext context, uint address, BusAccessType accessType, BusAccessInfo accessInfo)
        {
            if (accessInfo.HasFlag(BusAccessInfo.Write))
            {
                return;
            }

            uint pageIndex = (address >> BusContext.PageGranuality) - BusContext.WaitStatePageStart;
            long index = pageIndex + BusContext.WaitStatePageCount * (int)accessType;

            byte cycles;

            if (accessInfo.HasFlag(BusAccessInfo.Memory32))
            {
                cycles = _romWait32[index];
            }
            else
            {
                cycles = _romWaitBase[index];
            }

            if (cycles > 0)
            {
                context.UpdateCycles(cycles);
            }
        }
    }
}
