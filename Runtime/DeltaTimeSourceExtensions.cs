using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TimeSpace
{
    public static class DeltaTimeSourceExtensions
    {
        public static float DeltaTimeFor(this IDeltaTimeSource self, PlayerLoopTiming playerLoopTiming)
        {
            return playerLoopTiming switch
            {
                PlayerLoopTiming.FixedUpdate => self.FixedDeltaTime,
                PlayerLoopTiming.LastFixedUpdate => self.FixedDeltaTime,
                _ => self.DeltaTime
            };
        }

        public static async UniTask Delay(this IDeltaTimeSource self, float duration,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                await UniTask.Yield(playerLoopTiming, cancellationToken);
                elapsedTime += self.DeltaTimeFor(playerLoopTiming);
            }
        }

        public static async void DelayedCall(this IDeltaTimeSource self, float duration,
            Action onComplete = null, Action onCanceled = null, Action onFinally = null,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await self.Delay(duration, playerLoopTiming, cancellationToken);
                onComplete?.Invoke();
            }
            catch (OperationCanceledException)
            {
                onCanceled?.Invoke();
            }
            finally
            {
                onFinally?.Invoke();
            }
        }

        public static async void DelayedCall(this IDeltaTimeSource self, float duration,
            Action onComplete,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            await self.Delay(duration, playerLoopTiming, cancellationToken);
            onComplete?.Invoke();
        }


        public static async IAsyncEnumerable<float> LinearAsyncEnumerable(this IDeltaTimeSource self, float duration,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                yield return elapsedTime / duration;
                await UniTask.Yield(playerLoopTiming, cancellationToken);
                elapsedTime += self.DeltaTimeFor(playerLoopTiming);
            }

            yield return 1f;
        }
    }
}