using BehringerMonitor.ViewModels;

namespace BehringerMonitor.Models
{
    public class Bus : ViewModelBase, ISoundElement
    {
        public required Soundboard SoundBoard { get; init; }

        public required int BusNumber { get; init; }

        public bool Muted
        {
            get => field;
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }

        public float Level
        {
            get => field;
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }

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
