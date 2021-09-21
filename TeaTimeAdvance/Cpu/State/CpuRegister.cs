namespace TeaTimeAdvance.Cpu.State
{
    public enum CpuRegister : uint
    {
        // Normal registers
        R0,
        R1,
        R2,
        R3,
        R4,
        R5,
        R6,
        R7,
        R8,
        R9,
        R10,
        R11,
        R12,
        R13,
        R14,
        R15,
        CPSR,
        SPSR,

        // Aliases
        SP = R13,
        LR = R14,
        PC = R15,

        // User mode
        R8_usr = R8 | USR_flag,
        R9_usr = R9 | USR_flag,
        R10_usr = R10 | USR_flag,
        R11_usr = R11 | USR_flag,
        R12_usr = R12 | USR_flag,
        R13_usr = R13 | USR_flag,
        R14_usr = R14 | USR_flag,
        SP_usr = R13_usr,
        LR_usr = R14_usr,

        // System mode
        R8_sys = R8 | SYS_flag,
        R9_sys = R9 | SYS_flag,
        R10_sys = R10 | SYS_flag,
        R11_sys = R11 | SYS_flag,
        R12_sys = R12 | SYS_flag,
        R13_sys = R13 | SYS_flag,
        R14_sys = R14 | SYS_flag,
        SP_sys = R13_sys,
        LR_sys = R14_sys,

        // FIQ mode
        R8_fiq = R8 | FIQ_flag,
        R9_fiq = R9 | FIQ_flag,
        R10_fiq = R10 | FIQ_flag,
        R11_fiq = R11 | FIQ_flag,
        R12_fiq = R12 | FIQ_flag,
        R13_fiq = R13 | FIQ_flag,
        R14_fiq = R14 | FIQ_flag,
        SPSR_fiq = SPSR | FIQ_flag,
        SP_fiq = R13_fiq,
        LR_fiq = R14_fiq,

        // SVC mode
        R8_svc = R8 | SVC_flag,
        R9_svc = R9 | SVC_flag,
        R10_svc = R10 | SVC_flag,
        R11_svc = R11 | SVC_flag,
        R12_svc = R12 | SVC_flag,
        R13_svc = R13 | SVC_flag,
        R14_svc = R14 | SVC_flag,
        SPSR_svc = SPSR | SVC_flag,
        SP_svc = R13_svc,
        LR_svc = R14_svc,

        // ABT mode
        R8_abt = R8 | ABT_flag,
        R9_abt = R9 | ABT_flag,
        R10_abt = R10 | ABT_flag,
        R11_abt = R11 | ABT_flag,
        R12_abt = R12 | ABT_flag,
        R13_abt = R13 | ABT_flag,
        R14_abt = R14 | ABT_flag,
        SPSR_abt = SPSR | ABT_flag,
        SP_abt = R13_abt,
        LR_abt = R14_abt,

        // IRQ mode
        R8_irq = R8 | IRQ_flag,
        R9_irq = R9 | IRQ_flag,
        R10_irq = R10 | IRQ_flag,
        R11_irq = R11 | IRQ_flag,
        R12_irq = R12 | IRQ_flag,
        R13_irq = R13 | IRQ_flag,
        R14_irq = R14 | IRQ_flag,
        SPSR_irq = SPSR | IRQ_flag,
        SP_irq = R13_irq,
        LR_irq = R14_irq,

        // Undefined mode
        R8_und = R8 | UND_flag,
        R9_und = R9 | UND_flag,
        R10_und = R10 | UND_flag,
        R11_und = R11 | UND_flag,
        R12_und = R12 | UND_flag,
        R13_und = R13 | UND_flag,
        R14_und = R14 | UND_flag,
        SPSR_und = SPSR | UND_flag,
        SP_und = R13_und,
        LR_und = R14_und,

        USR_flag = 1 << (int)FlagShift,
        SYS_flag = USR_flag,
        FIQ_flag = 2 << (int)FlagShift,
        SVC_flag = 3 << (int)FlagShift,
        ABT_flag = 4 << (int)FlagShift,
        IRQ_flag = 5 << (int)FlagShift,
        UND_flag = 6 << (int)FlagShift,

        FlagShift = 5,
        FlagMask = 0xE0,
        RegisterMask = 0x1F,

        // R0-R15
        UserRegisterMask = 0x0F
    }
}
