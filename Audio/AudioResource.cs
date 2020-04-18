using System;

namespace engenious.Audio
{
    /// <summary>
    /// Defines an audio resource.
    /// </summary>
    public class AudioResource : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioResource"/> class.
        /// </summary>
        protected AudioResource()
        {
        }

        /// <summary>
        /// Gets whether the object is disposed.
        /// </summary>
        public bool IsDisposed{ get; private set; }

        #region IDisposable implementation

        /// <inheritdoc />
        public virtual void Dispose()
        {
            IsDisposed = true;
        }

        #endregion
    }
}

