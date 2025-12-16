using System;
using System.Collections.Generic;
using System.Text;

namespace BehringerMonitor.Models
{
    public class Bus
    {
        public required int BusNumber { get; init; }

        public bool Muted { get; set; }

        public float Level { get; set; }
    }
}
