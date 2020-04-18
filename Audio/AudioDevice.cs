using OpenTK.Audio;

namespace engenious.Audio
{
    /// <summary>
    /// Defines an audio device.
    /// </summary>
    public class AudioDevice : AudioResource
    {
        internal AudioContext Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDevice"/> class.
        /// </summary>
        public AudioDevice()
        {
            Context = new AudioContext();
            Context.MakeCurrent();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            Context.Dispose();
            base.Dispose();
        }
    }
}

