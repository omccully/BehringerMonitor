using BehringerMonitor.ViewModels;

namespace BehringerMonitor.Rules
{
    public class RuleSelector : ViewModelBase
    {
        public Type? RuleType
        {
            get => field;
            set
            {
                field = value;
                if (value != null)
                {
                    Rule = (RuleBase?)Activator.CreateInstance(value);
                }
                else
                {
                    Rule = null;
                }
            }
        }

        public RuleBase? Rule
        {
            get => field;
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }
    }
}