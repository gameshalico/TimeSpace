using Cysharp.Threading.Tasks;

namespace TimeSpace
{
    public class ReactiveTimer : Timer<ReactiveTimeCounter>
    {
        public ReactiveTimer(IDeltaTimeSource source, PlayerLoopTiming timing = PlayerLoopTiming.Update) : base(source,
            timing)
        {
        }
    }
}