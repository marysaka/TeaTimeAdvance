using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassemblePCRelativeLoad16(InstructionInfo info, uint opcode)
        {
            PCRelativeLoad16Format format = new PCRelativeLoad16Format
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, [PC, {FormatUnsignedImmediate((uint)format.Immediate)}]";
        }
    }
}
