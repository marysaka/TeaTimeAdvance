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
        [TestCase(0xE8800001, ExpectedResult = "STMIA", TestName = "STMIA naming")]
        [TestCase(0xE9800001, ExpectedResult = "STMIB", TestName = "STMIB naming")]
        [TestCase(0xE8000001, ExpectedResult = "STMDA", TestName = "STMDA naming")]
        [TestCase(0xE9000001, ExpectedResult = "STMDB", TestName = "STMDB naming")]
        [TestCase(0xE8900001, ExpectedResult = "LDMIA", TestName = "LDMIA naming")]
        [TestCase(0xE9900001, ExpectedResult = "LDMIB", TestName = "LDMIB naming")]
        [TestCase(0xE8100001, ExpectedResult = "LDMDA", TestName = "LDMDA naming")]
        [TestCase(0xE9100001, ExpectedResult = "LDMDB", TestName = "LDMDB naming")]

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
        [TestCase(0xE1A0000FU, ExpectedResult = "MOV R0, R15", TestName = "MOV disassemly (register)")]
        [TestCase(0xE1B0000FU, ExpectedResult = "MOVS R0, R15", TestName = "MOVS disassemly (register)")]
        [TestCase(0xE3E00301U, ExpectedResult = "MVN R0, #0x4000000", TestName = "MVN disassemly (immediate)")]
        [TestCase(0xE3F00301U, ExpectedResult = "MVNS R0, #0x4000000", TestName = "MVNS disassemly (immediate)")]
        [TestCase(0xE1E0000FU, ExpectedResult = "MVN R0, R15", TestName = "MVN disassemly (register)")]
        [TestCase(0xE1F0000FU, ExpectedResult = "MVNS R0, R15", TestName = "MVNS disassemly (register)")]
        [TestCase(0xE3A00301U, ExpectedResult = "MOV R0, #0x4000000", TestName = "MOV disassemly (immediate)")]
        [TestCase(0xE3B00301U, ExpectedResult = "MOVS R0, #0x4000000", TestName = "MOVS disassemly (immediate)")]
        [TestCase(0xE1A00010U, ExpectedResult = "MOV R0, R0, LSL R0", TestName = "MOV disassemly (register, LSL register)")]
        [TestCase(0xE1A00030U, ExpectedResult = "MOV R0, R0, LSR R0", TestName = "MOV disassemly (register, LSR register)")]
        [TestCase(0xE1A00050U, ExpectedResult = "MOV R0, R0, ASR R0", TestName = "MOV disassemly (register, ASR register)")]
        [TestCase(0xE1A00070U, ExpectedResult = "MOV R0, R0, ROR R0", TestName = "MOV disassemly (register, ROR register)")]
        [TestCase(0xE1A00100U, ExpectedResult = "MOV R0, R0, LSL #0x2", TestName = "MOV disassemly (register, LSL immediate)")]
        [TestCase(0xE1A00120U, ExpectedResult = "MOV R0, R0, LSR #0x2", TestName = "MOV disassemly (register, LSR immediate)")]
        [TestCase(0xE1A00140U, ExpectedResult = "MOV R0, R0, ASR #0x2", TestName = "MOV disassemly (register, ASR immediate)")]
        [TestCase(0xE1A00160U, ExpectedResult = "MOV R0, R0, ROR #0x2", TestName = "MOV disassemly (register, ROR immediate)")]
        [TestCase(0xE3500301U, ExpectedResult = "CMP R0, #0x4000000", TestName = "CMP disassemly (immediate)")]
        [TestCase(0xE1500000U, ExpectedResult = "CMP R0, R0", TestName = "CMP disassemly (register)")]
        [TestCase(0xE1500050U, ExpectedResult = "CMP R0, R0, ASR R0", TestName = "CMP disassemly (register, ASR register)")]
        [TestCase(0xE1500100U, ExpectedResult = "CMP R0, R0, LSL #0x2", TestName = "CMP disassemly (register, LSL immediate)")]
        [TestCase(0xE0000000U, ExpectedResult = "AND R0, R0, R0", TestName = "AND disassemly (register)")]
        [TestCase(0xE0000010U, ExpectedResult = "AND R0, R0, R0, LSL R0", TestName = "AND disassemly (register, LSL register)")]
        [TestCase(0xE0000100U, ExpectedResult = "AND R0, R0, R0, LSL #0x2", TestName = "AND disassemly (register, LSL immediate)")]

        // PSR Transfer Format
        [TestCase(0xE10F0000U, ExpectedResult = "MRS R0, CPSR", TestName = "MRS disassemly (register, CPSR)")]
        [TestCase(0xE14F0000U, ExpectedResult = "MRS R0, SPSR", TestName = "MRS disassemly (register, SPSR)")]
        [TestCase(0xE169F000U, ExpectedResult = "MSR SPSR, R0", TestName = "MSR disassemly (register, SPSR)")]
        [TestCase(0xE129F000U, ExpectedResult = "MSR CPSR, R0", TestName = "MSR disassemly (register, CPSR)")]
        [TestCase(0xE369F000U, ExpectedResult = "MSR SPSR, #0x0", TestName = "MSR disassemly (immediate, SPSR)")]
        [TestCase(0xE329F000U, ExpectedResult = "MSR CPSR, #0x0", TestName = "MSR disassemly (immediate, CPSR)")]
        [TestCase(0xE121F000U, ExpectedResult = "MSR CPSR_C, R0", TestName = "MSR disassemly (register, CPSR_c)")]

        // Multiply Format
        [TestCase(0xE0000090U, ExpectedResult = "MUL R0, R0, R0", TestName = "MUL disassemly")]
        [TestCase(0xE0200090U, ExpectedResult = "MLA R0, R0, R0, R0", TestName = "MLA disassemly")]

        // Multiply Long Format
        [TestCase(0xE0800090U, ExpectedResult = "UMULL R0, R0, R0, R0", TestName = "UMULL disassemly")]
        [TestCase(0xE0A00090U, ExpectedResult = "UMLAL R0, R0, R0, R0", TestName = "UMLAL disassemly")]
        [TestCase(0xE0C00090U, ExpectedResult = "SMULL R0, R0, R0, R0", TestName = "SMULL disassemly")]
        [TestCase(0xE0E00090U, ExpectedResult = "SMLAL R0, R0, R0, R0", TestName = "SMLAL disassemly")]

        // Single Data Transfer Format
        [TestCase(0xE7A21004U, ExpectedResult = "STR R1, [R2, R4]!", TestName = "STR disassemly (register, pre-indexing, writeback)")]
        [TestCase(0xE7821004U, ExpectedResult = "STR R1, [R2, R4]", TestName = "STR disassemly (register, pre-indexing, no writeback)")]
        [TestCase(0xE7021004U, ExpectedResult = "STR R1, [R2, -R4]", TestName = "STR disassemly (register, pre-indexing, negative, no writeback)")]
        [TestCase(0xE6821004U, ExpectedResult = "STR R1, [R2], R4", TestName = "STR disassemly (register, post-indexing, no writeback)")]
        [TestCase(0xE5921010U, ExpectedResult = "LDR R1, [R2, #0x10]", TestName = "LDR disassemly (immediate, pre-indexing, no writeback)")]
        [TestCase(0xE7921103U, ExpectedResult = "LDR R1, [R2, R3, LSL #0x2]", TestName = "LDR disassemly (register immediate, pre-indexing, no writeback)")]
        [TestCase(0xE7921103U, ExpectedResult = "LDR R1, [R2, R3, LSL #0x2]", TestName = "LDR disassemly (register immediate, pre-indexing, no writeback)")]
        [TestCase(0x05D61005U, ExpectedResult = "LDRBEQ R1, [R6, #0x5]", TestName = "LDR disassemly (immediate, pre-indexing, no writeback, byte transfer)")]

        // Halfword Data Transfer: register offset Format
        [TestCase(0xE1B210B3U, ExpectedResult = "LDRH R1, [R2, R3]!", TestName = "LDRH disassemly (register, pre-indexing, writeback)")]
        [TestCase(0xE13210B3U, ExpectedResult = "LDRH R1, [R2, -R3]!", TestName = "LDRH disassemly (register, pre-indexing, negative, writeback)")]
        [TestCase(0xE0528DDFU, ExpectedResult = "LDRSB R8, [R2], #-0xDF", TestName = "LDRSB disassemly (register, post-indexing, negative, no writeback)")]
        [TestCase(0x01D0B0B0U, ExpectedResult = "LDRHEQ R11, [R0]", TestName = "LDRH disassemly (register, post-indexing, negative, no writeback)")]
        [TestCase(0xE1C431B4U, ExpectedResult = "STRH R3, [R4, #0x14]", TestName = "STRH disassemly (register, pre-indexing, no writeback)")]

        // Data Transfer
        [TestCase(0xE8800007, ExpectedResult = "STMIA R0, {R0, R1, R2}", TestName = "STMIA disassemly (pre-indexing, no writeback)")]
        [TestCase(0xE8C00007, ExpectedResult = "STMIA R0, {R0, R1, R2}^", TestName = "STMIA disassemly (pre-indexing, no writeback, current bank)")]
        [TestCase(0xE8A00007, ExpectedResult = "STMIA R0!, {R0, R1, R2}", TestName = "STMIA disassemly (pre-indexing, writeback, current bank)")]
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
