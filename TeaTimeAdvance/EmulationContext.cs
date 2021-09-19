using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaTimeAdvance.Cpu;
using TeaTimeAdvance.Cpu.State;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance
{
    public class EmulationContext
    {
        private const int VerticalDimensionsCycles = 280896;

        private SchedulerContext _schedulerContext;
        private CpuContext _cpuContext;

        public EmulationContext()
        {
            _schedulerContext = new SchedulerContext();
            _cpuContext = new CpuContext();
        }

        public void LoadBios(string biosPath)
        {
            throw new NotImplementedException();
        }

        public void LoadRom(string romPath)
        {
            throw new NotImplementedException();
        }

        public void Reset(bool skipBios)
        {
            _cpuContext.Reset();

            if (skipBios)
            {
                _cpuContext.SetCpuMode(CpuMode.System);
                _cpuContext.SetRegister(CpuRegister.SP_svc, 0x03007FE0);
                _cpuContext.SetRegister(CpuRegister.SP_irq, 0x03007FA0);
                _cpuContext.SetRegister(CpuRegister.SP_sys, 0x03007F00);
                _cpuContext.SetRegister(CpuRegister.PC, 0x08000000);
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

        public void Execute(int cycles)
        {
            throw new NotImplementedException();
        }
    }
}
