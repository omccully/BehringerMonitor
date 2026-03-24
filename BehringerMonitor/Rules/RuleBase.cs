using BehringerMonitor.ViewModels;
using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    [JsonDerivedType(typeof(SoundElementRule), nameof(SoundElementRule))]
    [JsonDerivedType(typeof(DateTimeRangeRule), nameof(DateTimeRangeRule))]
    public abstract class RuleBase : ViewModelBase
    {
        [JsonIgnore]
        public abstract bool HasEffect { get; }

        public abstract RuleBase Clone();
    }
}
