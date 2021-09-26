using System.Collections.Generic;
using TeaTimeAdvance.Cpu.Instruction.Definition;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleBlockDataTransferDisassembler32(InstructionInfo info, uint opcode)
        {
            BlockDataTransferFormat format = new BlockDataTransferFormat
            {
                Opcode = opcode
            };

            string writeBackModifier = string.Empty;
            string specialModifier = string.Empty;

            if (format.WriteBack)
            {
                writeBackModifier = "!";
            }

            if (format.UseCurrentLevelBank)
            {
                specialModifier = "^";
            }

            List<CpuRegister> registerList = new List<CpuRegister>();

            for (CpuRegister reg = CpuRegister.R0; reg <= CpuRegister.R15; reg++)
            {
                if (format.HasCpuRegisterInRegisterList(reg))
                {
                    registerList.Add(reg);
                }
            }

            return $"{info.Name}{GetConditionCodeName(opcode)} {format.Rd}{writeBackModifier}, {{{string.Join(", ", registerList)}}}{specialModifier}";
        }
    }
}
