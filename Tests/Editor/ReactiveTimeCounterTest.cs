using NUnit.Framework;
using TimeSpace;

namespace Tests.Editor
{
    public class ReactiveTimeCounterTests
    {
        private const float InitialTime = 0f;
        private const float DeltaTime = 0.1f;

        [Test]
        public void ShouldInitializeWithZeroElapsedTime()
        {
            var timeCounter = new ReactiveTimeCounter();
            Assert.That(timeCounter.ElapsedTime, Is.EqualTo(InitialTime));
        }

        [Test]
        public void ShouldUpdateElapsedTimeCorrectly()
        {
            var timeCounter = new ReactiveTimeCounter();
            timeCounter.Update(DeltaTime);
            Assert.That(timeCounter.ElapsedTime, Is.EqualTo(DeltaTime));
        }

        [Test]
        public void TriggerAtOnce_ShouldTriggerEventOnceAfterSpecifiedTime()
        {
            var timeCounter = new ReactiveTimeCounter();
            var triggered = false;
            timeCounter.TriggerAtOnce(1f, () => triggered = true);
            while (timeCounter.ElapsedTime < 1f)
                timeCounter.Update(DeltaTime);
            Assert.That(triggered, Is.True);
        }

        [Test]
        public void TriggerAt_ShouldTriggerEventTwiceAfterSpecifiedTime()
        {
            var timeCounter = new ReactiveTimeCounter();
            var triggered = false;
            timeCounter.TriggerAt(1f, () => triggered = true);
            while (timeCounter.ElapsedTime < 1f)
                timeCounter.Update(DeltaTime);
            Assert.That(triggered, Is.True);

            triggered = false;
            timeCounter.Reset();

            Assert.That(triggered, Is.False);
            while (timeCounter.ElapsedTime < 1f)
                timeCounter.Update(DeltaTime);
            Assert.That(triggered, Is.True);
        }

        [Test]
        public void TriggerAt_ShouldTriggerEventAfterResetBeforeSpecifiedTime()
        {
            var timeCounter = new ReactiveTimeCounter();
            var triggered = false;
            timeCounter.TriggerAt(1f, () => triggered = true);

            timeCounter.Reset();

            while (timeCounter.ElapsedTime < 1f)
                timeCounter.Update(DeltaTime);

            Assert.That(triggered, Is.True);
        }

        [Test]
        public void ResetAt_ShouldResetTimeCounterAfterSpecifiedTime()
        {
            var timeCounter = new ReactiveTimeCounter();
            var triggered = false;
            timeCounter.ResetAt(1f, () => triggered = true);
            Assert.That(triggered, Is.False);
            while (!triggered)
                timeCounter.Update(DeltaTime);
            Assert.That(triggered, Is.True);
            Assert.That(timeCounter.ElapsedTime, Is.LessThan(1f));
        }
    }
}