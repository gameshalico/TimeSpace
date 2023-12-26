using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TimeSpace
{
    public class Timer<TTimeCounter> : IDisposable where TTimeCounter : ITimeCounter, new()
    {
        private TTimeCounter _timeCounter;
        private CancellationTokenSource _cancellationTokenSource;

        public Timer()
        {
            _timeCounter = new TTimeCounter();
        }

        public bool IsRunning { get; private set; }

        public TTimeCounter TimeCounter => _timeCounter;

        public float ElapsedTime => _timeCounter.ElapsedTime;

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        public void Start(IDeltaTimeSource source, PlayerLoopTiming timing = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            IsRunning = true;
            _cancellationTokenSource.Token.Register(() => IsRunning = false);

            _timeCounter.Run(source, timing, _cancellationTokenSource.Token).Forget();
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}