using BehringerMonitor.Rules;

namespace BehringerMonitor.Tests
{
    public class SoundElementRangeTests
    {
        [Fact]
        public void Test()
        {
            var range = new SoundElementRange()
            {
                Start = 2,
                End = 3,
            };

            var expected = new List<int>()
            {
                2, 3
            };

            Assert.Equal(expected, range.EnumerateValues().ToList());
        }

        [Fact]
        public void SingleNumber()
        {
            var range = new SoundElementRange()
            {
                Start = 2,
            };

            var expected = new List<int>()
            {
                2
            };

            Assert.Equal(expected, range.EnumerateValues().ToList());
        }
    }
}
