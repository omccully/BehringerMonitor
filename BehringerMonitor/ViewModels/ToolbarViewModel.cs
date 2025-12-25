using BehringerMonitor.Settings;
using System.Diagnostics;
using System.Windows.Input;

namespace BehringerMonitor.ViewModels
{
    public class ToolbarViewModel
    {

        public ToolbarViewModel()
        {
            OpenDataFolderCommand = new RelayCommand(OpenDataFolder);
            OpenGitHubCommand = new RelayCommand(OpenGitHub);
        }

        public ICommand OpenDataFolderCommand { get; }

        public ICommand OpenGitHubCommand { get; }

        private void OpenDataFolder()
        {
            string folderPath = SettingsHelper.DataFolderPath;
            Process.Start("explorer.exe", folderPath);
        }

        private void OpenGitHub()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://github.com/omccully/BehringerMonitor",
                UseShellExecute = true
            };

            Process.Start(psi);
        }

    }
}
