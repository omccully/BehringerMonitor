using BehringerMonitor.Models;
using BehringerMonitor.Rules;
using BehringerMonitor.Tests.TestHelpers;
using BehringerMonitor.ViewModels;
using Microsoft.Extensions.Time.Testing;

namespace BehringerMonitor.Tests
{
    public class TimeOfWeekTests
    {
        [Theory]
        [InlineData(23, DayOfWeek.Monday)]
        [InlineData(22, DayOfWeek.Sunday)]
        public void FromCurrentTime(int dayOfMonth, DayOfWeek dayOfWeek)
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

        [Theory]
        [InlineData(DayOfWeek.Sunday, 9, 15, false)]
        [InlineData(DayOfWeek.Sunday, 10, 15, true)]
        [InlineData(DayOfWeek.Monday, 10, 15, false)]
        [InlineData(DayOfWeek.Sunday, 10, 45, false)]
        public void IsInRange(DayOfWeek dayOfWeek, int hour, int minutes, bool expected)
        {
            var start = new TimeOfWeek()
            {
                DayOfWeek = DayOfWeek.Sunday,
                Time = new TimeOnly(10, 00),
            };

            var end = new TimeOfWeek()
            {
                DayOfWeek = DayOfWeek.Sunday,
                Time = new TimeOnly(10, 30),
            };

            var range = new TimeOfWeekRange()
            {
                StartTime = start,
                EndTime = end,
            };

            var time = new TimeOfWeek()
            {
                DayOfWeek = dayOfWeek,
                Time = new TimeOnly(hour, minutes),
            };

            bool actual = range.IsInRange(time);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(DayOfWeek.Sunday, 9, 15, 0, false)]
        [InlineData(DayOfWeek.Sunday, 10, 15, 0, true)]
        [InlineData(DayOfWeek.Sunday, 10, 29, 54, true)]
        [InlineData(DayOfWeek.Sunday, 10, 29, 55, true)]
        [InlineData(DayOfWeek.Sunday, 10, 29, 56, false)]
        [InlineData(DayOfWeek.Monday, 10, 15, 0, false)]
        [InlineData(DayOfWeek.Sunday, 10, 45, 0, false)]
        public void IsInRange_Seconds(DayOfWeek dayOfWeek, int hour, int minutes, int seconds, bool expected)
        {
            var start = new TimeOfWeek()
            {
                DayOfWeek = DayOfWeek.Sunday,
            };
            start.Hour = 10;
            start.Minute = 0;
            start.Second = 0;

            var end = new TimeOfWeek()
            {
                DayOfWeek = DayOfWeek.Sunday,
            };
            end.Hour = 10;
            end.Minute = 29;
            end.Second = 55;

            var range = new TimeOfWeekRange()
            {
                StartTime = start,
                EndTime = end,
            };

            var time = new TimeOfWeek()
            {
                DayOfWeek = dayOfWeek,
                Time = new TimeOnly(hour, minutes, seconds),
            };

            bool actual = range.IsInRange(time);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(22, 9, 15, false)]
        [InlineData(22, 10, 15, true)]
        [InlineData(23, 10, 15, false)]
        [InlineData(22, 10, 45, false)]
        public void DateTimeRangeRule_GetViolationMessages(int dayOfMonth, int hour, int minute, bool expected)
        {
            var fakeRule = new FakeRule();
            fakeRule.SetHasEffect(true);
            fakeRule.ViolationMessages = new List<SoundBoardWarning>()
            {
                new SoundBoardWarning()
                {
                    Text = "Test",
                    Level = SoundBoardWarningLevel.Critical,
                }
            };

            var dtrr = new DateTimeRangeRule()
            {
                TimeRange = new TimeOfWeekRange()
                {
                    StartTime = new TimeOfWeek()
                    {
                        DayOfWeek = DayOfWeek.Sunday,
                        Time = new TimeOnly(10, 00),
                    },
                    EndTime = new TimeOfWeek()
                    {
                        DayOfWeek = DayOfWeek.Sunday,
                        Time = new TimeOnly(10, 30),
                    },
                },

                Rule = new RuleSelector()
                {
                    Rule = fakeRule,
                }
            };

            var sb = new Soundboard();

            DateTimeOffset dateTimeOffset = new(
                    new DateTime(2026, 3, dayOfMonth, hour, minute, 00));
            sb.TimeProvider = new FakeTimeProvider(dateTimeOffset);

            var results = dtrr.GetViolationMessages(sb).ToList();

            if (expected)
            {
                SoundBoardWarning warning = Assert.Single(results);
                Assert.Equal("Test", warning.Text);
                Assert.Contains("time", warning.Text, StringComparison.OrdinalIgnoreCase);
                Assert.Equal(SoundBoardWarningLevel.Warning, warning.Level);
            }
            else
            {
                Assert.Empty(results);
            }
        }

        [Fact]
        public void DateTimeRangeRule_Clone()
        {
            var fakeRule = new FakeRule();
            fakeRule.SetHasEffect(true);
            fakeRule.ViolationMessages = new List<SoundBoardWarning>()
            {
                new SoundBoardWarning()
                {
                    Text = "Test",
                    Level = SoundBoardWarningLevel.Critical,
                }
            };

            var dtrr = new DateTimeRangeRule()
            {
                TimeRange = new TimeOfWeekRange()
                {
                    StartTime = new TimeOfWeek()
                    {
                        DayOfWeek = DayOfWeek.Sunday,
                        Time = new TimeOnly(10, 00),
                    },
                    EndTime = new TimeOfWeek()
                    {
                        DayOfWeek = DayOfWeek.Sunday,
                        Time = new TimeOnly(10, 30),
                    },
                },

                Rule = new RuleSelector()
                {
                    Rule = fakeRule,
                }
            };

            var cloned = Assert.IsType<DateTimeRangeRule>(dtrr.Clone());

            Assert.Equal(dtrr.TimeRange.StartTime.DayOfWeek, cloned.TimeRange.StartTime.DayOfWeek);
            Assert.Equal(dtrr.TimeRange.EndTime.DayOfWeek, cloned.TimeRange.EndTime.DayOfWeek);

        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void DateTimeRangeRule_HasEffect(bool fakeRuleHasEffect)
        {
            var fakeRule = new FakeRule();
            fakeRule.SetHasEffect(fakeRuleHasEffect);
            fakeRule.ViolationMessages = new List<SoundBoardWarning>()
            {
                new SoundBoardWarning()
                {
                    Text ="Test",
                    Level = SoundBoardWarningLevel.Critical,
                }
            };

            var dtrr = new DateTimeRangeRule()
            {
                TimeRange = new TimeOfWeekRange()
                {
                    StartTime = new TimeOfWeek()
                    {
                        DayOfWeek = DayOfWeek.Sunday,
                        Time = new TimeOnly(10, 00),
                    },
                    EndTime = new TimeOfWeek()
                    {
                        DayOfWeek = DayOfWeek.Sunday,
                        Time = new TimeOnly(10, 30),
                    },
                },

                Rule = new RuleSelector()
                {
                    Rule = fakeRule,
                }
            };

            Assert.Equal(fakeRuleHasEffect, dtrr.HasEffect);
        }
    }
}
