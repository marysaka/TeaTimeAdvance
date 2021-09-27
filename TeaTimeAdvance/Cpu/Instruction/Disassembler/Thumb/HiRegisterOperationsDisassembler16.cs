using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleHiRegisterOperations16(InstructionInfo info, uint opcode)
        {
            HiRegisterOperationsFormat16 format = new HiRegisterOperationsFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, {format.Rs}";
        }

        public static string DisassembleBranchExchange16(InstructionInfo info, uint opcode)
        {
            HiRegisterOperationsFormat16 format = new HiRegisterOperationsFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rs}";
        }
    }
}


