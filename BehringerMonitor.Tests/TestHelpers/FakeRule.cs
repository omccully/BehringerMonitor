using BehringerMonitor.Models;
using BehringerMonitor.Rules;
using BehringerMonitor.ViewModels;

namespace BehringerMonitor.Tests.TestHelpers;

internal class FakeRule : EvaluatableRuleBase
{
    private bool _hasEffect;

    public List<SoundBoardWarning> ViolationMessages { get; set; } = new List<SoundBoardWarning>();

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

    public override IEnumerable<SoundBoardWarning> GetViolationMessages(Soundboard soundBoard)
    {
        return ViolationMessages;
    }
}
