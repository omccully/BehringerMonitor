using BehringerMonitor.Models;

namespace BehringerMonitor.Rules
{
    public class DateTimeRangeRule : EvaluatableRuleBase
    {
        public RuleSelector Rule { get; set; } = new RuleSelector();

        public TimeOfWeekRange TimeRange { get; set; } = new TimeOfWeekRange();

        public override bool HasEffect => Rule.HasEffect;

        public override RuleBase Clone()
        {
            return new DateTimeRangeRule()
            {
                Rule = (RuleSelector)Rule.Clone(),
                TimeRange = (TimeOfWeekRange)TimeRange.Clone(),
            };
        }

        public override IEnumerable<string> GetViolationMessages(Soundboard soundBoard)
        {
            var currentTime = TimeOfWeek.FromCurrentTime(soundBoard.TimeProvider);
            if (TimeRange.IsInRange(currentTime))
            {
                return Rule.GetViolationMessages(soundBoard);
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
