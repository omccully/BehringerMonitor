using BehringerMonitor.ViewModels;

namespace BehringerMonitor.Rules
{
    public class SoundElementRangeToggle : ViewModelBase
    {
        public SoundElementRange? Range
        {
            get => field;
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }

        public bool EnableRange
        {
            get => Range != null;
            set
            {
                if (value && Range == null)
                {
                    Range = new SoundElementRange();
                }
                else if (!value && Range != null)
                {
                    Range = null;
                }

                NotifyPropertyChanged();
            }
        }
    }
}
