namespace BehringerMonitor.Rules
{
    public class TimeOfWeekRange
    {
        public required TimeOfWeek StartTime { get; init; }

        public required TimeOfWeek EndTime { get; init; }

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
