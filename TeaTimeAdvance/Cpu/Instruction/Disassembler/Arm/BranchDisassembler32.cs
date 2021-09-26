using TeaTimeAdvance.Cpu.Instruction.Definition.Arm;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleBranchExchange32(InstructionInfo info, uint opcode)
        {
            BranchExchangeFormat32 format = new BranchExchangeFormat32
            {
                Opcode = opcode
            };

            return $"{info.Name}{GetConditionCodeName(opcode)} {format.Rn}";
        }

        public static string DisassembleBranch32(InstructionInfo info, uint opcode)
        {
            BranchFormat32 format = new BranchFormat32
            {
                Opcode = opcode
            };

            return $"{info.Name}{GetConditionCodeName(opcode)} {FormatSignedImmediate(format.Offset)}";
        }
    }
}
