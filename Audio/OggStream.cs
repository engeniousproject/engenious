using System;
using NVorbis;

namespace engenious.Audio
{
    internal class OggStream
    {
        internal VorbisReader Reader { get; private set; }
        public OggStream(System.IO.Stream stream,bool closeStreamOnDispose=false)
        {
            Reader = new VorbisReader(stream,closeStreamOnDispose);
        }
        public OggStream(string filename)
        {
            Reader = new VorbisReader(filename);
            
        }
    }
}

