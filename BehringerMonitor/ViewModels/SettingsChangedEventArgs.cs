using BehringerMonitor.Settings;

namespace BehringerMonitor.ViewModels
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public BehringerMonitorSettings NewSettings { get; }

        public SettingsChangedEventArgs(BehringerMonitorSettings newSettings)
        {
            NewSettings = newSettings;
        }
    }
}

