namespace engenious.Graphics
{
    /// <summary>
    /// Interface describing basic transformation matrices used for effects.
    /// </summary>
    public interface ITransformationMatrixSeparated
    {
        /// <summary>
        /// Gets or sets the projection matrix.
        /// </summary>
        Matrix Projection{ get; set; }

        /// <summary>
        /// Gets or sets the view matrix.
        /// </summary>
        Matrix View{ get; set; }

        /// <summary>
        /// Gets or sets the world matrix.
        /// </summary>
        Matrix World{ get; set; }
    }
}

