using TeaTimeAdvance.Cpu.Instruction.Definition.Thumb;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        public static string DisassembleMoveShiftedRegister16(InstructionInfo info, uint opcode)
        {
            MoveShiftedRegisterFormat16 format = new MoveShiftedRegisterFormat16
            {
                Opcode = (ushort)opcode
            };

            return $"{info.Name} {format.Rd}, {format.Rs}, {FormatUnsignedImmediate(format.Offset)}";
        }
    }
}
