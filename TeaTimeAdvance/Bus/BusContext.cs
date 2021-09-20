using System;
using System.Diagnostics;
using TeaTimeAdvance.Common;
using TeaTimeAdvance.Device;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Bus
{
    public class BusContext
    {
        public const int PageGranuality = 24;

        public const uint MinAddress = 0;
        public const uint MaxAddress = uint.MaxValue;

        public const uint FirstPage = MinAddress >> PageGranuality;
        public const uint LastPage = MaxAddress >> PageGranuality;

        private SchedulerContext _schedulerContext;
        private IBusDevice[] _pagesDeviceMapping;

        public BusContext(SchedulerContext schedulerContext)
        {
            _schedulerContext = schedulerContext;
            _pagesDeviceMapping = new IBusDevice[LastPage + 1];
        }

        public void Initialize(ReadOnlySpan<byte> bios, ReadOnlySpan<byte> rom)
        {
            // BIOS [0x00000000 - 0x00003FFF]
            RegisterDevice(0x00000000, new BiosDevice(bios));

            // WRAM 1 (256 KiB) [0x02000000 - 0x0203FFFF]
            RegisterDevice(0x02000000, new MemoryBackedDevice(0x40000));

            // WRAM 2 (32 KiB) [0x03000000 - 0x03007FFF]
            RegisterDevice(0x03000000, new MemoryBackedDevice(0x8000));

            // IO registers [0x04000000 - 0x040003FE]
            // TODO

            // TODO: PPU memory mapping

            // ROM (variable up to 128 MiB) [0x08000000 - 0x0E00FFFF]
            RegisterDevice(0x08000000, new RomDevice(rom));
        }

        public void RegisterDevice(uint address, IBusDevice device)
        {
            Debug.Assert(address == BitUtils.AlignUp(address, 1 << PageGranuality));

            uint targetPageStart = address >> PageGranuality;
            uint targetPageCount = Math.Max(device.MappedSize >> PageGranuality, 1);

            for (uint i = targetPageStart; i < targetPageStart + targetPageCount; i++)
            {
                _pagesDeviceMapping[i] = device;
            }
        }

        private IBusDevice GetAssociatedDeviceAtAddress(uint address)
        {
            uint pageIndex = address >> PageGranuality;
            uint baseAddress = pageIndex << PageGranuality;

            IBusDevice device = _pagesDeviceMapping[pageIndex];

            if (address >= baseAddress + device.MappedSize)
            {
                return null;
            }

            return device;
        }

        private byte UnmappedRead8(uint address)
        {
            // TODO
            return 0;
        }

        private ushort UnmappedRead16(uint address)
        {
            // TODO
            return 0;
        }

        private uint UnmappedRead32(uint address)
        {
            // TODO
            return 0;
        }

        public void UnmappedWrite8(uint address, byte value)
        {
            // TODO
        }

        public void UnmappedWrite16(uint address, ushort value)
        {
            // TODO
        }

        public void UnmappedWrite32(uint address, uint value)
        {
            // TODO
        }

        public byte Read8(uint address, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address);

            // TODO: clock cycles infos.

            if (device == null)
            {
                return UnmappedRead8(address);
            }

            return device.Read8(address);
        }

        public ushort Read16(uint address, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address);

            // TODO: clock cycles infos.

            if (device == null)
            {
                return UnmappedRead16(address);
            }

            return device.Read16(address);
        }

        public uint Read32(uint address, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address);

            // TODO: clock cycles infos.

            if (device == null)
            {
                return UnmappedRead32(address);
            }

            return device.Read32(address);
        }

        public void Write8(uint address, byte value, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address);

            // TODO: clock cycles infos.

            if (device == null)
            {
                UnmappedWrite8(address, value);

                return;
            }

            device.Write8(address, value);
        }

        public void Write16(uint address, ushort value, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address);

            // TODO: clock cycles infos.

            if (device == null)
            {
                UnmappedWrite16(address, value);

                return;
            }

            device.Write16(address, value);
        }

        public void Write32(uint address, uint value, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address);

            // TODO: clock cycles infos.

            if (device == null)
            {
                UnmappedWrite32(address, value);

                return;
            }

            device.Write32(address, value);
        }
    }
}
