namespace engenious
{
    /// <summary>
    /// Specifies indicators for a collision type.
    /// </summary>
    public enum CollisionType
    {
        /// <summary>
        /// The tested object is outside the testing bounds.
        /// </summary>
        Outside,
        /// <summary>
        /// The tested object intersects with the testing bounds.
        /// </summary>
        Intersect,
        /// <summary>
        /// The tested object is inside the testing bounds.
        /// </summary>
        Inside
    }
}