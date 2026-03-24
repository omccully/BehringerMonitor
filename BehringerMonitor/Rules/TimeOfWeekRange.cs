namespace BehringerMonitor.Rules
{
    public class TimeOfWeekRange : RuleBase
    {
        public required TimeOfWeek StartTime { get; init; }

        public required TimeOfWeek EndTime { get; init; }

        public override bool HasEffect => true;

        public override RuleBase Clone()
        {
            return new TimeOfWeekRange()
            {
                StartTime = (TimeOfWeek)StartTime.Clone(),
                EndTime = (TimeOfWeek)EndTime.Clone(),
            };
        }

        public bool IsInRange(TimeOfWeek timeOfWeek)
        {
            if (StartTime.DayOfWeek == EndTime.DayOfWeek)
            {
                if (timeOfWeek.DayOfWeek == StartTime.DayOfWeek)
                {
                    return StartTime.Time <= timeOfWeek.Time && timeOfWeek.Time <= EndTime.Time;
                }
            }

            return false;
        }
    }
}
