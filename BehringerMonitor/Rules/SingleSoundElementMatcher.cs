using BehringerMonitor.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public class SingleSoundElementMatcher
    {
        public int? ChannelNumber { get; init; }

        public int? BusNumber { get; init; }

        public ISoundElement GetElement(Soundboard sb)
        {
            if (ChannelNumber.HasValue && BusNumber.HasValue)
            {
                return sb.GetChannel(ChannelNumber.Value)
                    .GetSend(BusNumber.Value);
            }
            else if (ChannelNumber.HasValue)
            {
                return sb.GetChannel(ChannelNumber.Value);
            }
            else if (BusNumber.HasValue)
            {
                return sb.GetBus(BusNumber.Value);
            }
            else
            {
                throw new Exception("Could not find sound element. No value set.");
            }
        }
    }
}
