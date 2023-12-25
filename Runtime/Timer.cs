using System.Threading;
using Cysharp.Threading.Tasks;

namespace TimeSpace
{
    public class Timer<TTimeCounter> where TTimeCounter : ITimeCounter, new()
    {
        private readonly IDeltaTimeSource _source;
        private readonly PlayerLoopTiming _timing;
        private TTimeCounter _timeCounter;
        private CancellationTokenSource _cts;

        public Timer(IDeltaTimeSource source, PlayerLoopTiming timing = PlayerLoopTiming.Update)
        {
            _timeCounter = new TTimeCounter();
            _source = source;
            _timing = timing;
        }

        public TTimeCounter TimeCounter => _timeCounter;

        public float ElapsedTime => _timeCounter.ElapsedTime;

        public void Start()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            _timeCounter.Run(_source, _timing, _cts.Token).Forget();
        }

        public void Stop()
        {
            _cts?.Cancel();
        }
    }
}