using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleLoadStoreHalfwordRegister16(InstructionInfo info, uint opcode)
        {
            LoadStoreHalfwordRegisterFormat16 format = new LoadStoreHalfwordRegisterFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, [{format.Rb}, {format.Ro}]";
        }
    }
}

