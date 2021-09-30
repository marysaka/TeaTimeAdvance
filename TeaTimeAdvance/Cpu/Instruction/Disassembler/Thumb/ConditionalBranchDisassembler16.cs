using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

using static TeaTimeAdvance.Cpu.Instruction.InstructionDecoderHelper;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleConditionalBranch16(InstructionInfo info, uint opcode)
        {
            ConditionalBranchFormat16 format = new ConditionalBranchFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name}{GetConditionCodeName(format.ConditionCode)} {FormatSignedImmediate((int)(format.Offset + ThumbInstructionSize * 2))}";
        }
    }
}
