using System;
using System.Runtime.InteropServices;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Device;
using TeaTimeAdvance.Device.IO;
using TeaTimeAdvance.Device.IO.LCD;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Ppu
{
    public class PpuContext
    {
        private static readonly uint Blank = new ColorRGB555(0x1F, 0x1F, 0x1F).ToRGBA8888();

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

        private ReadOnlySpan<ColorRGB555> Mode3FrameBuffer => MemoryMarshal.Cast<byte, ColorRGB555>(VideoMemory)[..(ScreenWidth * ScreenHeight)];

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
            _registersDevice.RegisterWriteCallback(nameof(IORegisters.BGAP), (ref IORegisters data, BusAccessInfo info) => _state.ReloadAffineRegisters(data.BGAP.ToSpan()));

            ref IORegisters registers = ref _registersDevice.Device;

            // TODO: Reset registers


            _scheduler.Register(CyclesPerHorizontalDraw, HandleHorizontalBlank);
            _scheduler.Register(CyclesPerScanline, HandleHorizontalDraw);
            _scheduler.Register(CyclesPerRefresh, HandleVerticalBlank);
        }

        private static bool IsOutOfScreen(int x, int y, int width, int height)
        {
            return x < 0 || y < 0 || x >= width || y >= height;
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

        private void RenderBitmapMode3Scanline(ref IORegisters registers, Span<uint> backgroundScanline)
        {
            BackgroundControl backgroundControl = registers.BGCNT[2];

            Point referencePoint = _state.ReferencePoints[0];

            ushort pa = registers.BGAP[0].PA;
            ushort pc = registers.BGAP[0].PC;

            for (int index = 0; index < backgroundScanline.Length; index++)
            {
                int x = referencePoint.X >> 3;
                int y = referencePoint.Y >> 3;

                if (backgroundControl.Mosaic)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    referencePoint.X += pa;
                    referencePoint.Y += pc;
                }

                if (!IsOutOfScreen(x, y, ScreenWidth, ScreenHeight))
                {
                    backgroundScanline[x] = Mode3FrameBuffer[y * ScreenWidth + x].ToRGBA8888();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void RenderScanline()
        {
            ref IORegisters registers = ref _registersDevice.Device;

            Span<uint> scanlineSpan = _state.ScreenBuffer.Slice(ScreenWidth * registers.VCOUNT, ScreenWidth);

            if (!registers.DISPCNT.ForcedBlank)
            {
                switch (registers.DISPCNT.BackgroundMode)
                {
                    case BackgroundMode.Mode3:
                        RenderBitmapMode3Scanline(ref registers, scanlineSpan);
                        break;
                    default:
                        throw new NotImplementedException(registers.DISPCNT.BackgroundMode.ToString());
                }
            }
            else
            {
                scanlineSpan.Fill(Blank);
            }
        }
    }
}
