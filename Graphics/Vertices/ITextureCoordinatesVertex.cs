namespace engenious.Graphics
{
    /// <summary>
    /// Interface for vertex type containing texture coordinates.
    /// </summary>
    public interface ITextureCoordinatesVertex : IVertexType
    {
        /// <summary>
        /// Gets or sets the texture coordinate of this vertex.
        /// </summary>
        Vector2 TextureCoordinate { get; set; }
    }
}