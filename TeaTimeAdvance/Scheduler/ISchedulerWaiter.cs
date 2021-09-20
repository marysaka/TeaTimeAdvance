using System;

namespace TeaTimeAdvance.Scheduler
{
    public interface ISchedulerWaiter
    {
        public ulong Threshold { get; }
        public Action Callback { get; }
    }
}
