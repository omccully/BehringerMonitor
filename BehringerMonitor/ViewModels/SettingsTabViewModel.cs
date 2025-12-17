using BehringerMonitor.Rules;
using BehringerMonitor.Settings;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BehringerMonitor.ViewModels
{
    public class SettingsTabViewModel
    {
        private ISettingsManager _settingsManager;

        public static IReadOnlyList<Type> BaseRuleTypes = new List<Type>()
        {
            typeof(SoundElementRule),
        };
        public event EventHandler<SettingsChangedEventArgs>? SettingsChanged;

        public BehringerMonitorSettings Settings { get; private set; }

        public ObservableCollection<RuleSelector> Rules { get; }

        public SettingsTabViewModel(ISettingsManager settingsManager)
        {
            SaveCommand = new RelayCommand(Save);
            AddRuleCommand = new RelayCommand(AddRule);
            _settingsManager = settingsManager;
            Settings = _settingsManager.ReadSettings() ?? new BehringerMonitorSettings();
            Rules = new()
            {
                new RuleSelector()
                {
                    RuleType = typeof(SoundElementRule),
                }
            };
            IpAddress = Settings.IpAddress ?? string.Empty;
            _settingsManager = settingsManager;
        }

        public ICommand SaveCommand { get; }

        public ICommand AddRuleCommand { get; }

        public string IpAddress { get; set; } = string.Empty;

        private void Save()
        {
            var newSettings = new BehringerMonitorSettings()
            {
                IpAddress = IpAddress,
                Rules = Rules.Where(r => r.HasEffect).ToList(),
            };

            _settingsManager.SaveSettings(newSettings);

            Settings = newSettings;
            SettingsChanged?.Invoke(this, new SettingsChangedEventArgs(newSettings));
        }

        private void AddRule()
        {
            Rules.Add(new RuleSelector());
        }
    }
}

