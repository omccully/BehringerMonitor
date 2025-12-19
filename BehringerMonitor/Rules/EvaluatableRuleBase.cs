using BehringerMonitor.Models;

namespace BehringerMonitor.Rules
{
    public abstract class EvaluatableRuleBase : RuleBase
    {
        public abstract IEnumerable<string> GetViolationMessages(Soundboard soundBoard);
    }
}
