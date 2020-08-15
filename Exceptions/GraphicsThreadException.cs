using System;

namespace engenious
{
    /// <summary>
    /// Exception for calling stuff from a thread that doesn't match the GraphicsDevice.
    /// </summary>
    public class GraphicsThreadException : Exception
    {
        /// <inheritdoc />
        public GraphicsThreadException()
            : base("Operation only permitted on the current thread. Only allowed on the Graphics thread(Thread the GraphicsDevice was initialized with)!")
        {
            
        }
    }
}