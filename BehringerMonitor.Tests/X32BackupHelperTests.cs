using BehringerMonitor.Helpers;

namespace BehringerMonitor.Tests
{
    public class X32BackupHelperTests
    {
        public static IEnumerable<object?[]> GetTestData()
        {
            yield return new object?[]
            {
                "X32_2026-01-11_11h37",
                new DateTime(2026, 01, 11, 11, 37, 00)
            };
            yield return new object?[]
            {
                "X32_2026-02-22_11h53",
                new DateTime(2026, 02, 22, 11, 53, 00)
            };

            yield return new object?[]
            {
                "Program Files",
                null
            };
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void ParseFolderDateTime(string folderName, DateTime? expectedDateTime)
        {
            DateTime? actual = X32BackupHelper.ParseFolderDateTime(folderName);

            Assert.Equal(expectedDateTime, actual);
        }
    }
}
