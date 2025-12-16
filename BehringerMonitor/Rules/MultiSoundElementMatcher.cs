namespace BehringerMonitor.Rules
{
    public class MultiSoundElementMatcher
    {
        public MultiSoundElementMatcher()
        {
            IncludedRanges.Add(new SoundElementRangeMatcher());
            ExcludedRanges.Add(new SoundElementRangeMatcher());
        }

        public List<SoundElementRangeMatcher> IncludedRanges { get; set; } = new();

        public List<SoundElementRangeMatcher> ExcludedRanges { get; set; } = new();
    }
}
