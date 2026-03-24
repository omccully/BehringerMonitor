using BehringerMonitor.Models;
using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    [JsonDerivedType(typeof(SoundElementRule), nameof(SoundElementRule))]
    [JsonDerivedType(typeof(DateTimeRangeRule), nameof(DateTimeRangeRule))]
    public abstract class EvaluatableRuleBase : RuleBase
    {
        public abstract IEnumerable<string> GetViolationMessages(Soundboard soundBoard);
    }
}
