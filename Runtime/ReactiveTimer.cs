using System;

namespace TimeSpace
{
    public class ReactiveTimer : Timer<ReactiveTimeCounter>
    {
        public IScheduledEvent StopAt(float triggerTime, Action onStop = null)
        {
            return TimeCounter.TriggerAt(triggerTime, () =>
            {
                onStop?.Invoke();
                Stop();
            });
        }
    }
}