using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Cpu.State;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Cpu
{
    public class CpuContext
    {
        public CpuState State { get; }
        public CpuPipeline Pipeline { get; }
        public SchedulerContext Scheduler { get; }
        public BusContext BusContext { get; }

        public CpuContext(SchedulerContext scheduler, BusContext busContext)
        {
            Scheduler = scheduler;
            BusContext = busContext;

            State = new CpuState();
            Pipeline = new CpuPipeline();
        }

        public void Reset()
        {
            Pipeline.Reset();
            State.Reset();
        }

        public void SetRegister(CpuRegister register, uint value)
        {
            ref uint reg = ref State.Register(register);

            reg = value;
        }

        public uint GetRegister(CpuRegister register)
        {
            return State.Register(register);
        }

        public void SetCpuMode(CpuMode mode)
        {
            State.SetCpuMode(mode);
        }

        public void Update()
        {
            Pipeline.Update(this);
        }
    }
}
