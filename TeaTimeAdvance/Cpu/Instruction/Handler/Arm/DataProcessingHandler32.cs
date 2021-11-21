using System;
using System.Numerics;
using TeaTimeAdvance.Cpu.Instruction.Definition;
using TeaTimeAdvance.Cpu.Instruction.Definition.Arm;
using TeaTimeAdvance.Cpu.State;

using static TeaTimeAdvance.Cpu.Instruction.InstructionDecoderHelper;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionHandler
    {
        public static void Move32(CpuContext context, uint opcode)
        {
            DataProcessingFormat32 format = new DataProcessingFormat32
            {
                Opcode = opcode
            };

            ref uint rd = ref context.State.Register(format.Rd);

            HandleDataProcesingOperands(context, format, out uint shifterOperand, out bool shifterCarry);

            rd = shifterOperand;

            if (format.SetCondition && format.Rd == CpuRegister.PC)
            {
                context.SetRegister(CpuRegister.CPSR, context.GetRegister(CpuRegister.SPSR));
            }
            else if (format.SetCondition)
            {
                HandleDataProcesingNZFlagsUpdate(context, rd);

                context.SetStatusFlag(CurrentProgramStatusRegister.Carry, shifterCarry);
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
