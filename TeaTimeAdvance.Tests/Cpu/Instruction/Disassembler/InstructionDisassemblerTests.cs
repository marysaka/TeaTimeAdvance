using NUnit.Framework;
using TeaTimeAdvance.Cpu.Instruction;

namespace TeaTimeAdvance.Tests.Cpu.Instruction.Disassembler
{
    class InstructionDisassemblerTests
    {
        // TODO: Test all variantes of all register and immediate variants.
        // Data Processing Format
        [TestCase(0xE0000000, ExpectedResult = "AND", TestName = "AND naming (register)")]
        [TestCase(0xE2000000, ExpectedResult = "AND", TestName = "AND naming (immediate)")]
        [TestCase(0xE0200000, ExpectedResult = "EOR", TestName = "EOR naming (register)")]
        [TestCase(0xE2200000, ExpectedResult = "EOR", TestName = "EOR naming (immediate)")]
        [TestCase(0xE0400000, ExpectedResult = "SUB", TestName = "SUB naming (register)")]
        [TestCase(0xE2400000, ExpectedResult = "SUB", TestName = "SUB naming (immediate)")]
        [TestCase(0xE0600000, ExpectedResult = "RSB", TestName = "RSB naming (register)")]
        [TestCase(0xE2600000, ExpectedResult = "RSB", TestName = "RSB naming (immediate)")]
        [TestCase(0xE0800000, ExpectedResult = "ADD", TestName = "ADD naming (register)")]
        [TestCase(0xE2800000, ExpectedResult = "ADD", TestName = "ADD naming (immediate)")]
        [TestCase(0xE0A00000, ExpectedResult = "ADC", TestName = "ADC naming (register)")]
        [TestCase(0xE2A00000, ExpectedResult = "ADC", TestName = "ADC naming (immediate)")]
        [TestCase(0xE0C00000, ExpectedResult = "SBC", TestName = "SBC naming (register)")]
        [TestCase(0xE2C00000, ExpectedResult = "SBC", TestName = "SBC naming (immediate)")]
        [TestCase(0xE0E00000, ExpectedResult = "RSC", TestName = "RSC naming (register)")]
        [TestCase(0xE2E00000, ExpectedResult = "RSC", TestName = "RSC naming (immediate)")]
        [TestCase(0xE1100000, ExpectedResult = "TST", TestName = "TST naming (register)")]
        [TestCase(0xE3100000, ExpectedResult = "TST", TestName = "TST naming (immediate)")]
        [TestCase(0xE1300000, ExpectedResult = "TEQ", TestName = "TEQ naming (register)")]
        [TestCase(0xE3300000, ExpectedResult = "TEQ", TestName = "TEQ naming (immediate)")]
        [TestCase(0xE1500000, ExpectedResult = "CMP", TestName = "CMP naming (register)")]
        [TestCase(0xE3500000, ExpectedResult = "CMP", TestName = "CMP naming (immediate)")]
        [TestCase(0xE1700000, ExpectedResult = "CMN", TestName = "CMN naming (register)")]
        [TestCase(0xE3700000, ExpectedResult = "CMN", TestName = "CMN naming (immediate)")]
        [TestCase(0xE1900000, ExpectedResult = "ORR", TestName = "ORR naming (register)")]
        [TestCase(0xE3900000, ExpectedResult = "ORR", TestName = "ORR naming (immediate)")]
        [TestCase(0xE1B00000, ExpectedResult = "MOV", TestName = "MOV naming (register)")]
        [TestCase(0xE3B00000, ExpectedResult = "MOV", TestName = "MOV naming (immediate)")]
        [TestCase(0xE1D00000, ExpectedResult = "BIC", TestName = "BIC naming (register)")]
        [TestCase(0xE3D00000, ExpectedResult = "BIC", TestName = "BIC naming (immediate)")]
        [TestCase(0xE1F00000, ExpectedResult = "MVN", TestName = "MVN naming (register)")]
        [TestCase(0xE3F00000, ExpectedResult = "MVN", TestName = "MVN naming (immediate)")]

        // PSR Transfer Format
        [TestCase(0xE10F0000, ExpectedResult = "MRS", TestName = "MRS naming (register)")]
        [TestCase(0xE129F000, ExpectedResult = "MSR", TestName = "MSR naming (register)")]
        [TestCase(0xE369F000, ExpectedResult = "MSR", TestName = "MSR naming (immediate)")]
        [TestCase(0xE321F000, ExpectedResult = "MSR", TestName = "MSR naming (immediate)")]

        // Multiply Format
        [TestCase(0xE0000090, ExpectedResult = "MUL", TestName = "MUL naming")]
        [TestCase(0xE0200090, ExpectedResult = "MLA", TestName = "MLA naming")]

        // Long Multiply Format
        [TestCase(0xE0800090, ExpectedResult = "UMULL", TestName = "UMULL naming")]
        [TestCase(0xE0A00090, ExpectedResult = "UMLAL", TestName = "UMLAL naming")]
        [TestCase(0xE0C00090, ExpectedResult = "SMULL", TestName = "SMULL naming")]
        [TestCase(0xE0E00090, ExpectedResult = "SMLAL", TestName = "SMLAL naming")]

        // Single Data Transfer Format
        [TestCase(0xE4000000, ExpectedResult = "STR", TestName = "STR naming (immediate)")]
        [TestCase(0xE5900000, ExpectedResult = "LDR", TestName = "LDR naming (immediate)")]
        [TestCase(0xE6800000, ExpectedResult = "STR", TestName = "STR naming (register)")]
        [TestCase(0xE7900000, ExpectedResult = "LDR", TestName = "LDR naming (register)")]

        // Halfword Data Transfer: register offset Format
        [TestCase(0xE19000B0, ExpectedResult = "LDRH", TestName = "LDRH naming (register)")]
        [TestCase(0xE18000B0, ExpectedResult = "STRH", TestName = "STRH naming (register)")]
        [TestCase(0xE19000D0, ExpectedResult = "LDRSB", TestName = "LDRSB naming (register)")]

        // Halfword Data Transfer: immediate offset Format
        [TestCase(0xE1D000F0, ExpectedResult = "LDRH", TestName = "LDRH naming (immediate)")]
        [TestCase(0xE1C000B0, ExpectedResult = "STRH", TestName = "STRH naming (immediate)")]
        [TestCase(0xE1D000D0, ExpectedResult = "LDRSB", TestName = "LDRSB naming (immediate)")]

        // Data Transfer
        [TestCase(0xE8800001, ExpectedResult = "STM", TestName = "STM naming")]
        [TestCase(0xE8900001, ExpectedResult = "LDM", TestName = "LDM naming")]

        // Singla Data Swap
        [TestCase(0xE1000090, ExpectedResult = "SWP", TestName = "SWP naming")]
        [TestCase(0xE1400090, ExpectedResult = "SWPB", TestName = "SWPB naming")]

        // Software Interrupt
        [TestCase(0xEF000000, ExpectedResult = "SWI", TestName = "SWI naming")]

        // Coprocessor Data Operations
        [TestCase(0xEE000000, ExpectedResult = "CDP", TestName = "CDP naming")]

        // Coprocessor Data Transfers
        [TestCase(0xED900000, ExpectedResult = "LDC", TestName = "LDC naming")]
        [TestCase(0xED800000, ExpectedResult = "STC", TestName = "STC naming")]

        //  Coprocessor Register Transfers
        [TestCase(0xEE000010, ExpectedResult = "MCR", TestName = "MCR naming")]
        [TestCase(0xEE100010, ExpectedResult = "MRC", TestName = "MRC naming")]

        // Undefined Format
        [TestCase(0xE6000010, ExpectedResult = "UND", TestName = "UND naming")]
        public string EnsureArmInstructionNameMatching(uint opcode)
        {
            InstructionInfo info = OpCodeTable.GetArmInstructionInfo(opcode);

            if (info == null)
            {
                return null;
            }

            return info.Name;
        }

        // Data Processing Format
        [TestCase(0xE1A0000F, ExpectedResult = "MOV R0, R15", TestName = "MOV disassemly (register)")]
        [TestCase(0xE1B0000F, ExpectedResult = "MOVS R0, R15", TestName = "MOVS disassemly (register)")]
        [TestCase(0xE3E00301, ExpectedResult = "MVN R0, #0x4000000", TestName = "MVN disassemly (immediate)")]
        [TestCase(0xE3F00301, ExpectedResult = "MVNS R0, #0x4000000", TestName = "MVNS disassemly (immediate)")]
        [TestCase(0xE1E0000F, ExpectedResult = "MVN R0, R15", TestName = "MVN disassemly (register)")]
        [TestCase(0xE1F0000F, ExpectedResult = "MVNS R0, R15", TestName = "MVNS disassemly (register)")]
        [TestCase(0xE3A00301, ExpectedResult = "MOV R0, #0x4000000", TestName = "MOV disassemly (immediate)")]
        [TestCase(0xE3B00301, ExpectedResult = "MOVS R0, #0x4000000", TestName = "MOVS disassemly (immediate)")]
        [TestCase(0xE1A00010, ExpectedResult = "MOV R0, R0, LSL R0", TestName = "MOV disassemly (register, LSL register)")]
        [TestCase(0xE1A00030, ExpectedResult = "MOV R0, R0, LSR R0", TestName = "MOV disassemly (register, LSR register)")]
        [TestCase(0xE1A00050, ExpectedResult = "MOV R0, R0, ASR R0", TestName = "MOV disassemly (register, ASR register)")]
        [TestCase(0xE1A00070, ExpectedResult = "MOV R0, R0, ROR R0", TestName = "MOV disassemly (register, ROR register)")]
        [TestCase(0xE1A00100, ExpectedResult = "MOV R0, R0, LSL #0x2", TestName = "MOV disassemly (register, LSL immediate)")]
        [TestCase(0xE1A00120, ExpectedResult = "MOV R0, R0, LSR #0x2", TestName = "MOV disassemly (register, LSR immediate)")]
        [TestCase(0xE1A00140, ExpectedResult = "MOV R0, R0, ASR #0x2", TestName = "MOV disassemly (register, ASR immediate)")]
        [TestCase(0xE1A00160, ExpectedResult = "MOV R0, R0, ROR #0x2", TestName = "MOV disassemly (register, ROR immediate)")]
        [TestCase(0xE3500301, ExpectedResult = "CMP R0, #0x4000000", TestName = "CMP disassemly (immediate)")]
        [TestCase(0xE1500000, ExpectedResult = "CMP R0, R0", TestName = "CMP disassemly (register)")]
        [TestCase(0xE1500050, ExpectedResult = "CMP R0, R0, ASR R0", TestName = "CMP disassemly (register, ASR register)")]
        [TestCase(0xE1500100, ExpectedResult = "CMP R0, R0, LSL #0x2", TestName = "CMP disassemly (register, LSL immediate)")]
        [TestCase(0xE0000000, ExpectedResult = "AND R0, R0, R0", TestName = "AND disassemly (register)")]
        [TestCase(0xE0000010, ExpectedResult = "AND R0, R0, R0, LSL R0", TestName = "AND disassemly (register, LSL register)")]
        [TestCase(0xE0000100, ExpectedResult = "AND R0, R0, R0, LSL #0x2", TestName = "AND disassemly (register, LSL immediate)")]

        // PSR Transfer Format
        [TestCase(0xE10F0000, ExpectedResult = "MRS R0, CPSR", TestName = "MRS disassemly (register, CPSR)")]
        [TestCase(0xE14F0000, ExpectedResult = "MRS R0, SPSR", TestName = "MRS disassemly (register, SPSR)")]
        [TestCase(0xE169F000, ExpectedResult = "MSR SPSR, R0", TestName = "MSR disassemly (register, SPSR)")]
        [TestCase(0xE129F000, ExpectedResult = "MSR CPSR, R0", TestName = "MSR disassemly (register, CPSR)")]
        [TestCase(0xE369F000, ExpectedResult = "MSR SPSR, #0x0", TestName = "MSR disassemly (immediate, SPSR)")]
        [TestCase(0xE329F000, ExpectedResult = "MSR CPSR, #0x0", TestName = "MSR disassemly (immediate, CPSR)")]
        [TestCase(0xE121F000, ExpectedResult = "MSR CPSR_C, R0", TestName = "MSR disassemly (register, CPSR_c)")]
        public string EnsureArmInstructionDisassemblyMatching(uint opcode)
        {
            InstructionInfo info = OpCodeTable.GetArmInstructionInfo(opcode);

            if (info == null)
            {
                return null;
            }

            return info.Disassemble(opcode);
        }
    }
}
