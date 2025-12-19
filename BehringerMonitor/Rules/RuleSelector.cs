using BehringerMonitor.Models;
using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    public class RuleSelector : EvaluatableRuleBase
    {
        [JsonIgnore]
        public Type? RuleType
        {
            get => Rule?.GetType();
            set
            {
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

        public override IEnumerable<string> GetViolationMessages(Soundboard soundBoard)
        {
            yield break;
            //Rule?.
        }
    }
}