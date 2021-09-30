using NUnit.Framework;
using System.Collections.Generic;
using TeaTimeAdvance.Cpu.Instruction;

namespace TeaTimeAdvance.Tests.Cpu.Instruction.Disassembler
{
    class InstructionDisassemblerTests
    {
        private class DisassemblerTest
        {
            public readonly uint Opcode;
            public readonly string Name;
            public readonly string Disassembly;

            public bool IsThumb => (Opcode >> 16) == 0;

            public DisassemblerTest(uint opcode, string name, string disassembly)
            {
                Opcode = opcode;
                Name = name;
                Disassembly = disassembly;
            }
        }

        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (DisassemblerTest test in Tests)
                {
                    yield return new TestCaseData(test.Opcode, test.IsThumb).SetName($"\"{test.Disassembly}\" (0x{test.Opcode:X4})").Returns((test.Name, test.Disassembly));
                }
            }
        }

        // TODO: Test all variantes of all register and immediate variants
        private static readonly DisassemblerTest[] Tests = new DisassemblerTest[]
        {
            // Test all Arm instructions.
            new DisassemblerTest(0xE0000000U, "AND", "AND R0, R0, R0"),
            new DisassemblerTest(0xE2000000U, "AND", "AND R0, R0, #0x0"),
            new DisassemblerTest(0xE0200000U, "EOR", "EOR R0, R0, R0"),
            new DisassemblerTest(0xE2200000U, "EOR", "EOR R0, R0, #0x0"),
            new DisassemblerTest(0xE0400000U, "SUB", "SUB R0, R0, R0"),
            new DisassemblerTest(0xE2400000U, "SUB", "SUB R0, R0, #0x0"),
            new DisassemblerTest(0xE0600000U, "RSB", "RSB R0, R0, R0"),
            new DisassemblerTest(0xE2600000U, "RSB", "RSB R0, R0, #0x0"),
            new DisassemblerTest(0xE0800000U, "ADD", "ADD R0, R0, R0"),
            new DisassemblerTest(0xE2800000U, "ADD", "ADD R0, R0, #0x0"),
            new DisassemblerTest(0xE0A00000U, "ADC", "ADC R0, R0, R0"),
            new DisassemblerTest(0xE2A00000U, "ADC", "ADC R0, R0, #0x0"),
            new DisassemblerTest(0xE0C00000U, "SBC", "SBC R0, R0, R0"),
            new DisassemblerTest(0xE2C00000U, "SBC", "SBC R0, R0, #0x0"),
            new DisassemblerTest(0xE0E00000U, "RSC", "RSC R0, R0, R0"),
            new DisassemblerTest(0xE2E00000U, "RSC", "RSC R0, R0, #0x0"),
            new DisassemblerTest(0xE1100000U, "TST", "TST R0, R0"),
            new DisassemblerTest(0xE3100000U, "TST", "TST R0, #0x0"),
            new DisassemblerTest(0xE1300000U, "TEQ", "TEQ R0, R0"),
            new DisassemblerTest(0xE3300000U, "TEQ", "TEQ R0, #0x0"),
            new DisassemblerTest(0xE1500000U, "CMP", "CMP R0, R0"),
            new DisassemblerTest(0xE3500000U, "CMP", "CMP R0, #0x0"),
            new DisassemblerTest(0xE1700000U, "CMN", "CMN R0, R0"),
            new DisassemblerTest(0xE3700000U, "CMN", "CMN R0, #0x0"),
            new DisassemblerTest(0xE1900000U, "ORR", "ORRS R0, R0, R0"),
            new DisassemblerTest(0xE3900000U, "ORR", "ORRS R0, R0, #0x0"),
            new DisassemblerTest(0xE1B00000U, "MOV", "MOVS R0, R0"),
            new DisassemblerTest(0xE3B00000U, "MOV", "MOVS R0, #0x0"),
            new DisassemblerTest(0xE1D00000U, "BIC", "BICS R0, R0, R0"),
            new DisassemblerTest(0xE3D00000U, "BIC", "BICS R0, R0, #0x0"),
            new DisassemblerTest(0xE1F00000U, "MVN", "MVNS R0, R0"),
            new DisassemblerTest(0xE3F00000U, "MVN", "MVNS R0, #0x0"),
            new DisassemblerTest(0xE10F0000U, "MRS", "MRS R0, CPSR"),
            new DisassemblerTest(0xE129F000U, "MSR", "MSR CPSR, R0"),
            new DisassemblerTest(0xE369F000U, "MSR", "MSR SPSR, #0x0"),
            new DisassemblerTest(0xE321F000U, "MSR", "MSR CPSR_C, #0x0"),
            new DisassemblerTest(0xE0000090U, "MUL", "MUL R0, R0, R0"),
            new DisassemblerTest(0xE0200090U, "MLA", "MLA R0, R0, R0, R0"),
            new DisassemblerTest(0xE0800090U, "UMULL", "UMULL R0, R0, R0, R0"),
            new DisassemblerTest(0xE0A00090U, "UMLAL", "UMLAL R0, R0, R0, R0"),
            new DisassemblerTest(0xE0C00090U, "SMULL", "SMULL R0, R0, R0, R0"),
            new DisassemblerTest(0xE0E00090U, "SMLAL", "SMLAL R0, R0, R0, R0"),
            new DisassemblerTest(0xE4000000U, "STR", "STR R0, R0"),
            new DisassemblerTest(0xE5900000U, "LDR", "LDR R0, R0"),
            new DisassemblerTest(0xE6800000U, "STR", "STR R0, [R0], R0"),
            new DisassemblerTest(0xE7900000U, "LDR", "LDR R0, [R0, R0]"),
            new DisassemblerTest(0xE19000B0U, "LDRH", "LDRH R0, [R0, R0]"),
            new DisassemblerTest(0xE18000B0U, "STRH", "STRH R0, [R0, R0]"),
            new DisassemblerTest(0xE19000D0U, "LDRSB", "LDRSB R0, [R0, R0]"),
            new DisassemblerTest(0xE1D000F0U, "LDRH", "LDRH R0, [R0]"),
            new DisassemblerTest(0xE1C000B0U, "STRH", "STRH R0, [R0]"),
            new DisassemblerTest(0xE1D000D0U, "LDRSB", "LDRSB R0, [R0]"),
            new DisassemblerTest(0xE8800001U, "STMIA", "STMIA R0, {R0}"),
            new DisassemblerTest(0xE9800001U, "STMIB", "STMIB R0, {R0}"),
            new DisassemblerTest(0xE8000001U, "STMDA", "STMDA R0, {R0}"),
            new DisassemblerTest(0xE9000001U, "STMDB", "STMDB R0, {R0}"),
            new DisassemblerTest(0xE8900001U, "LDMIA", "LDMIA R0, {R0}"),
            new DisassemblerTest(0xE9900001U, "LDMIB", "LDMIB R0, {R0}"),
            new DisassemblerTest(0xE8100001U, "LDMDA", "LDMDA R0, {R0}"),
            new DisassemblerTest(0xE9100001U, "LDMDB", "LDMDB R0, {R0}"),
            new DisassemblerTest(0xE1000090U, "SWP", "SWP R0, R0, [R0]"),
            new DisassemblerTest(0xE1400090U, "SWPB", "SWPB R0, R0, [R0]"),
            new DisassemblerTest(0xEF000000U, "SWI", "SWI #0x0"),
            // TODO: handle disassmbly of coprocessor operations.
            new DisassemblerTest(0xEE000000U, "CDP", "CDP"),
            new DisassemblerTest(0xED900000U, "LDC", "LDC"),
            new DisassemblerTest(0xED800000U, "STC", "STC"),
            new DisassemblerTest(0xEE000010U, "MCR", "MCR"),
            new DisassemblerTest(0xEE100010U, "MRC", "MRC"),
            new DisassemblerTest(0xE6000010U, "UND", "UND"),

            // Misc Arm disassembler tests
            new DisassemblerTest(0xE1A0000FU, "MOV", "MOV R0, PC"),
            new DisassemblerTest(0xE1B0000FU, "MOV", "MOVS R0, PC"),
            new DisassemblerTest(0xE3E00301U, "MVN", "MVN R0, #0x4000000"),
            new DisassemblerTest(0xE3F00301U, "MVN", "MVNS R0, #0x4000000"),
            new DisassemblerTest(0xE1E0000FU, "MVN", "MVN R0, PC"),
            new DisassemblerTest(0xE1F0000FU, "MVN", "MVNS R0, PC"),
            new DisassemblerTest(0xE3A00301U, "MOV", "MOV R0, #0x4000000"),
            new DisassemblerTest(0xE3B00301U, "MOV", "MOVS R0, #0x4000000"),
            new DisassemblerTest(0xE1A00010U, "MOV", "MOV R0, R0, LSL R0"),
            new DisassemblerTest(0xE1A00030U, "MOV", "MOV R0, R0, LSR R0"),
            new DisassemblerTest(0xE1A00050U, "MOV", "MOV R0, R0, ASR R0"),
            new DisassemblerTest(0xE1A00070U, "MOV", "MOV R0, R0, ROR R0"),
            new DisassemblerTest(0xE1A00100U, "MOV", "MOV R0, R0, LSL #0x2"),
            new DisassemblerTest(0xE1A00120U, "MOV", "MOV R0, R0, LSR #0x2"),
            new DisassemblerTest(0xE1A00140U, "MOV", "MOV R0, R0, ASR #0x2"),
            new DisassemblerTest(0xE1A00160U, "MOV", "MOV R0, R0, ROR #0x2"),
            new DisassemblerTest(0xE3500301U, "CMP", "CMP R0, #0x4000000"),
            new DisassemblerTest(0xE1500000U, "CMP", "CMP R0, R0"),
            new DisassemblerTest(0xE1500050U, "CMP", "CMP R0, R0, ASR R0"),
            new DisassemblerTest(0xE1500100U, "CMP", "CMP R0, R0, LSL #0x2"),
            new DisassemblerTest(0xE0000010U, "AND", "AND R0, R0, R0, LSL R0"),
            new DisassemblerTest(0xE0000100U, "AND", "AND R0, R0, R0, LSL #0x2"),
            new DisassemblerTest(0xE10F0000U, "MRS", "MRS R0, CPSR"),
            new DisassemblerTest(0xE14F0000U, "MRS", "MRS R0, SPSR"),
            new DisassemblerTest(0xE169F000U, "MSR", "MSR SPSR, R0"),
            new DisassemblerTest(0xE129F000U, "MSR", "MSR CPSR, R0"),
            new DisassemblerTest(0xE369F000U, "MSR", "MSR SPSR, #0x0"),
            new DisassemblerTest(0xE329F000U, "MSR", "MSR CPSR, #0x0"),
            new DisassemblerTest(0xE121F000U, "MSR", "MSR CPSR_C, R0"),
            new DisassemblerTest(0xE0800090U, "UMULL", "UMULL R0, R0, R0, R0"),
            new DisassemblerTest(0xE0A00090U, "UMLAL", "UMLAL R0, R0, R0, R0"),
            new DisassemblerTest(0xE0C00090U, "SMULL", "SMULL R0, R0, R0, R0"),
            new DisassemblerTest(0xE0E00090U, "SMLAL", "SMLAL R0, R0, R0, R0"),
            new DisassemblerTest(0xE7A21004U, "STR", "STR R1, [R2, R4]!"),
            new DisassemblerTest(0xE7821004U, "STR", "STR R1, [R2, R4]"),
            new DisassemblerTest(0xE7021004U, "STR", "STR R1, [R2, -R4]"),
            new DisassemblerTest(0xE6821004U, "STR", "STR R1, [R2], R4"),
            new DisassemblerTest(0xE5921010U, "LDR", "LDR R1, [R2, #0x10]"),
            new DisassemblerTest(0xE7921103U, "LDR", "LDR R1, [R2, R3, LSL #0x2]"),
            new DisassemblerTest(0xE7921103U, "LDR", "LDR R1, [R2, R3, LSL #0x2]"),
            new DisassemblerTest(0x05D61005U, "LDR", "LDRBEQ R1, [R6, #0x5]"),
            new DisassemblerTest(0xE1B210B3U, "LDRH", "LDRH R1, [R2, R3]!"),
            new DisassemblerTest(0xE13210B3U, "LDRH", "LDRH R1, [R2, -R3]!"),
            new DisassemblerTest(0xE0528DDFU, "LDRSB", "LDRSB R8, [R2], #-0xDF"),
            new DisassemblerTest(0x01D0B0B0U, "LDRH", "LDRHEQ R11, [R0]"),
            new DisassemblerTest(0xE1C431B4U, "STRH", "STRH R3, [R4, #0x14]"),
            new DisassemblerTest(0xE8800007U, "STMIA", "STMIA R0, {R0, R1, R2}"),
            new DisassemblerTest(0xE8C00007U, "STMIA", "STMIA R0, {R0, R1, R2}^"),
            new DisassemblerTest(0xE8A00007U, "STMIA", "STMIA R0!, {R0, R1, R2}"),
            new DisassemblerTest(0xE1020091U, "SWP", "SWP R0, R1, [R2]"),
            new DisassemblerTest(0xE1442093U, "SWPB", "SWPB R2, R3, [R4]"),
            new DisassemblerTest(0x01010090U, "SWP", "SWPEQ R0, R0, [R1]"),
            new DisassemblerTest(0xEFDEADBEU, "SWI", "SWI #0xDEADBE"),

            // Test all Thumb instructions.
            new DisassemblerTest(0x0051, "LSL", "LSL R1, R2, #0x1"),
            new DisassemblerTest(0x0851, "LSR", "LSR R1, R2, #0x1"),
            new DisassemblerTest(0x1051, "ASR", "ASR R1, R2, #0x1"),
            new DisassemblerTest(0x1851, "ADD", "ADD R1, R2, R1"),
            new DisassemblerTest(0x1C51, "ADD", "ADD R1, R2, #0x1"),
            new DisassemblerTest(0x1A51, "SUB", "SUB R1, R2, R1"),
            new DisassemblerTest(0x1E51, "SUB", "SUB R1, R2, #0x1"),
            new DisassemblerTest(0x2104, "MOV", "MOV R1, #0x4"),
            new DisassemblerTest(0x2904, "CMP", "CMP R1, #0x4"),
            new DisassemblerTest(0x3104, "ADD", "ADD R1, #0x4"),
            new DisassemblerTest(0x3904, "SUB", "SUB R1, #0x4"),
            new DisassemblerTest(0x4004, "AND", "AND R4, R0"),
            new DisassemblerTest(0x4044, "EOR", "EOR R4, R0"),
            new DisassemblerTest(0x4084, "LSL", "LSL R4, R0"),
            new DisassemblerTest(0x40C4, "LSR", "LSR R4, R0"),
            new DisassemblerTest(0x4104, "ASR", "ASR R4, R0"),
            new DisassemblerTest(0x4144, "ADC", "ADC R4, R0"),
            new DisassemblerTest(0x4184, "SBC", "SBC R4, R0"),
            new DisassemblerTest(0x41C4, "ROR", "ROR R4, R0"),
            new DisassemblerTest(0x4204, "TST", "TST R4, R0"),
            new DisassemblerTest(0x4244, "NEG", "NEG R4, R0"),
            new DisassemblerTest(0x4284, "CMP", "CMP R4, R0"),
            new DisassemblerTest(0x42C4, "CMN", "CMN R4, R0"),
            new DisassemblerTest(0x4304, "ORR", "ORR R4, R0"),
            new DisassemblerTest(0x4344, "MUL", "MUL R4, R0"),
            new DisassemblerTest(0x4384, "BIC", "BIC R4, R0"),
            new DisassemblerTest(0x43C4, "MVN", "MVN R4, R0"),
            new DisassemblerTest(0x440C, "ADD", "ADD R4, R1"),
            new DisassemblerTest(0x44CC, "ADD", "ADD R11, R8"),
            new DisassemblerTest(0x450C, "CMP", "CMP R4, R1"),
            new DisassemblerTest(0x460C, "MOV", "MOV R4, R1"),
            new DisassemblerTest(0x470C, "BX", "BX R1"),
            new DisassemblerTest(0x4901, "LDR", "LDR R1, [PC, #0x4]"),
            new DisassemblerTest(0x5049, "STR", "STR R1, [R1, R1]"),
            new DisassemblerTest(0x5449, "STRB", "STRB R1, [R1, R1]"),
            new DisassemblerTest(0x5849, "LDR", "LDR R1, [R1, R1]"),
            new DisassemblerTest(0x5C49, "LDRB", "LDRB R1, [R1, R1]"),
            new DisassemblerTest(0x5249, "STRH", "STRH R1, [R1, R1]"),
            new DisassemblerTest(0x5249, "STRH", "STRH R1, [R1, R1]"),
            new DisassemblerTest(0x5649, "LDSB", "LDSB R1, [R1, R1]"),
            new DisassemblerTest(0x5A49, "LDRH", "LDRH R1, [R1, R1]"),
            new DisassemblerTest(0x5E49, "LDSH", "LDSH R1, [R1, R1]"),
            new DisassemblerTest(0x6049, "STR", "STR R1, [R1, #0x1]"),
            new DisassemblerTest(0x6849, "LDR", "LDR R1, [R1, #0x1]"),
            new DisassemblerTest(0x7049, "STRB", "STRB R1, [R1, #0x1]"),
            new DisassemblerTest(0x7849, "LDRB", "LDRB R1, [R1, #0x1]"),
            new DisassemblerTest(0x8049, "STRH", "STRH R1, [R1, #0x1]"),
            new DisassemblerTest(0x8849, "LDRH", "LDRH R1, [R1, #0x1]"),
            new DisassemblerTest(0x9101, "STR", "STR R1, [SP, #0x4]"),
            new DisassemblerTest(0x9901, "LDR", "LDR R1, [SP, #0x4]"),
            new DisassemblerTest(0xA101, "ADD", "ADD R1, PC, #0x4"),
            new DisassemblerTest(0xA901, "ADD", "ADD R1, SP, #0x4"),
            new DisassemblerTest(0xB001, "ADD", "ADD SP, #0x4"),
            new DisassemblerTest(0xB081, "ADD", "ADD SP, #-0x4"),
            new DisassemblerTest(0xB401, "PUSH", "PUSH {R0}"),
            new DisassemblerTest(0xB501, "PUSH", "PUSH {R0, R14}"),
            new DisassemblerTest(0xBC01, "POP", "POP {R0}"),
            new DisassemblerTest(0xBD01, "POP", "POP {R0, PC}"),
            new DisassemblerTest(0xC102, "STMIA", "STMIA R1!, {R1}"),
            new DisassemblerTest(0xC902, "LDMIA", "LDMIA R1!, {R1}"),
            new DisassemblerTest(0xD001, "B", "BEQ #0x6"),
            new DisassemblerTest(0xD0FE, "B", "BEQ #0x0"),
            new DisassemblerTest(0xD0FC, "B", "BEQ #-0x4")
        };

        [TestCaseSource(nameof(TestCases))]
        public (string, string) EnsureArmInstructionDisassemblyMatching(uint opcode, bool isThumb)
        {
            InstructionInfo info = isThumb ? OpCodeTable.GetThumbInstructionInfo((ushort)opcode) : OpCodeTable.GetArmInstructionInfo(opcode);

            if (info == null)
            {
                return (null, null);
            }

            return (info.Name, info.Disassemble(opcode));
        }
    }
}
