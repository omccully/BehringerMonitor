using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    public class TimeOfWeek : RuleBase
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly Time { get; set; }

        public static IReadOnlyList<DayOfWeek> DayOfWeekOptions = Enum.GetValues<DayOfWeek>();

        [JsonIgnore]
        public int Hour
        {
            get
            {
                return Time.Hour;
            }
            set
            {
                Time = new TimeOnly(value, Time.Minute);
                NotifyPropertyChanged();
            }
        }


        [JsonIgnore]
        public int Minute
        {
            get
            {
                return Time.Minute;
            }
            set
            {
                Time = new TimeOnly(Time.Hour, value);
                NotifyPropertyChanged();
            }
        }
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
