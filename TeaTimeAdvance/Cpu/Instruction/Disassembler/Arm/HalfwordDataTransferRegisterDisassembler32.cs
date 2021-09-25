using TeaTimeAdvance.Cpu.Instruction.Definition;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleHalfwordDataTransferRegister32(InstructionInfo info, uint opcode)
        {
            HalfwordDataTransferRegisterFormat format = new HalfwordDataTransferRegisterFormat
            {
                Opcode = opcode
            };

            string address;
            string expression;

            if (format.IsUp)
            {
                expression = format.Rm.ToString();
            }
            else
            {
                expression = $"-{format.Rm}";
            }

            if (format.IsPreIndexing)
            {
                string writeBackAddress = string.Empty;

                if (format.WriteBack)
                {
                    writeBackAddress = "!";
                }

                address = $"[{format.Rn}, {expression}]{writeBackAddress}";
            }
            else
            {
                address = $"[{format.Rn}], {expression}";
            }

            return $"{info.Name}{GetConditionCodeName(opcode)} {format.Rd}, {address}";
        }
    }
}
