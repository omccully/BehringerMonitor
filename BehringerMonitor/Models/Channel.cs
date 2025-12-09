using System;
using System.Collections.Generic;
using System.Text;

namespace BehringerMonitor.Models;

public class Channel
{
    private float fader;

    public required int ChannelNumber { get; set; }

    public float Fader
    {
        get => fader; set
        {
            Console.WriteLine($"Channel {ChannelNumber} fader to {value}");
            fader = value;
        }
    }

    public bool Muted { get; set; }

}
