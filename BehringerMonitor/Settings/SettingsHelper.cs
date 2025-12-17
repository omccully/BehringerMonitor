using System.IO;

namespace BehringerMonitor.Settings
{
    public static class SettingsHelper
    {
        private static Lazy<string> _settingsFilePath = new Lazy<string>(
            () =>
            {
                string temp = Path.GetTempPath();
                return Path.Combine(temp, "BehingerMonitor", "Settings.json");
            });

        public static string SettingsFilePath => _settingsFilePath.Value;

        //public static BehringerMonitorSettings? ReadSettings()
        //{
        //    string settingsFilePath = _settingsFilePath.Value;

        //    if (!File.Exists(settingsFilePath))
        //    {
        //        return null;
        //    }

        //    string jsonText = File.ReadAllText(settingsFilePath);
        //    var result = JsonSerializer.Deserialize<BehringerMonitorSettings>(jsonText);

        //    if (result == null)
        //    {
        //        throw new Exception("Failed to parse JSON");
        //    }

        //    return result;
        //}

        //public static void SaveSettings(BehringerMonitorSettings settings)
        //{
        //    string settingsFilePath = _settingsFilePath.Value;

        //    string jsonText = JsonSerializer.Serialize(settings);

        //    File.WriteAllText(settingsFilePath, jsonText);
        //}
    }
}
