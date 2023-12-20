namespace TimeSpace
{
    public interface IDeltaTimeSource
    {
        public float DeltaTime { get; }
        public float FixedDeltaTime { get; }
    }
}