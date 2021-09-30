using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

using static TeaTimeAdvance.Cpu.Instruction.InstructionDecoderHelper;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleUnconditionalBranch16(InstructionInfo info, uint opcode)
        {
            UnconditionalBranchFormat16 format = new UnconditionalBranchFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {FormatSignedImmediate((int)(format.Offset + ThumbInstructionSize * 2))}";
        }
    }
}
