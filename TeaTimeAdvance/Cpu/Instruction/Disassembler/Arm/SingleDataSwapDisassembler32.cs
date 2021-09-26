using TeaTimeAdvance.Cpu.Instruction.Definition.Arm;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleSingleDataSwapDisassembler32(InstructionInfo info, uint opcode)
        {
            SingleDataSwapFormat32 format = new SingleDataSwapFormat32
            {
                Opcode = opcode
            };

            return $"{info.Name}{GetConditionCodeName(opcode)} {format.Rd}, {format.Rm}, [{format.Rn}]";
        }
    }
}
