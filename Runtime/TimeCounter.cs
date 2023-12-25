namespace TimeSpace
{
    public class TimeCounter : ITimeCounter
    {
        public TimeCounter(float elapsedTime = 0f)
        {
            ElapsedTime = elapsedTime;
        }

        public float ElapsedTime { get; private set; }

        public void Reset(float accumulatedTime = 0f)
        {
            ElapsedTime = accumulatedTime;
        }

        public void Update(float deltaTime)
        {
            ElapsedTime += deltaTime;
        }
    }
}