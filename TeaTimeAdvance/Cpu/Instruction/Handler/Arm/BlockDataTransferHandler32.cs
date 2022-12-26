using System;
using System.Diagnostics;
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
        private static void StoreMultiple(CpuContext context, uint startAddress, uint endAddress, ReadOnlySpan<CpuRegister> regs)
        {
            uint address = startAddress;

            BusAccessType accessType = BusAccessType.NonSequential;

            foreach (CpuRegister i in regs)
            {
                context.BusContext.Write32(address, context.GetRegister(i), accessType);
                accessType = BusAccessType.Sequential;
                address += sizeof(uint);
            }

            Debug.Assert(endAddress == address - sizeof(uint));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ReadOnlySpan<CpuRegister> GetRegisterList(BlockDataTransferFormat32 format, Span<CpuRegister> storage, out bool transferOnPC)
        {
            int regsCount = 0;

            if (!format.IsRegisterListEmpty)
            {
                for (CpuRegister i = CpuRegister.R0; i <= CpuRegister.R15; i++)
                {
                    if (format.HasCpuRegisterInRegisterList(i))
                    {
                        storage[regsCount++] = i;
                    }
                }

                transferOnPC = format.HasCpuRegisterInRegisterList(CpuRegister.R15);
            }
            else
            {
                storage[regsCount++] = CpuRegister.R15;
                transferOnPC = true;
            }

            return storage[..regsCount];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreMultiple(CpuContext context, uint opcode, Func<uint, uint, (uint, uint)> computeAddresses)
        {
            BlockDataTransferFormat32 format = new BlockDataTransferFormat32
            {
                Opcode = opcode
            };

            uint rn = context.GetRegister(format.Rn);

            Span<CpuRegister> registersStorage = stackalloc CpuRegister[15];

            var regs = GetRegisterList(format, registersStorage, out bool transferOnPC);

            (uint startAddress, uint endAddress) = computeAddresses(rn, (uint)regs.Length * sizeof(uint));

            bool changeCpuMode = format.UseUserLevelBank && !transferOnPC;
            CpuMode oldCpuMode = context.State.Mode;

            if (changeCpuMode)
            {
                context.SetCpuMode(CpuMode.User);
            }

            if (format.WriteBack)
            {
                uint writeBackValue;

                if (format.IsUp)
                {
                    writeBackValue = (uint)(rn + regs.Length);
                }
                else
                {
                    writeBackValue = (uint)(rn - regs.Length);
                }

                context.SetRegister(format.Rn, writeBackValue);
            }

            StoreMultiple(context, startAddress, endAddress, regs);

            if (changeCpuMode)
            {
                context.SetCpuMode(oldCpuMode);
            }
        }

        public static void StoreMultipleDecrementAfter(CpuContext context, uint opcode)
        {
            StoreMultiple(context, opcode, (rn, size) => (rn - size + sizeof(uint), rn));
        }

        public static void StoreMultipleIncrementAfter(CpuContext context, uint opcode)
        {
            StoreMultiple(context, opcode, (rn, size) => (rn, rn + size - sizeof(uint)));
        }

        public static void StoreMultipleDecrementBefore(CpuContext context, uint opcode)
        {
            StoreMultiple(context, opcode, (rn, size) => (rn - size, rn - sizeof(uint)));
        }

        public static void StoreMultipleIncrementBefore(CpuContext context, uint opcode)
        {
            StoreMultiple(context, opcode, (rn, size) => (rn + sizeof(uint), rn + size));
        }
    }
}
