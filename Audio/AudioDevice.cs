using System;
using OpenTK.Audio;

namespace engenious.Audio
{
    public class AudioDevice : AudioResource
    {
        internal AudioContext context;

        public AudioDevice()
        {
            context = new AudioContext();
            context.MakeCurrent();
        }

        public override void Dispose()
        {
            context.Dispose();
            base.Dispose();
        }
    }
}

