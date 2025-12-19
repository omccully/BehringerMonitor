namespace BehringerMonitor.Models
{
    public class Bus : ISoundElement
    {
        public required Soundboard SoundBoard { get; init; }

        public required int BusNumber { get; init; }

        public bool Muted { get; set; }

        public float Level { get; set; }

        public ChannelSend GetSendFromChannel(int channelNumber)
        {
            return SoundBoard.GetChannel(channelNumber)
                .GetSend(BusNumber);
        }

        public override string ToString()
        {
            return $"Bus {BusNumber}";
        }
    }
}
