using BehringerMonitor.ViewModels;

namespace BehringerMonitor.Rules
{
    /// <summary>
    /// If only ChannelRange is set, then it represents a channel range
    /// If only BusRange is set, then it represents a bus range
    /// If both are set, it represents channel sends to bus.
    /// Ex: ChannelRange 2-3 and BusRange 6-7 represents the
    /// 4 total sends between those channels and buses.
    /// </summary>
    public class SoundElementRangeMatcher : ViewModelBase
    {
        public SoundElementRangeToggle ChannelRange { get; set; } = new();

        public SoundElementRangeToggle BusRange { get; set; } = new();



        //public SoundElementRange? ChannelRange
        //{
        //    get => field;
        //    set
        //    {
        //        field = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        //public SoundElementRange? BusRange
        //{
        //    get => field;
        //    set
        //    {
        //        field = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        //public bool UseChannelRange
        //{
        //    get => ChannelRange != null;
        //    set
        //    {
        //        if (value && ChannelRange == null)
        //        {
        //            ChannelRange = new SoundElementRange();
        //        }
        //        else if (!value && ChannelRange != null)
        //        {
        //            ChannelRange = null;
        //        }

        //        NotifyPropertyChanged();
        //    }
        //}
    }
}
