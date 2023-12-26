using System;

namespace TimeSpace
{
    public interface IScheduledEvent : IDisposable
    {
        public ReactiveTimeCounter TimeCounter { get; }
        public Action OnTrigger { get; set; }
        public float TriggerTime { get; set; }
        public bool IsTriggered { get; }
        public void Resubscribe();
    }
}