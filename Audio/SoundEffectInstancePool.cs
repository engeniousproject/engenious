using System;
using System.Collections.Generic;

namespace engenious.Audio
{
    /// <summary>
    /// Defines a pool for the <see cref="SoundEffectInstance"/> class.
    /// </summary>
    public class SoundEffectInstancePool
    {
        private static SoundEffectInstancePool _instance;
        /// <summary>
        /// Gets the <see cref="SoundEffectInstancePool"/> singleton.
        /// </summary>
        public static SoundEffectInstancePool Instance => _instance ?? (_instance = new SoundEffectInstancePool());

        private const int InstanceCount = 256;
        private readonly Stack<PooledSoundEffectInstance> _stack = new Stack<PooledSoundEffectInstance>(InstanceCount);

        private SoundEffectInstancePool()
        {
            for (var i = 0; i < InstanceCount; i++)
            {
                _stack.Push(new PooledSoundEffectInstance());
            }
        }

        /// <summary>
        /// Acquires a <see cref="SoundEffectInstance"/> from this pool.
        /// </summary>
        /// <param name="effect">The effect to be played with the acquired <see cref="SoundEffectInstance"/>.</param>
        /// <returns>The acquired <see cref="SoundEffectInstance"/>.</returns>
        public SoundEffectInstance Acquire(SoundEffect effect)
        {
            var inst = _stack.Count == 0 ? new PooledSoundEffectInstance() : _stack.Pop();
            inst.Init(effect);
            return inst;
        }

        /// <summary>
        /// Releases the <see cref="PooledSoundEffectInstance"/> back into the pool.
        /// </summary>
        /// <param name="instance">The <see cref="PooledSoundEffectInstance"/> to release.</param>
        public void Release(PooledSoundEffectInstance instance)
        {
            _stack.Push(instance);
        }
    }

    /// <summary>
    /// A pooled <see cref="SoundEffectInstance"/>.
    /// </summary>
    public class PooledSoundEffectInstance : SoundEffectInstance
    {
        internal PooledSoundEffectInstance()
            :base()
        {

        }

        internal void Init(SoundEffect effect)
        {
            _effect = effect;
            Duration = TimeSpan.Zero;
            _isLooped = false;
            _volume = 1f;
            _pitch = 1f;
            _pan = 0;
        }

        private SoundState _state;

        /// <summary>
        /// Gets the <see cref="SoundState"/> of the current <see cref="SoundEffectInstance"/>.
        /// </summary>
        public override SoundState State
        {
            get { return _state; }
            internal set
            {
                _state = value;
                if (value == SoundState.Stopped)
                    SoundEffectInstancePool.Instance.Release(this);
            }
        }
    }
}