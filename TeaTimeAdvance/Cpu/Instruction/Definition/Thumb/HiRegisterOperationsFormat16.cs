using System;
using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct HiRegisterOperationsFormat16 : IInstructionFormat16
    {
        public ushort Opcode;

        ushort IInstructionFormat16.Opcode => Opcode;

        public bool H1 => (Opcode & 1 << 6) != 0;
        public bool H2 => (Opcode & 1 << 7) != 0;

        public CpuRegister Rd => ((IInstructionFormat16)this).GetRegisterByIndex(0, H1);
        public CpuRegister Rs => ((IInstructionFormat16)this).GetRegisterByIndex(1, H2);
    }
}