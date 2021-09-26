using TeaTimeAdvance.Cpu.Instruction.Definition;
using TeaTimeAdvance.Cpu.State;

using static TeaTimeAdvance.Cpu.Instruction.InstructionDecoderHelper;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionHandler
    {
        public static void Branch32(CpuContext context, uint opcode)
        {
            BranchFormat format = new BranchFormat
            {
                Opcode = opcode
            };

            ref uint pc = ref context.State.Register(CpuRegister.PC);

            pc = (uint)((int)pc + format.Offset);

            context.Pipeline.ReloadForArm(context);
        }

        public static void BranchAndLink32(CpuContext context, uint opcode)
        {
            BranchFormat format = new BranchFormat
            {
                Opcode = opcode
            };

            ref uint pc = ref context.State.Register(CpuRegister.PC);

            context.SetRegister(CpuRegister.LR, pc - ArmInstructionSize);

            pc = (uint)((int)pc + format.Offset);

            context.Pipeline.ReloadForArm(context);
        }

        public static void BranchAndExchange32(CpuContext context, uint opcode)
        {
            BranchExchangeFormat format = new BranchExchangeFormat
            {
                Opcode = opcode
            };

            ref uint pc = ref context.State.Register(CpuRegister.PC);

            uint address = context.GetRegister(format.Rn);

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
