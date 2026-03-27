namespace BehringerMonitor.Rules;

public class TimeOfWeekRange : RuleBase
{
    public TimeOfWeek StartTime { get; set; } = new();

    public TimeOfWeek EndTime { get; set; } = new();

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

    public override string ToString()
    {
        if (StartTime.DayOfWeek == EndTime.DayOfWeek)
        {
            return $"{StartTime.DayOfWeek} {StartTime.Time} to {EndTime.Time}";
        }

        return $"{StartTime} to {EndTime}";
    }
}
