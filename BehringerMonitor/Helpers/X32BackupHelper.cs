using System.Text.RegularExpressions;

namespace BehringerMonitor.Helpers
{
    public static partial class X32BackupHelper
    {
        public static DateTime? ParseFolderDateTime(string folderName)
        {
            Match m = FolderNameRegex().Match(folderName);

            if (m.Success)
            {
                return new DateTime(
                    int.Parse(m.Groups[1].Value),
                    int.Parse(m.Groups[2].Value),
                    int.Parse(m.Groups[3].Value),
                    int.Parse(m.Groups[4].Value),
                    int.Parse(m.Groups[5].Value),
                    0);
            }

            return null;
        }

        [GeneratedRegex(@"X32_(\d+)-(\d+)-(\d+)_(\d+)h(\d+)")]
        private static partial Regex FolderNameRegex();
    }
}
