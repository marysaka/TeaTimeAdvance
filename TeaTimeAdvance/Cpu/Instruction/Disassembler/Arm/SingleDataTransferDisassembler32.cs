using System;
using TeaTimeAdvance.Cpu.Instruction.Definition;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        private static string FormatSingleDataTransferAddressExpression(SingleDataTransferFormat format)
        {
            if (format.IsImmediate)
            {
                return FormatUnsignedImmediate(format.ImmediateOffset, !format.IsUp);
            }
            else
            {
                string shiftType = FormatShiftType(format.ShiftType);

                string shiftArgument;

                if (!format.IsShiftImmediate)
                {
                    throw new NotImplementedException();
                }

                if (format.ShiftImmediate == 0)
                {
                    if (format.IsUp)
                    {
                        return format.Rm.ToString();
                    }
                    else
                    {
                        return $"-{format.Rm}";
                    }
                }

                shiftArgument = FormatUnsignedImmediate(format.ShiftImmediate, !format.IsUp);

                return $"{format.Rm}, {shiftType} {shiftArgument}";
            }
        }

        public static string DisassembleSingleDataTransfer32(InstructionInfo info, uint opcode)
        {
            SingleDataTransferFormat format = new SingleDataTransferFormat
            {
                Opcode = opcode
            };

            string modifier = string.Empty;

            if (format.IsByteTransfer)
            {
                modifier += 'B';
            }

            if (format.WriteBack && !format.IsPreIndexing)
            {
                modifier += 'T';
            }

            string address;

            if (format.IsImmediate && format.ImmediateOffset == 0)
            {
                address = format.Rn.ToString();
            }
            else
            {
                string expression = FormatSingleDataTransferAddressExpression(format);

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
            }

            return $"{info.Name}{modifier}{GetConditionCodeName(opcode)} {format.Rd}, {address}";
        }
    }
}

