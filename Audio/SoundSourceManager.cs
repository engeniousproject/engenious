using System;
using OpenTK.Audio.OpenAL;
using System.Collections.Generic;
using System.Linq;

namespace engenious.Audio
{
    public class SoundSourceManager
    {
        private static SoundSourceManager instance;
        public static SoundSourceManager Instance{
            get{
                if (instance == null)
                    instance = new SoundSourceManager();
                return instance;
            }
        }



        private const int MAX_SOURCES=256;

        private int[] sources;
        private List<int> inUse;
        private List<int> available;
        private List<int> playing;
        public SoundSourceManager()
        {
            sources = AL.GenSources(MAX_SOURCES);

            inUse = new List<int>();
            playing = new List<int>();
            available = new List<int>(sources);
        }

        public int Dequeue()
        {
            int source = available.Last();
            available.RemoveAt(available.Count-1);
            inUse.Add(source);
            return source;
        }
        public void Enqueue(int source)
        {
            inUse.Remove(source);
            available.Add(source);
        }
        public void Enqueue(int[] sources)
        {
            foreach(var source in sources)
            {
                Enqueue(source);
            }
        }

        public void Update()
        {
            lock(playing){
                for (int i=playing.Count-1;i>=0;i--)
                {
                    int sid = playing[i];
                    if (AL.GetSourceState(sid) == ALSourceState.Stopped)
                    {
                        playing.RemoveAt(sid);
                        AL.Source(sid, ALSourcei.Buffer, 0);

                        inUse.Remove(sid);
                        available.Add(sid);
                    }
                }
            }
        }

        public void PlaySound(SoundEffectInstance inst)
        {
            lock (playing)
            {
                playing.Add(inst.sid);
            }
            AL.SourcePlay(inst.sid);
            inst.State = SoundState.Playing;
        }
        public void FreeSource(SoundEffectInstance inst)
        {
            lock (playing) {
                playing.Remove(inst.sid);
            }
            Enqueue(inst.sid);
            inst.sid = 0;
            inst.State = SoundState.Stopped;
        }
    }
}

