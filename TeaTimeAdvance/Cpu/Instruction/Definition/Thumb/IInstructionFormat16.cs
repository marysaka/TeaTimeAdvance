﻿using System.Diagnostics;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    public interface IInstructionFormat16
    {
        const int BitsPerByte = 8;

        ushort Opcode { get; }

        public CpuRegister GetRegisterByOffset(int offset, bool isHigh = false)
        {
            Debug.Assert(offset < sizeof(ushort) * BitsPerByte);

            CpuRegister result = (CpuRegister)(Opcode >> offset) & CpuRegister.UserRegisterMask16;

            if (isHigh)
            {
                result = (uint)result + CpuRegister.R7;
            }

            return result;
        }

        /// <summary>
        /// Get a <see cref="CpuRegister"/> by index.
        /// </summary>
        /// <param name="index">index per group of 3 bits.</param>
        /// <param name="isHigh">If the bits represent the top part of the register space.</param>
        /// <returns>The <see cref="CpuRegister"/> at that position</returns>
        public CpuRegister GetRegisterByIndex(int index, bool isHigh = false)
        {
            const int BitsPerCpuRegister = 3;

            Debug.Assert(index < (sizeof(ushort) * BitsPerByte) / BitsPerCpuRegister);

            CpuRegister result = (CpuRegister)(Opcode >> (index * BitsPerCpuRegister)) & CpuRegister.UserRegisterMask16;

            if (isHigh)
            {
                result = (uint)result + CpuRegister.R7;
            }

            return result;
        }
    }
}
