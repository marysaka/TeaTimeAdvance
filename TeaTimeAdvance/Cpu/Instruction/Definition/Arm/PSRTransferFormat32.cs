using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Arm
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct PSRTransferFormat32 : IInstructionFormat32
    {
        public uint Opcode;

        uint IInstructionFormat32.Opcode => Opcode;

        public CpuRegister Rd => ((IInstructionFormat32)this).GetRegisterByIndex(3);

        public bool IsStore => (Opcode & (1 << 21)) != 0;
        public bool IsPossiblyImmediate => (Opcode & (1 << 16)) != 0;
        public bool IsImmediate => IsPossiblyImmediate && (Opcode & (1 << 25)) != 0;
        public bool IsSpecialPurposeSpecialRegister => (Opcode & (1 << 22)) != 0;

        public CpuPSRFields Fields => (CpuPSRFields)(Opcode >> 16) & CpuPSRFields.Mask;

        // Register immediate form
        public CpuRegister Rm => ((IInstructionFormat32)this).GetRegisterByIndex(0);

        // Immediate form
        public byte Imm => (byte)Opcode;
        public byte Rotate => (byte)(((Opcode >> 8) & 0xF) << 1);
    }
}
