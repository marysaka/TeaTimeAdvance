using TeaTimeAdvance.Cpu.Instruction.Definition;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleSingleDataSwapDisassembler32(InstructionInfo info, uint opcode)
        {
            SingleDataSwapFormat format = new SingleDataSwapFormat
            {
                Opcode = opcode
            };

            return $"{info.Name}{GetConditionCodeName(opcode)} {format.Rd}, {format.Rm}, [{format.Rn}]";
        }
    }
}
