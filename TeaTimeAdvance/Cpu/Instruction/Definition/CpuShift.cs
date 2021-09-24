namespace TeaTimeAdvance.Cpu.Instruction.Definition
{
    public enum CpuShift : byte
    {
        LogicalLeft,
        LogicalRight,
        ArithmeticRight,
        RotationRight,

        Mask = 0x7
    }
}
