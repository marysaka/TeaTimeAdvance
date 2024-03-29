﻿using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Arm
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct DataProcessingFormat32 : IInstructionFormat32
    {
        public uint Opcode;

        uint IInstructionFormat32.Opcode => Opcode;

        public CpuRegister Rd => ((IInstructionFormat32)this).GetRegisterByIndex(3);
        public CpuRegister Rn => ((IInstructionFormat32)this).GetRegisterByIndex(4);

        public bool SetCondition => (Opcode & (1 << 20)) != 0;
        public bool IsImmediate => (Opcode & (1 << 25)) != 0;

        // Register form
        public CpuRegister Rm => ((IInstructionFormat32)this).GetRegisterByIndex(0);
        public byte Shift => (byte)(Opcode >> 4);
        public bool IsShiftImmediate => (Shift & 1) == 0;
        public CpuShift ShiftType => (CpuShift)(Shift >> 1) & CpuShift.Mask;

        // Register shift register form
        public CpuRegister Rs => (CpuRegister)(Shift >> 4);

        // Register shift immediate form
        public byte ShiftImmediate => (byte)((Shift >> 3) & 0x1F);

        // Immediate form
        public byte Imm => (byte)Opcode;
        public byte Rotate => (byte)(((Opcode >> 8) & 0xF) << 1);
    }
}
