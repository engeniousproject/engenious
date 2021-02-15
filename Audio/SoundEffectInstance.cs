using System;
using OpenTK.Audio.OpenAL;
using OpenTK.OpenAL;

namespace engenious.Audio
{
    /// <summary>
    /// Defines a <see cref="SoundEffectInstance"/> for playing audio.
    /// </summary>
    public class SoundEffectInstance : AudioResource
    {
        internal int Sid;
        /// <summary>
        /// A value indicating whether the <see cref="SoundEffectInstance"/> is to be looped.
        /// </summary>
        protected bool _isLooped;

        /// <summary>
        /// A value indicating the volume.
        /// </summary>
        protected float _volume = 1f;

        /// <summary>
        /// A value indicating the pitch.
        /// </summary>
        protected float _pitch = 1f;

        /// <summary>
        /// A value indicating the pan.
        /// </summary>
        protected float _pan;
        /// <summary>
        /// The underlying <see cref="SoundEffect"/> instance.
        /// </summary>
        protected SoundEffect? _effect;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffectInstance"/>.
        /// </summary>
        public SoundEffectInstance()
            :this(null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffectInstance"/>.
        /// </summary>
        /// <param name="effect">The <see cref="SoundEffect"/> to use.</param>
        public SoundEffectInstance(SoundEffect? effect)
            : this(effect,TimeSpan.Zero)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEffectInstance"/>.
        /// </summary>
        /// <param name="effect">The <see cref="SoundEffect"/> to use.</param>
        /// <param name="duration">The duration of the <see cref="SoundEffect"/>.</param>
        public SoundEffectInstance(SoundEffect? effect, TimeSpan duration)
        {
            _effect = effect;
            Duration = duration;
        }

        /// <summary>
        /// Gets the duration of the 
        /// </summary>
        public TimeSpan Duration { get; protected set; }

        /// <summary>
        /// Gets or sets whether the sound should be looped.
        /// </summary>
        public bool IsLooped
        { 
            get => _isLooped;
            set
            { 
                if (value != _isLooped && Sid != 0)
                {
                    AL.Source(Sid, ALSourceb.Looping, value);
                }
                _isLooped = value;
            }
        }

        /// <summary>
        /// Gets or sets the volume of the sound.
        /// </summary>
        public float Volume{
            get => _volume;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_volume != value)
                {
                    AL.Source (Sid, ALSourcef.Gain, value);
                }
                _volume = value;
            }
            
        }

        /// <summary>
        /// Gets or sets the pitch of the sound.
        /// </summary>
        public float Pitch{
            get => _pitch;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_pitch != value)
                {
                    AL.Source (Sid, ALSourcef.Pitch,value);
                }
                _pitch = value;
            }
        }

        /// <summary>
        /// Gets or sets the pan of the sound.
        /// </summary>
        public float Pan{
            get => _pan;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_pan != value)
                {
                    AL.Source (Sid, ALSource3f.Position,value, 0, 0.1f);
                }
                _pan = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="SoundState"/> of the sound.
        /// </summary>
        public virtual SoundState State { get; internal set; }

        /// <summary>
        /// Applies
        /// </summary>
        /// <param name="listeners">The listeners hearing the sound.</param>
        /// <param name="emitter">The properties of the object the sound is coming from.</param>
        /// <exception cref="NotSupportedException">Just one listener supported for now.</exception>
        public void Apply3D(AudioListener[] listeners, AudioEmitter emitter)
        {
            if (listeners.Length > 1)
                throw new NotSupportedException("Just one listener is supported!");
            foreach(var l in listeners)
                Apply3D(l,emitter);
        }

        private void Apply3D(AudioListener listener,AudioEmitter emitter)
        {
            if (Sid==0)
                return;


            float x, y, z;
            AL.GetListener(ALListener3f.Position, out x, out y, out z);

            var posOffset = emitter.Position - listener.Position;
            // set up orientation matrix
            var orientation = Matrix.Identity;
            var forward = listener.Forward.Normalized();
            var right = Vector3.Cross(forward, listener.Up);
            var up = Vector3.Cross(right,forward);
            orientation.M11 = right.X;
            orientation.M21 = right.Y;
            orientation.M31 = right.Z;
            orientation.M12 = up.X;
            orientation.M22 = up.Y;
            orientation.M32 = up.Z;
            orientation.M13 = -forward.X;
            orientation.M23 = -forward.Y;
            orientation.M33 = -forward.Z;

            // set up our final position and velocity according to orientation of listener
            var finalPos = new Vector3(x + posOffset.X, y + posOffset.Y, z + posOffset.Z);
            finalPos = Vector3.Transform(orientation, finalPos);
            var finalVel = emitter.Velocity;
            finalVel = Vector3.Transform(orientation, finalVel);

            // set the position based on relative positon
            AL.Source(Sid, ALSource3f.Position, finalPos.X, finalPos.Y, finalPos.Z);
            AL.Source(Sid, ALSource3f.Velocity, finalVel.X, finalVel.Y, finalVel.Z);
        }

        /// <summary>
        /// Starts playing the sound.
        /// </summary>
        public virtual void Play()
        {
            if (_effect == null)
                return;
            if (Sid==0)
            {
                Sid = SoundSourceManager.Instance.Dequeue();
                var bufferId = _effect.Buffer;
                AL.Source(Sid, ALSourcei.Buffer, bufferId);
            }

            if (Sid==0)
                return;

            AL.DistanceModel (ALDistanceModel.InverseDistanceClamped);
            AL.Source (Sid, ALSource3f.Position,Pan, 0, 0.1f);
            AL.Source (Sid, ALSourcef.Gain, Volume);
            AL.Source (Sid, ALSourceb.Looping, IsLooped);
            AL.Source (Sid, ALSourcef.Pitch,Pitch);

            SoundSourceManager.Instance.PlaySound(this);
        }

        /// <summary>
        /// Pauses the sound.
        /// </summary>
        public void Pause()
        {
            State = SoundState.Paused;

            AL.SourcePause(Sid);
        }

        /// <summary>
        /// Resumes the sound where it was previously paused.
        /// </summary>
        public void Resume()
        {
            Play();
        }

        /// <summary>
        /// Stops a sound from looping, or stops it immediately.
        /// </summary>
        /// <param name="immediate">Whether to stop the sound immediately.</param>
        public void Stop(bool immediate = true)
        {
            if (immediate)
            {
                AL.SourceStop(Sid);
                AL.Source(Sid, ALSourcei.Buffer, 0);
                SoundSourceManager.Instance.FreeSource(this);
            }
            else
            {
                IsLooped = false;
            }
        }

        #region IDisposable implementation

        /// <inheritdoc />
        public override void Dispose()
        {
            SoundSourceManager.Instance.FreeSource(this);
            base.Dispose();
        }

        #endregion


    }
}

