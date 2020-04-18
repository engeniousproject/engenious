namespace engenious.Graphics
{
    /// <summary>
    /// A simple mesh class with indexed vertices.
    /// </summary>
    public class MeshIndexed : Mesh
    {
        /// <inheritdoc />
        public MeshIndexed(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="IndexBuffer"/>.
        /// </summary>
        public IndexBuffer IB{ get; set; }

        /// <inheritdoc />
        public override void Draw()
        {
            GraphicsDevice.VertexBuffer = VB;
            GraphicsDevice.IndexBuffer = IB;
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.Triangles, 0,0, VB.VertexCount,0,PrimitiveCount);
        }
    }
}