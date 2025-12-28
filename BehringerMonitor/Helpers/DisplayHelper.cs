namespace BehringerMonitor.Helpers
{
    public static class DisplayHelper
    {
        public static float FloatToDb(float f)
        {
            float d = 0;
            // float to dB 
            // “f” represents OSC float data. f: [0.0, 1.0] 
            // “d” represents the dB float data. d:[-oo, +10] 
            if (f >= 0.5)
                d = f * 40f - 30f;
            else if (f >= 0.25)
                d = f * 80f - 50f;
            else if (f >= 0.0625)
                d = f * 160f - 70f;
            else if (f >= 0.0)
                d = f * 480f - 90f;

            return d;
        }
    }
}
