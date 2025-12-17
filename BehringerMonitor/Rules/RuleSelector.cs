using BehringerMonitor.ViewModels;
using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    public class RuleSelector : ViewModelBase
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
        public bool HasEffect => Rule != null;
    }
}