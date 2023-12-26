namespace TimeSpace
{
    public class ReactiveTimer : Timer<ReactiveTimeCounter>
    {
        public IScheduledEvent StopAt(float triggerTime)
        {
            return TimeCounter.TriggerAt(triggerTime, Stop);
        }
    }
}