namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionHandler
    {
        private static ref uint GetRegister(CpuContext context, uint opcode, int shift)
        {
            return ref context.State.Register(InstructionDecoderHelper.GetCpuRegisterFromOpcode(opcode, shift));
        }
    }
}
