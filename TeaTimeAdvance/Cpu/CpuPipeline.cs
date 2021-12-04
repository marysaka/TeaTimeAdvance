using System;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Common.Memory;
using TeaTimeAdvance.Cpu.Instruction;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu
{
    public class CpuPipeline
    {
        private const uint ArmInstructionSize = InstructionDecoderHelper.ArmInstructionSize;
        private const uint ThumbInstructionSize = InstructionDecoderHelper.ThumbInstructionSize;

        private enum PipelineIndex : uint
        {
            FetchStage,
            DecodeStage,
            ExecuteStage
        }

        // FIXME: currently assuming NOP
        private const uint UndefinedStateInstruction = 0x00F020E3;

        private Array3<uint> _pipelineCache;
        private BusAccessType _busAccessType;

        public CpuPipeline()
        {
            Reset();
        }

        public void Reset()
        {
            _pipelineCache.ToSpan().Fill(UndefinedStateInstruction);
            _busAccessType = BusAccessType.NonSequential;
        }

        public void ReloadForArm(CpuContext context)
        {
            ref uint pc = ref context.State.Register(CpuRegister.PC);

            _pipelineCache[(int)PipelineIndex.ExecuteStage] = context.BusContext.Read32(pc, BusAccessType.NonSequential);
            _pipelineCache[(int)PipelineIndex.FetchStage] = context.BusContext.Read32(pc + ArmInstructionSize, BusAccessType.Sequential);
            _busAccessType = BusAccessType.Sequential;

            pc += ArmInstructionSize * 2;
        }

        public void ReloadForThumb(CpuContext context)
        {
            ref uint pc = ref context.State.Register(CpuRegister.PC);

            _pipelineCache[(int)PipelineIndex.ExecuteStage] = context.BusContext.Read16(pc, BusAccessType.NonSequential);
            _pipelineCache[(int)PipelineIndex.FetchStage] = context.BusContext.Read16(pc + ThumbInstructionSize, BusAccessType.Sequential);
            _busAccessType = BusAccessType.Sequential;

            pc += ThumbInstructionSize * 2;
        }

        private void Fetch(CpuContext context)
        {
            _pipelineCache[(int)PipelineIndex.DecodeStage] = _pipelineCache[(int)PipelineIndex.FetchStage];

            ref uint pc = ref context.State.Register(CpuRegister.PC);

            uint fetchedValue;

            if (context.State.StatusRegister.HasFlag(CurrentProgramStatusRegister.Thumb))
            {
                pc &= ~(ThumbInstructionSize - 1);

                fetchedValue = context.BusContext.Read16(pc, _busAccessType);
            }
            else
            {
                pc &= ~(ArmInstructionSize - 1);

                fetchedValue = context.BusContext.Read32(pc, _busAccessType);
            }

            _pipelineCache[(int)PipelineIndex.FetchStage] = fetchedValue;
        }

        private void Decode(CpuContext context)
        {
            // Advance the previous decode stage value to the execution stage
            _pipelineCache[(int)PipelineIndex.ExecuteStage] = _pipelineCache[(int)PipelineIndex.DecodeStage];
        }

        private static bool ShouldExecute(CpuContext context, uint opcode)
        {
            CpuConditionCode cc = InstructionDecoderHelper.GetConditionCodeFromOpcode(opcode);
            CurrentProgramStatusRegister cpsr = context.State.StatusRegister;

            switch (cc)
            {
                case CpuConditionCode.EQ:
                    return cpsr.HasFlag(CurrentProgramStatusRegister.Zero);
                case CpuConditionCode.NE:
                    return !cpsr.HasFlag(CurrentProgramStatusRegister.Zero);
                case CpuConditionCode.CS:
                    return cpsr.HasFlag(CurrentProgramStatusRegister.Carry);
                case CpuConditionCode.CC:
                    return !cpsr.HasFlag(CurrentProgramStatusRegister.Carry);
                case CpuConditionCode.MI:
                    return cpsr.HasFlag(CurrentProgramStatusRegister.Negative);
                case CpuConditionCode.PL:
                    return !cpsr.HasFlag(CurrentProgramStatusRegister.Negative);
                case CpuConditionCode.VS:
                    return cpsr.HasFlag(CurrentProgramStatusRegister.Overflow);
                case CpuConditionCode.VC:
                    return !cpsr.HasFlag(CurrentProgramStatusRegister.Overflow);
                case CpuConditionCode.HI:
                    return cpsr.HasFlag(CurrentProgramStatusRegister.Carry) && !cpsr.HasFlag(CurrentProgramStatusRegister.Zero);
                case CpuConditionCode.LS:
                    return !cpsr.HasFlag(CurrentProgramStatusRegister.Carry) || cpsr.HasFlag(CurrentProgramStatusRegister.Zero);
                case CpuConditionCode.GE:
                    return cpsr.HasFlag(CurrentProgramStatusRegister.Negative) == cpsr.HasFlag(CurrentProgramStatusRegister.Overflow);
                case CpuConditionCode.LT:
                    return cpsr.HasFlag(CurrentProgramStatusRegister.Negative) != cpsr.HasFlag(CurrentProgramStatusRegister.Overflow);
                case CpuConditionCode.GT:
                    return !cpsr.HasFlag(CurrentProgramStatusRegister.Zero) &&
                        cpsr.HasFlag(CurrentProgramStatusRegister.Negative) != cpsr.HasFlag(CurrentProgramStatusRegister.Overflow);
                case CpuConditionCode.LE:
                    return cpsr.HasFlag(CurrentProgramStatusRegister.Zero) ||
                        cpsr.HasFlag(CurrentProgramStatusRegister.Negative) == cpsr.HasFlag(CurrentProgramStatusRegister.Overflow);
                case CpuConditionCode.AL:
                    return true;
                case CpuConditionCode.NV:
                    return false;
                default:
                    throw new NotImplementedException(cc.ToString());
            }
        }

        private void Execute(CpuContext context, uint opcode)
        {
            bool isThumb = context.State.StatusRegister.HasFlag(CurrentProgramStatusRegister.Thumb);

            // First make sure that we don't have to skip this instruction
            if (!isThumb && !ShouldExecute(context, opcode))
            {
                _busAccessType = BusAccessType.Sequential;
                context.UpdateProgramCounter32();
            }
            else
            {
                InstructionInfo info;

                if (isThumb)
                {
                    info = OpCodeTable.GetThumbInstructionInfo((ushort)opcode);
                }
                else
                {
                    info = OpCodeTable.GetArmInstructionInfo(opcode);
                }

                if (info == null)
                {
                    throw new NotImplementedException($"Unknown opcode: 0x{opcode:X8}");
                }
                else if (info.ExecutionHandler == null)
                {
                    throw new NotImplementedException($"Unimplemented instruction: \"{info.Disassemble(opcode)}\"");
                }

                Console.WriteLine($"0x{context.GetRegister(CpuRegister.PC):X8}: {info.Disassemble(opcode)} (0x{opcode:X8})");

                info.ExecutionHandler(context, opcode);
            }
        }

        public void Update(CpuContext context)
        {
            // First we grab the execution stage opcode to avoid possible pipeline shenanigan (understand reload)
            uint opcode = _pipelineCache[(int)PipelineIndex.ExecuteStage];

            // 3 stage pipeline

            Fetch(context);
            Decode(context);
            Execute(context, opcode);
        }
    }
}
