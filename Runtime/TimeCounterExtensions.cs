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
    }
}