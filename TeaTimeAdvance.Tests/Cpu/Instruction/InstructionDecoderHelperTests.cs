using NUnit.Framework;
using TeaTimeAdvance.Cpu.Instruction;

namespace TeaTimeAdvance.Tests.Cpu.Instruction
{
    class InstructionDecoderHelperTests
    {
        [TestCase(0x0U, 24, ExpectedResult = 0)]
        [TestCase(0xFFFFFFU, 24, ExpectedResult = -1)]
        [TestCase(0x0U, 8, ExpectedResult = 0)]
        [TestCase(0xFFU, 8, ExpectedResult = -1)]
        public int EnsureSignedImmediateLogic(uint value, int valueBits)
        {
            return InstructionDecoderHelper.SignExtendImmediate(value, valueBits);
        }
    }
}
