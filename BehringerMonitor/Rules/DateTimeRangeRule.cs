using BehringerMonitor.Models;

namespace BehringerMonitor.Rules
{
    public class DateTimeRangeRule : EvaluatableRuleBase
    {
        public required RuleSelector Rule { get; init; }

        public required TimeOfWeekRange TimeRange { get; init; }

        public override bool HasEffect => throw new NotImplementedException();

        public override RuleBase Clone()
        {
            throw new NotImplementedException();
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
