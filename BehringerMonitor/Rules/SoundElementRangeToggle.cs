using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    public class SoundElementRangeToggle : RuleBase
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

        public override bool HasEffect => Range != null && Range.HasEffect;

        public override RuleBase Clone()
        {
            return new SoundElementRangeToggle()
            {
                Range = (SoundElementRange?)Range?.Clone(),
            };
        }
    }
}
