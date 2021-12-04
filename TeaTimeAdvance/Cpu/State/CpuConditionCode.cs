namespace TeaTimeAdvance.Cpu.State
{
    public enum CpuConditionCode : byte
    {
        EQ,
        NE,
        CS,
        CC,
        MI,
        PL,
        VS,
        VC,
        HI,
        LS,
        GE,
        LT,
        GT,
        LE,
        AL,
        // Invalid or NEVER
        NV,

        Mask = 0xF
    }
}
