using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Arm
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct MulitplyLongFormat32 : IInstructionFormat32
    {
        public uint Opcode;

        uint IInstructionFormat32.Opcode => Opcode;

        public CpuRegister Rm => ((IInstructionFormat32)this).GetRegisterByIndex(0);
        public CpuRegister Rs => ((IInstructionFormat32)this).GetRegisterByIndex(2);
        public CpuRegister RdLo => ((IInstructionFormat32)this).GetRegisterByIndex(3);
        public CpuRegister RdHi => ((IInstructionFormat32)this).GetRegisterByIndex(4);
        public bool SetCondition => (Opcode & (1 << 20)) != 0;
        public bool IsAccumulator => (Opcode & (1 << 21)) != 0;
        public bool IsUnsigned => (Opcode & (1 << 22)) != 0;
    }
}
