using BehringerMonitor.Models;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace BehringerMonitor.Rules
{
    public class SoundElementRule : EvaluatableRuleBase
    {
        private const float _tolerance = 0.001f;

        public SoundElementRule()
        {
            SoundElementMatcher = new MultiSoundElementMatcher();
            AddRuleCommand = new RelayCommand(AddRule);
        }

        public ICommand AddRuleCommand { get; }

        public override bool HasEffect => SoundElementMatcher.HasEffect && (LevelRules.Any(lr => lr.HasEffect) || MuteRule?.HasEffect == true);

        public MultiSoundElementMatcher SoundElementMatcher { get; set; }

        public ObservableCollection<LevelRule> LevelRules { get; set; } = new ObservableCollection<LevelRule>();



        //public LevelRule? LevelRule
        //{
        //    get => field;
        //    set
        //    {
        //        field = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        //[JsonIgnore]
        //public bool LevelRuleEnabled
        //{
        //    get => LevelRule != null;
        //    set
        //    {
        //        if (value && LevelRule == null)
        //        {
        //            LevelRule = new LevelRule();
        //        }
        //        else if (!value && LevelRule != null)
        //        {
        //            LevelRule = null;
        //        }

        //        NotifyPropertyChanged();
        //    }
        //}

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

        private void AddRule()
        {
            LevelRules.Add(new LevelRule());
        }

        public override RuleBase Clone()
        {
            return new SoundElementRule()
            {
                SoundElementMatcher = (MultiSoundElementMatcher)SoundElementMatcher.Clone(),
                LevelRules = new ObservableCollection<LevelRule>(LevelRules.Select(lr => (LevelRule)lr.Clone())),
                MuteRule = (MuteRule?)MuteRule?.Clone(),
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
                }

                foreach (var levelRule in LevelRules)
                {
                    if (levelRule.Operator.HasValue)
                    {
                        switch (levelRule.Operator)
                        {
                            case LevelOperator.LessThanOrEqualTo:
                                if (ele.Level > levelRule.Level + _tolerance)
                                {
                                    yield return $"Expected {ele} to have a level less than {levelRule.Level}, but it is {ele.Level}";
                                }
                                break;

                            case LevelOperator.GreaterThanOrEqualTo:
                                if (ele.Level < levelRule.Level - _tolerance)
                                {
                                    yield return $"Expected {ele} to have a level greater than {levelRule.Level}, but it is {ele.Level}";
                                }
                                break;
                        }
                    }
                }

            }
        }
    }
}
