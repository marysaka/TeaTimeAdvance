﻿using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct HalfwordDataTransferImmediateFormat : IInstructionFormat
    {
        public uint Opcode;

        uint IInstructionFormat.Opcode => Opcode;

        public byte Offset => (byte)((Opcode & 0xF) | (((Opcode >> 8) & 0xF) << 4));
        public CpuRegister Rd => ((IInstructionFormat)this).GetRegisterByIndex(3);
        public CpuRegister Rn => ((IInstructionFormat)this).GetRegisterByIndex(4);

        public bool IsStore => (Opcode & (1 << 20)) == 0;
        public bool WriteBack => (Opcode & (1 << 21)) != 0;
        public bool IsUp => (Opcode & (1 << 23)) != 0;
        public bool IsPreIndexing => (Opcode & (1 << 24)) != 0;
    }
}
