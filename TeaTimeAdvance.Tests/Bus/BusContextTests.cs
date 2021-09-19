using NUnit.Framework;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Device;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Tests.Bus
{
    class BusContextTests
    {
        [Test]
        public void EsureSimpleTest()
        {
            BusContext bus = new BusContext(new SchedulerContext());

            bus.RegisterDevice(0x0, new MemoryBackedDevice(new byte[0x4] { 0xCA, 0xFE, 0xBA, 0xBE }));

            Assert.AreEqual(bus.Read32(0x0), 0xBEBAFECA);

            bus.Write32(0x0, 0xCAFEBABE);

            Assert.AreEqual(bus.Read32(0x0), 0xCAFEBABE);
        }
    }
}
