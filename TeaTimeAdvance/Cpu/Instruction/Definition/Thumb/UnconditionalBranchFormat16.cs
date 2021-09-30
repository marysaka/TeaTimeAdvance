using System.Runtime.InteropServices;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct UnconditionalBranchFormat16 : IInstructionFormat16
    {
        public ushort Opcode;

        ushort IInstructionFormat16.Opcode => Opcode;

        public int Offset => InstructionDecoderHelper.SignExtendImmediate11((uint)(Opcode & 0x7FF)) << 1;
    }
}
