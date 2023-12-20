using UnityEngine;

namespace TimeSpace
{
    public class DeltaTimeSource
    {
        public static IDeltaTimeSource Scaled { get; } = new ScaledDeltaTimeSource();
        public static IDeltaTimeSource Unscaled { get; } = new UnscaledDeltaTimeSource();

        private class ScaledDeltaTimeSource : IDeltaTimeSource
        {
            public float DeltaTime => Time.deltaTime;
            public float FixedDeltaTime => Time.fixedDeltaTime;
        }

        private class UnscaledDeltaTimeSource : IDeltaTimeSource
        {
            public float DeltaTime => Time.unscaledDeltaTime;
            public float FixedDeltaTime => Time.fixedUnscaledDeltaTime;
        }
    }
}