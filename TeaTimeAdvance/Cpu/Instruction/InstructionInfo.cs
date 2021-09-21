namespace TeaTimeAdvance.Cpu.Instruction
{
    public class InstructionInfo
    {
        public delegate void ExecuteInstruction(CpuContext context, uint opcode);
        public delegate string DisassembleInstruction(InstructionInfo info, uint opcode);

        public uint Mask { get; }
        public uint Value { get; }
        public string Name { get; }

        public ExecuteInstruction ExecutionHandler { get; }
        public DisassembleInstruction DisassembleHandler { get; }

        public InstructionInfo(uint mask, uint value, string name, ExecuteInstruction executionHandler, DisassembleInstruction disassembleHandler)
        {
            Mask = mask;
            Value = value;
            Name = name;
            ExecutionHandler = executionHandler;
            DisassembleHandler = disassembleHandler;
        }

        public string Disassemble(uint opcode)
        {
            if (DisassembleHandler != null)
            {
                return DisassembleHandler(this, opcode);
            }

            return Name;
        }
    }
}
