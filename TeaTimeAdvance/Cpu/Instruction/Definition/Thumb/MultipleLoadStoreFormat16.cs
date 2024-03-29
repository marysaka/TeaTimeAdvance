﻿using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct MultipleLoadStoreFormat16 : IInstructionFormat16
    {
        public ushort Opcode;

        ushort IInstructionFormat16.Opcode => Opcode;

        public CpuRegister Rb => ((IInstructionFormat16)this).GetRegisterByOffset(8);
        public bool IsStore => (Opcode & (1 << 11)) == 0;

        public bool HasCpuRegisterInRegisterList(CpuRegister register)
        {
            if (register > CpuRegister.UserRegisterMask16)
            {
                return false;
            }

            register &= CpuRegister.UserRegisterMask16;

            return (Opcode & (1 << (int)register)) != 0;
        }
    }
}
