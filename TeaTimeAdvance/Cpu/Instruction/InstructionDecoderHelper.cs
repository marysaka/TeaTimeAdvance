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
            return (CpuRegister)(opcode >> shift) & CpuRegister.UserRegisterMask32;
        }

        public static int SignExtendImmediate(uint value, int valueBits)
        {
            uint sourceMaxValue = (uint)((1 << valueBits) - 1);
            uint targetMinStart = uint.MaxValue - sourceMaxValue;
            uint sourceSignMask = (uint)(1 << (valueBits - 1));

            value &= sourceMaxValue;

            if ((value & sourceSignMask) != 0)
            {
                return (int)(value | targetMinStart);
            }
            else
            {
                return (int)value;
            }
        }

        public static int SignExtendImmediate24(uint immediate)
        {
            return SignExtendImmediate(immediate, 24);
        }

        public static int SignExtendImmediate8(uint immediate)
        {
            return SignExtendImmediate(immediate, 8);
        }
    }
}
