using System;

namespace engenious.Audio
{
    public class SoundEffect : IDisposable
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

        public System.IO.Stream OpenStream()
        {
            return null;
        }
        #region IDisposable implementation
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

