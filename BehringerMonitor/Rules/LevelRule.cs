using System.Text.Json.Serialization;
using System.Windows.Input;

namespace BehringerMonitor.Rules
{
    public class LevelRule : RuleBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LevelOperator? Operator { get; set; }

        public float Level { get; set; }

        public static IReadOnlyList<LevelOperator> OperatorOptions = Enum.GetValues<LevelOperator>();

        public LevelRule()
        {
            RemoveLevelRuleCommand = new RelayCommand(RemoveLevelRule);
        }

        public ICommand RemoveLevelRuleCommand { get; set; }

        private void RemoveLevelRule()
        {

        }

        public override bool HasEffect => Operator != null;

        public override RuleBase Clone()
        {
            return new LevelRule()
            {
                Operator = Operator,
                Level = Level,
            };
        }
    }
}
