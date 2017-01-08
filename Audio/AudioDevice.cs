using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

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

