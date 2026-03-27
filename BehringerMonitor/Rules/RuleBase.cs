using BehringerMonitor.ViewModels;
using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    [JsonDerivedType(typeof(SoundElementRule), nameof(SoundElementRule))]
    [JsonDerivedType(typeof(DateTimeRangeRule), nameof(DateTimeRangeRule))]
    public abstract class RuleBase : ViewModelBase
    {
        /// <summary>
        /// Determines if the rule is worth saving to the config.
        /// </summary>
        [JsonIgnore]
        public abstract bool HasEffect { get; }

        public abstract RuleBase Clone();
    }
}
