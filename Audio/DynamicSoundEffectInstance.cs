using System;
using OpenTK.Audio.OpenAL;


namespace engenious.Audio
{
    /// <summary>
    /// Defines a dynamic sound effect instance which is streamable.
    /// </summary>
    public class DynamicSoundEffectInstance : SoundEffectInstance
    {
        private int _pendingBufferCount;
        /// <summary>
        /// Event for requesting new buffer data.
        /// </summary>
        public event EventHandler BufferNeeded;
        private const int MinimumBufferCheck = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSoundEffectInstance"/> class.
        /// </summary>
        /// <param name="effect">The effect to play.</param>
        public DynamicSoundEffectInstance(SoundEffect effect)
            :base(effect)
        {
            _pendingBufferCount = 0;
        }

        /// <inheritdoc />
        public override void Play()
        {
            if (State != SoundState.Stopped)
                return;
            
            for (var i=0;i<MinimumBufferCheck && BufferNeeded != null;i++)
            {
                BufferNeeded(this,EventArgs.Empty);
            }

            base.Play();
        }

        /// <summary>
        /// Processes the sound buffers.
        /// </summary>
        public void Update()
        {
            int processedBuffers;
            AL.GetSource(Sid,ALGetSourcei.BuffersProcessed,out processedBuffers);
            if (processedBuffers==0)
                return;

            SoundSourceManager.Instance.Enqueue(AL.SourceUnqueueBuffers(Sid,processedBuffers));
            _pendingBufferCount-=processedBuffers;


            BufferNeeded?.Invoke(this,EventArgs.Empty);

            for (var i = MinimumBufferCheck - _pendingBufferCount;(i > 0) && BufferNeeded != null;i--)
            {
                BufferNeeded(this,EventArgs.Empty);
            }
        }
        
        /// <summary>
        /// Submits a buffer to the <see cref="DynamicSoundEffectInstance"/>.
        /// </summary>
        /// <param name="buffer">The buffer to submit.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SubmitBuffer(byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}

