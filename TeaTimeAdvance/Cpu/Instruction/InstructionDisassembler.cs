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
                return $"#-0x{value:X}";
            }
            else
            {
                return $"#0x{value:X}";
            }
        }

        private static string FormatUnsignedImmediate(uint value)
        {
            return $"#0x{value:X}";
        }

        private static string FormatSignedImmediate24(uint rawValue)
        {
            return FormatSignedImmediate(DecodeS24(rawValue));
        }

        private static string GetRegisterName(uint opcode, int shift)
        {
            return GetCpuRegisterFromOpcode(opcode, shift).ToString();
        }

        public static string GetConditionCodeName(uint opcode)
        {
            CpuConditionCode conditionCode = GetConditionCodeFromOpcode(opcode);

            if (conditionCode == CpuConditionCode.AL)
            {
                return string.Empty;
            }

            return conditionCode.ToString();
        }
    }
}
