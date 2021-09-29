using System.Collections.Generic;
using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassemblePushPopRegisters16(InstructionInfo info, uint opcode)
        {
            PushPopRegistersFormat16 format = new PushPopRegistersFormat16
            {
                Opcode = (ushort)opcode
            };

            List<CpuRegister> registerList = new List<CpuRegister>();

            for (CpuRegister reg = CpuRegister.R0; reg <= CpuRegister.R15; reg++)
            {
                if (format.HasCpuRegisterInRegisterList(reg))
                {
                    registerList.Add(reg);
                }
            }

            return $"{info.Name} {{{string.Join(", ", registerList)}}}";
        }
    }
}
