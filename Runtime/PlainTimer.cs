using Cysharp.Threading.Tasks;

namespace TimeSpace
{
    public class PlainTimer : Timer<TimeCounter>
    {
        public PlainTimer(IDeltaTimeSource source, PlayerLoopTiming timing = PlayerLoopTiming.Update) : base(source,
            timing)
        {
        }
    }
}