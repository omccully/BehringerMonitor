using BehringerMonitor.Models;
using BehringerMonitor.Service;
using BehringerMonitor.Settings;
using BehringerMonitor.ViewModels;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Windows.Input;

namespace BehringerMonitor
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private const int BehringerPort = 10023;

        private SoundboardStateUpdater _updater;
        private UdpClient? _udpClient;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Soundboard Soundboard { get; }

        public SettingsTabViewModel SettingsTab { get; }

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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Warnings)));
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
            for (int ch = 1; ch <= 32; ch++)
            {
                _udpClient.Send(EncodeOscString($"/ch/{ch:D2}/mix/fader"));
                _udpClient.Send(EncodeOscString($"/ch/{ch:D2}/mix/on"));
                for (int send = 1; send <= 16; send++)
                {
                    _udpClient.Send(EncodeOscString($"/ch/{ch:D2}/mix/{send:D2}/on"));
                    _udpClient.Send(EncodeOscString($"/ch/{ch:D2}/mix/{send:D2}/level"));
                }
            }

            while (true)
            {
                _udpClient.Send(EncodeOscString($"/xremote"));
                await Task.Delay(7000);
            }
        }

        private async Task ReadLoop()
        {
            while (true)
            {
                var packet = await _udpClient.ReceiveAsync();
                _updater.Update(packet.Buffer);

                List<SoundBoardWarning> warnings = new();
                StringBuilder errors = new StringBuilder();
                for (int ch = 1; ch <= 32; ch++)
                {
                    Channel channel = Soundboard.GetChannel(ch);
                    ChannelSend send = channel.GetSend(8);

                    if (send.Muted)
                    {
                        warnings.Add(new SoundBoardWarning()
                        {
                            Text = $"ch{ch} is muted on send to bus {send.Id}",
                            Level = SoundBoardWarningLevel.Critical,
                        });
                        errors.AppendLine($"ch{ch} is muted on send to bus {send.Id}");
                    }

                    if (send.Level < 0.25)
                    {
                        warnings.Add(new SoundBoardWarning()
                        {
                            Text = $"ch{ch} is a very low level to {send.Id}",
                            Level = SoundBoardWarningLevel.Critical,
                        });
                        errors.AppendLine($"ch{ch} is a very low level to {send.Id}");
                    }
                }

                Warnings = warnings;
                Result = errors.ToString();
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
