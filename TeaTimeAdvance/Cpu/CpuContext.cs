using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu
{
    public class CpuContext
    {
        private CpuState _state;

        public CpuContext()
        {
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
    }
}
