using BehringerMonitor.Models;

namespace BehringerMonitor.Helpers
{
    public static class SoundElementExtensions
    {
        const float _zeroDb = 0.7498f;

        public static void SetToZeroDb(this ISoundElement soundElement)
        {
            soundElement.Level = _zeroDb;
        }

        public static void OnAndNeutral(this ISoundElement soundElement)
        {
            soundElement.SetToZeroDb();
            soundElement.Muted = false;
        }
    }
}
