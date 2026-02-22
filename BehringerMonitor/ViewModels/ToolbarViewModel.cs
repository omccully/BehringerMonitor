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
            OpenX32ConfigRepository = new RelayCommand(OpenX32ConfigRepo);
            UpdateCommand = new RelayCommand(Update);
        }

        public ICommand OpenDataFolderCommand { get; }

        public ICommand OpenGitHubCommand { get; }
        public ICommand OpenX32ConfigRepository { get; }

        public ICommand UpdateCommand { get; }

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

        private void OpenX32ConfigRepo()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://github.com/omccully/X32-Config",
                UseShellExecute = true
            };

            Process.Start(psi);
        }

        public void Update()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://github.com/omccully/BehringerMonitor/releases",
                UseShellExecute = true
            };

            Process.Start(psi);
        }

    }
}
