using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleLoadStoreHalfword16(InstructionInfo info, uint opcode)
        {
            LoadStoreHalfwordFormat16 format = new LoadStoreHalfwordFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, [{format.Rb}, {format.Ro}]";
        }
    }
}

