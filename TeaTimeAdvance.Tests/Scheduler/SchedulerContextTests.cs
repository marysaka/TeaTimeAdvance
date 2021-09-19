using NUnit.Framework;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Tests.Scheduler
{
    class SchedulerContextTests
    {
        [TestCase(1UL, 1UL, ExpectedResult = true)]
        [TestCase(2UL, 1UL, ExpectedResult = false)]
        [TestCase(3UL, 4UL, ExpectedResult = true)]
        [TestCase(4UL, 3UL, ExpectedResult = false)]
        public bool EsureSimpleTest(ulong thresholdCyclesCount, ulong updateCyclesCount)
        {
            SchedulerContext scheduler = new SchedulerContext();

            bool callbackCalled = false;

            void Callback()
            {
                callbackCalled = true;
            }

            scheduler.Register(thresholdCyclesCount, Callback);
            scheduler.UpdateCycles(updateCyclesCount);

            return callbackCalled;
        }

        [Test(ExpectedResult = false)]
        public bool EsureSimpleCancelTest()
        {
            SchedulerContext scheduler = new SchedulerContext();

            bool callbackCalled = false;

            void Callback()
            {
                callbackCalled = true;
            }

            ISchedulerWaiter waiter = scheduler.Register(1, Callback);
            scheduler.Cancel(waiter);
            scheduler.UpdateCycles(1);

            return callbackCalled;
        }
    }
}
