using OpenTK.Audio.OpenAL;
using OpenTK.OpenAL;

namespace engenious.Audio
{
    /// <summary>
    /// Defines an audio device.
    /// </summary>
    public class AudioDevice : AudioResource
    {
        internal ALDevice Device;
        internal ALContext Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDevice"/> class.
        /// </summary>
        public AudioDevice()
        {
            Device = ALC.OpenDevice(null);
            Context = ALC.CreateContext(Device, new ALContextAttributes(44100, 1, 2, null, null));
            ALC.MakeContextCurrent(Context);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            ALC.DestroyContext(Context);
            ALC.CloseDevice(Device);
            base.Dispose();
        }
    }
}

