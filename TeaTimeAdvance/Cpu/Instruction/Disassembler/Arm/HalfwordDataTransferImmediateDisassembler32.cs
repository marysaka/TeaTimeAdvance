using TeaTimeAdvance.Cpu.Instruction.Definition;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleHalfwordDataTransferImmediate32(InstructionInfo info, uint opcode)
        {
            HalfwordDataTransferImmediateFormat format = new HalfwordDataTransferImmediateFormat
            {
                Opcode = opcode
            };

            string address;
            string expression = FormatUnsignedImmediate(format.Offset, !format.IsUp);

            if (format.IsPreIndexing)
            {
                string writeBackAddress = string.Empty;

                if (format.WriteBack)
                {
                    writeBackAddress = "!";
                }

                if (format.Offset == 0)
                {
                    address = $"[{format.Rn}]{writeBackAddress}";
                }
                else
                {
                    address = $"[{format.Rn}, {expression}]{writeBackAddress}";
                }
            }
            else
            {
                if (format.Offset == 0)
                {
                    address = $"[{format.Rn}]";
                }
                else
                {
                    address = $"[{format.Rn}], {expression}";
                }
            }

            return $"{info.Name}{GetConditionCodeName(opcode)} {format.Rd}, {address}";
        }
    }
}
