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

        public float ElapsedTime { get; set; }

        public void Reset(float elapsedTime = 0f)
        {
            ElapsedTime = elapsedTime;
        }

        public void IncrementTimer(float deltaTime)
        {
            ElapsedTime += deltaTime;
        }
    }
}