namespace BehringerMonitor.Rules
{
    public class MultiSoundElementMatcher
    {
        public List<SoundElementRangeMatcher> IncludedRanges { get; set; } = new();

        public List<SoundElementRangeMatcher> ExcludedRanges { get; set; } = new();
    }
}
