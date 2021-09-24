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
            SetArm("<<<<00010x001111xxxx000000000000", "MRS");
            SetArm("<<<<00010x10xxxx111100000000xxxx", "MSR");
            SetArm("<<<<00x10x10xxxx1111xxxxxxxxxxxx", "MSR");

            // Halfword Data Transfer: register offset Format
            // TODO: implement
            SetArm("<<<<000xx0x1xxxxxxxx00001xx1xxxx", "LDRH");
            SetArm("<<<<000xx0x0xxxxxxxx00001xx1xxxx", "STRH");
            SetArm("<<<<000xx0x1xxxxxxxx00001x01xxxx", "LDRSB");

            // Halfword Data Transfer: immediate offset Format
            // TODO: implement
            SetArm("<<<<000xx1x1xxxxxxxxxxxx1xx1xxxx", "LDRH");
            SetArm("<<<<000xx1x0xxxxxxxxxxxx1xx1xxxx", "STRH");
            SetArm("<<<<000xx1x1xxxxxxxxxxxx1x01xxxx", "LDRSB");

            // Multiply Long Format
            // TODO: implement
            SetArm("<<<<0000100xxxxxxxxxxxxx1001xxxx", "UMULL");
            SetArm("<<<<0000101xxxxxxxxxxxxx1001xxxx", "UMLAL");
            SetArm("<<<<0000110xxxxxxxxxxxxx1001xxxx", "SMULL");
            SetArm("<<<<0000111xxxxxxxxxxxxx1001xxxx", "SMLAL");

            // Single Data Transfer Format
            // TODO: implement
            SetArm("<<<<01xxxxx0xxxxxxxxxxxxxxxxxxxx", "STR");
            SetArm("<<<<01xxxxx1xxxxxxxxxxxxxxxxxxxx", "LDR");

            // Branch and Exchange Format
            SetArm("<<<<000100101111111111110001xxxx", "BX", BranchAndExchange32, DisassembleBranchAndExchange32);

            // Multiply Format
            // TODO: implement
            SetArm("<<<<0000000xxxxxxxxxxxxx1001xxxx", "MUL");
            SetArm("<<<<0000001xxxxxxxxxxxxx1001xxxx", "MLA");

            // Block Data Transfer
            // TODO: implement
            SetArm("<<<<100xxxx0xxxxxxxxxxxxxxxxxxxx", "STM");
            SetArm("<<<<100xxxx1xxxxxxxxxxxxxxxxxxxx", "LDM");

            // Single Data Swap Format
            // TODO: implement
            SetArm("<<<<00010000xxxxxxxx00001001xxxx", "SWP");
            SetArm("<<<<00010100xxxxxxxx00001001xxxx", "SWPB");

            // Software Interrupt
            // TODO: implement
            SetArm("<<<<1111xxxxxxxxxxxxxxxxxxxxxxxx", "SWI");

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

            // TODO: Thumb
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
