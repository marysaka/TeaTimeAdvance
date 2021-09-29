using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct LoadAddressFormat16 : IInstructionFormat16
    {
        public ushort Opcode;

        ushort IInstructionFormat16.Opcode => Opcode;

        public CpuRegister Rd => ((IInstructionFormat16)this).GetRegisterByOffset(8);
        public short Immediate => (short)((Opcode & 0xFF) << 2);
        public bool IsStackPointer => (Opcode & (1 << 11)) != 0;
    }
}
