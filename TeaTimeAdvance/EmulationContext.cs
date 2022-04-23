using System;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Cpu;
using TeaTimeAdvance.Cpu.State;
using TeaTimeAdvance.Ppu;
using TeaTimeAdvance.Scheduler;

using static TeaTimeAdvance.Ppu.PpuContext;

namespace TeaTimeAdvance
{
    public class EmulationContext
    {
        private SchedulerContext _schedulerContext;
        private BusContext _busContext;
        private CpuContext _cpuContext;
        private PpuContext _ppuContext;

        public EmulationContext()
        {
            _schedulerContext = new SchedulerContext();
            _busContext = new BusContext(_schedulerContext);
            _cpuContext = new CpuContext(_schedulerContext, _busContext);
            _ppuContext = new PpuContext(_schedulerContext, _busContext);
        }

        public void Initialize(byte[] bios, byte[] rom)
        {
            _busContext.Initialize(_ppuContext, bios, rom);

            Reset(true);
        }

        private void Reset(bool skipBios)
        {
            _busContext.Reset();
            _schedulerContext.Reset();
            _cpuContext.Reset();
            _ppuContext.Reset();

            if (skipBios)
            {
                _cpuContext.SetCpuMode(CpuMode.System);
                _cpuContext.SetRegister(CpuRegister.SP_svc, 0x03007FE0);
                _cpuContext.SetRegister(CpuRegister.SP_irq, 0x03007FA0);
                _cpuContext.SetRegister(CpuRegister.SP_sys, 0x03007F00);
                _cpuContext.SetRegister(CpuRegister.PC, 0x08000000);
                _cpuContext.Pipeline.ReloadForArm(_cpuContext);
            }
        }

        public bool IsRunning()
        {
            // TODO
            return true;
        }

        public ReadOnlySpan<uint> ExecuteFrame()
        {
            Execute(CyclesPerRefresh);

            return _ppuContext.State.ScreenBuffer;
        }

        public void Execute(ulong cycles)
        {
            ulong targetCycle = _schedulerContext.CurrentCycle + cycles;

            while (_schedulerContext.CurrentCycle <= targetCycle)
            {
                // TODO: Handle CPU sleep state.
                _cpuContext.Update();
            }
        }
    }
}
