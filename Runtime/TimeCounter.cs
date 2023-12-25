namespace TimeSpace
{
    public class TimeCounter : ITimeCounter
    {
        public TimeCounter()
        {
            ElapsedTime = 0f;
        }

        public TimeCounter(float elapsedTime)
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