using System;

namespace engenious.Graphics
{
    /// <summary>
    /// A base class for graphics resources allocated on a <see cref="GraphicsDevice"/>.
    /// </summary>
    public abstract class GraphicsResource : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsResource"/> class.
        /// </summary>
        protected GraphicsResource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsResource"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> the resource is allocated on.</param>
        protected GraphicsResource(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> the resource is allocated on.
        /// </summary>
        public GraphicsDevice GraphicsDevice{ get; internal set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="GraphicsResource"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a tag of the <see cref="GraphicsResource"/>.
        /// </summary>
        public object Tag{ get; set; }

        /// <summary>
        /// Gets whether the <see cref="GraphicsResource"/> is disposed.
        /// </summary>
        public bool IsDisposed{ get; private set; }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            if (IsDisposed)
                return;
			
            IsDisposed = true;
        }
    }
}

