using BehringerMonitor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace BehringerMonitor.Service
{
    public partial class SoundboardStateUpdater
    {
        private Soundboard _soundBoard;

        public SoundboardStateUpdater(Soundboard soundBoard)
        {
            _soundBoard = soundBoard;
        }

        List<byte> _buffer = new List<byte>();


        void TryParseMessage(List<byte> buffer)
        {
            //Debug.WriteLine(string.Join(",", buffer));
            while (true)
            {
                if (buffer.Count == 0)
                {
                    return;
                }

                if (buffer[0] == 0)
                {
                    buffer.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }

            int previousBufferLength;
            do
            {
                previousBufferLength = buffer.Count;

                if (buffer.Count == 0)
                {
                    return;
                }

                if (buffer[0] == '/')
                {
                    // beginning of a prop message

                    int i = 0;
                    for (; i < buffer.Count; i++)
                    {
                        if (buffer[i] == 0)
                        {
                            break;
                        }
                    }

                    string str = Encoding.UTF8.GetString(buffer.ToArray(), 0, i);
                    i++;


                    // based on the prop message, it needs to process the rest of the message differently.

                    void FinishedMessage()
                    {
                        buffer.RemoveRange(0, i);
                        i = 0;
                    }

                    float? ReadFloat()
                    {
                        int mod = i % 4;
                        int remaining = 4 - mod;
                        i += remaining;

                        // skip ,f~~
                        i += 4;

                        if (buffer.Count < i + 4)
                        {
                            return null;
                        }

                        float v = ParseBigEndianFloat(buffer.ToArray(), i);
                        i += 4;
                        return v;
                    }

                    bool? ReadBool()
                    {
                        int mod = i % 4;
                        int remaining = 4 - mod;
                        i += remaining;

                        // skip type tag
                        i += 4;

                        if (buffer.Count < i + 4)
                        {
                            return null;
                        }

                        byte onVal = buffer[i + 3];
                        i += 4;
                        switch (onVal)
                        {
                            case 0:
                                return false;
                            case 1:
                                return true;
                            default:
                                Console.WriteLine($"Invalid on value: {onVal}");
                                return null;
                        }
                    }

                    var channelFaderMatch = ChannelFaderRegex().Match(str);

                    if (channelFaderMatch.Success)
                    {
                        Console.WriteLine(str);

                        string debug = string.Join(",", buffer.Skip(i));
                        Console.WriteLine(debug);

                        float? parseFloat = ReadFloat();
                        if (!parseFloat.HasValue)
                        {
                            return;
                        }

                        int channelNum = int.Parse(channelFaderMatch.Groups[1].Value);
                        Channel? ch = _soundBoard.TryGetChannel(channelNum);

                        if (ch == null)
                        {
                            Console.WriteLine($"Invalid channel: {channelNum}");
                        }
                        else
                        {
                            ch.Fader = parseFloat.Value;
                        }

                        FinishedMessage();
                    }

                    var channelOnMatch = ChannelOnRegex().Match(str);

                    if (channelOnMatch.Success)
                    {
                        int channelNum = int.Parse(channelOnMatch.Groups[1].Value);

                        bool? on = ReadBool();

                        if (!on.HasValue)
                        {
                            return;
                        }

                        Channel? ch = _soundBoard.TryGetChannel(channelNum);
                        if (ch == null)
                        {
                            Console.WriteLine($"Invalid channel: {channelNum}");
                        }
                        else
                        {
                            Debug.WriteLine($"Muted {channelNum}");
                            ch.Muted = !on.Value;
                        }

                        FinishedMessage();
                    }

                    var chSendLevelMatch = SendLevelRegex().Match(str);

                    if (chSendLevelMatch.Success)
                    {
                        int channelNum = int.Parse(chSendLevelMatch.Groups[1].Value);
                        int busNum = int.Parse(chSendLevelMatch.Groups[2].Value);

                        float? parseFloat = ReadFloat();
                        if (!parseFloat.HasValue)
                        {
                            return;
                        }

                        Channel? ch = _soundBoard.TryGetChannel(channelNum);

                        if (ch == null)
                        {
                            Console.WriteLine($"Invalid channel: {channelNum}");
                        }
                        else
                        {
                            ChannelSend? send = ch.TryGetSend(busNum);
                            if(send != null)
                            {
                                Debug.WriteLine($"Setting send {channelNum} -> {send.Id}: {parseFloat.Value}");
                                send.Level = parseFloat.Value;
                            }
                            else
                            {
                                Console.WriteLine($"Invalid channel send: level={busNum}");
                            }
                        }

                        FinishedMessage();
                    }

                    var chSendOnMatch = SendOnRegex().Match(str);

                    if (chSendOnMatch.Success)
                    {
                        int channelNum = int.Parse(chSendOnMatch.Groups[1].Value);
                        int busNum = int.Parse(chSendOnMatch.Groups[2].Value);

                        bool? on = ReadBool();

                        if (!on.HasValue)
                        {
                            return;
                        }

                        Channel? ch = _soundBoard.TryGetChannel(channelNum);

                        if (ch == null)
                        {
                            Console.WriteLine($"Invalid channel: {channelNum}");
                        }
                        else
                        {
                            ChannelSend? send = ch.TryGetSend(busNum);
                            if (send != null)
                            {
                                Debug.WriteLine($"Setting send {channelNum} -> {send.Id}: on={on.Value}");
                                send.Muted = !on.Value;
                            }
                            else
                            {
                                Console.WriteLine($"Invalid channel send: {busNum}");
                            }
                        }

                        FinishedMessage();
                    }
                }
            }
            while (previousBufferLength != buffer.Count);
        }


        public void Update(byte[] packet)
        {
            _buffer.AddRange(packet);

            TryParseMessage(_buffer);
        }


        /// <summary>
        /// Parses a 32-bit big-endian IEEE 754 floating-point number from a byte array.
        /// </summary>
        /// <param name="data">Byte array containing the big-endian float.</param>
        /// <param name="startIndex">Index where the 4-byte float starts.</param>
        /// <returns>The parsed float value.</returns>
        /// <exception cref="ArgumentException">Thrown if data is invalid.</exception>
        public static float ParseBigEndianFloat(byte[] data, int startIndex = 0)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (startIndex < 0 || startIndex + 4 > data.Length)
                throw new ArgumentException("Insufficient data for a 32-bit float.");

            // Extract the 4 bytes
            byte[] floatBytes = new byte[4];
            Array.Copy(data, startIndex, floatBytes, 0, 4);

            // Reverse if system is little-endian
            if (BitConverter.IsLittleEndian)
                Array.Reverse(floatBytes);

            // Convert to float
            return BitConverter.ToSingle(floatBytes, 0);
        }

        [GeneratedRegex(@"^/ch/(\d+)/mix/fader$")]
        private static partial Regex ChannelFaderRegex();

        [GeneratedRegex(@"^/ch/(\d+)/mix/on$")]
        private static partial Regex ChannelOnRegex();

        [GeneratedRegex(@"^/ch/(\d+)/mix/(\d+)/on")]
        private static partial Regex SendOnRegex();

        [GeneratedRegex(@"^/ch/(\d+)/mix/(\d+)/level$")]
        private static partial Regex SendLevelRegex();
    }
}
