using System;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Cpu.State;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Cpu
{
    public class CpuContext
    {
        private CpuState _state;
        private SchedulerContext _schedulerContext;
        private BusContext _busContext;

        public CpuContext(SchedulerContext schedulerContext, BusContext busContext)
        {
            _schedulerContext = schedulerContext;
            _busContext = busContext;
            _state = new CpuState();
            _state.Reset();
        }

        public void Reset()
        {
            _state.Reset();
        }

        public void SetRegister(CpuRegister register, uint value)
        {
            ref uint reg = ref _state.Register(register);

            reg = value;
        }

        public uint GetRegister(CpuRegister register)
        {
            return _state.Register(register);
        }

        public void SetCpuMode(CpuMode mode)
        {
            _state.SetCpuMode(mode);
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
