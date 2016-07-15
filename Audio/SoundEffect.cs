using System;

namespace engenious
{
    public class SoundEffect
    {
        public static float SpeedOfSound { get; set; }

        public static float MasterVolume { get; set; }

        static SoundEffect()
        {
            SpeedOfSound = 345.5f;
            MasterVolume = 1.0f;
        }


        public SoundEffect()
        {
        }
    }
}

