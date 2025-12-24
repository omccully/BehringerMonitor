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
                    Rule = (EvaluatableRuleBase?)Activator.CreateInstance(value);
                }
                else
                {
                    Rule = null;
                }
            }
        }

        public EvaluatableRuleBase? Rule
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
                Rule = (EvaluatableRuleBase?)Rule?.Clone(),
            };
        }

        public override IEnumerable<string> GetViolationMessages(Soundboard soundBoard)
        {
            if (Rule != null)
            {
                return Rule.GetViolationMessages(soundBoard);
            }

            return Enumerable.Empty<string>();
        }
    }
}