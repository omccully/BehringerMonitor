using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    public class RuleSelector : RuleBase
    {
        [JsonIgnore]
        public Type? RuleType
        {
            get => field;
            set
            {
                field = value;
                if (value != null)
                {
                    Rule = (RuleBase?)Activator.CreateInstance(value);
                }
                else
                {
                    Rule = null;
                }
            }
        }

        public RuleBase? Rule
        {
            get => field;
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }

        [JsonIgnore]
        public override bool HasEffect => Rule != null;

        public override RuleBase Clone()
        {
            return new RuleSelector()
            {
                Rule = Rule?.Clone(),
            };
        }
    }
}