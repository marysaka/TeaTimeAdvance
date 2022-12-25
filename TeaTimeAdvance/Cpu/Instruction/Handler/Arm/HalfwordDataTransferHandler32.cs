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
        public static void LoadHalfwordUnsignedDataImmediate32(CpuContext context, uint opcode)
        {
            HalfwordDataTransferImmediateFormat32 format = new HalfwordDataTransferImmediateFormat32
            {
                Opcode = opcode
            };

            PrepareDataTransfer32(context, format.Rn, format.IsUp, format.Offset, out uint addressBase, out uint addressWithOffset);
            HandlePreDataTransfer32(context, format.IsPreIndexing, addressBase, addressWithOffset, out uint address);

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

            HandlePostDataTransfer32(context, format.Rn, format.IsPreIndexing, format.WriteBack, addressWithOffset);

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

            PrepareDataTransfer32(context, format.Rn, format.IsUp, format.Offset, out uint addressBase, out uint addressWithOffset);
            HandlePreDataTransfer32(context, format.IsPreIndexing, addressBase, addressWithOffset, out uint address);

            if ((address & 1) != 0)
            {
                // TODO: UB

                throw new NotImplementedException();
            }
            else
            {
                context.BusContext.Write16(address, (ushort)context.GetRegister(format.Rd), BusAccessType.NonSequential);
            }

            HandlePostDataTransfer32(context, format.Rn, format.IsPreIndexing, format.WriteBack, addressWithOffset);
        }
    }
}
