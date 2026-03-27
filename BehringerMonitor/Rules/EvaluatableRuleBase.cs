using BehringerMonitor.Models;
using BehringerMonitor.ViewModels;
using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules;

[JsonDerivedType(typeof(SoundElementRule), nameof(SoundElementRule))]
[JsonDerivedType(typeof(DateTimeRangeRule), nameof(DateTimeRangeRule))]
public abstract class EvaluatableRuleBase : RuleBase
{
    public abstract IEnumerable<SoundBoardWarning> GetViolationMessages(Soundboard soundBoard);
}
