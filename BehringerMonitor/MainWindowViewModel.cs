using BehringerMonitor.Models;
using BehringerMonitor.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;

namespace BehringerMonitor
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private SoundboardStateUpdater _updater;
        private UdpClient _udpClient;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Soundboard Soundboard { get; }

        public MainWindowViewModel()
        {
            Soundboard = new Soundboard();
            _updater = new SoundboardStateUpdater(Soundboard);
            _udpClient = new UdpClient("10.0.0.120", 10023);

            _ = Task.Run(() => ReadLoop());
            _ = Initialize();
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

        private async Task Initialize()
        {
            for(int ch = 1; ch <= 32; ch++)
            {
                _udpClient.Send(EncodeOscString($"/ch/{ch:D2}/mix/fader"));
                _udpClient.Send(EncodeOscString($"/ch/{ch:D2}/mix/on"));
                for (int send = 1; send <= 16; send++)
                {
                    _udpClient.Send(EncodeOscString($"/ch/{ch:D2}/mix/{send:D2}/on"));
                    _udpClient.Send(EncodeOscString($"/ch/{ch:D2}/mix/{send:D2}/level"));
                }
            }
 
        }

        private async Task ReadLoop()
        {
            while (true)
            {
                var packet = await _udpClient.ReceiveAsync();
                _updater.Update(packet.Buffer);

                StringBuilder errors = new StringBuilder();
                for(int ch = 1; ch <= 32; ch++)
                {
                    Channel? channel = Soundboard.TryGetChannel(ch);
                    if(channel == null)
                    {
                        throw new Exception("Channel invalid");
                    }

                    ChannelSend? send = channel.TryGetSend(8);
                    if(send == null)
                    {
                        throw new Exception("Channdl send invalid");
                    }

                    if (send.Muted)
                    {
                        errors.AppendLine($"ch{ch} is muted on send to bus {send.Id}");
                    }

                    if (send.Level < 0.25)
                    {
                        errors.AppendLine($"ch{ch} is a very low level to {send.Id}");
                    }
                }
                
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
