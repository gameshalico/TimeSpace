using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;

namespace TimeSpace
{
    /// <summary>
    ///     This class set the value with the highest priority.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class PriorityValueArbiter<T>
    {
        private readonly List<PriorityValueHandler> _priorityValues = new();
        private readonly ReactiveProperty<T> _valueReactiveProperty = new();

        private T _defaultValue;

        public PriorityValueArbiter(T defaultValue = default)
        {
            _defaultValue = defaultValue;
            _valueReactiveProperty.Value = defaultValue;
        }

        public IReadOnlyReactiveProperty<T> ValueReactiveProperty => _valueReactiveProperty;

        public T Value => _valueReactiveProperty.Value;

        public void SetDefaultValue(T defaultValue)
        {
            _defaultValue = defaultValue;
            UpdateValue();
        }

        [MustUseReturnValue]
        public IPriorityValueHandler<T> Register(int priority = 0, T value = default)
        {
            var valueHandler = new PriorityValueHandler(this, priority, value);
            InsertHandler(valueHandler);
            UpdateValue();

            return valueHandler;
        }

        private void Remove(PriorityValueHandler priorityValueHandler)
        {
            if (!_priorityValues.Contains(priorityValueHandler))
                return;

            _priorityValues.Remove(priorityValueHandler);
            UpdateValue();
        }

        private bool IsTopPriorityValueHandler(PriorityValueHandler priorityValueHandler)
        {
            return _priorityValues[0] == priorityValueHandler;
        }

        private void UpdateHandlerValue(PriorityValueHandler priorityValueHandler)
        {
            if (!IsTopPriorityValueHandler(priorityValueHandler))
                return;

            UpdateValue();
        }

        private void UpdateHandlerPriority(PriorityValueHandler priorityValueHandler)
        {
            _priorityValues.Remove(priorityValueHandler);
            InsertHandler(priorityValueHandler);
            UpdateHandlerValue(priorityValueHandler);
        }


        private void InsertHandler(PriorityValueHandler priorityValueHandler)
        {
            var index = 0;
            for (; index < _priorityValues.Count; index++)
                if (priorityValueHandler.Priority > _priorityValues[index].Priority)
                    break;

            _priorityValues.Insert(index, priorityValueHandler);
        }

        private void UpdateValue()
        {
            _valueReactiveProperty.Value = GetTopPriorityValue();
        }


        private T GetTopPriorityValue()
        {
            if (_priorityValues.Count == 0)
                return _defaultValue;

            return _priorityValues[0].Value;
        }


        private class PriorityValueHandler : IPriorityValueHandler<T>
        {
            private readonly PriorityValueArbiter<T> _owner;
            private bool _isDisposed;
            private int _priority;
            private T _value;

            internal PriorityValueHandler(PriorityValueArbiter<T> owner, int priority, T value)
            {
                _owner = owner;
                _priority = priority;
                _value = value;
            }

            public bool IsTopPriority => _owner.IsTopPriorityValueHandler(this);

            public T Value
            {
                get => _value;
                set
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(PriorityValueHandler));
                    _value = value;
                    _owner.UpdateHandlerValue(this);
                }
            }

            public int Priority
            {
                get => _priority;
                set
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(PriorityValueHandler));
                    if (_priority == value)
                        return;
                    _priority = value;
                    _owner.UpdateHandlerPriority(this);
                }
            }

            public void Dispose()
            {
                if (_isDisposed)
                    return;
                _isDisposed = true;
                _owner.Remove(this);
            }
        }
    }
}