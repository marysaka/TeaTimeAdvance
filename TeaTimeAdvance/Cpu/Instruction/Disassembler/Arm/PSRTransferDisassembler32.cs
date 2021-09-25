using System;
using System.Numerics;
using TeaTimeAdvance.Cpu.Instruction.Definition;

namespace TeaTimeAdvance.Cpu.Instruction
{
    public static partial class InstructionDisassembler
    {
        private static string FormatPSRTransferPsrf(string psr, CpuPSRFields pSRFields)
        {
            string fields = string.Empty;

            if (pSRFields.HasFlag(CpuPSRFields.Control))
            {
                fields += 'C';
            }

            if (pSRFields.HasFlag(CpuPSRFields.Extension))
            {
                fields += 'X';
            }

            if (pSRFields.HasFlag(CpuPSRFields.Status))
            {
                fields += 'S';

            }

            if (pSRFields.HasFlag(CpuPSRFields.Flags))
            {
                fields += 'F';
            }

            return $"{psr}_{fields}";
        }

        private static string FormatPSRTransferOp2(PSRTransferFormat format)
        {
            if (format.IsImmediate)
            {
                uint immediateValue = BitOperations.RotateRight(format.Imm, format.Rotate);

                return FormatUnsignedImmediate(immediateValue);
            }
            else
            {
                return format.Rm.ToString();
            }
        }

        public static string DisassemblePSRTransferDisassembler32(InstructionInfo info, uint opcode)
        {
            PSRTransferFormat format = new PSRTransferFormat
            {
                Opcode = opcode
            };

            string psr = format.IsSpecialPurposeSpecialRegister ? "SPSR" : "CPSR";

            if (format.IsStore)
            {
                string psrf = FormatPSRTransferPsrf(psr, format.Fields);

                if (psrf.EndsWith("_CF"))
                {
                    psrf = psr;
                }

                return $"{info.Name}{GetConditionCodeName(opcode)} {psrf}, {FormatPSRTransferOp2(format)}";
            }
            else
            {
                return $"{info.Name}{GetConditionCodeName(opcode)} {format.Rd}, {psr}";
            }
        }
    }
}
