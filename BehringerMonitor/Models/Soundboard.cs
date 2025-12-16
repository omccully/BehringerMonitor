using System;
using System.Collections.Generic;
using System.Text;

namespace BehringerMonitor.Models;

public class Soundboard
{
    private readonly IReadOnlyList<Channel> _channels;
    private readonly IReadOnlyList<Bus> _buses;

    public Soundboard()
    {
        _channels = Enumerable.Range(1, 32)
            .Select(channelNum => new Channel()
            {
                ChannelNumber = channelNum
            }).ToList();

        _buses = Enumerable.Range(1, 16)
            .Select(busNum => new Bus()
            {
                BusNumber = busNum,
                SoundBoard = this,
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

    public Channel GetChannel(int channelNum)
    {
        return TryGetChannel(channelNum) ?? throw new Exception("Invalid Channel");
    }

    public Bus? TryGetBus(int busNum)
    {
        if (busNum > _buses.Count || busNum <= 0)
        {
            return null;
        }
        return _buses[busNum - 1];
    }

    public Bus GetBus(int busNum)
    {
        return TryGetBus(busNum) ?? throw new Exception("Invalid bus");
    }
}
