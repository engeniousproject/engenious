namespace engenious.Graphics
{
    /// <summary>
    /// Interface for vertex type containing position information.
    /// </summary>
    public interface IPositionVertex : IVertexType
    {
        /// <summary>
        /// Gets or sets the position information of this vertex.
        /// </summary>
        Vector3 Position { get; set; }
    }
}