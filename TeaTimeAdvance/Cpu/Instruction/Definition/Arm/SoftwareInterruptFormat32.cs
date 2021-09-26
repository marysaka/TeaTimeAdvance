using System.Runtime.InteropServices;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Arm
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct SoftwareInterruptFormat32 : IInstructionFormat32
    {
        public uint Opcode;

        uint IInstructionFormat32.Opcode => Opcode;

        public uint CommentField => Opcode & 0x00FFFFFF;
    }
}
