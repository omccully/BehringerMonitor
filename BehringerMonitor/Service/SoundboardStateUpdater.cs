using BehringerMonitor.Models;
using System;
using System.Collections.Generic;
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


        bool TryParseMessage(List<byte> buffer, out byte[] message)
        {
            while (true)
            {
                if (buffer.Count == 0)
                {
                    message = new byte[0];
                    return false;
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

                    return ParseBigEndianFloat(buffer.ToArray(), i);
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
                        message = new byte[0];
                        return false;
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
                }

                var channelOnMatch = ChannelOnRegex().Match(str);

                if (channelOnMatch.Success)
                {
                    int channelNum = int.Parse(channelOnMatch.Groups[1].Value);

                    bool? on = ReadBool();

                    if (!on.HasValue)
                    {
                        message = new byte[0];
                        return false;
                    }

                    Channel? ch = _soundBoard.TryGetChannel(channelNum);
                    if (ch == null)
                    {
                        Console.WriteLine($"Invalid channel: {channelNum}");
                    }
                    else
                    {
                        ch.Muted = !on.Value;
                    }
                }
            }


            // Example: length-prefixed messages
            if (buffer.Count < 4)
            {
                message = null;
                return false;
            }

            int length = BitConverter.ToInt32(buffer.ToArray(), 0);
            if (buffer.Count < 4 + length)
            {
                message = null;
                return false;
            }

            message = buffer.GetRange(4, length).ToArray();
            buffer.RemoveRange(0, 4 + length);
            return true;
        }


        public void Update(byte[] packet)
        {
            _buffer.AddRange(packet);

            while (TryParseMessage(_buffer, out var message))
            {
                //ProcessMessage(message);
            }

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
