using BehringerMonitor.Models;
using BehringerMonitor.Rules;

namespace BehringerMonitor.Tests.TestHelpers
{
    internal class FakeRule : EvaluatableRuleBase
    {
        private bool _hasEffect;

        public List<string> ViolationMessages { get; set; } = new List<string>();

        public void SetHasEffect(bool hasEffect)
        {
            _hasEffect = hasEffect;
        }

        public override bool HasEffect => _hasEffect;

        public override RuleBase Clone()
        {
            var fr = new FakeRule()
            {
                ViolationMessages = ViolationMessages.ToList(),
            };
            fr.SetHasEffect(HasEffect);

            return fr;
        }

        public override IEnumerable<string> GetViolationMessages(Soundboard soundBoard)
        {
            return ViolationMessages;
        }
    }
}
