using System;
using System.Collections.Generic;
using System.Text;

namespace BehringerMonitor.ViewModels
{
    public class SoundBoardWarning
    {
        public required string Text { get; init; }
        
        public required SoundBoardWarningLevel Level { get; init; }
    }
}
