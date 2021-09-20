using System;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Common.Memory;

namespace TeaTimeAdvance.Cpu.State
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

        private void Execute(CpuContext context)
        {
            throw new NotImplementedException();
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
