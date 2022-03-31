using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Common;
using TeaTimeAdvance.Cpu.Instruction.Definition;
using TeaTimeAdvance.Cpu.Instruction.Definition.Arm;
using TeaTimeAdvance.Cpu.State;

using static TeaTimeAdvance.Cpu.Instruction.InstructionDecoderHelper;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionHandler
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void PrepareHalfwordDataTransfer32(CpuContext context, CpuRegister rn, bool isUp, uint offset, out uint addressBase, out uint addressWithOffset)
        {
            context.BusAccessType = BusAccessType.NonSequential;
            context.UpdateProgramCounter32();

            addressBase = context.GetRegister(rn);

            if (isUp)
            {
                addressWithOffset = addressBase + offset;
            }
            else
            {
                addressWithOffset = addressBase - offset;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandlePreHalfwordDataTransfer32(CpuContext context, bool isPreIndexing, uint addressBase, uint addressWithOffset, out uint address)
        {
            if (isPreIndexing)
            {
                address = addressWithOffset;
            }
            else
            {
                address = addressBase;
            }
        }

        private static void HandlePostHalfwordDataTransfer32(CpuContext context, CpuRegister rn, bool isPreIndexing, bool writeBack, uint address)
        {
            if (!isPreIndexing || writeBack)
            {
                context.SetRegister(rn, address);
            }
        }

        public static void LoadHalfwordUnsignedDataImmediate32(CpuContext context, uint opcode)
        {
            HalfwordDataTransferImmediateFormat32 format = new HalfwordDataTransferImmediateFormat32
            {
                Opcode = opcode
            };

            PrepareHalfwordDataTransfer32(context, format.Rn, format.IsUp, format.Offset, out uint addressBase, out uint addressWithOffset);
            HandlePreHalfwordDataTransfer32(context, format.IsPreIndexing, addressBase, addressWithOffset, out uint address);

            uint readValue;

            if ((address & 1) != 0)
            {
                // TODO: UB
                throw new NotImplementedException();
            }
            else
            {
                readValue = context.BusContext.Read16(address, BusAccessType.NonSequential);
            }

            HandlePostHalfwordDataTransfer32(context, format.Rn, format.IsPreIndexing, format.WriteBack, addressWithOffset);

            context.Idle();
            context.SetRegister(format.Rd, readValue);

            if (format.Rd == CpuRegister.PC)
            {
                context.ReloadPipeline();
            }
        }

        public static void StoreHalfwordUnsignedDataImmediate32(CpuContext context, uint opcode)
        {
            HalfwordDataTransferImmediateFormat32 format = new HalfwordDataTransferImmediateFormat32
            {
                Opcode = opcode
            };

            PrepareHalfwordDataTransfer32(context, format.Rn, format.IsUp, format.Offset, out uint addressBase, out uint addressWithOffset);
            HandlePreHalfwordDataTransfer32(context, format.IsPreIndexing, addressBase, addressWithOffset, out uint address);

            if ((address & 1) != 0)
            {
                // TODO: UB

                throw new NotImplementedException();
            }
            else
            {
                context.BusContext.Write16(address, (ushort)context.GetRegister(format.Rd), BusAccessType.NonSequential);
            }

            HandlePostHalfwordDataTransfer32(context, format.Rn, format.IsPreIndexing, format.WriteBack, addressWithOffset);
        }
    }
}
