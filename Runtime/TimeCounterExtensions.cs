using System.Threading;
using Cysharp.Threading.Tasks;

namespace TimeSpace
{
    public static class TimeCounterExtensions
    {
        public static async UniTask WaitUntil(this ITimeCounter self, float duration,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            while (self.ElapsedTime < duration)
                await UniTask.Yield(playerLoopTiming, cancellationToken);
        }

        public static async UniTask Run(this ITimeCounter self, IDeltaTimeSource deltaTimeSource,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            while (true)
            {
                self.IncrementTimer(deltaTimeSource.DeltaTimeFor(playerLoopTiming));
                await UniTask.Yield(playerLoopTiming, cancellationToken);
            }
        }
    }
}