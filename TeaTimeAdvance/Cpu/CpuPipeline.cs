using System;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Common.Memory;
using TeaTimeAdvance.Cpu.Instructions;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu
{
    public class CpuPipeline
    {
        private const uint ArmInstructionSize = sizeof(uint);
        private const uint ThumbInstructionSize = sizeof(ushort);

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

        private void Fetch(CpuContext context)
        {
            // Advance the previous fetch stage value to the decode stage
            _pipelineCache[(int)PipelineIndex.DecodeStage] = _pipelineCache[(int)PipelineIndex.FetchStage];

            ref uint pc = ref context.State.Register(CpuRegister.PC);

            uint fetchedValue;

            if (context.State.StatusRegister.HasFlag(CurrentProgramStatusRegister.Thumb))
            {
                pc &= ~ThumbInstructionSize;

                fetchedValue = context.BusContext.Read16(pc, _busAccessType);
            }
            else
            {
                pc &= ~ArmInstructionSize;

                fetchedValue = context.BusContext.Read32(pc, _busAccessType);
            }

            _pipelineCache[(int)PipelineIndex.FetchStage] = fetchedValue;
        }

        private void Decode(CpuContext context)
        {
            // Advance the previous decode stage value to the execution stage
            _pipelineCache[(int)PipelineIndex.ExecuteStage] = _pipelineCache[(int)PipelineIndex.DecodeStage];

            // FIXME: Do anything else here?
        }

        private static bool ShouldExecute(CpuContext context, uint opcode)
        {
            CpuConditionCode cc = (CpuConditionCode)(opcode >> 28);
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
                default:
                    throw new NotImplementedException(cc.ToString());
            }
        }

        private void Execute(CpuContext context)
        {
            uint opcode = _pipelineCache[(int)PipelineIndex.ExecuteStage];

            bool isThumb = context.State.StatusRegister.HasFlag(CurrentProgramStatusRegister.Thumb);

            // First make sure that we don't have to skip this instruction
            if (!isThumb && !ShouldExecute(context, opcode))
            {
                _busAccessType = BusAccessType.Sequential;
                context.State.Register(CpuRegister.PC) += ArmInstructionSize;
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
                    throw new NotImplementedException($"Unknown opcode: 0x{opcode:X4}");
                }

                info.ExecutionHandler(context, opcode);
            }
        }

        public void Update(CpuContext context)
        {
            // 3 stage pipeline
            Fetch(context);
            Decode(context);
            Execute(context);
        }
    }
}
