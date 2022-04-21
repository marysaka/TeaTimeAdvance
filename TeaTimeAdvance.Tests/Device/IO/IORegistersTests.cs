using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaTimeAdvance.Common;
using TeaTimeAdvance.Device.IO;

namespace TeaTimeAdvance.Tests.Device.IO
{
    internal class IORegistersTests
    {
        private static void CheckRegister(string fieldName, int expectedOffset)
        {
            int offset = UnsafeHelper.OffsetOf<IORegisters>(fieldName);

            Assert.AreEqual(expectedOffset, offset, $"{fieldName} should be at offset 0x{expectedOffset:x3} (was 0x{offset:x3})");
        }

        [Test]
        public void EnsureCorrectOffsets()
        {
            // LCD I/O Registers
            CheckRegister(nameof(IORegisters.DISPCNT), 0x0);
            CheckRegister(nameof(IORegisters.GREENSWAP), 0x2);
            CheckRegister(nameof(IORegisters.DISPSTAT), 0x4);
            CheckRegister(nameof(IORegisters.VCOUNT), 0x6);
            CheckRegister(nameof(IORegisters.BGCNT), 0x8);
            CheckRegister(nameof(IORegisters.BGOFS), 0x10);
            CheckRegister(nameof(IORegisters.BGAP), 0x20);
            CheckRegister(nameof(IORegisters.WIN0H), 0x40);
            CheckRegister(nameof(IORegisters.WIN1H), 0x42);
            CheckRegister(nameof(IORegisters.WIN0V), 0x44);
            CheckRegister(nameof(IORegisters.WIN1V), 0x46);
            CheckRegister(nameof(IORegisters.WININ), 0x48);
            CheckRegister(nameof(IORegisters.WINOUT), 0x4A);
            CheckRegister(nameof(IORegisters.MOSAIC), 0x4C);
            CheckRegister(nameof(IORegisters.BLDCNT), 0x50);
            CheckRegister(nameof(IORegisters.BLDALPHA), 0x52);
            CheckRegister(nameof(IORegisters.BLDY), 0x54);

            // TODO: Sound Registers
            // TODO: DMA Transfer Channels
            // TODO: Timer Registers
            // TODO: Serial Communication (1)
            // TODO: Keypad Input
            // TODO: Serial Communication (2)
            // TODO: Interrupt, Waitstate, and Power-Down Control
        }
    }
}
