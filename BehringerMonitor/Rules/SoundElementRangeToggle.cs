using BehringerMonitor.ViewModels;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
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
