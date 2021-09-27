using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct PCRelativeLoad16Format : IInstructionFormat16
    {
        public ushort Opcode;

        ushort IInstructionFormat16.Opcode => Opcode;

        public CpuRegister Rd => ((IInstructionFormat16)this).GetRegisterByOffset(8);
        public short Immediate => (short)((Opcode & 0x1F) << 2);
    }
}
