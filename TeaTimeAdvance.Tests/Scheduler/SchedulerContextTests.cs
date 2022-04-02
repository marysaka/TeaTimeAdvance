using NUnit.Framework;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Tests.Scheduler
{
    class SchedulerContextTests
    {
        [TestCase(1, 1UL, ExpectedResult = true)]
        [TestCase(2, 1UL, ExpectedResult = false)]
        [TestCase(3, 4UL, ExpectedResult = true)]
        [TestCase(4, 3UL, ExpectedResult = false)]
        public bool EnsureSimpleTest(int thresholdCyclesCount, ulong updateCyclesCount)
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
        public bool EnsureSimpleCancelTest()
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
