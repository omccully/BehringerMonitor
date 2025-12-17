using BehringerMonitor.ViewModels;
using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    [JsonDerivedType(typeof(SoundElementRule), nameof(SoundElementRule))]
    public abstract class RuleBase : ViewModelBase
    {
    }
}
