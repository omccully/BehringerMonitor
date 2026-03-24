namespace BehringerMonitor.Rules
{
    public class TimeOfWeekRange
    {
        public required TimeOfWeek StartTime { get; init; }

        public required TimeOfWeek EndTime { get; init; }

        public bool IsInRange(TimeOfWeek timeOfWeek)
        {
            return false;
        }
    }
}
