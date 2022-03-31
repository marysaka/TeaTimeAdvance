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
            CheckRegister(nameof(IORegisters.BG0CNT), 0x8);
            CheckRegister(nameof(IORegisters.BG1CNT), 0xA);
            CheckRegister(nameof(IORegisters.BG2CNT), 0xC);
            CheckRegister(nameof(IORegisters.BG3CNT), 0xE);
            CheckRegister(nameof(IORegisters.BG0HOFS), 0x10);
            CheckRegister(nameof(IORegisters.BG0VOFS), 0x12);
            CheckRegister(nameof(IORegisters.BG1HOFS), 0x14);
            CheckRegister(nameof(IORegisters.BG1VOFS), 0x16);
            CheckRegister(nameof(IORegisters.BG2HOFS), 0x18);
            CheckRegister(nameof(IORegisters.BG2VOFS), 0x1A);
            CheckRegister(nameof(IORegisters.BG3HOFS), 0x1C);
            CheckRegister(nameof(IORegisters.BG3VOFS), 0x1E);
            CheckRegister(nameof(IORegisters.BG2PA), 0x20);
            CheckRegister(nameof(IORegisters.BG2PB), 0x22);
            CheckRegister(nameof(IORegisters.BG2PC), 0x24);
            CheckRegister(nameof(IORegisters.BG2PD), 0x26);
            CheckRegister(nameof(IORegisters.BG2X), 0x28);
            CheckRegister(nameof(IORegisters.BG2Y), 0x2C);
            CheckRegister(nameof(IORegisters.BG3PA), 0x30);
            CheckRegister(nameof(IORegisters.BG3PB), 0x32);
            CheckRegister(nameof(IORegisters.BG3PC), 0x34);
            CheckRegister(nameof(IORegisters.BG3PD), 0x36);
            CheckRegister(nameof(IORegisters.BG3X), 0x38);
            CheckRegister(nameof(IORegisters.BG3Y), 0x3C);
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
