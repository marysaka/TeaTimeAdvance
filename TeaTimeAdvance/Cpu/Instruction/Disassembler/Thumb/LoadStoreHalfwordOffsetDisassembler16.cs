using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleLoadStoreHalfwordOffset16(InstructionInfo info, uint opcode)
        {
            LoadStoreHalfwordOffsetFormat16 format = new LoadStoreHalfwordOffsetFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, [{format.Rb}, {FormatUnsignedImmediate(format.Offset)}]";
        }
    }
}

