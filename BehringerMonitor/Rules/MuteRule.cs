namespace BehringerMonitor.Rules
{
    public class MuteRule : RuleBase
    {
        public bool? ExpectedMuted { get; set; }

        public override bool HasEffect => ExpectedMuted.HasValue;

        public override RuleBase Clone()
        {
            return new MuteRule()
            {
                ExpectedMuted = ExpectedMuted,
            };
        }
    }
}
