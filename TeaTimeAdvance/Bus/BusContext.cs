using System;
using System.Diagnostics;
using TeaTimeAdvance.Common;
using TeaTimeAdvance.Device;
using TeaTimeAdvance.Device.IO;
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

        public StructBackedDevice<IORegisters> Registers { get; }

        public BusContext(SchedulerContext schedulerContext)
        {
            _schedulerContext = schedulerContext;
            _pagesDeviceMapping = new IBusDevice[LastPage + 1];
            Registers = new StructBackedDevice<IORegisters>();

            Registers.RegisterWriteCallback(nameof(IORegisters.DISPCNT), (ref IORegisters data) =>
            {
                Console.WriteLine("TOUCHED DISPSTAT");
            });
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
            RegisterDevice(0x04000000, Registers);

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

        private IBusDevice GetAssociatedDeviceAtAddress(uint address, out uint baseAddress)
        {
            uint pageIndex = address >> PageGranuality;
            baseAddress = pageIndex << PageGranuality;

            IBusDevice device = _pagesDeviceMapping[pageIndex];

            if (device == null || address >= baseAddress + device.MappedSize)
            {
                return null;
            }

            return device;
        }

        private byte UnmappedRead8(uint address)
        {
            Console.Error.WriteLine($"Invalid 8 bits read at 0x{address:X8}");

            // TODO
            return 0;
        }

        private ushort UnmappedRead16(uint address)
        {
            Console.Error.WriteLine($"Invalid 16 bits read at 0x{address:X8}");

            // TODO
            return 0;
        }

        private uint UnmappedRead32(uint address)
        {
            Console.Error.WriteLine($"Invalid 32 bits read at 0x{address:X8}");

            // TODO
            return 0;
        }

        public void UnmappedWrite8(uint address, byte value)
        {
            Console.Error.WriteLine($"Invalid 8 bits write at 0x{address:X8} (value = 0x{value:X2})");

            // TODO
        }

        public void UnmappedWrite16(uint address, ushort value)
        {
            Console.Error.WriteLine($"Invalid 16 bits write at 0x{address:X8} (value = 0x{value:X4})");

            // TODO
        }

        public void UnmappedWrite32(uint address, uint value)
        {
            Console.Error.WriteLine($"Invalid 32 bits write at 0x{address:X8} (value = 0x{value:X8})");

            // TODO
        }

        public byte Read8(uint address, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address, out uint baseAddress);

            // TODO: clock cycles infos.

            if (device == null)
            {
                return UnmappedRead8(address);
            }

            return device.Read8(baseAddress, address);
        }

        public ushort Read16(uint address, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address, out uint baseAddress);

            // TODO: clock cycles infos.

            if (device == null)
            {
                return UnmappedRead16(address);
            }

            return device.Read16(baseAddress, address);
        }

        public uint Read32(uint address, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address, out uint baseAddress);

            // TODO: clock cycles infos.

            if (device == null)
            {
                return UnmappedRead32(address);
            }

            return device.Read32(baseAddress, address);
        }

        public void Write8(uint address, byte value, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address, out uint baseAddress);

            // TODO: clock cycles infos.

            if (device == null)
            {
                UnmappedWrite8(address, value);

                return;
            }

            device.Write8(baseAddress, address, value);
        }

        public void Write16(uint address, ushort value, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address, out uint baseAddress);

            // TODO: clock cycles infos.

            if (device == null)
            {
                UnmappedWrite16(address, value);

                return;
            }

            device.Write16(baseAddress, address, value);
        }

        public void Write32(uint address, uint value, BusAccessType accessType)
        {
            IBusDevice device = GetAssociatedDeviceAtAddress(address, out uint baseAddress);

            // TODO: clock cycles infos.

            if (device == null)
            {
                UnmappedWrite32(address, value);

                return;
            }

            device.Write32(baseAddress, address, value);
        }
    }
}
