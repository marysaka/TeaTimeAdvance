using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct ALUOperationsFormat16 : IInstructionFormat16
    {
        public ushort Opcode;

        ushort IInstructionFormat16.Opcode => Opcode;

        public CpuRegister Rd => ((IInstructionFormat16)this).GetRegisterByIndex(0);
        public CpuRegister Rs => ((IInstructionFormat16)this).GetRegisterByIndex(1);
    }
}