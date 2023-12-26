using System;
using System.Collections.Generic;
using UniRx;

namespace TimeSpace
{
    public class ReactiveTimeCounter : ITimeCounter
    {
        private readonly ReactiveProperty<float> _elapsedTimeReactiveProperty;
        private readonly List<ScheduledEvent> _triggerEvents = new();

        public ReactiveTimeCounter()
        {
            _elapsedTimeReactiveProperty = new ReactiveProperty<float>(0);
        }

        public ReactiveTimeCounter(float elapsedTime)
        {
            _elapsedTimeReactiveProperty = new ReactiveProperty<float>(elapsedTime);
        }

        public IReadOnlyReactiveProperty<float> ElapsedTimeReactiveProperty => _elapsedTimeReactiveProperty;

        public float ElapsedTime
        {
            get => _elapsedTimeReactiveProperty.Value;
            set => _elapsedTimeReactiveProperty.Value = value;
        }

        public void Reset(float elapsedTime = 0)
        {
            _elapsedTimeReactiveProperty.Value = elapsedTime;

            foreach (var triggerEvent in _triggerEvents)
                triggerEvent.Resubscribe();
        }

        public void IncrementTimer(float deltaTime)
        {
            _elapsedTimeReactiveProperty.Value += deltaTime;
        }

        public IScheduledEvent ResetAt(float limitTime, Action onReset = null)
        {
            return TriggerAt(limitTime, () =>
            {
                onReset?.Invoke();
                Reset(ElapsedTime - limitTime);
            });
        }

        public IScheduledEvent TriggerAt(float triggerTime, Action onTrigger)
        {
            var triggerEvent = new ScheduledEvent(this, triggerTime, onTrigger);
            triggerEvent.Register();
            return triggerEvent;
        }

        private IDisposable TriggerAtOnce(float triggerTime, Action onTrigger)
        {
            return _elapsedTimeReactiveProperty
                .Where(x => x >= triggerTime)
                .Take(1)
                .Subscribe(_ => { onTrigger?.Invoke(); });
        }

        private class ScheduledEvent : IScheduledEvent
        {
            private Action _onTrigger;
            private float _triggerTime;
            private IDisposable _currentSubscription;

            public ScheduledEvent(ReactiveTimeCounter reactiveTimeCounter, float triggerTime, Action onTrigger)
            {
                TimeCounter = reactiveTimeCounter;
                _triggerTime = triggerTime;
                _onTrigger = onTrigger;
            }

            public ReactiveTimeCounter TimeCounter { get; }
            public bool IsTriggered { get; private set; }

            public Action OnTrigger
            {
                get => _onTrigger;
                set
                {
                    _onTrigger = value;
                    if (!IsTriggered) Resubscribe();
                }
            }

            public float TriggerTime
            {
                get => _triggerTime;
                set
                {
                    _triggerTime = value;
                    if (!IsTriggered) Resubscribe();
                }
            }

            public void Dispose()
            {
                _currentSubscription?.Dispose();
                TimeCounter._triggerEvents.Remove(this);
            }

            public void Resubscribe()
            {
                IsTriggered = false;
                _currentSubscription?.Dispose();
                _currentSubscription = TimeCounter.TriggerAtOnce(_triggerTime, () =>
                {
                    IsTriggered = true;
                    _onTrigger?.Invoke();
                });
            }

            public void Register()
            {
                TimeCounter._triggerEvents.Add(this);
                Resubscribe();
            }
        }
    }
}