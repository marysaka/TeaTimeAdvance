using System;

namespace TeaTimeAdvance.Scheduler
{
    internal class SchedulerWaiterHandle : ISchedulerWaiter
    {
        public ulong Threshold { get; }
        public Action Callback { get; }

        public SchedulerWaiterHandle(ulong threshold, Action callback)
        {
            Threshold = threshold;
            Callback = callback;
        }
    }
}
