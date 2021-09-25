using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct MulitplyLongFormat : IInstructionFormat
    {
        public uint Opcode;

        uint IInstructionFormat.Opcode => Opcode;

        public CpuRegister Rm => ((IInstructionFormat)this).GetRegisterByIndex(0);
        public CpuRegister Rs => ((IInstructionFormat)this).GetRegisterByIndex(2);
        public CpuRegister RdLo => ((IInstructionFormat)this).GetRegisterByIndex(3);
        public CpuRegister RdHi => ((IInstructionFormat)this).GetRegisterByIndex(4);
        public bool SetCondition => (Opcode & (1 << 20)) != 0;
        public bool IsAccumulator => (Opcode & (1 << 21)) != 0;
        public bool IsUnsigned => (Opcode & (1 << 22)) != 0;
    }
}
