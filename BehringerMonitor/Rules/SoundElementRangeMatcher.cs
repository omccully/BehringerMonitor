namespace BehringerMonitor.Rules
{
    /// <summary>
    /// If only ChannelRange is set, then it represents a channel range
    /// If only BusRange is set, then it represents a bus range
    /// If both are set, it represents channel sends to bus.
    /// Ex: ChannelRange 2-3 and BusRange 6-7 represents the
    /// 4 total sends between those channels and buses.
    /// </summary>
    public class SoundElementRangeMatcher
    {
        public required SoundElementRange? ChannelRange { get; init; }

        public required SoundElementRange? BusRange { get; init; }
    }
}
