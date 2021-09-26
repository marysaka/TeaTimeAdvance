using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Arm
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct SingleDataSwapFormat32 : IInstructionFormat32
    {
        public uint Opcode;

        uint IInstructionFormat32.Opcode => Opcode;

        public CpuRegister Rm => ((IInstructionFormat32)this).GetRegisterByIndex(0);
        public CpuRegister Rd => ((IInstructionFormat32)this).GetRegisterByIndex(3);
        public CpuRegister Rn => ((IInstructionFormat32)this).GetRegisterByIndex(4);
        public bool IsByteTransfer => (Opcode & (1 << 22)) != 0;
    }
}
