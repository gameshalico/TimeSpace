using System;
using UniRx;

namespace TimeSpace
{
    public class HierarchicalScaler
    {
        private readonly ReactiveProperty<float> _scaleReactiveProperty = new();
        private float _inheritScale;

        private HierarchicalScaler _parentHierarchicalScaler;
        private IDisposable _parentSubscription;

        private float _selfScale;

        public HierarchicalScaler(HierarchicalScaler parentHierarchicalScaler = null, float selfScale = 1f)
        {
            SelfScale = selfScale;
            SetParent(parentHierarchicalScaler);
        }

        public float SelfScale
        {
            get => _selfScale;
            set
            {
                _selfScale = value;
                UpdateCurrentScale();
            }
        }

        public IReadOnlyReactiveProperty<float> ScaleReactiveProperty => _scaleReactiveProperty;

        public void SetParent(HierarchicalScaler parentHierarchicalScaler)
        {
#if DEBUG
            if (IsCircularReference(parentHierarchicalScaler))
                throw new ArgumentException("Circular reference detected");
#endif
            _parentSubscription?.Dispose();
            _parentHierarchicalScaler = parentHierarchicalScaler;

            if (parentHierarchicalScaler == null)
            {
                _inheritScale = 1f;
                return;
            }

            _parentSubscription = parentHierarchicalScaler.ScaleReactiveProperty
                .Subscribe(scale =>
                {
                    _inheritScale = scale;
                    UpdateCurrentScale();
                });
        }

        public bool IsCircularReference(HierarchicalScaler parentHierarchicalScaler)
        {
            if (parentHierarchicalScaler == null)
                return false;

            var hierarchyScaler = parentHierarchicalScaler;
            do
            {
                if (hierarchyScaler == this)
                    return true;
                hierarchyScaler = hierarchyScaler?._parentHierarchicalScaler;
            } while (hierarchyScaler != null);

            return false;
        }

        private void UpdateCurrentScale()
        {
            _scaleReactiveProperty.Value = _inheritScale * SelfScale;
        }
    }
}