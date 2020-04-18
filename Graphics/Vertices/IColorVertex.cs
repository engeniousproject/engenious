namespace engenious.Graphics
{
    /// <summary>
    /// Interface for vertex type containing color information.
    /// </summary>
    public interface IColorVertex : IVertexType
    {
        /// <summary>
        /// Gets or sets the color of this vertex.
        /// </summary>
        Color Color { get; set; }
    }
}