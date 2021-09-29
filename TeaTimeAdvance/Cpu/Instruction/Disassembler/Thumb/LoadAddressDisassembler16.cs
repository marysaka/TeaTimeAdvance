using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleLoadAddress16(InstructionInfo info, uint opcode)
        {
            LoadAddressFormat16 format = new LoadAddressFormat16
            {
                Opcode = (ushort)opcode
            };

            CpuRegister rb = format.IsStackPointer ? CpuRegister.SP : CpuRegister.PC;

            return $"{info.Name} {format.Rd}, {rb}, {FormatUnsignedImmediate((uint)format.Immediate)}";
        }
    }
}



