namespace BehringerMonitor.Models
{
    public class ChannelSend : ISoundElement
    {
        public required int BusNumber { get; init; }

        public required int ChannelNumber { get; init; }

        public bool Muted { get; set; }

        public float Level { get; set; }

        public override string ToString()
        {
            return $"ch{ChannelNumber} -> bus{BusNumber}";
        }
    }
}
