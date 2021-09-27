using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleALUOperations16(InstructionInfo info, uint opcode)
        {
            ALUOperationsFormat16 format = new ALUOperationsFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, {format.Rs}";
        }
    }
}


