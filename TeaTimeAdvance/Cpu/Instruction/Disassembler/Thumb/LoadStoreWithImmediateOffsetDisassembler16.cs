using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleLoadStoreWithImmediateOffset16(InstructionInfo info, uint opcode)
        {
            LoadStoreWithImmediateOffsetFormat16 format = new LoadStoreWithImmediateOffsetFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, [{format.Rb}, {FormatUnsignedImmediate(format.Offset)}]";
        }
    }
}
