using System;
using System.Runtime.InteropServices;
using TeaTimeAdvance.Common.Memory;

namespace TeaTimeAdvance.Cpu.State
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class CpuState
    {
        public GeneralPurposeRegisters Registers;

        // NOTE: Use an Array1 here to trick the compiler into accepting ref on struct :)
        public Array1<uint> RawCPSR;
        public Array6<uint> SPSR;

        // NOTE: Contains R8-R12 for FIQ.
        public Array5<uint> BankedFIQ;
        public Array6<uint> BankedR13;
        public Array6<uint> BankedR14;

        public ref uint CPSR => ref RawCPSR.ToSpan()[0];

        public CurrentProgramStatusRegister StatusRegister => (CurrentProgramStatusRegister)CPSR;
        public CpuMode Mode => (CpuMode)(CPSR & 0x3F);

        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = sizeof(uint) * RegisterCount)]
        public struct GeneralPurposeRegisters
        {
            public const int RegisterCount = 16;

            [FieldOffset(0x0)]
            public Array16<uint> Raw;

            [FieldOffset(0x00)]
            public uint R0;

            [FieldOffset(0x04)]
            public uint R1;

            [FieldOffset(0x08)]
            public uint R2;

            [FieldOffset(0x0C)]
            public uint R3;

            [FieldOffset(0x10)]
            public uint R4;

            [FieldOffset(0x14)]
            public uint R5;

            [FieldOffset(0x18)]
            public uint R6;

            [FieldOffset(0x1C)]
            public uint R7;

            [FieldOffset(0x20)]
            public uint R8;

            [FieldOffset(0x24)]
            public uint R9;

            [FieldOffset(0x28)]
            public uint R10;

            [FieldOffset(0x2C)]
            public uint R11;

            [FieldOffset(0x30)]
            public uint R12;

            [FieldOffset(0x34)]
            public uint R13;

            [FieldOffset(0x38)]
            public uint R14;

            [FieldOffset(0x3C)]
            public uint R15;

            // Alias for simplicy

            [FieldOffset(0x34)]
            public uint SP;

            [FieldOffset(0x38)]
            public uint LR;

            [FieldOffset(0x3C)]
            public uint PC;


            // Forward suggar to array.
            public ref uint this[int index] => ref Raw[index];
        }

        public ref uint Register(CpuRegister register)
        {
            CpuRegister mode = register & CpuRegister.FlagMask;
            CpuRegister realRegister = register & CpuRegister.RegisterMask;

            // First of all, we handle non banked registers.
            if (realRegister == CpuRegister.CPSR)
            {
                return ref CPSR;
            }
            else if (realRegister == CpuRegister.R15)
            {
                return ref Registers[15];
            }
            else if (realRegister >= CpuRegister.R0 && realRegister <= CpuRegister.R7)
            {
                return ref Registers[(int)realRegister];
            }

            // If we are here, this must be a banked register.

            // If we don't have a mode modifier, grab it from CPSR.
            if (mode == 0)
            {
                switch (Mode)
                {
                    case CpuMode.User:
                        mode = CpuRegister.USR_flag;
                        break;
                    case CpuMode.FIQ:
                        mode = CpuRegister.FIQ_flag;
                        break;
                    case CpuMode.IRQ:
                        mode = CpuRegister.IRQ_flag;
                        break;
                    case CpuMode.Supervisor:
                        mode = CpuRegister.SVC_flag;
                        break;
                    case CpuMode.Abort:
                        mode = CpuRegister.ABT_flag;
                        break;
                    case CpuMode.Undefined:
                        mode = CpuRegister.UND_flag;
                        break;
                    case CpuMode.System:
                        mode = CpuRegister.SYS_flag;
                        break;
                    default:
                        throw new NotSupportedException($"{Mode}");
                }
            }

            int modeIndex = ((int)mode >> (int)CpuRegister.FlagShift) - 1;

            // Check for SPSR
            if (realRegister == CpuRegister.SPSR)
            {
                if (mode == CpuRegister.USR_flag)
                {
                    throw new NotSupportedException("Invalid access to SPSR in System & User mode!");
                }

                // NOTE: Ignore USR
                return ref SPSR[modeIndex - 1];
            }

            // Now  we grab the execution mode.
            bool isThumbExecutionMode = StatusRegister.HasFlag(CurrentProgramStatusRegister.Thumb);

            // Check R8-R12 range
            if (realRegister >= CpuRegister.R8 && realRegister <= CpuRegister.R12)
            {
                // Ensure we are in ARM execution mode.
                if (!isThumbExecutionMode)
                {
                    // Only FIQ have special mapping on R8-R12.
                    if (mode != CpuRegister.FIQ_flag)
                    {
                        return ref Registers[(int)realRegister];
                    }
                    else
                    {
                        return ref BankedFIQ[(int)(realRegister - CpuRegister.R8)];
                    }
                }

                throw new NotSupportedException($"Invalid register {realRegister} access in Thumb mode!");
            }

            // Finally the only registers left are R13 and R14.
            if (realRegister == CpuRegister.R13)
            {
                return ref BankedR13[modeIndex];
            }
            else if (realRegister == CpuRegister.R14)
            {
                return ref BankedR14[modeIndex];
            }

            // Should never happen.
            throw new NotImplementedException(realRegister.ToString());
        }

        // FIXME: make a less dumb variant as this is highly inefficiant to set flags.
        public void SetStatusFlag(CurrentProgramStatusRegister field, bool value)
        {
            if (!value && StatusRegister.HasFlag(field))
            {
                CPSR &= ~(uint)field;
            }
            else if (value)
            {
                CPSR |= (uint)field;
            }
        }

        public void SetCpuMode(CpuMode mode)
        {
            if (mode == CpuMode.Invalid)
            {
                throw new NotSupportedException();
            }

            CPSR = (uint)mode | (uint)(CPSR & ~0x3F);
        }

        public void Reset()
        {
            // Clear all registers
            Registers.Raw.ToSpan().Fill(0);
            SPSR.ToSpan().Fill(0);
            BankedFIQ.ToSpan().Fill(0);
            BankedR13.ToSpan().Fill(0);
            BankedR14.ToSpan().Fill(0);

            // Setup CPSR
            CPSR = (int)CpuMode.Supervisor | (int)(CurrentProgramStatusRegister.MaskFIQ | CurrentProgramStatusRegister.MaskIRQ);
        }
    }
}
