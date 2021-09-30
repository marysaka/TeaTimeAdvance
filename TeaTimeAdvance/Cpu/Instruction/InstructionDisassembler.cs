using TeaTimeAdvance.Cpu.State;
using static TeaTimeAdvance.Cpu.Instruction.InstructionDecoderHelper;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        private static string FormatSignedImmediate(int value)
        {
            if (value < 0)
            {
                return $"#-0x{-value:X}";
            }
            else
            {
                return $"#0x{value:X}";
            }
        }

        private static string FormatUnsignedImmediate(uint value, bool forceNegative = false)
        {
            if (forceNegative)
            {
                return $"#-0x{value:X}";
            }
            else
            {
                return $"#0x{value:X}";
            }
        }

        public static string GetConditionCodeName(uint opcode)
        {
            return GetConditionCodeName(GetConditionCodeFromOpcode(opcode));
        }

        public static string GetConditionCodeName(CpuConditionCode conditionCode)
        {
            if (conditionCode == CpuConditionCode.AL)
            {
                return string.Empty;
            }

            return conditionCode.ToString();
        }
    }
}
