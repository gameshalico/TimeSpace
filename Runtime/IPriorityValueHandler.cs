using System;

namespace TimeSpace
{
    /// <summary>
    ///     Represents a value handler with priority.
    ///     This is used by <see cref="PriorityValueArbiter{T}" />.
    ///     This handler must be disposed when it is no longer needed.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public interface IPriorityValueHandler<T> : IDisposable
    {
        public T Value { get; set; }

        public int Priority { get; set; }

        public bool IsTopPriority { get; }
    }
}