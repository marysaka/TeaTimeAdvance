using System;
using System.Numerics;
using TeaTimeAdvance.Common;
using TeaTimeAdvance.Cpu.Instruction.Definition;
using TeaTimeAdvance.Cpu.Instruction.Definition.Arm;
using TeaTimeAdvance.Cpu.State;

using static TeaTimeAdvance.Cpu.Instruction.InstructionDecoderHelper;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionHandler
    {
        public static void LogicalAnd32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, shifterCarry) =>
            {
                ref uint rn = ref context.State.Register(format.Rn);

                return (rn & shifterOperand, shifterCarry);
            });
        }

        public static void LogicalExclusiveOr32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, shifterCarry) =>
            {
                ref uint rn = ref context.State.Register(format.Rn);

                return (rn ^ shifterOperand, shifterCarry);
            });
        }

        public static void Subtract32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, carry) =>
            {
                ref uint rn = ref context.State.Register(format.Rn);

                uint result = ArithmeticHelper.Subtraction(out carry, rn, shifterOperand);

                return (result, carry);
            });
        }

        public static void ReverseSubtract32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, carry) =>
            {
                ref uint rn = ref context.State.Register(format.Rn);

                uint result = ArithmeticHelper.Subtraction(out carry, shifterOperand, rn);

                return (result, carry);
            });
        }

        public static void Add32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, carry) =>
            {
                ref uint rn = ref context.State.Register(format.Rn);

                uint result = ArithmeticHelper.Addition(out carry, rn, shifterOperand);

                return (result, carry);
            });
        }

        public static void AddCarry32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, carry) =>
            {
                ref uint rn = ref context.State.Register(format.Rn);

                uint result = ArithmeticHelper.Addition(out carry, rn, shifterOperand, Convert.ToUInt32(carry));

                return (result, carry);
            });
        }

        public static void SubtractCarry32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, carry) =>
            {
                ref uint rn = ref context.State.Register(format.Rn);

                uint result = ArithmeticHelper.Subtraction(out carry, rn, shifterOperand, Convert.ToUInt32(carry));

                return (result, carry);
            });
        }

        public static void ReverseSubtractCarry32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, carry) =>
            {
                ref uint rn = ref context.State.Register(format.Rn);

                uint result = ArithmeticHelper.Subtraction(out carry, shifterOperand, rn, Convert.ToUInt32(carry));

                return (result, carry);
            });
        }

        public static void LogicalOr32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, shifterCarry) =>
            {
                ref uint rn = ref context.State.Register(format.Rn);

                return (rn | shifterOperand, shifterCarry);
            });
        }

        public static void Move32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, shifterCarry) => (shifterOperand, shifterCarry));
        }

        public static void BitClear32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, shifterCarry) =>
            {
                ref uint rn = ref context.State.Register(format.Rn);

                return (rn & ~shifterOperand, shifterCarry);
            });
        }

        public static void MoveNot32(CpuContext context, uint opcode)
        {
            HandleDataProcessingFormat32(context, opcode, (format, shifterOperand, shifterCarry) => (~shifterOperand, shifterCarry));
        }

        private static void HandleDataProcessingFormat32(CpuContext context, uint opcode, Func<DataProcessingFormat32, uint, bool, (uint, bool)> processing)
        {
            DataProcessingFormat32 format = new DataProcessingFormat32
            {
                Opcode = opcode
            };

            ref uint rd = ref context.State.Register(format.Rd);

            HandleDataProcesingOperands(context, format, out uint shifterOperand, out bool carry);

            (rd, carry) = processing(format, shifterOperand, carry);

            if (format.SetCondition && format.Rd == CpuRegister.PC)
            {
                context.SetRegister(CpuRegister.CPSR, context.GetRegister(CpuRegister.SPSR));
            }
            else if (format.SetCondition)
            {
                HandleDataProcesingNZFlagsUpdate(context, rd);

                context.SetStatusFlag(CurrentProgramStatusRegister.Carry, carry);
            }

            context.UpdateProgramCounter32();
        }

        private static uint GetRegisterValueForDataProcesingOperand(CpuContext context, CpuRegister register)
        {
            uint value = context.GetRegister(register);

            if (register == CpuRegister.PC)
            {
                return value + ArmInstructionSize * 2;
            }

            return value;
        }
        private static void HandleDataProcesingOperandImmediate(CpuContext context, DataProcessingFormat32 format, out uint shifterOperand, out bool shifterCarry)
        {
            shifterOperand = BitOperations.RotateRight(format.Imm, format.Rotate);

            if (format.Rotate == 0)
            {
                shifterCarry = context.State.StatusRegister.HasFlag(CurrentProgramStatusRegister.Carry);
            }
            else
            {
                shifterCarry = (shifterOperand & (1 << 31)) != 0;
            }
        }

        private static void HandleDataProcesingOperandRegister(CpuContext context, DataProcessingFormat32 format, out uint shifterOperand, out bool shifterCarry)
        {
            uint rmValue = GetRegisterValueForDataProcesingOperand(context, format.Rm);

            shifterOperand = rmValue;
            shifterCarry = context.State.StatusRegister.HasFlag(CurrentProgramStatusRegister.Carry);
        }

        private static void HandleDataProcesingOperandLslByImmediate(CpuContext context, DataProcessingFormat32 format, out uint shifterOperand, out bool shifterCarry)
        {
            uint rmValue = GetRegisterValueForDataProcesingOperand(context, format.Rm);

            shifterOperand = rmValue << format.ShiftImmediate;
            shifterCarry = (shifterOperand & (1 << (32 - format.ShiftImmediate))) != 0;
        }

        private static void HandleDataProcesingOperandLslByRegister(CpuContext context, DataProcessingFormat32 format, out uint shifterOperand, out bool shifterCarry)
        {
            uint rmValue = GetRegisterValueForDataProcesingOperand(context, format.Rm);
            byte rnValue = (byte)GetRegisterValueForDataProcesingOperand(context, format.Rn);

            if (rnValue == 0)
            {
                shifterOperand = rmValue;
                shifterCarry = context.State.StatusRegister.HasFlag(CurrentProgramStatusRegister.Carry);
            }
            else if (rnValue < 32)
            {
                shifterOperand = rmValue << rnValue;
                shifterCarry = (shifterOperand & (1 << (32 - rnValue))) != 0;
            }
            else if (rnValue == 32)
            {
                shifterOperand = 0;
                shifterCarry = (shifterOperand & 1) != 0;
            }
            else
            {
                shifterOperand = 0;
                shifterCarry = false;
            }
        }

        private static void HandleDataProcesingOperandLsrByImmediate(CpuContext context, DataProcessingFormat32 format, out uint shifterOperand, out bool shifterCarry)
        {
            uint rmValue = GetRegisterValueForDataProcesingOperand(context, format.Rm);

            shifterOperand = rmValue >> format.ShiftImmediate;

            if (format.ShiftImmediate == 0)
            {
                shifterCarry = (shifterOperand & (1 << 31)) != 0;
            }
            else
            {
                shifterCarry = (shifterOperand & (1 << (format.ShiftImmediate - 1))) != 0;
            }
        }

        private static void HandleDataProcesingOperandLsrByRegister(CpuContext context, DataProcessingFormat32 format, out uint shifterOperand, out bool shifterCarry)
        {
            uint rmValue = GetRegisterValueForDataProcesingOperand(context, format.Rm);
            byte rnValue = (byte)GetRegisterValueForDataProcesingOperand(context, format.Rn);

            if (rnValue == 0)
            {
                shifterOperand = rmValue;
                shifterCarry = context.State.StatusRegister.HasFlag(CurrentProgramStatusRegister.Carry);
            }
            else if (rnValue < 32)
            {
                shifterOperand = rmValue << rnValue;
                shifterCarry = (shifterOperand & (1 << (format.ShiftImmediate - 1))) != 0;
            }
            else if (rnValue == 32)
            {
                shifterOperand = 0;
                shifterCarry = (shifterOperand & (1 << 31)) != 0;
            }
            else
            {
                shifterOperand = 0;
                shifterCarry = false;
            }
        }

        private static void HandleDataProcesingOperands(CpuContext context, DataProcessingFormat32 format, out uint shifterOperand, out bool shifterCarry)
        {
            if (format.IsImmediate)
            {
                HandleDataProcesingOperandImmediate(context, format, out shifterOperand, out shifterCarry);
            }
            else
            {
                if (format.IsShiftImmediate)
                {
                    // Special register format (formated as LSL #0x0)
                    if (format.ShiftType == CpuShift.LogicalLeft && format.ShiftImmediate == 0)
                    {
                        HandleDataProcesingOperandRegister(context, format, out shifterOperand, out shifterCarry);
                    }
                    else
                    {
                        // Immediate
                        switch (format.ShiftType)
                        {
                            case CpuShift.LogicalLeft:
                                HandleDataProcesingOperandLslByImmediate(context, format, out shifterOperand, out shifterCarry);
                                break;
                            case CpuShift.LogicalRight:
                                HandleDataProcesingOperandLsrByImmediate(context, format, out shifterOperand, out shifterCarry);
                                break;
                            default:
                                throw new NotImplementedException(format.ShiftType.ToString());
                        }
                    }
                }
                else
                {
                    // Register
                    switch (format.ShiftType)
                    {
                        case CpuShift.LogicalLeft:
                            HandleDataProcesingOperandLslByRegister(context, format, out shifterOperand, out shifterCarry);
                            break;
                        case CpuShift.LogicalRight:
                            HandleDataProcesingOperandLsrByRegister(context, format, out shifterOperand, out shifterCarry);
                            break;
                        default:
                            throw new NotImplementedException(format.ShiftType.ToString());
                    }
                }
            }
        }

        private static void HandleDataProcesingNZFlagsUpdate(CpuContext context, uint value)
        {
            context.SetStatusFlag(CurrentProgramStatusRegister.Negative, (value & (1 << 31)) != 0);
            context.SetStatusFlag(CurrentProgramStatusRegister.Zero, value == 0);
        }
    }
}
