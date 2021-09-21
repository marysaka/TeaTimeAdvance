using TeaTimeAdvance.Cpu.State;

using static TeaTimeAdvance.Cpu.Instruction.InstructionDecoderHelper;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionHandler
    {
        public static void Branch32(CpuContext context, uint opcode)
        {
            ref uint pc = ref context.State.Register(CpuRegister.PC);

            int immediate = DecodeS24(opcode);

            pc = (uint)((int)pc + immediate);

            context.Pipeline.ReloadForArm(context);
        }

        public static void BranchAndLink32(CpuContext context, uint opcode)
        {
            ref uint pc = ref context.State.Register(CpuRegister.PC);

            int address = DecodeS24(opcode);

            context.SetRegister(CpuRegister.LR, pc - ArmInstructionSize);

            pc = (uint)((int)pc + address);

            context.Pipeline.ReloadForArm(context);
        }

        public static void BranchAndExchange32(CpuContext context, uint opcode)
        {
            ref uint pc = ref context.State.Register(CpuRegister.PC);

            uint address = GetRegister(context, opcode, 0);

            if ((address & 1) != 0)
            {
                pc = address & ~ThumbInstructionSize;

                context.State.CPSR |= (int)CurrentProgramStatusRegister.Thumb;

                context.Pipeline.ReloadForThumb(context);
            }
            else
            {
                pc = address & ~ArmInstructionSize;

                context.Pipeline.ReloadForArm(context);
            }
        }
    }
}
