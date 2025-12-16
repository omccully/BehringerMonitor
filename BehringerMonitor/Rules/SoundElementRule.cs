namespace BehringerMonitor.Rules
{
    public class SoundElementRule : RuleBase
    {
        public SoundElementRule()
        {
            SoundElementMatcher = new MultiSoundElementMatcher();
        }

        public MultiSoundElementMatcher SoundElementMatcher { get; set; }

        public LevelRule? LevelRule { get; set; }

        public MuteRule? MuteRule { get; set; }
    }
}
