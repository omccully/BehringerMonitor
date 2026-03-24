namespace BehringerMonitor.Rules
{
    public class TimeOfWeek : RuleBase
    {
        public required DayOfWeek DayOfWeek { get; init; }

        public required TimeOnly Time { get; init; }

        public override bool HasEffect => true;

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

        public override RuleBase Clone()
        {
            return new TimeOfWeek()
            {
                DayOfWeek = DayOfWeek,
                Time = Time,
            };
        }
    }
}
