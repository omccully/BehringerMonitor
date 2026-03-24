using BehringerMonitor.Rules;
using Microsoft.Extensions.Time.Testing;

namespace BehringerMonitor.Tests
{
    public class TimeOfWeekTests
    {
        [Theory]
        [InlineData(23, DayOfWeek.Monday)]
        [InlineData(22, DayOfWeek.Sunday)]
        public void Test(int dayOfMonth, DayOfWeek dayOfWeek)
        {
            DateTimeOffset dto = new(
                new DateTime(2026, 03, dayOfMonth, 07, 10, 20),
                TimeSpan.FromHours(-5));
            var tow = TimeOfWeek.FromCurrentTime(new FakeTimeProvider(dto));

            Assert.Equal(7, tow.Time.Hour);
            Assert.Equal(10, tow.Time.Minute);
            Assert.Equal(20, tow.Time.Second);
            Assert.Equal(dayOfWeek, tow.DayOfWeek);
        }
    }
}
