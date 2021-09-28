﻿using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct LoadStoreWithImmediateOffsetFormat16 : IInstructionFormat16
    {
        public ushort Opcode;

        ushort IInstructionFormat16.Opcode => Opcode;

        public CpuRegister Rd => ((IInstructionFormat16)this).GetRegisterByIndex(0);
        public CpuRegister Rb => ((IInstructionFormat16)this).GetRegisterByIndex(1);
        public byte Offset => (byte)((Opcode >> 6) & 0x1F);
        public bool IsStore => (Opcode & (1 << 11)) == 0;
        public bool IsByteTransfer => (Opcode & (1 << 12)) != 0;
    }
}
