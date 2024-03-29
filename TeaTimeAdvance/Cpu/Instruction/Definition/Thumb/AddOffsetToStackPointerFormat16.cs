﻿using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct AddOffsetToStackPointerFormat16 : IInstructionFormat16
    {
        public ushort Opcode;

        ushort IInstructionFormat16.Opcode => Opcode;

        public short Immediate => (short)((Opcode & 0x7F) << 2);
        public bool IsNegative => (Opcode & (1 << 7)) != 0;
    }
}
