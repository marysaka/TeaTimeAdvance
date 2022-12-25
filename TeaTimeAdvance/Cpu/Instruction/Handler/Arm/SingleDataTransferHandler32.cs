using System;
using System.Runtime.CompilerServices;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Cpu.Instruction.Definition.Arm;
using TeaTimeAdvance.Cpu.State;

using static TeaTimeAdvance.Cpu.Instruction.InstructionDecoderHelper;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionHandler
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void PrepareDataTransfer32(CpuContext context, CpuRegister rn, bool isUp, uint offset, out uint addressBase, out uint addressWithOffset)
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
        private static void HandlePreDataTransfer32(CpuContext context, bool isPreIndexing, uint addressBase, uint addressWithOffset, out uint address)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandlePostDataTransfer32(CpuContext context, CpuRegister rn, bool isPreIndexing, bool writeBack, uint address)
        {
            if (!isPreIndexing || writeBack)
            {
                context.SetRegister(rn, address);
            }
        }

        private static void HandleSingleDataTransferFormat32(CpuContext context, uint opcode, Action<CpuContext, uint, uint> processing)
        {
            SingleDataTransferFormat32 format = new SingleDataTransferFormat32
            {
                Opcode = opcode
            };

            PrepareDataTransfer32(context, format.Rn, format.IsUp, format.Offset, out uint addressBase, out uint addressWithOffset);
            HandlePreDataTransfer32(context, format.IsPreIndexing, addressBase, addressWithOffset, out uint address);

            processing(context, address, context.GetRegister(format.Rd));

            HandlePostDataTransfer32(context, format.Rn, format.IsPreIndexing, format.WriteBack, addressWithOffset);
        }

        public static void LoadSingleDataTransfer32(CpuContext context, uint opcode)
        {
            SingleDataTransferFormat32 format = new SingleDataTransferFormat32
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
                if (format.IsByteTransfer)
                {
                    readValue = context.BusContext.Read8(address, BusAccessType.NonSequential);

                }
                else
                {
                    readValue = context.BusContext.Read32(address, BusAccessType.NonSequential);
                }
            }

            HandlePostDataTransfer32(context, format.Rn, format.IsPreIndexing, format.WriteBack, addressWithOffset);

            context.Idle();
            context.SetRegister(format.Rd, readValue);

            if (format.Rd == CpuRegister.PC)
            {
                context.ReloadPipeline();
            }
        }

        public static void StoreSingleDataTransfer32(CpuContext context, uint opcode)
        {
            SingleDataTransferFormat32 format = new SingleDataTransferFormat32
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
                uint data = context.GetRegister(format.Rd);

                if (format.IsByteTransfer)
                {
                    context.BusContext.Write8(address, (byte)data, BusAccessType.NonSequential);

                }
                else
                {
                    context.BusContext.Write32(address, data, BusAccessType.NonSequential);
                }
            }

            HandlePostDataTransfer32(context, format.Rn, format.IsPreIndexing, format.WriteBack, addressWithOffset);
        }
    }
}
