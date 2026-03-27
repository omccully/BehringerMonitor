using System.IO;
using System.Windows;

namespace BehringerMonitor.Settings;

public static class SettingsHelper
{
    private static Lazy<string> _dataFolderPath = new Lazy<string>(
        () =>
        {
            // for some reason I originally saved this data to the temp folder
            // which gets periodically deleted.
            // so when the user upgrades to this version, everything from
            // there should be copied if the folder exists
            string oldFolder = Path.Combine(Path.GetTempPath(), "BehingerMonitor");

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string newFolder = Path.Combine(appDataPath, "BehringerMonitor");

            if (Directory.Exists(oldFolder) && !Directory.Exists(newFolder))
            {
                CopyDirectory(oldFolder, newFolder);
                MessageBox.Show($"Moved data from {oldFolder} to {newFolder}");
            }

            return newFolder;
        });

    /// <summary>
    /// Recursively copies all files and subdirectories from source to destination.
    /// </summary>
    /// <param name="sourceDir">The source directory path.</param>
    /// <param name="destDir">The destination directory path.</param>
    /// <param name="overwrite">Whether to overwrite existing files.</param>
    private static void CopyDirectory(string sourceDir, string destDir, bool overwrite = true)
    {
        // Validate source directory
        if (string.IsNullOrWhiteSpace(sourceDir) || !Directory.Exists(sourceDir))
        {
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
        }

        // Create destination directory if it doesn't exist
        Directory.CreateDirectory(destDir);

        // Copy all files
        foreach (string filePath in Directory.GetFiles(sourceDir))
        {
            string fileName = Path.GetFileName(filePath);
            string destFilePath = Path.Combine(destDir, fileName);

            try
            {
                File.Copy(filePath, destFilePath, overwrite);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"[ERROR] Could not copy file '{fileName}': {ex.Message}");
            }
        }

        // Recursively copy subdirectories
        foreach (string subDirPath in Directory.GetDirectories(sourceDir))
        {
            string subDirName = Path.GetFileName(subDirPath);
            string destSubDirPath = Path.Combine(destDir, subDirName);

            CopyDirectory(subDirPath, destSubDirPath, overwrite);
        }
    }

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

    public static string DataRecordFolderPath
    {
        get
        {
            string path = Path.Combine(DataFolderPath, "DataRecord");
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
