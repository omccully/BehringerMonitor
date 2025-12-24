using BehringerMonitor.Helpers;
using BehringerMonitor.Models;
using BehringerMonitor.Rules;
using System.Collections.ObjectModel;

namespace BehringerMonitor.Tests
{
    public class RuleTests
    {
        [Theory]
        [InlineData(true, true, false)]
        [InlineData(false, false, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public void Channel_MuteRule(bool muted, bool expectMuted, bool expectMessage)
        {
            var sb = CreateNeutralSoundboard();

            var baseRule = new RuleSelector()
            {
                Rule = new SoundElementRule()
                {
                    SoundElementMatcher = new MultiSoundElementMatcher()
                    {
                        IncludedRanges = new ObservableCollection<SoundElementRangeMatcher>()
                        {
                            new SoundElementRangeMatcher()
                            {
                                ChannelRange = new SoundElementRangeToggle()
                                {
                                    Range = new SoundElementRange()
                                    {
                                        Start = 2,
                                        End = 2,
                                    }
                                }
                            }
                        }
                    },
                    MuteRule = new MuteRule()
                    {
                        ExpectedMuted = expectMuted,
                    }
                }
            };

            sb.GetChannel(2).Muted = muted;

            var messages = baseRule.GetViolationMessages(sb);

            if (expectMessage)
            {
                Assert.NotEmpty(messages);
            }
            else
            {
                Assert.Empty(messages);
            }
        }

        [Theory]
        [InlineData(0.2f, LevelOperator.LessThanOrEqualTo, 0.5f, false)]
        [InlineData(0.5f, LevelOperator.LessThanOrEqualTo, 0.5f, false)]
        [InlineData(0.001f, LevelOperator.LessThanOrEqualTo, 0.0f, false)] // a little bit of tolerance
        [InlineData(0.7f, LevelOperator.LessThanOrEqualTo, 0.5f, true)]


        [InlineData(0.2f, LevelOperator.GreaterThanOrEqualTo, 0.5f, true)]
        [InlineData(0.5f, LevelOperator.GreaterThanOrEqualTo, 0.5f, false)]
        [InlineData(0.499, LevelOperator.GreaterThanOrEqualTo, 0.5f, false)] // a little bit of tolerance
        [InlineData(0.7f, LevelOperator.GreaterThanOrEqualTo, 0.5f, false)]
        public void Channel_LevelRule(float actualLevel, LevelOperator levelOperator, float expectedLevel, bool expectMessage)
        {
            var sb = CreateNeutralSoundboard();

            var baseRule = new RuleSelector()
            {
                Rule = new SoundElementRule()
                {
                    SoundElementMatcher = new MultiSoundElementMatcher()
                    {
                        IncludedRanges = new ObservableCollection<SoundElementRangeMatcher>()
                        {
                            new SoundElementRangeMatcher()
                            {
                                ChannelRange = new SoundElementRangeToggle()
                                {
                                    Range = new SoundElementRange()
                                    {
                                        Start = 2,
                                        End = 2,
                                    }
                                }
                            }
                        }
                    },
                    LevelRule = new LevelRule()
                    {
                        Level = expectedLevel,
                        Operator = levelOperator,
                    }
                }
            };

            sb.GetChannel(2).Level = actualLevel;

            var messages = baseRule.GetViolationMessages(sb);

            if (expectMessage)
            {
                Assert.NotEmpty(messages);
            }
            else
            {
                Assert.Empty(messages);
            }
        }

        private Soundboard CreateNeutralSoundboard()
        {
            var sb = new Soundboard();
            foreach (var ch in sb.Channels)
            {
                ch.OnAndNeutral();

                foreach (var send in ch.Sends)
                {
                    send.OnAndNeutral();
                }
            }

            foreach (var bus in sb.Buses)
            {
                bus.OnAndNeutral();
            }

            return sb;
        }
    }
}
