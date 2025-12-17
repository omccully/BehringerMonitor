using System.IO;
using System.Text.Json;

namespace BehringerMonitor.Settings
{
    public class SettingsManager : ISettingsManager
    {
        public BehringerMonitorSettings? ReadSettings()
        {
            string? latestSettingsFile = Directory.GetFiles(SettingsHelper.SettingsFolderPath)
                .OrderDescending().FirstOrDefault();

            if (latestSettingsFile == null)
            {
                return null;
            }

            string jsonText = File.ReadAllText(latestSettingsFile);
            var result = JsonSerializer.Deserialize<BehringerMonitorSettings>(jsonText);

            if (result == null)
            {
                throw new Exception("Failed to parse JSON");
            }

            return result;
        }

        public void SaveSettings(BehringerMonitorSettings settings)
        {
            string settingsFilePath = Path.Combine(
                SettingsHelper.SettingsFolderPath,
                "Settings-" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".json");

            string jsonText = JsonSerializer.Serialize(settings, new JsonSerializerOptions()
            {
                WriteIndented = true,
            });

            File.WriteAllText(settingsFilePath, jsonText);
        }
    }
}
