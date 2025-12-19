using BehringerMonitor.Models;
using BehringerMonitor.Rules;
using System.Collections.ObjectModel;

namespace BehringerMonitor.Tests
{
    public class MultiSoundElementMatcherTests
    {
        [Fact]
        public void IncludeChannels()
        {
            var matcher = new MultiSoundElementMatcher()
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
            };

            var sb = new Soundboard();
            var expected = new List<ISoundElement>()
            {
                sb.GetChannel(2),
                sb.GetChannel(3),
            };

            Assert.Equal(expected, matcher.GetMatchingSoundElements(sb).ToList());
        }

        [Fact]
        public void OverlappingIncludes()
        {
            var matcher = new MultiSoundElementMatcher()
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
                    },
                    new SoundElementRangeMatcher()
                    {
                        ChannelRange = new SoundElementRangeToggle()
                        {
                            Range = new SoundElementRange()
                            {
                                Start = 3,
                                End = 4,
                            }
                        }
                    }
                }
            };

            var sb = new Soundboard();
            var expected = new List<ISoundElement>()
            {
                sb.GetChannel(2),
                sb.GetChannel(3),
                sb.GetChannel(4),
            };

            Assert.Equal(expected, matcher.GetMatchingSoundElements(sb).ToList());
        }

        [Fact]
        public void IncludeChannels_AndExcludeChannel()
        {
            var matcher = new MultiSoundElementMatcher()
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
                                End = 5,
                            }
                        }
                    }
                },
                ExcludedRanges = new ObservableCollection<SoundElementRangeMatcher>()
                {
                    new SoundElementRangeMatcher()
                    {
                        ChannelRange = new SoundElementRangeToggle()
                        {
                            Range = new SoundElementRange()
                            {
                                Start = 3,
                                End = 4,
                            }
                        }
                    }
                }
            };

            var sb = new Soundboard();
            var expected = new List<ISoundElement>()
            {
                sb.GetChannel(2),
                sb.GetChannel(5),
            };

            Assert.Equal(expected, matcher.GetMatchingSoundElements(sb).ToList());
        }
    }
}
