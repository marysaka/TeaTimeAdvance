using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleMoveCompareAddSubtractImmediate16(InstructionInfo info, uint opcode)
        {
            MoveCompareAddSubtractImmediateFormat16 format = new MoveCompareAddSubtractImmediateFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, {FormatUnsignedImmediate(format.Offset)}";
        }
    }
}
