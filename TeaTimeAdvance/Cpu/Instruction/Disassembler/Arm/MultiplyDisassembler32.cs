﻿using TeaTimeAdvance.Cpu.Instruction.Definition;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleMultiply32(InstructionInfo info, uint opcode)
        {
            MulitplyFormat format = new MulitplyFormat
            {
                Opcode = opcode
            };


            string modifier = string.Empty;

            if (format.SetCondition)
            {
                modifier = "S";
            }

            if (format.IsAccumulator)
            {
                return $"{info.Name}{modifier}{GetConditionCodeName(opcode)} {format.Rd}, {format.Rm}, {format.Rs}, {format.Rn}";

            }
            else
            {
                return $"{info.Name}{modifier}{GetConditionCodeName(opcode)} {format.Rd}, {format.Rm}, {format.Rs}";
            }
        }
    }
}
