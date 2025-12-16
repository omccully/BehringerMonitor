using System;
using System.Collections.Generic;
using System.Text;

namespace BehringerMonitor.Rules
{
    /// <summary>
    /// If only <see cref="ChannelNumber"/> is set,
    /// then it is a channel.
    /// If only <see cref="BusNumber"/> is set,
    /// then it is a bus.
    /// If both are set, then it is a send.
    /// </summary>
    public class SoundElementMatcher
    {
        public int? ChannelNumber { get; init; }
        public int? BusNumber { get; init; }
    }
}
