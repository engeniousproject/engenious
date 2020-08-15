using System;
using System.IO;
using System.Runtime.CompilerServices;
using OpenToolkit.Audio.OpenAL;
using OpenToolkit.OpenAL;

namespace engenious.Audio
{
    /// <summary>
    /// Defines a <see cref="SoundEffect"/>.
    /// </summary>
    public class SoundEffect : IDisposable
    {
        /// <summary>
        /// Gets or sets the speed of sound in the current medium.
        /// </summary>
        public static float SpeedOfSound { get; set; }

        /// <summary>
        /// Gets or sets the master volume.
        /// </summary>
        public static float MasterVolume { get; set; }

        static SoundEffect()
        {
            SpeedOfSound = 345.5f;
            MasterVolume = 1.0f;
        }
        //TODO dynamic sound
        internal int Buffer;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffect"/> class.
        /// </summary>
        /// <param name="fileName">The file to load the <see cref="SoundEffect"/> from.</param>
        public SoundEffect(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                LoadStream(fs);
            }
        }

        /// <summary>
        /// Specifies available audio formats.
        /// </summary>
        public enum AudioFormat
        {
            /// <summary>
            /// The ogg vorbis audio format.
            /// </summary>
            Ogg,
            /// <summary>
            /// The wave audio format.
            /// </summary>
            Wav
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffect"/> class.
        /// </summary>
        /// <param name="stream">The stream to load the <see cref="SoundEffect"/> from.</param>
        /// <param name="format">The <see cref="AudioFormat"/> of the <paramref name="stream"/>.</param>
        public SoundEffect(Stream stream, AudioFormat format)
        {
            LoadStream(stream,format);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffect"/> class.
        /// </summary>
        /// <param name="stream">The stream to load the <see cref="SoundEffect"/> from.</param>
        public SoundEffect(Stream stream)
        {
            LoadStream(stream);
        }

        private void LoadStream(Stream stream)
        {
            LoadStream(stream,ReadAudioFormat(stream));
        }

        private void LoadStream(Stream stream, AudioFormat format)
        {
            //TODO: dynamic sound - buffer encapsulated
            AL.GenBuffers(1, ref Buffer);
            switch (format)
            {
                case AudioFormat.Wav:
                    using (var reader = new BinaryReader(stream))
                    {
                        ALFormat alFormat;
                        int size, frequency;
                        var buffer = LoadWave(reader, out alFormat, out size, out frequency);
                        AL.BufferData(Buffer, alFormat, buffer, size, frequency);
                    }
                    break;
                case AudioFormat.Ogg:
                    using (var offsetedStream = new OffsetedStream(stream))
                    using (var ogg = new OggStream(offsetedStream))
                    {
                        var bitsPerSample = 16;
                        var alFormat = GetSoundFormat(ogg.Reader.Channels, bitsPerSample);
                        var frequency = ogg.Reader.SampleRate;
                        var samples = (int)(((long)ogg.Reader.TotalTime.TotalMilliseconds * ogg.Reader.SampleRate * ogg.Reader.Channels)/1000);

                        var conversionBuffer = new float[samples];
                        var buffer = new byte[samples*bitsPerSample/8];

                        ogg.Reader.ReadSamples(conversionBuffer, 0, samples);
                        unsafe
                        {
                            fixed (byte* ptr = buffer)
                            {
                                var ptr2 = ptr;
                                for (var i = 0; i < samples; i++,ptr2+=(bitsPerSample/8))
                                {
                                    ConvertShit(conversionBuffer[i],ptr2);
                                }
                            }
                        }
                        AL.BufferData(Buffer,alFormat,buffer,(buffer.Length - buffer.Length % 4),frequency);
                    }
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void ConvertShit(float val,byte* ptr)
        {
            val += 1;
            val = (val * ushort.MaxValue / 2) + short.MinValue;

            var tmp = (short) val;
            *ptr++ = (byte)(tmp & 0xFF);
            *ptr = (byte) (tmp >> 8);
        }
        private static AudioFormat ReadAudioFormat(Stream stream)
        {
            if (stream.CanSeek)
            {
                var magic = new byte[4];
                stream.Read(magic, 0, magic.Length);

                AudioFormat format;
                if (magic[0] == 'R' && magic[1] == 'I' && magic[2] == 'F' && magic[3] == 'F')
                {
                    format = AudioFormat.Wav;
                }
                else if (magic[0] == 'O' && magic[1] == 'g' && magic[2] == 'g' && magic[3] == 'S')
                {
                    format = AudioFormat.Ogg;
                }
                else
                {
                    throw new NotSupportedException("Fileformat not supported!");
                }
                stream.Seek(-4, SeekOrigin.Current);
                return format;
            }
            throw new ArgumentException("Can't determine audio format of unseekable stream");
        }
        private static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        private static byte[] LoadWave(BinaryReader reader, out ALFormat format, out int size, out int frequency)
        {
            // code based on opentk exemple

            byte[] audioData;

            //header
            var signature = new string(reader.ReadChars(4));
            if (signature != "RIFF")
            {
                throw new NotSupportedException("Specified stream is not a wave file.");
            }

            reader.ReadInt32(); // riff_chunck_size

            var wformat = new string(reader.ReadChars(4));
            if (wformat != "WAVE")
            {
                throw new NotSupportedException("Specified stream is not a wave file.");
            }

            // WAVE header
            var formatSignature = new string(reader.ReadChars(4));
            while (formatSignature != "fmt ")
            {
                reader.ReadBytes(reader.ReadInt32());
                formatSignature = new string(reader.ReadChars(4));
            }

            var formatChunkSize = reader.ReadInt32();

            // total bytes read: tbp
            int audioFormat = reader.ReadInt16(); // 2
            int numChannels = reader.ReadInt16(); // 4
            var sampleRate = reader.ReadInt32(); // 8
            reader.ReadInt32(); // 12, byte_rate
            reader.ReadInt16(); // 14, block_align
            int bitsPerSample = reader.ReadInt16(); // 16

            if (audioFormat != 1)
            {
                throw new NotSupportedException("Wave compression is not supported.");
            }

            // reads residual bytes
            if (formatChunkSize > 16)
                reader.ReadBytes(formatChunkSize - 16);

            var dataSignature = new string(reader.ReadChars(4));

            while (dataSignature.ToLowerInvariant() != "data")
            {
                reader.ReadBytes(reader.ReadInt32());
                dataSignature = new string(reader.ReadChars(4));
            }

            if (dataSignature != "data")
            {
                throw new NotSupportedException("Specified wave file is not supported.");
            }

            var dataChunkSize = reader.ReadInt32();

            frequency = sampleRate;
            format = GetSoundFormat(numChannels, bitsPerSample);
            audioData = reader.ReadBytes((int) reader.BaseStream.Length);
            if (dataChunkSize == -1)
                size = audioData.Length;
            else
                size = dataChunkSize;

            return audioData;
        }

        /// <summary>
        /// Plays the <see cref="SoundEffect"/> using a <see cref="SoundEffectInstance"/> from the <see cref="SoundEffectInstancePool"/>.
        /// </summary>
        public void Play()
        {
            SoundEffectInstancePool.Instance.Acquire(this).Play();
        }

        /// <summary>
        /// Creates a pooled <see cref="SoundEffectInstance"/> from the <see cref="SoundEffectInstancePool"/>.
        /// </summary>
        public virtual SoundEffectInstance CreateInstance()
        {
            return SoundEffectInstancePool.Instance.Acquire(this);
        }
        #region IDisposable implementation

        /// <inheritdoc />
        public void Dispose()
        {
            AL.DeleteBuffer(Buffer);
        }
        #endregion
    }
}

