using System;
using System.Collections.Generic;
using static TeaTimeAdvance.Cpu.Instruction.InstructionDisassembler;
using static TeaTimeAdvance.Cpu.Instruction.InstructionInfo;
using static TeaTimeAdvance.Cpu.Instruction.InstructionHandler;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static class OpCodeTable
    {
        private static List<InstructionInfo> _armInstructions = new List<InstructionInfo>();
        private static List<InstructionInfo> _thumbInstructions = new List<InstructionInfo>();

        static OpCodeTable()
        {
            // Data Processing Format
            // TODO: implement
            SetArm("<<<<00x0000xxxxxxxxxxxxxxxxxxxxx", "AND", null, DisassembleDataProcesingArithmetic32);
            SetArm("<<<<00x0001xxxxxxxxxxxxxxxxxxxxx", "EOR", null, DisassembleDataProcesingArithmetic32);
            SetArm("<<<<00x0010xxxxxxxxxxxxxxxxxxxxx", "SUB", null, DisassembleDataProcesingArithmetic32);
            SetArm("<<<<00x0011xxxxxxxxxxxxxxxxxxxxx", "RSB", null, DisassembleDataProcesingArithmetic32);
            SetArm("<<<<00x0100xxxxxxxxxxxxxxxxxxxxx", "ADD", null, DisassembleDataProcesingArithmetic32);
            SetArm("<<<<00x0101xxxxxxxxxxxxxxxxxxxxx", "ADC", null, DisassembleDataProcesingArithmetic32);
            SetArm("<<<<00x0110xxxxxxxxxxxxxxxxxxxxx", "SBC", null, DisassembleDataProcesingArithmetic32);
            SetArm("<<<<00x0111xxxxxxxxxxxxxxxxxxxxx", "RSC", null, DisassembleDataProcesingArithmetic32);
            SetArm("<<<<00x10001xxxxxxxxxxxxxxxxxxxx", "TST", null, DisassembleDataProcesingOneNoRdOperand32);
            SetArm("<<<<00x10011xxxxxxxxxxxxxxxxxxxx", "TEQ", null, DisassembleDataProcesingOneNoRdOperand32);
            SetArm("<<<<00x10101xxxxxxxxxxxxxxxxxxxx", "CMP", null, DisassembleDataProcesingOneNoRdOperand32);
            SetArm("<<<<00x10111xxxxxxxxxxxxxxxxxxxx", "CMN", null, DisassembleDataProcesingOneNoRdOperand32);
            SetArm("<<<<00x1100xxxxxxxxxxxxxxxxxxxxx", "ORR", null, DisassembleDataProcesingArithmetic32);
            SetArm("<<<<00x1101xxxxxxxxxxxxxxxxxxxxx", "MOV", null, DisassembleDataProcesingOneOperand32);
            SetArm("<<<<00x1110xxxxxxxxxxxxxxxxxxxxx", "BIC", null, DisassembleDataProcesingArithmetic32);
            SetArm("<<<<00x1111xxxxxxxxxxxxxxxxxxxxx", "MVN", null, DisassembleDataProcesingOneOperand32);

            // PSR Transfer Format
            // TODO: implement
            SetArm("<<<<00010x001111xxxx000000000000", "MRS", null, DisassemblePSRTransferDisassembler32);
            SetArm("<<<<00010x10xxxx111100000000xxxx", "MSR", null, DisassemblePSRTransferDisassembler32);
            SetArm("<<<<00x10x10xxxx1111xxxxxxxxxxxx", "MSR", null, DisassemblePSRTransferDisassembler32);

            // Halfword Data Transfer: register offset Format
            // TODO: implement
            SetArm("<<<<000xx0x1xxxxxxxx00001xx1xxxx", "LDRH", null, DisassembleHalfwordDataTransferRegister32);
            SetArm("<<<<000xx0x0xxxxxxxx00001xx1xxxx", "STRH", null, DisassembleHalfwordDataTransferRegister32);
            SetArm("<<<<000xx0x1xxxxxxxx00001x01xxxx", "LDRSB", null, DisassembleHalfwordDataTransferRegister32);

            // Halfword Data Transfer: immediate offset Format
            // TODO: implement
            SetArm("<<<<000xx1x1xxxxxxxxxxxx1xx1xxxx", "LDRH", null, DisassembleHalfwordDataTransferImmediate32);
            SetArm("<<<<000xx1x0xxxxxxxxxxxx1xx1xxxx", "STRH", null, DisassembleHalfwordDataTransferImmediate32);
            SetArm("<<<<000xx1x1xxxxxxxxxxxx1x01xxxx", "LDRSB", null, DisassembleHalfwordDataTransferImmediate32);

            // Multiply Long Format
            // TODO: implement
            SetArm("<<<<0000100xxxxxxxxxxxxx1001xxxx", "UMULL", null, DisassembleMultiplyLong32);
            SetArm("<<<<0000101xxxxxxxxxxxxx1001xxxx", "UMLAL", null, DisassembleMultiplyLong32);
            SetArm("<<<<0000110xxxxxxxxxxxxx1001xxxx", "SMULL", null, DisassembleMultiplyLong32);
            SetArm("<<<<0000111xxxxxxxxxxxxx1001xxxx", "SMLAL", null, DisassembleMultiplyLong32);

            // Single Data Transfer Format
            // TODO: implement
            SetArm("<<<<01xxxxx0xxxxxxxxxxxxxxxxxxxx", "STR", null, DisassembleSingleDataTransfer32);
            SetArm("<<<<01xxxxx1xxxxxxxxxxxxxxxxxxxx", "LDR", null, DisassembleSingleDataTransfer32);

            // Branch and Exchange Format
            SetArm("<<<<000100101111111111110001xxxx", "BX", BranchAndExchange32, DisassembleBranchExchange32);

            // Multiply Format
            // TODO: implement
            SetArm("<<<<0000000xxxxxxxxxxxxx1001xxxx", "MUL", null, DisassembleMultiply32);
            SetArm("<<<<0000001xxxxxxxxxxxxx1001xxxx", "MLA", null, DisassembleMultiply32);

            // Block Data Transfer
            // TODO: implement
            SetArm("<<<<10000xx0xxxxxxxxxxxxxxxxxxxx", "STMDA", null, DisassembleBlockDataTransferDisassembler32);
            SetArm("<<<<10001xx0xxxxxxxxxxxxxxxxxxxx", "STMIA", null, DisassembleBlockDataTransferDisassembler32);
            SetArm("<<<<10010xx0xxxxxxxxxxxxxxxxxxxx", "STMDB", null, DisassembleBlockDataTransferDisassembler32);
            SetArm("<<<<10011xx0xxxxxxxxxxxxxxxxxxxx", "STMIB", null, DisassembleBlockDataTransferDisassembler32);
            SetArm("<<<<10000xx1xxxxxxxxxxxxxxxxxxxx", "LDMDA", null, DisassembleBlockDataTransferDisassembler32);
            SetArm("<<<<10001xx1xxxxxxxxxxxxxxxxxxxx", "LDMIA", null, DisassembleBlockDataTransferDisassembler32);
            SetArm("<<<<10010xx1xxxxxxxxxxxxxxxxxxxx", "LDMDB", null, DisassembleBlockDataTransferDisassembler32);
            SetArm("<<<<10011xx1xxxxxxxxxxxxxxxxxxxx", "LDMIB", null, DisassembleBlockDataTransferDisassembler32);


            // Single Data Swap Format
            // TODO: implement
            SetArm("<<<<00010000xxxxxxxx00001001xxxx", "SWP", null, DisassembleSingleDataSwapDisassembler32);
            SetArm("<<<<00010100xxxxxxxx00001001xxxx", "SWPB", null, DisassembleSingleDataSwapDisassembler32);

            // Software Interrupt
            // TODO: implement
            SetArm("<<<<1111xxxxxxxxxxxxxxxxxxxxxxxx", "SWI", null, DisassembleSoftwareInterruptDisassembler32);

            // Coprocessor Data Operations Format
            // TODO: implement
            SetArm("<<<<1110xxxxxxxxxxxxxxxxxxx0xxxx", "CDP");

            // Coprocessor Data Transfers Format
            // TODO: implement
            SetArm("<<<<110xxxx0xxxxxxxxxxxxxxxxxxxx", "STC");
            SetArm("<<<<110xxxx1xxxxxxxxxxxxxxxxxxxx", "LDC");
            SetArm("<<<<1110xxx0xxxxxxxxxxxxxxx1xxxx", "MCR");
            SetArm("<<<<1110xxx1xxxxxxxxxxxxxxx1xxxx", "MRC");

            // Undefined Format
            // TODO: implement
            SetArm("<<<<011xxxxxxxxxxxxxxxxxxxx1xxxx", "UND");

            // Branch Format
            SetArm("<<<<1010xxxxxxxxxxxxxxxxxxxxxxxx", "B",  Branch32,            DisassembleBranch32);
            SetArm("<<<<1011xxxxxxxxxxxxxxxxxxxxxxxx", "BL", BranchAndLink32,     DisassembleBranch32);


            // Move Shifted Register Format
            // TODO: implement
            SetThumb("00000xxxxxxxxxxx", "LSL", null, DisassembleMoveShiftedRegister16);
            SetThumb("00001xxxxxxxxxxx", "LSR", null, DisassembleMoveShiftedRegister16);
            SetThumb("00010xxxxxxxxxxx", "ASR", null, DisassembleMoveShiftedRegister16);

            // Add / Subtract Format
            // TODO: implement
            SetThumb("00011x0xxxxxxxxx", "ADD", null, DisassembleAddSubtract16);
            SetThumb("00011x1xxxxxxxxx", "SUB", null, DisassembleAddSubtract16);

            // Move / Compare / Add / Subtract Immediate Format
            // TODO: implement
            SetThumb("00100xxxxxxxxxxx", "MOV", null, DisassembleMoveCompareAddSubtractImmediate16);
            SetThumb("00101xxxxxxxxxxx", "CMP", null, DisassembleMoveCompareAddSubtractImmediate16);
            SetThumb("00110xxxxxxxxxxx", "ADD", null, DisassembleMoveCompareAddSubtractImmediate16);
            SetThumb("00111xxxxxxxxxxx", "SUB", null, DisassembleMoveCompareAddSubtractImmediate16);


            // ALU Operations Format
            // TODO: implement
            SetThumb("0100000000xxxxxx", "AND", null, DisassembleALUOperations16);
            SetThumb("0100000001xxxxxx", "EOR", null, DisassembleALUOperations16);
            SetThumb("0100000010xxxxxx", "LSL", null, DisassembleALUOperations16);
            SetThumb("0100000011xxxxxx", "LSR", null, DisassembleALUOperations16);
            SetThumb("0100000100xxxxxx", "ASR", null, DisassembleALUOperations16);
            SetThumb("0100000101xxxxxx", "ADC", null, DisassembleALUOperations16);
            SetThumb("0100000110xxxxxx", "SBC", null, DisassembleALUOperations16);
            SetThumb("0100000111xxxxxx", "ROR", null, DisassembleALUOperations16);
            SetThumb("0100001000xxxxxx", "TST", null, DisassembleALUOperations16);
            SetThumb("0100001001xxxxxx", "NEG", null, DisassembleALUOperations16);
            SetThumb("0100001010xxxxxx", "CMP", null, DisassembleALUOperations16);
            SetThumb("0100001011xxxxxx", "CMN", null, DisassembleALUOperations16);
            SetThumb("0100001100xxxxxx", "ORR", null, DisassembleALUOperations16);
            SetThumb("0100001101xxxxxx", "MUL", null, DisassembleALUOperations16);
            SetThumb("0100001110xxxxxx", "BIC", null, DisassembleALUOperations16);
            SetThumb("0100001111xxxxxx", "MVN", null, DisassembleALUOperations16);

            // Hi Register Operations / Branch Exchange Format
            // TODO: implement
            SetThumb("01000100xxxxxxxx", "ADD", null, DisassembleHiRegisterOperations16);
            SetThumb("01000101xxxxxxxx", "CMP", null, DisassembleHiRegisterOperations16);
            SetThumb("01000110xxxxxxxx", "MOV", null, DisassembleHiRegisterOperations16);
            SetThumb("01000111xxxxxxxx", "BX", null, DisassembleBranchExchange16);

            // PC-relative Load Format
            // TODO: implement
            SetThumb("01001xxxxxxxxxxx", "LDR", null, DisassemblePCRelativeLoad16);

            // Load / Store With Register Offset Format
            // TODO: implement
            SetThumb("0101000xxxxxxxxx", "STR", null, DisassembleLoadStoreWithRegisterOffset16);
            SetThumb("0101010xxxxxxxxx", "STRB", null, DisassembleLoadStoreWithRegisterOffset16);
            SetThumb("0101100xxxxxxxxx", "LDR", null, DisassembleLoadStoreWithRegisterOffset16);
            SetThumb("0101110xxxxxxxxx", "LDRB", null, DisassembleLoadStoreWithRegisterOffset16);

            // Load / Store sign-extended Byte / Halfword Format
            // TODO: implement
            SetThumb("0101001xxxxxxxxx", "STRH", null, DisassembleLoadStoreHalfword16);
            SetThumb("0101011xxxxxxxxx", "LDSB", null, DisassembleLoadStoreHalfword16);
            SetThumb("0101101xxxxxxxxx", "LDRH", null, DisassembleLoadStoreHalfword16);
            SetThumb("0101111xxxxxxxxx", "LDSH", null, DisassembleLoadStoreHalfword16);

            // Load / Store With Immediate Offset Format
            // TODO: implement
            SetThumb("01100xxxxxxxxxxx", "STR", null, DisassembleLoadStoreWithImmediateOffset16);
            SetThumb("01101xxxxxxxxxxx", "LDR", null, DisassembleLoadStoreWithImmediateOffset16);
            SetThumb("01110xxxxxxxxxxx", "STRB", null, DisassembleLoadStoreWithImmediateOffset16);
            SetThumb("01111xxxxxxxxxxx", "LDRB", null, DisassembleLoadStoreWithImmediateOffset16);

            // Load / Store Halfword Format
            // TODO: implement
            SetThumb("10000xxxxxxxxxxx", "STRH");
            SetThumb("10001xxxxxxxxxxx", "LDRH");

            // SP-relative Load / Store Format
            // TODO: implement
            SetThumb("10010xxxxxxxxxxx", "STR");
            SetThumb("10011xxxxxxxxxxx", "LDR");

            // Load Address Format
            // TODO: implement
            SetThumb("1010xxxxxxxxxxxx", "ADD");

            // Add Offset To Stack Pointer Format
            // TODO: implement
            SetThumb("1011xxxxxxxxxxxx", "ADD");

            // Push / Pop Registers Format
            // TODO: implement
            SetThumb("1011010xxxxxxxxx", "PUSH");
            SetThumb("1011110xxxxxxxxx", "POP");

            // Multiple Load / Store Format
            // TODO: implement
            SetThumb("11000xxxxxxxxxxx", "LDMIA");
            SetThumb("11001xxxxxxxxxxx", "STMIA");

            // Conditional Branch Format
            // TODO: implement
            SetThumb("1101xxxxxxxxxxxx", "B");

            // Software Interrupt Format
            // TODO: implement
            SetThumb("11011111xxxxxxxx", "SMI");

            // Unconditional Branch Format
            // TODO: implement
            SetThumb("11100xxxxxxxxxxx", "B");

            // Long Branch With Link Format
            // TODO: implement
            SetThumb("1111xxxxxxxxxxxx", "BL");
        }

        private static void SetThumb(string encoding, string name, ExecuteInstruction executionHandler = null, DisassembleInstruction disassembleHandler = null) => Set(_thumbInstructions, encoding, name, executionHandler, disassembleHandler);

        private static void SetArm(string encoding, string name, ExecuteInstruction executionHandler = null, DisassembleInstruction disassembleHandler = null) => Set(_armInstructions, encoding, name, executionHandler, disassembleHandler);

        private static void Set(List<InstructionInfo> instructionsRegistry, string encoding, string name, ExecuteInstruction executionHandler, DisassembleInstruction disassembleHandler)
        {
            int bit = encoding.Length - 1;
            int value = 0;
            int xMask = 0;
            int xBits = 0;

            int[] xPos = new int[encoding.Length];

            int blacklisted = 0;

            for (int index = 0; index < encoding.Length; index++, bit--)
            {
                // NOTE: Based on Ryujinx implementation.
                // Note: < and > are used on special encodings.
                // The < means that we should never have ALL bits with the '<' set.
                // So, when the encoding has <<, it means that 00, 01, and 10 are valid,
                // but not 11. <<< is 000, 001, ..., 110 but NOT 111, and so on...
                // For >, the invalid value is zero. So, for >> 01, 10 and 11 are valid,
                // but 00 isn't.
                char chr = encoding[index];

                if (chr == '1')
                {
                    value |= 1 << bit;
                }
                else if (chr == 'x')
                {
                    xMask |= 1 << bit;
                }
                else if (chr == '>')
                {
                    xPos[xBits++] = bit;
                }
                else if (chr == '<')
                {
                    xPos[xBits++] = bit;

                    blacklisted |= 1 << bit;
                }
                else if (chr != '0')
                {
                    throw new ArgumentException(nameof(encoding));
                }
            }

            xMask = ~xMask;

            if (xBits == 0)
            {
                instructionsRegistry.Insert(0, new InstructionInfo((uint)xMask, (uint)value, name, executionHandler, disassembleHandler));

                return;
            }

            for (int index = 0; index < (1 << xBits); index++)
            {
                int mask = 0;

                for (int x = 0; x < xBits; x++)
                {
                    mask |= ((index >> x) & 1) << xPos[x];
                }

                if (mask != blacklisted)
                {
                    instructionsRegistry.Insert(0, new InstructionInfo((uint)xMask, (uint)(value | mask), name, executionHandler, disassembleHandler));
                }
            }
        }

        public static InstructionInfo GetArmInstructionInfo(uint opcode)
        {
            // TODO: fast LUT
            return GetInstructionInfo(_armInstructions, opcode);
        }

        public static InstructionInfo GetThumbInstructionInfo(ushort opcode)
        {
            // TODO: fast LUT
            return GetInstructionInfo(_thumbInstructions, opcode);
        }

        private static InstructionInfo GetInstructionInfo(List<InstructionInfo> instructionsRegistry, uint opcode)
        {
            foreach (InstructionInfo info in instructionsRegistry)
            {
                if ((opcode & info.Mask) == info.Value)
                {
                    return info;
                }
            }

            return null;
        }
    }
}
