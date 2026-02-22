using BehringerMonitor.Helpers;
using System.IO;
using System.Management;

namespace BehringerMonitor.ViewModels
{
    public class DriveBackupViewModel : ViewModelBase, IDisposable
    {
        private ManagementEventWatcher _insertWatcher = new ManagementEventWatcher();
        public DriveBackupViewModel()
        {
            Status = string.Empty;
            _insertWatcher.EventArrived += DeviceInsertedEvent;
            _insertWatcher.Query = new WqlEventQuery(
                    "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2"); // 2 = Config change (insert)
            _insertWatcher.Start();
        }

        public string Status
        {
            get
            {
                return field;
            }
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }

        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            PropertyData? driveNameData = e.NewEvent.Properties.OfType<PropertyData>()
                .FirstOrDefault(p => p.Name == "DriveName");

            if (driveNameData != null)
            {
                if (driveNameData.Value is string driveName)
                {
                    var driveInfo = new DriveInfo(driveName);

                    DateOnly today = DateOnly.FromDateTime(DateTime.Now);

                    bool foundFolder = false;
                    foreach (string dir in Directory.EnumerateDirectories(driveName))
                    {
                        string? dirName = Path.GetDirectoryName(dir);
                        if (dirName == null)
                        {
                            continue;
                        }

                        DateTime? folderDateTime = X32BackupHelper.ParseFolderDateTime(dirName);
                        if (!folderDateTime.HasValue)
                        {
                            continue;
                        }

                        DateOnly folderDate = DateOnly.FromDateTime(folderDateTime.Value);
                        if (folderDate == today)
                        {
                            Status = $"Found backup folder from today in connected drive {driveName}";
                            foundFolder = true;
                        }
                    }

                    if (!foundFolder)
                    {
                        Status = $"Did not find backup folder from today in connected drive {driveName}";
                    }
                }
            }
        }

        public void Dispose()
        {
            _insertWatcher.Stop();
            _insertWatcher.Dispose();
        }
    }
}
