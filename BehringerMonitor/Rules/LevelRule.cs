namespace BehringerMonitor.Rules
{
    public class LevelRule
    {
        public LevelOperator? Operator { get; set; }

        public float Level { get; set; }

        public static IReadOnlyList<LevelOperator> OperatorOptions = Enum.GetValues<LevelOperator>();
    }
}
