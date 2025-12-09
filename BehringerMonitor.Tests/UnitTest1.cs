using BehringerMonitor.Models;
using BehringerMonitor.Service;

namespace BehringerMonitor.Tests;


public class UnitTest1
{
    private static readonly IReadOnlyList<byte> _faderMsg = new byte[] { 47, 99, 104, 47, 48, 49, 47, 109, 105, 120, 47, 102, 97, 100, 101, 114, 0, 0, 0, 0, 44, 102, 0, 0, 62, 224, 184, 46 };
    private static readonly IReadOnlyList<byte> _ch2Off = new byte[] { 47, 99, 104, 47, 48, 50, 47, 109, 105, 120, 47, 111, 110, 0, 0, 0, 44, 105, 0, 0, 0, 0, 0, 0 };
    private static readonly IReadOnlyList<byte> _ch2On = new byte[] { 47, 99, 104, 47, 48, 50, 47, 109, 105, 120, 47, 111, 110, 0, 0, 0, 44, 105, 0, 0, 0, 0, 0, 1 };

    // on 0 47,99,104,47,48,50,47,109,105,120,47,111,110,0,0,0,44,105,0,0,0,0,0,0
    // on 1 47,99,104,47,48,50,47,109,105,120,47,111,110,0,0,0,44,105,0,0,0,0,0,1
    public static IEnumerable<object?[]> GetTestData()
    {
        for (int i = 1; i < _faderMsg.Count - 1; i++)
        {
            yield return new object?[] { i };
        }
    }

    //[Theory]
    //[InlineData(
    //    new byte[] { 47, 99, 104, 47, 48, 49, 47, 109, 105, 120, 47, 102, 97, 100, 101, 114, 0, 0, 0, 0, 44, 102, 0, 0, 62, 224, 184, 46 },
    //    "/ch/01/mix/fader",
    //    0.4389)]
    //public void ParseMessage(byte[] arr, string property, float expected)
    //{
    //    var osc = Program.ReadFaderOsc(arr);

    //    Assert.Equal(property, osc.Property);
    //    Assert.Equal(expected, osc.ValueFloat, 0.001);
    //}

    [Fact]
    public void SoundBoardUpdater_FullMessage()
    {
        byte[] arr = _faderMsg.ToArray();
        var sb = new Soundboard();
        var updater = new SoundboardStateUpdater(sb);
        updater.Update(arr);
        Channel? ch = sb.TryGetChannel(1);
        Assert.NotNull(ch);
        Assert.Equal(0.4389, ch.Fader, 0.001);
    }

    [Fact]
    public void SoundBoardUpdater_PartialMessage10()
    {
        byte[] arr = _faderMsg.ToArray();
        var sb = new Soundboard();
        var updater = new SoundboardStateUpdater(sb);
        updater.Update(_faderMsg.Take(10).ToArray());
        Channel? ch = sb.TryGetChannel(1);
        Assert.NotNull(ch);

        updater.Update(_faderMsg.Skip(10).ToArray());

        Assert.Equal(0.4389, ch.Fader, 0.001);
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void SoundBoardUpdater_PartialMessageCutoff(int cutoffPoint)
    {
        byte[] arr = _faderMsg.ToArray();
        var sb = new Soundboard();
        var updater = new SoundboardStateUpdater(sb);
        updater.Update(_faderMsg.Take(cutoffPoint).ToArray());
        Channel? ch = sb.TryGetChannel(1);
        Assert.NotNull(ch);

        updater.Update(_faderMsg.Skip(cutoffPoint).ToArray());

        Assert.Equal(0.4389, ch.Fader, 0.001);
    }

    [Fact]
    public void OnMessage()
    {
        byte[] arr = _ch2On.ToArray();
        var sb = new Soundboard();
        var updater = new SoundboardStateUpdater(sb);

        updater.Update(arr);

        Channel? ch = sb.TryGetChannel(2);
        Assert.NotNull(ch);
        Assert.False(ch.Muted);
    }

    [Fact]
    public void OffMessage()
    {
        byte[] arr = _ch2Off.ToArray();
        var sb = new Soundboard();
        var updater = new SoundboardStateUpdater(sb);

        updater.Update(arr);

        Channel? ch = sb.TryGetChannel(2);
        Assert.NotNull(ch);
        Assert.True(ch.Muted);
    }

    [Fact]
    public void CombinedMessages()
    {
        var sb = new Soundboard();
        var updater = new SoundboardStateUpdater(sb);

        updater.Update(_ch2Off.Concat(_faderMsg).ToArray());

        Channel? ch2 = sb.TryGetChannel(2);
        Assert.NotNull(ch2);
        Assert.True(ch2.Muted);

        Channel? ch1 = sb.TryGetChannel(1);
        Assert.NotNull(ch1);
        Assert.Equal(0.4389, ch1.Fader, 0.001);
    }
}