using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static class InstructionDecoderHelper
    {
        public const uint ThumbInstructionSize = sizeof(ushort);
        public const uint ArmInstructionSize = sizeof(uint);

        private const int ConditionCodeOpcodeStart = 28;

        public static CpuConditionCode GetConditionCodeFromOpcode(uint opcode)
        {
            return (CpuConditionCode)(opcode >> ConditionCodeOpcodeStart) & CpuConditionCode.Mask;
        }

        public static CpuRegister GetCpuRegisterFromOpcode(uint opcode, int shift)
        {
            return (CpuRegister)(opcode >> shift) & CpuRegister.UserRegisterMask;
        }

        public static int DecodeS24(uint value)
        {
            return (int)(((ulong)value << 40) >> 38);
        }
    }
}
