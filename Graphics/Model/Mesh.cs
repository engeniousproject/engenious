namespace engenious.Graphics
{
    /// <summary>
    /// A simple mesh class.
    /// </summary>
    public class Mesh : BaseMesh
    {
        /// <inheritdoc />
        public Mesh(GraphicsDevice graphicsDevice, int primitiveCount, VertexBuffer vertexBuffer)
            : base(graphicsDevice, primitiveCount)
        {
            VB = vertexBuffer;
        }

        /// <summary>
        /// Gets or sets the <see cref="VertexBuffer"/>.
        /// </summary>
        public VertexBuffer VB{ get; }

        /// <inheritdoc />
        public override void Draw()
        {
            GraphicsDevice.VertexBuffer = VB;
            GraphicsDevice.DrawPrimitives(PrimitiveType.Triangles, 0, (int)VB.VertexCount);
        }
    }
}

