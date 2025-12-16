using BehringerMonitor.Rules;
using BehringerMonitor.Settings;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BehringerMonitor.ViewModels
{
    public class SettingsTabViewModel
    {

        public static IReadOnlyList<Type> BaseRuleTypes = new List<Type>()
        {
            typeof(SoundElementRule),
        };
        public event EventHandler<SettingsChangedEventArgs>? SettingsChanged;

        public BehringerMonitorSettings Settings { get; }

        public ObservableCollection<RuleSelector> Rules { get; }

        public SettingsTabViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            AddRuleCommand = new RelayCommand(AddRule);
            Settings = SettingsHelper.ReadSettings() ?? new BehringerMonitorSettings();
            Rules = new()
            {
                new RuleSelector()
                {
                    RuleType = typeof(SoundElementRule),
                }
            };
        }

        public ICommand SaveCommand { get; }

        public ICommand AddRuleCommand { get; }

        public string IpAddress { get; set; } = string.Empty;

        private void Save()
        {
            SettingsHelper.SaveSettings(Settings);
            SettingsChanged?.Invoke(this, new SettingsChangedEventArgs(Settings));
        }

        private void AddRule()
        {
            Rules.Add(new RuleSelector());
        }
    }
}

