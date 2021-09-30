using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct ConditionalBranchFormat16 : IInstructionFormat16
    {
        public ushort Opcode;

        ushort IInstructionFormat16.Opcode => Opcode;

        public CpuConditionCode ConditionCode => (CpuConditionCode)(Opcode >> 8) & CpuConditionCode.Mask;

        public int Offset => InstructionDecoderHelper.SignExtendImmediate8((uint)(Opcode & 0x1FF)) << 1;
    }
}
