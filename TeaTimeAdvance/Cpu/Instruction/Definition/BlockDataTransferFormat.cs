using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct BlockDataTransferFormat : IInstructionFormat
    {
        public uint Opcode;

        uint IInstructionFormat.Opcode => Opcode;

        public CpuRegister Rd => ((IInstructionFormat)this).GetRegisterByIndex(4);
        public bool IsStore => (Opcode & (1 << 20)) == 0;
        public bool WriteBack => (Opcode & (1 << 21)) != 0;
        public bool UseCurrentLevelBank => (Opcode & (1 << 22)) != 0;
        public bool IsUp => (Opcode & (1 << 23)) != 0;
        public bool IsPreIndexing => (Opcode & (1 << 24)) != 0;

        public bool HasCpuRegisterInRegisterList(CpuRegister register)
        {
            register &= CpuRegister.UserRegisterMask;

            return (Opcode & (1 << (int)register)) != 0;
        }
    }
}
