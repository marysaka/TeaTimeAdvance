using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleSoftwareInterruptDisassembler16(InstructionInfo info, uint opcode)
        {
            SoftwareInterruptFormat16 format = new SoftwareInterruptFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {FormatUnsignedImmediate(format.CommentField)}";
        }
    }
}
