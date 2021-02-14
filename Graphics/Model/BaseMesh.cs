namespace engenious.Graphics
{
    /// <summary>
    /// Abstract basic mesh class.
    /// </summary>
    public abstract class BaseMesh : GraphicsResource, IMesh
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMesh"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="primitiveCount">The number of primitives of this mesh.</param>
        protected BaseMesh(GraphicsDevice graphicsDevice, int primitiveCount)
            : base(graphicsDevice)
        {
            PrimitiveCount = primitiveCount;
        }

        /// <inheritdoc cref="GraphicsResource.GraphicsDevice"/>
        public new GraphicsDevice GraphicsDevice => base.GraphicsDevice!;
        
        /// <summary>
        /// Gets or sets the minimal bounding box containing this mesh.
        /// </summary>
        public BoundingBox BoundingBox { get; protected internal set; }

        /// <summary>
        /// Gets or sets the number of primitives.
        /// </summary>
        public int PrimitiveCount { get; }

        /// <inheritdoc />
        public abstract void Draw();
    }
}