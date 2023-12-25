using UnityEngine;

namespace TimeSpace
{
    public class ScaledDeltaTimeSource : IDeltaTimeSource
    {
        public float DeltaTime => Time.deltaTime;
        public float FixedDeltaTime => Time.fixedDeltaTime;
    }

    public class UnscaledDeltaTimeSource : IDeltaTimeSource
    {
        public float DeltaTime => Time.unscaledDeltaTime;
        public float FixedDeltaTime => Time.fixedUnscaledDeltaTime;
    }

    public class DeltaTimeSource
    {
        public static IDeltaTimeSource Scaled { get; } = new ScaledDeltaTimeSource();
        public static IDeltaTimeSource Unscaled { get; } = new UnscaledDeltaTimeSource();
    }
}