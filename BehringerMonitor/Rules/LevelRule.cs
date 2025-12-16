namespace BehringerMonitor.Rules
{
    public class LevelRule
    {
        public required LevelOperator Operator { get; set; }

        public required float Level { get; set; }
    }
}
