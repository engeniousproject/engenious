namespace engenious.Graphics
{
    /// <summary>
    /// Interface for vertex type containing normals.
    /// </summary>
    public interface INormalVertex : IVertexType
    {
        /// <summary>
        /// Gets or sets the normal for this vertex.
        /// </summary>
        Vector3 Normal { get; set; }
    }
}