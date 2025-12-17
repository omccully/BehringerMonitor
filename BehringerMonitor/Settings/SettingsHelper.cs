using System.IO;

namespace BehringerMonitor.Settings
{
    public static class SettingsHelper
    {
        private static Lazy<string> _dataFolderPath = new Lazy<string>(
            () =>
            {
                string temp = Path.GetTempPath();
                string folder = Path.Combine(temp, "BehingerMonitor");
                Directory.CreateDirectory(folder);
                return folder;
            });

        public static string DataFolderPath => _dataFolderPath.Value;

        public static string SettingsFolderPath
        {
            get
            {
                string path = Path.Combine(DataFolderPath, "Settings");
                Directory.CreateDirectory(path);
                return path;
            }
        }

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
