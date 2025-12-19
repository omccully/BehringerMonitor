using BehringerMonitor.Models;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace BehringerMonitor.Rules
{
    public class MultiSoundElementMatcher : RuleBase
    {
        public MultiSoundElementMatcher()
        {
            IncludedRanges.Add(new SoundElementRangeMatcher());
            ExcludedRanges.Add(new SoundElementRangeMatcher());

            AddIncludedRangeCommand = new RelayCommand(AddIncludedRange);
            AddExcludedRangeCommand = new RelayCommand(AddExcludedRange);
        }

        public ObservableCollection<SoundElementRangeMatcher> IncludedRanges { get; set; } = new();

        public ObservableCollection<SoundElementRangeMatcher> ExcludedRanges { get; set; } = new();

        [JsonIgnore]
        public ICommand AddIncludedRangeCommand { get; }

        [JsonIgnore]
        public ICommand AddExcludedRangeCommand { get; }

        public override bool HasEffect => IncludedRanges.Any(r => r.HasEffect);

        private void AddIncludedRange()
        {
            IncludedRanges.Add(new SoundElementRangeMatcher());
        }

        private void AddExcludedRange()
        {
            ExcludedRanges.Add(new SoundElementRangeMatcher());
        }

        public IEnumerable<ISoundElement> GetMatchingSoundElements(Soundboard sb)
        {

            var includedChannels = IncludedRanges.Where(ir => ir.ChannelRange.Range != null && ir.BusRange.Range == null)
                .SelectMany(ir => ir.ChannelRange.Range!.EnumerateValues())
                .Distinct()
                .Order();

            var excludedChannels = ExcludedRanges.Where(ir => ir.ChannelRange.Range != null && ir.BusRange.Range == null)
                .SelectMany(ir => ir.ChannelRange.Range!.EnumerateValues())
                .Order();

            foreach (int ch in includedChannels.Except(excludedChannels))
            {
                yield return sb.GetChannel(ch);
            }


            var includedBuss = IncludedRanges.Where(ir => ir.BusRange.Range != null && ir.ChannelRange.Range == null)
                .SelectMany(ir => ir.BusRange.Range!.EnumerateValues())
                .Distinct()
                .Order();

            var excludedBuss = ExcludedRanges.Where(ir => ir.BusRange.Range != null && ir.ChannelRange.Range == null)
                .SelectMany(ir => ir.BusRange.Range!.EnumerateValues())
                .Order();

            foreach (int ch in includedBuss.Except(excludedBuss))
            {
                yield return sb.GetBus(ch);
            }

            var includedSendsRanges = IncludedRanges.Where(ir => ir.BusRange.Range != null && ir.ChannelRange.Range != null);
            List<ChannelSend> sends = new();

            foreach (var includedSendsRange in includedSendsRanges)
            {
                SoundElementRange includedChannelRange = includedSendsRange.ChannelRange.Range!;
                SoundElementRange includedBusRange = includedSendsRange.BusRange.Range!;

                sends.AddRange(GetSendsInRanges(sb, includedChannelRange, includedBusRange));
            }
            var excludedSendsRanges = ExcludedRanges.Where(ir => ir.BusRange.Range != null && ir.ChannelRange.Range != null);

            List<ChannelSend> excludedSends = new();
            foreach (var excludedSendsRange in excludedSendsRanges)
            {
                SoundElementRange includedChannelRange = excludedSendsRange.ChannelRange.Range!;
                SoundElementRange includedBusRange = excludedSendsRange.BusRange.Range!;

                excludedSends.AddRange(GetSendsInRanges(sb, includedChannelRange, includedBusRange));
            }

            foreach (ChannelSend send in sends.Except(excludedSends))
            {
                yield return send;
            }

            //var excludedSends = ExcludedRanges.Where(ir => ir.BusRange.Range != null && ir.ChannelRange.Range == null)
            //    .SelectMany(ir => ir.BusRange.Range!.EnumerateValues())
            //    .Order();

            //foreach (var includedRange in IncludedRanges)
            //{
            //    var range = includedRange.ChannelRange.Range;
            //    if (range != null)
            //    {
            //        foreach (int val in range.EnumerateValues())
            //        {
            //            yield return sb.GetChannel(val);
            //        }
            //    }
            //}
        }

        private IEnumerable<ChannelSend> GetSendsInRanges(Soundboard sb, SoundElementRange includedChannels, SoundElementRange includedBuses)
        {
            foreach (int chNum in includedChannels.EnumerateValues())
            {
                var channel = sb.GetChannel(chNum);

                foreach (int busNum in includedBuses.EnumerateValues())
                {
                    yield return channel.GetSend(busNum);
                }
            }
        }

        public override RuleBase Clone()
        {
            return new MultiSoundElementMatcher()
            {
                IncludedRanges = new ObservableCollection<SoundElementRangeMatcher>(IncludedRanges.Select(ir => ir.Clone()).Cast<SoundElementRangeMatcher>()),
                ExcludedRanges = new ObservableCollection<SoundElementRangeMatcher>(ExcludedRanges.Select(ir => ir.Clone()).Cast<SoundElementRangeMatcher>()),
            };
        }
    }
}
