using System.Diagnostics;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Arm
{
    public interface IInstructionFormat32
    {
        uint Opcode { get; }

        CpuConditionCode ConditionCode => (CpuConditionCode)(Opcode >> 28);

        /// <summary>
        /// Get a <see cref="CpuRegister"/> by index.
        /// </summary>
        /// <param name="index">index per group of 4 bits.</param>
        /// <returns>The <see cref="CpuRegister"/> at that position</returns>
        public CpuRegister GetRegisterByIndex(int index)
        {
            const int BitsPerByte = 8;
            const int BitsPerCpuRegister = 4;

            Debug.Assert(index < (sizeof(uint) * BitsPerByte) / BitsPerCpuRegister);

            return (CpuRegister)(Opcode >> (index * BitsPerCpuRegister)) & CpuRegister.UserRegisterMask;
        }
    }
}
