using System.IO;
using System.Text.Json;

namespace BehringerMonitor.Settings
{
    public class SettingsManager : ISettingsManager
    {
        public BehringerMonitorSettings? ReadSettings()
        {
            string settingsFilePath = SettingsHelper.SettingsFilePath;

            if (!File.Exists(settingsFilePath))
            {
                return null;
            }

            string jsonText = File.ReadAllText(settingsFilePath);
            var result = JsonSerializer.Deserialize<BehringerMonitorSettings>(jsonText);

            if (result == null)
            {
                throw new Exception("Failed to parse JSON");
            }

            return result;
        }

        public void SaveSettings(BehringerMonitorSettings settings)
        {
            string settingsFilePath = SettingsHelper.SettingsFilePath;

            string jsonText = JsonSerializer.Serialize(settings);

            File.WriteAllText(settingsFilePath, jsonText);
        }
    }
}
