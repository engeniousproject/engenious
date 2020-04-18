namespace engenious.Graphics
{
    /// <summary>
    /// Abstract basic mesh class.
    /// </summary>
    public abstract class BaseMesh : GraphicsResource,IMesh
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMesh"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        protected BaseMesh(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            
        }

        /// <summary>
        /// Gets or sets the minimal bounding box containing this mesh.
        /// </summary>
        public BoundingBox BoundingBox { get; protected internal set; }

        /// <summary>
        /// Gets or sets the number of primitives.
        /// </summary>
        public int PrimitiveCount { get;protected internal set; }

        /// <inheritdoc />
        public abstract void Draw();
    }
}