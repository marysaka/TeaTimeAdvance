using TeaTimeAdvance.Cpu.Instruction.Definition;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleMultiplyLong32(InstructionInfo info, uint opcode)
        {
            MulitplyLongFormat format = new MulitplyLongFormat
            {
                Opcode = opcode
            };


            string modifier = string.Empty;

            if (format.SetCondition)
            {
                modifier = "S";
            }

            return $"{info.Name}{modifier}{GetConditionCodeName(opcode)} {format.RdLo}, {format.RdHi}, {format.Rm}, {format.Rs}";
        }
    }
}
