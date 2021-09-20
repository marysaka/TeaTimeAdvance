namespace TeaTimeAdvance.Cpu.Instructions
{
    public class InstructionInfo
    {
        public delegate void ExecuteInstruction(CpuContext context, uint opcode);

        public int Mask { get; }
        public int Value { get; }
        public string Name { get; }

        public ExecuteInstruction ExecutionHandler { get; }

        public InstructionInfo(int mask, int value, string name, ExecuteInstruction executionHandler)
        {
            Mask = mask;
            Value = value;
            Name = name;
            ExecutionHandler = executionHandler;
        }
    }
}
