using BehringerMonitor.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehringerMonitor.Models
{
    public class Bus
    {
        public required Soundboard SoundBoard { get; init; }

        public required int BusNumber { get; init; }

        public bool Muted { get; set; }

        public float Level { get; set; }

        public ChannelSend GetSendFromChannel(int channelNumber)
        {
            return SoundBoard.GetChannel(channelNumber)
                .GetSend(BusNumber);
        }
    }
}
