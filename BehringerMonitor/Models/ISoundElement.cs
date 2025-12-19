namespace BehringerMonitor.Models
{
    public interface ISoundElement
    {
        bool Muted { get; set; }

        float Level { get; set; }
    }
}
