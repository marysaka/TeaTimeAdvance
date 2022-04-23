using System;
using System.Diagnostics;
using TeaTimeAdvance.Common;
using TeaTimeAdvance.Device;
using TeaTimeAdvance.Device.IO;
using TeaTimeAdvance.Ppu;
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
        }

        public void Initialize(PpuContext ppuContext, byte[] bios, byte[] rom)
        {
            // BIOS [0x00000000 - 0x00003FFF]
            RegisterDevice(0x00000000, new BiosDevice(bios));

            // WRAM 1 (256 KiB) [0x02000000 - 0x0203FFFF]
            RegisterDevice(0x02000000, new MemoryBackedDevice(0x40000, new byte[]
            {
                3, 3, 6,
                3, 3, 6,
            }));

            // WRAM 2 (32 KiB) [0x03000000 - 0x03007FFF]
            RegisterDevice(0x03000000, new MemoryBackedDevice(0x8000));

            // IO registers [0x04000000 - 0x040003FE]
            RegisterDevice(0x04000000, Registers);

            // BG/OBJ Palette RAM (1 KiB) [0x05000000 - 0x050003FF]
            RegisterDevice(0x05000000, new MemoryBackedDevice(ppuContext.PaletteMemory, new byte[]
            {
                1, 1, 2,
                1, 1, 2,
            }));

            // VRAM - Video RAM (96 KiB) [0x06000000 - 0x06017FFF]
            RegisterDevice(0x06000000, new MemoryBackedDevice(ppuContext.VideoMemory, new byte[]
            {
                1, 1, 2,
                1, 1, 2,
            }));

            // OAM - OBJ Attributes (1 KiB) [0x07000000 - 0x070003FF]
            RegisterDevice(0x07000000, new MemoryBackedDevice(ppuContext.ObjectAttributesMemory));

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

            UpdateCycles(1);

            // TODO
            return 0;
        }

        private ushort UnmappedRead16(uint address)
        {
            Console.Error.WriteLine($"Invalid 16 bits read at 0x{address:X8}");

            UpdateCycles(1);

            // TODO
            return 0;
        }

        private uint UnmappedRead32(uint address)
        {
            Console.Error.WriteLine($"Invalid 32 bits read at 0x{address:X8}");

            UpdateCycles(1);

            // TODO
            return 0;
        }

        public void UnmappedWrite8(uint address, byte value)
        {
            Console.Error.WriteLine($"Invalid 8 bits write at 0x{address:X8} (value = 0x{value:X2})");

            UpdateCycles(1);

            // TODO
        }

        public void UnmappedWrite16(uint address, ushort value)
        {
            Console.Error.WriteLine($"Invalid 16 bits write at 0x{address:X8} (value = 0x{value:X4})");

            UpdateCycles(1);

            // TODO
        }

        public void UnmappedWrite32(uint address, uint value)
        {
            Console.Error.WriteLine($"Invalid 32 bits write at 0x{address:X8} (value = 0x{value:X8})");

            UpdateCycles(1);

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

            device.UpdateScheduler(this, address, accessType, BusAccessInfo.Read | BusAccessInfo.Memory8);
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

            device.UpdateScheduler(this, address, accessType, BusAccessInfo.Read | BusAccessInfo.Memory16);
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

            device.UpdateScheduler(this, address, accessType, BusAccessInfo.Read | BusAccessInfo.Memory32);
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

            device.UpdateScheduler(this, address, accessType, BusAccessInfo.Write | BusAccessInfo.Memory8);
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

            device.UpdateScheduler(this, address, accessType, BusAccessInfo.Write | BusAccessInfo.Memory16);
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

            device.UpdateScheduler(this, address, accessType, BusAccessInfo.Write | BusAccessInfo.Memory32);
            device.Write32(baseAddress, address, value);
        }

        public void UpdateCycles(ulong cycles)
        {
            _schedulerContext.UpdateCycles(cycles);
        }
    }
}
