using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace BehringerMonitor.Models;

public class Channel
{
    public Channel()
    {
        Sends = Enumerable.Range(1, 16).Select(c => new ChannelSend()
        {
            Id = c,
        }).ToList();
    }

    public IReadOnlyList<ChannelSend> Sends { get; }

    public required int ChannelNumber { get; set; }

    public float Fader
    {
        get => field; set
        {
            Console.WriteLine($"Channel {ChannelNumber} fader to {value}");
            field = value;
        }
    }

    public bool Muted { get; set; }

    public ChannelSend? TryGetSend(int sendNum)
    {
        if (sendNum > Sends.Count || sendNum <= 0)
        {
            return null;
        }

        return Sends.ElementAt(sendNum - 1);
    }

}
