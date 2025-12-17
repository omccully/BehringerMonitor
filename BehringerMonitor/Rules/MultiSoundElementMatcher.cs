using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BehringerMonitor.Rules
{
    public class MultiSoundElementMatcher : RuleBase
    {
        public MultiSoundElementMatcher()
        {
            IncludedRanges.Add(new SoundElementRangeMatcher());
            ExcludedRanges.Add(new SoundElementRangeMatcher());

            AddIncludedRangeCommand = new RelayCommand(AddIncludedRange);
            AddExcludedRangeCommand = new RelayCommand(AddExcludedRange);
        }

        public ObservableCollection<SoundElementRangeMatcher> IncludedRanges { get; set; } = new();

        public ObservableCollection<SoundElementRangeMatcher> ExcludedRanges { get; set; } = new();

        public ICommand AddIncludedRangeCommand { get; }

        public ICommand AddExcludedRangeCommand { get; }

        public override bool HasEffect => IncludedRanges.Any(r => r.HasEffect);

        private void AddIncludedRange()
        {
            IncludedRanges.Add(new SoundElementRangeMatcher());
        }

        private void AddExcludedRange()
        {
            ExcludedRanges.Add(new SoundElementRangeMatcher());
        }

        public override RuleBase Clone()
        {
            return new MultiSoundElementMatcher()
            {
                IncludedRanges = new ObservableCollection<SoundElementRangeMatcher>(IncludedRanges.Select(ir => ir.Clone()).Cast<SoundElementRangeMatcher>()),
                ExcludedRanges = new ObservableCollection<SoundElementRangeMatcher>(ExcludedRanges.Select(ir => ir.Clone()).Cast<SoundElementRangeMatcher>()),
            };
        }
    }
}
