namespace engenious.Graphics
{
    /// <summary>
    /// Interface containing basic mesh information.
    /// </summary>
    public interface IMesh
    {
        /// <summary>
        /// Gets the minimal bounding box containing this mesh.
        /// </summary>
        BoundingBox BoundingBox{get;}

        /// <summary>
        /// Gets the number of primitives.
        /// </summary>
        int PrimitiveCount{ get; }

        /// <summary>
        /// Renders the mesh.
        /// </summary>
        void Draw();
    }
}