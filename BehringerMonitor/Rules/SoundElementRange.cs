namespace BehringerMonitor.Rules
{
    public class SoundElementRange : RuleBase
    {
        public int? Start { get; set; }

        public int? End { get; set; }

        public override bool HasEffect => Start.HasValue;

        public override RuleBase Clone()
        {
            return new SoundElementRange()
            {
                Start = Start,
                End = End,
            };
        }
    }
}
