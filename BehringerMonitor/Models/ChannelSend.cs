using BehringerMonitor.ViewModels;

namespace BehringerMonitor.Models
{
    public class ChannelSend : ViewModelBase, ISoundElement
    {
        public required int BusNumber { get; init; }

        public required int ChannelNumber { get; init; }

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

        public override string ToString()
        {
            return $"ch{ChannelNumber} -> bus{BusNumber}";
        }
    }
}
