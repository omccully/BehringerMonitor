using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    public class SoundElementRule : RuleBase
    {
        public SoundElementRule()
        {
            SoundElementMatcher = new MultiSoundElementMatcher();
        }

        public override bool HasEffect => SoundElementMatcher.HasEffect && (LevelRule?.HasEffect == true || MuteRule?.HasEffect == true);

        public MultiSoundElementMatcher SoundElementMatcher { get; set; }

        public LevelRule? LevelRule
        {
            get => field;
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }

        [JsonIgnore]
        public bool LevelRuleEnabled
        {
            get => LevelRule != null;
            set
            {
                if (value && LevelRule == null)
                {
                    LevelRule = new LevelRule();
                }
                else if (!value && LevelRule != null)
                {
                    LevelRule = null;
                }

                NotifyPropertyChanged();
            }
        }

        public MuteRule? MuteRule
        {
            get => field;
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }

        [JsonIgnore]
        public bool MuteRuleEnabled
        {
            get => MuteRule != null;
            set
            {
                if (value && MuteRule == null)
                {
                    MuteRule = new MuteRule();
                }
                else if (!value && MuteRule != null)
                {
                    MuteRule = null;
                }

                NotifyPropertyChanged();
            }
        }

        public override RuleBase Clone()
        {
            return new SoundElementRule()
            {
                SoundElementMatcher = (MultiSoundElementMatcher)SoundElementMatcher.Clone(),
                LevelRule = (LevelRule?)LevelRule,
                MuteRule = (MuteRule?)MuteRule,
            };
        }
    }
}
