namespace TimeSpace
{
    public interface ITimeCounter
    {
        public float ElapsedTime { get; }
        public void Reset(float accumulatedTime = 0f);
        public void Update(float deltaTime);
    }
}