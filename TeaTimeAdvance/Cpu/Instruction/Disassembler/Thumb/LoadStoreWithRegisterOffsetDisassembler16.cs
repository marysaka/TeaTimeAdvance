using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleLoadStoreWithRegisterOffset16(InstructionInfo info, uint opcode)
        {
            LoadStoreWithRegisterOffsetFormat16 format = new LoadStoreWithRegisterOffsetFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, [{format.Rb}, {format.Ro}]";
        }
    }
}

