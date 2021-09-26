using TeaTimeAdvance.Cpu.Instruction.Definition.Arm;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleMultiplyLong32(InstructionInfo info, uint opcode)
        {
            MulitplyLongFormat32 format = new MulitplyLongFormat32
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
