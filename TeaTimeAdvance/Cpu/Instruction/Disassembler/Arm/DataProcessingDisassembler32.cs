using System;
using System.Numerics;
using TeaTimeAdvance.Cpu.Instruction.Definition;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        private static string FormatShiftType(CpuShift shift)
        {
            switch (shift)
            {
                case CpuShift.LogicalLeft:
                    return "LSL";
                case CpuShift.LogicalRight:
                    return "LSR";
                case CpuShift.ArithmeticRight:
                    return "ASR";
                case CpuShift.RotationRight:
                    return "ROR";
                default:
                    throw new NotImplementedException();
            }
        }

        private static string FormatDataProcesingOp2(DataProcessingFormat format)
        {
            if (format.IsImmediate)
            {
                uint immediateValue = BitOperations.RotateRight(format.Imm, format.Rotate);

                return FormatUnsignedImmediate(immediateValue);
            }
            else
            {
                string shiftType = FormatShiftType(format.ShiftType);

                string shiftArgument;

                if (format.IsShiftImmediate)
                {
                    if (format.ShiftImmediate == 0)
                    {
                        return format.Rm.ToString();
                    }

                    shiftArgument = FormatUnsignedImmediate(format.ShiftImmediate);
                }
                else
                {
                    shiftArgument = format.Rs.ToString();
                }

                return $"{format.Rm}, {shiftType} {shiftArgument}";
            }
        }

        public static string DisassembleDataProcesingOneOperand32(InstructionInfo info, uint opcode)
        {
            DataProcessingFormat format = new DataProcessingFormat
            {
                Opcode = opcode
            };

            string modifier = string.Empty;

            if (format.SetCondition)
            {
                modifier = "S";
            }

            string op2 = FormatDataProcesingOp2(format);

            return $"{info.Name}{GetConditionCodeName(opcode)}{modifier} {format.Rd}, {op2}";
        }

        public static string DisassembleDataProcesingOneNoRdOperand32(InstructionInfo info, uint opcode)
        {
            DataProcessingFormat format = new DataProcessingFormat
            {
                Opcode = opcode
            };

            string op2 = FormatDataProcesingOp2(format);

            return $"{info.Name}{GetConditionCodeName(opcode)} {format.Rn}, {op2}";
        }

        public static string DisassembleDataProcesingArithmetic32(InstructionInfo info, uint opcode)
        {
            DataProcessingFormat format = new DataProcessingFormat
            {
                Opcode = opcode
            };

            string modifier = string.Empty;

            if (format.SetCondition)
            {
                modifier = "S";
            }

            string op2 = FormatDataProcesingOp2(format);

            return $"{info.Name}{GetConditionCodeName(opcode)}{modifier} {format.Rd}, {format.Rn}, {op2}";
        }
    }
}
