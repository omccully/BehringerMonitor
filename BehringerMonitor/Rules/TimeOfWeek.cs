namespace BehringerMonitor.Rules
{
    public class TimeOfWeek
    {
        public required DayOfWeek DayOfWeek { get; init; }

        public required TimeOnly Time { get; init; }

        public static TimeOfWeek FromCurrentTime(TimeProvider timeProvider)
        {
            var dateTime = timeProvider.GetLocalNow().DateTime;
            return FromDateTime(dateTime);
        }

        public static TimeOfWeek FromDateTime(DateTime dateTime)
        {
            return new TimeOfWeek()
            {
                DayOfWeek = dateTime.DayOfWeek,
                Time = TimeOnly.FromDateTime(dateTime),
            };
        }
    }
}
