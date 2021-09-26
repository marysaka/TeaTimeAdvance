using TeaTimeAdvance.Cpu.Instruction.Definition;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleSoftwareInterruptDisassembler32(InstructionInfo info, uint opcode)
        {
            SoftwareInterruptFormat format = new SoftwareInterruptFormat
            {
                Opcode = opcode
            };

            return $"{info.Name}{GetConditionCodeName(opcode)} {FormatUnsignedImmediate(format.CommentField)}";
        }
    }
}
