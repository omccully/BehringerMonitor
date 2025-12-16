using System;
using System.Collections.Generic;
using System.Text;

namespace BehringerMonitor.Models
{
    public class ChannelSend : ISoundElement
    {
        public required int Id { get; init; }

        public bool Muted { get; set; }

        public float Level { get; set; }
    }
}
