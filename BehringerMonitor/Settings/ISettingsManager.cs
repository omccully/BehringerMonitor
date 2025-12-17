namespace BehringerMonitor.Settings
{
    public interface ISettingsManager
    {
        BehringerMonitorSettings? ReadSettings();
        void SaveSettings(BehringerMonitorSettings settings);
    }
}
