using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition.Thumb
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct PushPopRegistersFormat16 : IInstructionFormat16
    {
        public ushort Opcode;

        ushort IInstructionFormat16.Opcode => Opcode;

        public bool UseSpecialRegister => (Opcode & (1 << 8)) != 0;
        public bool IsStore => (Opcode & (1 << 11)) == 0;

        public bool HasCpuRegisterInRegisterList(CpuRegister register)
        {
            if (IsStore && register == CpuRegister.LR && UseSpecialRegister)
            {
                return true;
            }

            if (!IsStore && register == CpuRegister.PC && UseSpecialRegister)
            {
                return true;
            }

            if (register > CpuRegister.UserRegisterMask16)
            {
                return false;
            }

            register &= CpuRegister.UserRegisterMask16;

            return (Opcode & (1 << (int)register)) != 0;
        }
    }
}
