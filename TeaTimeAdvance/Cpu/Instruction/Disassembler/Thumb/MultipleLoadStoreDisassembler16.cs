using System.Collections.Generic;
using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleMultipleLoadStore16(InstructionInfo info, uint opcode)
        {
            MultipleLoadStoreFormat16 format = new MultipleLoadStoreFormat16
            {
                Opcode = (ushort)opcode
            };

            List<CpuRegister> registerList = new List<CpuRegister>();

            for (CpuRegister reg = CpuRegister.R0; reg <= CpuRegister.R7; reg++)
            {
                if (format.HasCpuRegisterInRegisterList(reg))
                {
                    registerList.Add(reg);
                }
            }

            return $"{info.Name} {format.Rb}!, {{{string.Join(", ", registerList)}}}";
        }
    }
}
