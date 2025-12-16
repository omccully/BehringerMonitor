using System;
using System.Collections.Generic;
using System.Text;

namespace BehringerMonitor.Models
{
    public interface ISoundElement
    {
        bool Muted { get; }

        float Level { get; }
    }
}
