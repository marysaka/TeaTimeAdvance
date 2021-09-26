using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Arm
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct SingleDataTransferFormat32 : IInstructionFormat32
    {
        public uint Opcode;

        uint IInstructionFormat32.Opcode => Opcode;

        public ushort Offset => (ushort)(Opcode & 0xFFF);
        public CpuRegister Rd => ((IInstructionFormat32)this).GetRegisterByIndex(3);
        public CpuRegister Rn => ((IInstructionFormat32)this).GetRegisterByIndex(4);
        public bool IsStore => (Opcode & (1 << 20)) == 0;
        public bool WriteBack => (Opcode & (1 << 21)) != 0;
        public bool IsByteTransfer => (Opcode & (1 << 22)) != 0;
        public bool IsUp => (Opcode & (1 << 23)) != 0;
        public bool IsPreIndexing => (Opcode & (1 << 24)) != 0;
        public bool IsImmediate => (Opcode & (1 << 25)) == 0;

        // Immediate form
        public ushort ImmediateOffset => Offset;

        // Register form
        public CpuRegister Rm => ((IInstructionFormat32)this).GetRegisterByIndex(0);
        public byte Shift => (byte)(Opcode >> 4);
        public bool IsShiftImmediate => (Shift & 1) == 0;
        public CpuShift ShiftType => (CpuShift)(Shift >> 1) & CpuShift.Mask;

        // Register shift immediate form
        public byte ShiftImmediate => (byte)(Shift >> 3);
    }
}
