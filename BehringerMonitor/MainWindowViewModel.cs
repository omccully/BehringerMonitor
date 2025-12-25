using BehringerMonitor.Models;
using BehringerMonitor.Service;
using BehringerMonitor.Settings;
using BehringerMonitor.ViewModels;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BehringerMonitor
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const int BehringerPort = 10023;

        private SoundboardStateUpdater _updater;
        private UdpClient? _udpClient;

        public Soundboard Soundboard { get; }

        public SettingsTabViewModel SettingsTab { get; }

        public int ReceivedPacketCount
        {
            get => field;
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }

        public int ReceivedMessageCount
        {
            get => field;
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }

        public bool Initialized
        {
            get => field;
            set
            {
                field = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            SettingsTab = new(new SettingsManager());
            SettingsTab.SettingsChanged += SettingsTab_SettingsChanged;

            Debug = new MyCommand(this);
            Soundboard = new Soundboard();
            _updater = new SoundboardStateUpdater(Soundboard);

            Warnings = new List<SoundBoardWarning>();
            Result = string.Empty;

            InitializeUdpClientIfNot();
        }

        private void SettingsTab_SettingsChanged(object? sender, SettingsChangedEventArgs e)
        {
            // if IP address was entered and saved for the first time, it will start up
            InitializeUdpClientIfNot();
        }

        private void InitializeUdpClientIfNot()
        {
            string? ipAddress = SettingsTab.Settings.IpAddress;
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                _udpClient = new UdpClient(ipAddress, BehringerPort);

                _ = Task.Run(() => ReadLoop());
                _ = Initialize();
            }
        }

        public string Result
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

        public IReadOnlyList<SoundBoardWarning> Warnings
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

        public ICommand Debug { get; }

        class MyCommand : ICommand
        {
            private object _parent;

            public MyCommand(object parent)
            {
                _parent = parent;
            }

            public event EventHandler? CanExecuteChanged;

            public bool CanExecute(object? parameter)
            {
                return true;
            }

            public void Execute(object? parameter)
            {

            }
        }

        private async Task Initialize()
        {
            try
            {
                if (_udpClient == null)
                {
                    throw new Exception("UDP client not set");
                }

                // request the initial values for each
                for (int ch = 1; ch <= 32; ch++)
                {
                    await _udpClient.SendAsync(EncodeOscString($"/ch/{ch:D2}/mix/fader"));
                    _udpClient.Send(EncodeOscString($"/ch/{ch:D2}/mix/on"));
                    for (int send = 1; send <= 16; send++)
                    {
                        await _udpClient.SendAsync(EncodeOscString($"/ch/{ch:D2}/mix/{send:D2}/on"));
                        await _udpClient.SendAsync(EncodeOscString($"/ch/{ch:D2}/mix/{send:D2}/level"));
                    }
                }

                for (int bus = 1; bus <= 16; bus++)
                {
                    await _udpClient.SendAsync(EncodeOscString($"/bus/{bus:D2}/mix/fader"));
                    await _udpClient.SendAsync(EncodeOscString($"/bus/{bus:D2}/mix/on"));
                }

                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    Initialized = true;
                });

                while (true)
                {
                    await _udpClient.SendAsync(EncodeOscString($"/xremote"));
                    await Task.Delay(7000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while sending messages: " + Environment.NewLine + ex);
            }
        }

        private async Task ReadLoop()
        {
            try
            {
                if (_udpClient == null)
                {
                    throw new Exception("UDP client not set");
                }

                while (true)
                {
                    var packet = await _udpClient.ReceiveAsync();

                    ReceivedPacketCount++;

                    ReceivedMessageCount += _updater.Update(packet.Buffer);

                    List<SoundBoardWarning> warnings = new();
                    StringBuilder errors = new StringBuilder();

                    foreach (var rule in SettingsTab.Settings.Rules)
                    {
                        foreach (string violationMessage in rule.GetViolationMessages(Soundboard))
                        {
                            warnings.Add(new SoundBoardWarning()
                            {
                                Text = violationMessage,
                                Level = SoundBoardWarningLevel.Critical,
                            });
                            errors.AppendLine(violationMessage);
                        }
                    }

                    //for (int ch = 1; ch <= 32; ch++)
                    //{
                    //    Channel channel = Soundboard.GetChannel(ch);
                    //    ChannelSend send = channel.GetSend(8);

                    //    if (send.Muted)
                    //    {
                    //        warnings.Add(new SoundBoardWarning()
                    //        {
                    //            Text = $"ch{ch} is muted on send to bus {send.BusNumber}",
                    //            Level = SoundBoardWarningLevel.Critical,
                    //        });
                    //        errors.AppendLine($"ch{ch} is muted on send to bus {send.BusNumber}");
                    //    }

                    //    if (send.Level < 0.25)
                    //    {
                    //        warnings.Add(new SoundBoardWarning()
                    //        {
                    //            Text = $"ch{ch} is a very low level to {send.BusNumber}",
                    //            Level = SoundBoardWarningLevel.Critical,
                    //        });
                    //        errors.AppendLine($"ch{ch} is a very low level to {send.BusNumber}");
                    //    }
                    //}

                    Warnings = warnings;
                    Result = errors.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while sending messages: " + Environment.NewLine + ex);
            }
        }

        private static byte[] EncodeOscString(string str)
        {
            // OSC string - A sequence of non-null ASCII characters followed by a null,
            // followed by 0-3 additional null characters to make the
            // total number of bits a multiple of 32.
            byte[] bytes = Encoding.UTF8.GetBytes(str).Append((byte)0).ToArray();


            int mod = bytes.Length % 4;

            if (mod == 0)
            {
                return bytes;
            }

            int remaining = (4 - mod);

            return bytes.Concat(new byte[remaining]).ToArray();
        }
    }
}
