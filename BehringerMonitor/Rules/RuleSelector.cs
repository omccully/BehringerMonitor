namespace BehringerMonitor.Rules
{
    public class RuleSelector
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

        public RuleBase? Rule { get; set; }
    }
}