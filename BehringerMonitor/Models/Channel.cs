using BehringerMonitor.ViewModels;

namespace BehringerMonitor.Models;

public class Channel : ViewModelBase, ISoundElement
{
    public Channel(int channelNumber)
    {
        Sends = Enumerable.Range(1, 16).Select(c => new ChannelSend()
        {
            ChannelNumber = channelNumber,
            BusNumber = c,
        }).ToList();

        ChannelNumber = channelNumber;
    }

    public IReadOnlyList<ChannelSend> Sends { get; }

    public int ChannelNumber { get; }

    public float Fader
    {
        get => field;
        set
        {
            field = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(Level));
        }
    }

    public float Level
    {
        get
        {
            return Fader;
        }
        set
        {
            Fader = value;
        }
    }

    public bool Muted
    {
        get => field;
        set
        {
            field = value;
            NotifyPropertyChanged();
        }
    }

    public ChannelSend? TryGetSend(int sendNum)
    {
        if (sendNum > Sends.Count || sendNum <= 0)
        {
            return null;
        }

        return Sends.ElementAt(sendNum - 1);
    }

    public ChannelSend GetSend(int sendNum)
    {
        return TryGetSend(sendNum) ?? throw new Exception("Invalid send");
    }

    public override string ToString()
    {
        return $"Channel {ChannelNumber}";
    }
}
