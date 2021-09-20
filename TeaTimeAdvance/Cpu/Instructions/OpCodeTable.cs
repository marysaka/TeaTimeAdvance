using System;
using System.Collections.Generic;
using static TeaTimeAdvance.Cpu.Instructions.InstructionInfo;

namespace TeaTimeAdvance.Cpu.Instructions
{
    public static class OpCodeTable
    {
        private static List<InstructionInfo> _armInstructions = new List<InstructionInfo>();
        private static List<InstructionInfo> _thumbInstructions = new List<InstructionInfo>();

        static OpCodeTable()
        {

        }

        private static void SetThumb(string encoding, string name, ExecuteInstruction executionHandler) => Set(encoding, name, executionHandler, _thumbInstructions);

        private static void SetArm(string encoding, string name, ExecuteInstruction executionHandler) => Set(encoding, name, executionHandler, _armInstructions);

        private static void Set(string encoding, string name, ExecuteInstruction executionHandler, List<InstructionInfo> instructionsRegistry)
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
                instructionsRegistry.Add(new InstructionInfo(xMask, value, name, executionHandler));

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
                    instructionsRegistry.Add(new InstructionInfo(xMask, value | mask, name, executionHandler));
                }
            }
        }

        public static InstructionInfo GetArmInstructionInfo(uint opcode)
        {
            return GetInstructionInfo(_armInstructions, opcode);
        }

        public static InstructionInfo GetThumbInstructionInfo(ushort opcode)
        {
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
