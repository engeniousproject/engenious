using System;
using System.IO;
using OpenTK.Audio.OpenAL;

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
        //TODO dynamic sound
        internal int Buffer;
        public SoundEffect(string fileName)
        {
            //TODO: dynamic sound - buffer encapsulated
            AL.GenBuffers(1, out Buffer);
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                ALFormat format;
                int size, frequency;
                var buffer = LoadWave(reader, out format, out size, out frequency);
                AL.BufferData(Buffer,format,buffer,size,frequency);
            }
        }
        private static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? OpenTK.Audio.OpenAL.ALFormat.Mono8 : OpenTK.Audio.OpenAL.ALFormat.Mono16;
                case 2: return bits == 8 ? OpenTK.Audio.OpenAL.ALFormat.Stereo8 : OpenTK.Audio.OpenAL.ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        private static byte[] LoadWave(BinaryReader reader, out ALFormat format, out int size, out int frequency)
        {
            // code based on opentk exemple

            byte[] audioData;

            //header
            string signature = new string(reader.ReadChars(4));
            if (signature != "RIFF")
            {
                throw new NotSupportedException("Specified stream is not a wave file.");
            }

            reader.ReadInt32(); // riff_chunck_size

            string wformat = new string(reader.ReadChars(4));
            if (wformat != "WAVE")
            {
                throw new NotSupportedException("Specified stream is not a wave file.");
            }

            // WAVE header
            string format_signature = new string(reader.ReadChars(4));
            while (format_signature != "fmt ")
            {
                reader.ReadBytes(reader.ReadInt32());
                format_signature = new string(reader.ReadChars(4));
            }

            int format_chunk_size = reader.ReadInt32();

            // total bytes read: tbp
            int audio_format = reader.ReadInt16(); // 2
            int num_channels = reader.ReadInt16(); // 4
            int sample_rate = reader.ReadInt32(); // 8
            reader.ReadInt32(); // 12, byte_rate
            reader.ReadInt16(); // 14, block_align
            int bits_per_sample = reader.ReadInt16(); // 16

            if (audio_format != 1)
            {
                throw new NotSupportedException("Wave compression is not supported.");
            }

            // reads residual bytes
            if (format_chunk_size > 16)
                reader.ReadBytes(format_chunk_size - 16);

            string data_signature = new string(reader.ReadChars(4));

            while (data_signature.ToLowerInvariant() != "data")
            {
                reader.ReadBytes(reader.ReadInt32());
                data_signature = new string(reader.ReadChars(4));
            }

            if (data_signature != "data")
            {
                throw new NotSupportedException("Specified wave file is not supported.");
            }

            int data_chunk_size = reader.ReadInt32();

            frequency = sample_rate;
            format = GetSoundFormat(num_channels, bits_per_sample);
            audioData = reader.ReadBytes((int) reader.BaseStream.Length);
            size = data_chunk_size;

            return audioData;
        }

        /*public System.IO.Stream OpenStream()
        {
            return null;
        }*/
        public virtual SoundEffectInstance CreateInstance()
        {
            return new SoundEffectInstance(this);
        }
        #region IDisposable implementation
        public void Dispose()
        {
            AL.DeleteBuffer(Buffer);
        }
        #endregion
    }
}

