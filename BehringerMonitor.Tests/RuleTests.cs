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
                                        End = 3,
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
