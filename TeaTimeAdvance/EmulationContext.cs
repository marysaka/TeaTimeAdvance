using System;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Cpu;
using TeaTimeAdvance.Cpu.State;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance
{
    public class EmulationContext
    {
        private const int VerticalDimensionsCycles = 280896;

        private SchedulerContext _schedulerContext;
        private BusContext _busContext;
        private CpuContext _cpuContext;

        public EmulationContext()
        {
            _schedulerContext = new SchedulerContext();
            _busContext = new BusContext(_schedulerContext);
            _cpuContext = new CpuContext(_schedulerContext, _busContext);
        }

        public void Initialize(ReadOnlySpan<byte> bios, ReadOnlySpan<byte> rom)
        {
            _busContext.Initialize(bios, rom);

            Reset(true);
        }

        private void Reset(bool skipBios)
        {
            _cpuContext.Reset();

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

        public void ExecuteFrame()
        {
            Execute(VerticalDimensionsCycles);
        }

        public void Execute(ulong cycles)
        {
            ulong targetCycle = _schedulerContext.CurrentCycle + cycles;

            while (_schedulerContext.CurrentCycle <= targetCycle)
            {
                _cpuContext.Update();
            }
        }
    }
}
