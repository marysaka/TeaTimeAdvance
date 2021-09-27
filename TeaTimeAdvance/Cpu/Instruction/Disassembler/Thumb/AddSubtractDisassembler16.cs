using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleAddSubtract16(InstructionInfo info, uint opcode)
        {
            AddSubtractFormat16 format = new AddSubtractFormat16
            {
                Opcode = (ushort)opcode
            };

            if (format.IsImmediate)
            {
                return $"{info.Name} {format.Rd}, {format.Rs}, {FormatUnsignedImmediate(format.Offset)}";
            }
            else
            {
                return $"{info.Name} {format.Rd}, {format.Rs}, {format.Rn}";
            }
        }
    }
}
