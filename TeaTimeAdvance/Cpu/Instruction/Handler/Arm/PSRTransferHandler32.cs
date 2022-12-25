using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Cpu.Instruction.Definition;
using TeaTimeAdvance.Cpu.Instruction.Definition.Arm;
using TeaTimeAdvance.Cpu.State;

using static TeaTimeAdvance.Cpu.Instruction.InstructionDecoderHelper;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionHandler
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MoveRegisterToStatusRegister32(CpuContext context, PSRTransferFormat32 format, uint operand)
        {
            ref uint statusRegister = ref context.State.Register(format.IsSpecialPurposeSpecialRegister ? CpuRegister.SPSR : CpuRegister.CPSR);

            uint mask = 0x0;

            if (format.Fields.HasFlag(CpuPSRFields.Control))
            {
                mask |= 0xFF;
            }

            if (format.IsSpecialPurposeSpecialRegister || context.State.Mode != CpuMode.User)
            {
                if (format.Fields.HasFlag(CpuPSRFields.Extension))
                {
                    mask |= 0xFF00;
                }

                if (format.Fields.HasFlag(CpuPSRFields.Status))
                {
                    mask |= 0xFF0000;
                }

                if (format.Fields.HasFlag(CpuPSRFields.Flags))
                {
                    mask |= 0xFF000000;
                }
            }

            statusRegister = (statusRegister & ~mask) | (operand & mask);
            context.BusAccessType = BusAccessType.Sequential;
        }

        public static void MoveStatusRegisterToRegister32(CpuContext context, uint opcode)
        {
            PSRTransferFormat32 format = new PSRTransferFormat32
            {
                Opcode = opcode
            };

            context.SetRegister(format.Rd, context.GetRegister(format.IsSpecialPurposeSpecialRegister ? CpuRegister.SPSR : CpuRegister.CPSR));
            context.BusAccessType = BusAccessType.Sequential;
        }

        public static void MoveRegisterToStatusRegister32(CpuContext context, uint opcode)
        {
            PSRTransferFormat32 format = new PSRTransferFormat32
            {
                Opcode = opcode
            };

            uint operand = BitOperations.RotateRight(format.Imm, format.Rotate);
            MoveRegisterToStatusRegister32(context, format, operand);
        }

        public static void MoveImmediateToStatusRegister32(CpuContext context, uint opcode)
        {
            PSRTransferFormat32 format = new PSRTransferFormat32
            {
                Opcode = opcode
            };

            MoveRegisterToStatusRegister32(context, format, context.GetRegister(format.Rm));
        }
    }
}
