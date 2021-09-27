using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassemblePCRelativeLoad16(InstructionInfo info, uint opcode)
        {
            PCRelativeLoadFormat16 format = new PCRelativeLoadFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, [PC, {FormatUnsignedImmediate((uint)format.Immediate)}]";
        }
    }
}
