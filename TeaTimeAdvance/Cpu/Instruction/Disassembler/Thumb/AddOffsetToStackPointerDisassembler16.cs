using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleAddOffsetToStackPointer16(InstructionInfo info, uint opcode)
        {
            AddOffsetToStackPointerFormat16 format = new AddOffsetToStackPointerFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} SP, {FormatUnsignedImmediate((uint)format.Immediate, format.IsNegative)}";
        }
    }
}
