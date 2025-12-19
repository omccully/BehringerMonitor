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

        public IEnumerable<int> EnumerateValues()
        {
            if (Start.HasValue)
            {
                if (End.HasValue)
                {
                    return Enumerable.Range(Start.Value, (End.Value - Start.Value) + 1);
                }

                return Enumerable.Range(Start.Value, 1);
            }

            if (End.HasValue)
            {
                throw new Exception("Range has an end value but not a start value");
            }

            return Enumerable.Empty<int>();
        }
    }
}
