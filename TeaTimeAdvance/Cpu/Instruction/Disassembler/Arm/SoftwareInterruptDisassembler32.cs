using TeaTimeAdvance.Cpu.Instruction.Definition.Arm;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleSoftwareInterruptDisassembler32(InstructionInfo info, uint opcode)
        {
            SoftwareInterruptFormat32 format = new SoftwareInterruptFormat32
            {
                Opcode = opcode
            };

            return $"{info.Name}{GetConditionCodeName(opcode)} {FormatUnsignedImmediate(format.CommentField)}";
        }
    }
}
