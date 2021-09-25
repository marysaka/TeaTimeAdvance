namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleBranchRegister32(InstructionInfo info, uint opcode)
        {
            return $"{info.Name}{GetConditionCodeName(opcode)} {GetRegisterName(opcode, 0)}";
        }

        public static string DisassembleBranchImmediate32(InstructionInfo info, uint opcode)
        {
            return $"{info.Name}{GetConditionCodeName(opcode)} {FormatSignedImmediate24(opcode)}";
        }
    }
}
