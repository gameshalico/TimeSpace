namespace TimeSpace
{
    public interface ITimeCounter
    {
        public float ElapsedTime { get; set; }
        public void Reset(float elapsedTime = 0f);
        public void IncrementTimer(float deltaTime);
    }
}