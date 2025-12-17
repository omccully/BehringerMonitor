using BehringerMonitor.Rules;

namespace BehringerMonitor.Settings
{
    public class BehringerMonitorSettings
    {
        public string? IpAddress { get; set; }

        public IReadOnlyList<RuleSelector> Rules { get; set; } = new List<RuleSelector>();
    }
}
