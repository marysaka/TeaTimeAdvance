namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleBranchAndExchange32(InstructionInfo info, uint opcode)
        {
            return $"{info.Name}{GetConditionCodeName(opcode)} {GetRegisterName(opcode, 0)}";
        }

        public static string DisassembleBranch32(InstructionInfo info, uint opcode)
        {
            return $"{info.Name}{GetConditionCodeName(opcode)} {FormatSignedImmediate24(opcode)}";
        }
    }
}
