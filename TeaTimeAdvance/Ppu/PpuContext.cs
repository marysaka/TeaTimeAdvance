using System;
using System.Drawing;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Device;
using TeaTimeAdvance.Device.IO;
using TeaTimeAdvance.Device.IO.LCD;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Ppu
{
    public class PpuContext
    {
        private static readonly Color Blank = ConvertRGB555(0x1F, 0x1F, 0x1F);

        public const int ScreenWidth = 240;
        public const int ScreenHeight = 160;
        public const int HorizontalBlank = 68;
        public const int VerticalBlank = 68;

        public const int TotalScreenWidth = ScreenWidth + HorizontalBlank;
        public const int TotalScreenHeight = ScreenHeight + VerticalBlank;

        public const int CyclesPerPixel = 4;
        public const int CyclesPerHorizontalDraw = CyclesPerPixel * ScreenWidth;
        public const int CyclesPerHorizontalBlank = CyclesPerPixel * HorizontalBlank;
        public const int CyclesPerScanline = CyclesPerHorizontalDraw + CyclesPerHorizontalBlank;
        public const int CyclesPerVerticalDraw = CyclesPerScanline * ScreenHeight;
        public const int CyclesPerVerticalBlank = CyclesPerScanline * VerticalBlank;
        public const int CyclesPerRefresh = CyclesPerVerticalDraw + CyclesPerVerticalBlank;

        private const int PaletteMemorySize = 0x1000;
        private const int VideoMemorySize = 0x18000;
        private const int ObjectAttributesMemorySize = 0x1000;

        private PpuState _state;
        private StructBackedDevice<IORegisters> _registersDevice;
        private SchedulerContext _scheduler;

        public byte[] PaletteMemory { get; }
        public byte[] VideoMemory { get; }
        public byte[] ObjectAttributesMemory { get; }

        public PpuContext(SchedulerContext scheduler, BusContext busContext)
        {
            _state = new PpuState();
            _scheduler = scheduler;
            _registersDevice = busContext.Registers;

            PaletteMemory = new byte[PaletteMemorySize];
            VideoMemory = new byte[VideoMemorySize];
            ObjectAttributesMemory = new byte[ObjectAttributesMemorySize];
        }

        public void Reset()
        {
            ref IORegisters registers = ref _registersDevice.Device;

            // TODO: Reset registers


            _scheduler.Register(CyclesPerHorizontalDraw, HandleHorizontalBlank);
            _scheduler.Register(CyclesPerScanline, HandleHorizontalDraw);
            _scheduler.Register(CyclesPerRefresh, HandleVerticalBlank);
        }

        private static Color ConvertRGB555(byte r, byte g, byte b)
        {
            r <<= 3;
            g <<= 3;
            b <<= 3;

            return Color.FromArgb(r, g, b);
        }

        private void HandleHorizontalDraw()
        {
            ref IORegisters registers = ref _registersDevice.Device;

            registers.VCOUNT++;

            // TODO: not elegant, make this cleaner
            if (registers.DISPSTAT.VerticalCount == registers.VCOUNT)
            {
                registers.DISPSTAT.Flags |= LCDStatusFlags.VerticalCount;
            }
            else
            {
                registers.DISPSTAT.Flags &= ~LCDStatusFlags.VerticalCount;
            }

            // TODO: not elegant, make this cleaner
            if (registers.VCOUNT >= ScreenHeight && registers.VCOUNT < TotalScreenHeight)
            {
                registers.DISPSTAT.Flags |= LCDStatusFlags.VerticalBlank;
            }
            else
            {
                registers.DISPSTAT.Flags &= ~LCDStatusFlags.VerticalBlank;
            }

            registers.DISPSTAT.Flags &= ~LCDStatusFlags.HorizontalBlank;

            if (registers.DISPSTAT.Flags.HasFlag(LCDStatusFlags.VerticalCount) && registers.DISPSTAT.Flags.HasFlag(LCDStatusFlags.VerticalCountIRQ))
            {
                // TODO: IRQ
                throw new NotImplementedException();
            }

            _scheduler.Register(CyclesPerScanline, HandleHorizontalDraw);
        }

        private void HandleHorizontalBlank()
        {
            ref IORegisters registers = ref _registersDevice.Device;

            if (registers.VCOUNT < ScreenHeight)
            {
                RenderScanline();
            }

            registers.DISPSTAT.Flags |= LCDStatusFlags.HorizontalBlank;

            if (registers.DISPSTAT.Flags.HasFlag(LCDStatusFlags.HorizontalBlankIRQ))
            {
                // TODO: IRQ
                throw new NotImplementedException();
            }

            // TODO: the reset

            _scheduler.Register(CyclesPerHorizontalDraw, HandleHorizontalBlank);
        }

        private void HandleVerticalBlank()
        {
            ref IORegisters registers = ref _registersDevice.Device;

            registers.VCOUNT = 0;

            if (registers.DISPSTAT.Flags.HasFlag(LCDStatusFlags.VerticalBlankIRQ))
            {
                // TODO: IRQ
                throw new NotImplementedException();
            }

            _scheduler.Register(CyclesPerRefresh, HandleVerticalBlank);
        }

        private void RenderScanline()
        {
            ref IORegisters registers = ref _registersDevice.Device;

            if (!registers.DISPCNT.ForcedBlank)
            {
                throw new NotImplementedException();
            }
            else
            {
                _state.ScreenBuffer.Slice(ScreenWidth * registers.VCOUNT, ScreenWidth).Fill(Blank);
            }
        }
    }
}
