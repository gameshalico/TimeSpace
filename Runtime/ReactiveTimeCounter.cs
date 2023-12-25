using System;
using System.Collections.Generic;
using UniRx;

namespace TimeSpace
{
    public class ReactiveTimeCounter : ITimeCounter
    {
        private readonly ReactiveProperty<float> _elapsedTimeReactiveProperty;
        private readonly List<TriggerEvent> _triggerEvents = new();

        public ReactiveTimeCounter(float elapsedTime = 0)
        {
            _elapsedTimeReactiveProperty = new ReactiveProperty<float>(elapsedTime);
        }

        public IReadOnlyReactiveProperty<float> ElapsedTimeReactiveProperty => _elapsedTimeReactiveProperty;

        public float ElapsedTime => _elapsedTimeReactiveProperty.Value;

        public void Reset(float accumulatedTime = 0)
        {
            _elapsedTimeReactiveProperty.Value = accumulatedTime;

            foreach (var triggerEvent in _triggerEvents)
                triggerEvent.Resubscribe();
        }

        public void Update(float deltaTime)
        {
            _elapsedTimeReactiveProperty.Value += deltaTime;
        }

        public IDisposable ResetAt(float limitTime, Action onReset = null)
        {
            return _elapsedTimeReactiveProperty
                .Where(x => x >= limitTime)
                .Subscribe(_ =>
                {
                    onReset?.Invoke();
                    Reset(_elapsedTimeReactiveProperty.Value - limitTime);
                });
        }

        public IDisposable TriggerAtOnce(float triggerTime, Action onTrigger)
        {
            return _elapsedTimeReactiveProperty
                .Where(x => x >= triggerTime)
                .Take(1)
                .Subscribe(_ =>
                {
                    onTrigger?.Invoke();
                });
        }

        public IDisposable TriggerAt(float triggerTime, Action onTrigger)
        {
            var triggerEvent = new TriggerEvent(this, triggerTime, onTrigger);
            _triggerEvents.Add(triggerEvent);
            triggerEvent.Resubscribe();
            return triggerEvent;
        }

        private class TriggerEvent : IDisposable
        {
            private readonly Action _onTrigger;
            private readonly ReactiveTimeCounter _reactiveTimeCounter;
            private readonly float _triggerTime;
            private IDisposable _currentSubscription;

            public TriggerEvent(ReactiveTimeCounter reactiveTimeCounter, float triggerTime, Action onTrigger)
            {
                _reactiveTimeCounter = reactiveTimeCounter;
                _triggerTime = triggerTime;
                _onTrigger = onTrigger;
            }

            public void Dispose()
            {
                _currentSubscription?.Dispose();
                _reactiveTimeCounter._triggerEvents.Remove(this);
            }

            public void Resubscribe()
            {
                _currentSubscription?.Dispose();
                _currentSubscription = _reactiveTimeCounter.TriggerAtOnce(_triggerTime, _onTrigger);
            }
        }
    }
}