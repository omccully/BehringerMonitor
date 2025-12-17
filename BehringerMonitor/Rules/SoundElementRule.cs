namespace BehringerMonitor.Rules
{
    public class SoundElementRule : RuleBase
    {
        public SoundElementRule()
        {
            SoundElementMatcher = new MultiSoundElementMatcher();
        }

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
    }
}
