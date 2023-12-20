using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace TimeSpace
{
    public class TimeSpace : IDeltaTimeSource
    {
        private readonly HierarchicalScaler _hierarchicalScaler;
        private readonly PriorityValueArbiter<float> _timeScaleArbiter;

        public TimeSpace(TimeSpace parentTimeSpace = null)
        {
            _hierarchicalScaler = new HierarchicalScaler();
            _timeScaleArbiter = new PriorityValueArbiter<float>(1f);
            _timeScaleArbiter.ValueReactiveProperty.Subscribe(scale => _hierarchicalScaler.SelfScale = scale);

            if (parentTimeSpace != null)
                SetParent(parentTimeSpace);
        }

        public IReadOnlyReactiveProperty<float> TimeScaleReactiveProperty => _hierarchicalScaler.ScaleReactiveProperty;
        public float TimeScale => _hierarchicalScaler.ScaleReactiveProperty.Value;

        public float DeltaTime => Time.unscaledDeltaTime * TimeScale;
        public float FixedDeltaTime => Time.fixedUnscaledDeltaTime * TimeScale;

        [MustUseReturnValue]
        public IPriorityValueHandler<float> Register(int priority, float timeScale)
        {
            return _timeScaleArbiter.Register(priority, timeScale);
        }

        public void SetDefaultTimeScale(float timeScale)
        {
            _timeScaleArbiter.SetDefaultValue(timeScale);
        }

        public void SetParent(TimeSpace parent)
        {
            _hierarchicalScaler.SetParent(parent?._hierarchicalScaler);
        }
    }
}