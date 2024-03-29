﻿using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleSPRelativeLoadStore16(InstructionInfo info, uint opcode)
        {
            SPRelativeLoadStoreFormat16 format = new SPRelativeLoadStoreFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, [SP, {FormatUnsignedImmediate((uint)format.Immediate)}]";
        }
    }
}
