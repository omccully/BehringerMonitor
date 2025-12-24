using BehringerMonitor.Models;
using System.Text.Json.Serialization;

namespace BehringerMonitor.Rules
{
    public class SoundElementRule : EvaluatableRuleBase
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

        public override IEnumerable<string> GetViolationMessages(Soundboard soundBoard)
        {
            var eles = SoundElementMatcher.GetMatchingSoundElements(soundBoard).ToList();
            foreach (var ele in eles)
            {
                if (MuteRule != null)
                {
                    if (MuteRule.ExpectedMuted.HasValue)
                    {
                        if (MuteRule.ExpectedMuted.Value)
                        {
                            if (!ele.Muted)
                            {
                                yield return $"Expected {ele} to be muted, but it is not.";
                            }
                        }
                        else
                        {
                            if (ele.Muted)
                            {
                                yield return $"Expected {ele} to be not muted, but it is.";
                            }
                        }
                    }

                    //foreach(soundBoard.Channels)
                }

            }

            yield break;
        }
    }
}
