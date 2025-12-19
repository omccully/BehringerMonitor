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
        public void OverlappingIncludeChannels()
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
















        [Fact]
        public void IncludeBuses()
        {
            var matcher = new MultiSoundElementMatcher()
            {
                IncludedRanges = new ObservableCollection<SoundElementRangeMatcher>()
                {
                    new SoundElementRangeMatcher()
                    {
                        BusRange = new SoundElementRangeToggle()
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
                sb.GetBus(2),
                sb.GetBus(3),
            };

            Assert.Equal(expected, matcher.GetMatchingSoundElements(sb).ToList());
        }

        [Fact]
        public void OverlappingIncludeBuses()
        {
            var matcher = new MultiSoundElementMatcher()
            {
                IncludedRanges = new ObservableCollection<SoundElementRangeMatcher>()
                {
                    new SoundElementRangeMatcher()
                    {
                        BusRange = new SoundElementRangeToggle()
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
                        BusRange = new SoundElementRangeToggle()
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
                sb.GetBus(2),
                sb.GetBus(3),
                sb.GetBus(4),
            };

            Assert.Equal(expected, matcher.GetMatchingSoundElements(sb).ToList());
        }

        [Fact]
        public void IncludeBuss_AndExcludeBus()
        {
            var matcher = new MultiSoundElementMatcher()
            {
                IncludedRanges = new ObservableCollection<SoundElementRangeMatcher>()
                {
                    new SoundElementRangeMatcher()
                    {
                        BusRange = new SoundElementRangeToggle()
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
                        BusRange = new SoundElementRangeToggle()
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
                sb.GetBus(2),
                sb.GetBus(5),
            };

            Assert.Equal(expected, matcher.GetMatchingSoundElements(sb).ToList());
        }

        [Fact]
        public void Sends()
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
                                Start = 6,
                                End = 7
                            }
                        },
                        BusRange = new SoundElementRangeToggle()
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
                sb.GetChannel(6).GetSend(2),
                sb.GetChannel(6).GetSend(3),
                sb.GetChannel(7).GetSend(2),
                sb.GetChannel(7).GetSend(3),
            };

            Assert.Equal(expected, matcher.GetMatchingSoundElements(sb).ToList());
        }
    }
}
