using System;
using OpenTK.Audio.OpenAL;

namespace engenious.Audio
{
    public class SoundEffectInstance : AudioResource
    {
        internal int sid;
        private bool isLooped;

        public SoundEffectInstance()
        {
            sid = AL.GenSource();


            //AL.Source(sid, ALSourcei.Buffer);
        }



        public TimeSpan Duration { get; }


        public bool IsLooped
        { 
            get { return isLooped; } 
            set
            { 
                if (value != isLooped)
                {
                    AL.Source(sid, ALSourceb.Looping, value);
                    isLooped = value;
                }
            }
        }

        public float Volume{ get; set; }

        public float Pitch{ get; set; }

        public float Pan{ get; set; }

        public SoundState State { get; protected set; }

        public void Apply3D(AudioListener[] listeners, AudioEmitter emitter)
        {
            
        }


        public virtual void Play()
        {
            State = SoundState.Playing;

            AL.SourcePlay(sid);
        }

        public void Pause()
        {
            State = SoundState.Paused;

            AL.SourcePause(sid);
        }

        public void Resume()
        {
            Play();
        }

        private bool stopped = false;

        public void Stop(bool immediate = true)
        {
            if (immediate)
            {
                State = SoundState.Stopped;

                AL.SourceStop(sid);
            }
            else
            {
                stopped = true;
            }
        }

        #region IDisposable implementation

        public override void Dispose()
        {
            AL.DeleteSource(sid);
            base.Dispose();
        }

        #endregion

        public static float SpeedOfSound { get; set; }

        public static float MasterVolume { get; set; }

        static SoundEffectInstance()
        {
            SpeedOfSound = 345.5f;
            MasterVolume = 1.0f;
        }
    }
}

