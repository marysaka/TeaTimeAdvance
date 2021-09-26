using TeaTimeAdvance.Cpu.Instruction.Definition;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleBranchExchange32(InstructionInfo info, uint opcode)
        {
            BranchExchangeFormat format = new BranchExchangeFormat
            {
                Opcode = opcode
            };

            return $"{info.Name}{GetConditionCodeName(opcode)} {format.Rn}";
        }

        public static string DisassembleBranch32(InstructionInfo info, uint opcode)
        {
            BranchFormat format = new BranchFormat
            {
                Opcode = opcode
            };

            return $"{info.Name}{GetConditionCodeName(opcode)} {FormatSignedImmediate(format.Offset)}";
        }
    }
}
