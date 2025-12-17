using BehringerMonitor.Rules;
using BehringerMonitor.Settings;
using BehringerMonitor.ViewModels;
using System.Text.Json;

namespace BehringerMonitor.Tests
{
    public class RuleSerializationTests
    {
        [Fact]
        public void Serialize_SoundElementRule()
        {
            var settingsManager = new TestSettingsManager();
            var vm = new SettingsTabViewModel(settingsManager);

            const string ipAddress = "10.1.2.3";
            vm.IpAddress = ipAddress;

            var singleRule = Assert.Single(vm.Rules);
            singleRule.RuleType = typeof(SoundElementRule);

            SoundElementRule ser = Assert.IsType<SoundElementRule>(singleRule.Rule);

            var includeMatcher = Assert.Single(ser.SoundElementMatcher.IncludedRanges);

            includeMatcher.ChannelRange.EnableRange = true;

            Assert.NotNull(includeMatcher.ChannelRange.Range);

            includeMatcher.ChannelRange.Range.Start = 2;
            includeMatcher.ChannelRange.Range.End = 5;

            ser.LevelRule = new LevelRule()
            {
                Level = 0.5f,
                Operator = LevelOperator.LessThanOrEqualTo,
            };

            vm.SaveCommand.Execute(null);

            var newVm = new SettingsTabViewModel(settingsManager);

            Assert.Equal(ipAddress, newVm.IpAddress);
            var firstRule = newVm.Settings.Rules.First().Rule;
            var resultSer = Assert.IsType<SoundElementRule>(firstRule);
            var resultIncludedRange = resultSer.SoundElementMatcher.IncludedRanges.First();

            Assert.Equal(2, resultIncludedRange.ChannelRange.Range!.Start);
            Assert.Equal(5, resultIncludedRange.ChannelRange.Range!.End);

            var firstRuleView = newVm.Rules.First().Rule;
            var resultSerView = Assert.IsType<SoundElementRule>(firstRuleView);
            var resultIncludedRangeView = resultSerView.SoundElementMatcher.IncludedRanges.First();
            Assert.Equal(2, resultIncludedRangeView.ChannelRange.Range!.Start);
            Assert.Equal(5, resultIncludedRangeView.ChannelRange.Range!.End);

            Assert.NotSame(newVm.Settings.Rules, newVm.Rules);
            Assert.NotSame(resultSerView, resultSer);
            Assert.NotSame(resultIncludedRangeView, resultIncludedRange);
            Assert.NotSame(resultIncludedRangeView.ChannelRange, resultIncludedRange.ChannelRange);
            Assert.NotSame(resultIncludedRangeView.ChannelRange.Range, resultIncludedRange.ChannelRange.Range);
        }

        private class TestSettingsManager : ISettingsManager
        {
            private string? _currentVal;

            public BehringerMonitorSettings? ReadSettings()
            {
                if (_currentVal != null)
                {
                    var result = JsonSerializer.Deserialize<BehringerMonitorSettings>(_currentVal);
                    return result;
                }
                return null;
            }

            public void SaveSettings(BehringerMonitorSettings settings)
            {
                string jsonText = JsonSerializer.Serialize(settings);
                _currentVal = jsonText;
            }
        }
    }
}
