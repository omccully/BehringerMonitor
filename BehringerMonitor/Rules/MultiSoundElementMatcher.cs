using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BehringerMonitor.Rules
{
    public class MultiSoundElementMatcher
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

        private void AddIncludedRange()
        {
            IncludedRanges.Add(new SoundElementRangeMatcher());
        }

        private void AddExcludedRange()
        {
            ExcludedRanges.Add(new SoundElementRangeMatcher());
        }
    }
}
