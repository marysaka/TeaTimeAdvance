using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TeaTimeAdvance.Scheduler
{
    public class SchedulerContext
    {
        public ulong CurrentCycle { get; private set; }

        private List<ISchedulerWaiter> _waiters;

        public SchedulerContext()
        {
            CurrentCycle = 0;
            _waiters = new List<ISchedulerWaiter>();
        }

        public void Reset()
        {
            CurrentCycle = 0;
            _waiters.Clear();
        }

        public ISchedulerWaiter Register(ulong cycles, Action callback)
        {
            ISchedulerWaiter waiter = new SchedulerWaiterHandle(CurrentCycle + cycles, callback);

            int targetIndex;

            for (targetIndex = 0; targetIndex < _waiters.Count; targetIndex++)
            {
                if (_waiters[targetIndex].Threshold >= waiter.Threshold)
                {
                    break;
                }
            }

            if (targetIndex == _waiters.Count)
            {
                _waiters.Add(waiter);
            }
            else
            {
                _waiters.Insert(targetIndex, waiter);
            }

            return waiter;
        }

        public void Cancel(ISchedulerWaiter handle)
        {
            bool wasRemoved = _waiters.Remove(handle);

            Debug.Assert(wasRemoved);
        }

        public void UpdateCycles(ulong cycles)
        {
            ulong newCurrentCycle = CurrentCycle + cycles;

            ISchedulerWaiter expired = null;
            List<ISchedulerWaiter> expiredList = null;

            _waiters.RemoveAll(item =>
            {
                bool isPastThreshold = newCurrentCycle >= item.Threshold;

                if (isPastThreshold)
                {
                    if (expired == null)
                    {
                        expired = item;
                    }
                    else
                    {
                        if (expiredList == null)
                        {
                            expiredList = new List<ISchedulerWaiter>();
                        }

                        expiredList.Add(item);
                    }
                }

                return isPastThreshold;
            });

            if (expired != null)
            {
                expired.Callback();

                if (expiredList != null)
                {
                    for (int i = 0; i < expiredList.Count; i++)
                    {
                        expiredList[i].Callback();
                    }
                }
            }

            CurrentCycle = newCurrentCycle;
        }
    }
}
