using BehringerMonitor.Helpers;
using Octokit;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Windows;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace BehringerMonitor.ViewModels
{
    public class DriveBackupViewModel : ViewModelBase, IDisposable
    {
        private ManagementEventWatcher _insertWatcher = new ManagementEventWatcher();

        private SettingsTabViewModel _settingsTab;

        public DriveBackupViewModel(SettingsTabViewModel settingsTab)
        {
            Status = string.Empty;
            _insertWatcher.EventArrived += DeviceInsertedEvent;
            _insertWatcher.Query = new WqlEventQuery(
                    "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2"); // 2 = Config change (insert)
            _insertWatcher.Start();
            _settingsTab = settingsTab;
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

        public bool Uploading
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

        private async void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            PropertyData? driveNameData = e.NewEvent.Properties.OfType<PropertyData>()
                .FirstOrDefault(p => p.Name == "DriveName");

            if (driveNameData != null)
            {
                if (driveNameData.Value is string driveName)
                {
                    DateOnly today = DateOnly.FromDateTime(DateTime.Now);

                    bool foundFolder = false;
                    foreach (string dir in Directory.EnumerateDirectories(driveName))
                    {
                        string? dirName = Path.GetFileName(dir);
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
                            await Application.Current.Dispatcher.InvokeAsync(async () =>
                            {
                                Status = $"Found backup folder from today in connected drive {driveName}";
                                foundFolder = true;

                                await UploadFolder(dir);
                            });
                        }
                    }

                    if (!foundFolder)
                    {
                        await Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            Status = $"Did not find backup folder from today in connected drive {driveName}";
                        });
                    }
                }
            }
        }

        private async Task UploadFolder(string folderPath)
        {
            try
            {
                string backupFolderName = Path.GetFileName(folderPath);
                string? gitHubApiKey = _settingsTab.Settings.GitHubApiKey;

                if (!string.IsNullOrWhiteSpace(gitHubApiKey))
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Status = $"Uploading X32 config from {folderPath}";
                        Uploading = true;
                    });

                    var github = new GitHubClient(new ProductHeaderValue("BehringerMonitor"));

                    github.Credentials = new Credentials(gitHubApiKey);

                    Task<Reference> mainRefTask = github.Git.Reference.Get("omccully", "X32-Config", "refs/heads/main");

                    NewTree newTree = new();

                    List<NewNewBlob> blobs = new();

                    using var sem = new SemaphoreSlim(5);

                    foreach (string file in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories))
                    {
                        Task<BlobReference> task = Task.Run(async () =>
                        {
                            await sem.WaitAsync();
                            try
                            {
                                return await github.Git.Blob.Create("omccully", "X32-Config", new()
                                {
                                    Content = File.ReadAllText(file),
                                    Encoding = EncodingType.Utf8,
                                });
                            }
                            finally
                            {
                                sem.Release();
                            }
                        });

                        blobs.Add(new NewNewBlob()
                        {
                            BlobRef = task,
                            Path = file,
                        });
                    }

                    foreach (var newBlob in blobs)
                    {
                        string relativePath = Path.GetRelativePath(folderPath, newBlob.Path).Replace("\\", "/");

                        var blobRef = await newBlob.BlobRef;

                        newTree.Tree.Add(new NewTreeItem()
                        {
                            Mode = "100644",
                            Type = TreeType.Blob,
                            Path = relativePath,
                            Sha = blobRef.Sha,
                        });
                    }

                    TreeResponse treeRef = await github.Git.Tree.Create("omccully", "X32-Config", newTree);

                    Reference mainRef = await mainRefTask;

                    Commit commit = await github.Git.Commit.Create("omccully", "X32-Config", new NewCommit(
                        $"Backup {DateTime.Now} - {backupFolderName}", treeRef.Sha, mainRef.Object.Sha));

                    await github.Git.Reference.Update("omccully", "X32-Config", mainRef.Ref, new ReferenceUpdate(commit.Sha));

                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Status = $"X32 config upload successful from {folderPath}";
                        Uploading = false;
                    });

                    string url = $"https://github.com/omccully/X32-Config/commit/{commit.Sha}";
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else
                {
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        Status = "No GitHub API key configured. Backup cannot be uploaded.";
                    });
                }
            }
            catch (Exception ex)
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    Status = "Error during backup: " + ex.Message;
                });
                MessageBox.Show(ex.Message);
            }
        }

        class NewNewBlob
        {
            public required Task<BlobReference> BlobRef { get; init; }

            public required string Path { get; init; }
        }

        public void Dispose()
        {
            _insertWatcher.Stop();
            _insertWatcher.Dispose();
        }
    }
}
