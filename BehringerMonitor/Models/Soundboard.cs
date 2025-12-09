using System;
using System.Collections.Generic;
using System.Text;

namespace BehringerMonitor.Models;

public class Soundboard
{
    private readonly IReadOnlyList<Channel> _channels;

    public Soundboard()
    {
        _channels = Enumerable.Range(1, 32)
            .Select(channelNum => new Channel()
            {
                ChannelNumber = channelNum
            }).ToList();
    }

    public Channel? TryGetChannel(int channelNum)
    {
        if (channelNum > _channels.Count || channelNum <= 0)
        {
            return null;
        }
        return _channels[channelNum - 1];
    }
}
