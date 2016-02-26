using System;

namespace engenious.Audio
{
    public class AudioResource : IDisposable
    {
        protected AudioResource()
        {
        }

        public bool IsDisposed{ get; private set; }

        #region IDisposable implementation

        public virtual void Dispose()
        {
            IsDisposed = true;
        }

        #endregion
    }
}

