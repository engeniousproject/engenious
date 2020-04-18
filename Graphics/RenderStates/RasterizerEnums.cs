namespace engenious.Graphics
{
    /// <summary>
    /// Specifies the cull modes.
    /// </summary>
    public enum CullMode
    {
        /// <summary>
        /// No culling.
        /// </summary>
        None,
        /// <summary>
        /// Culls clockwise polygons.
        /// </summary>
        Clockwise = 2304,
        /// <summary>
        /// Culls counter clockwise polygons.
        /// </summary>
        CounterClockwise = 2305
    }

    /// <summary>
    /// Specifies the polygon mode.
    /// </summary>
    public enum PolygonMode
    {
        /// <summary>
        /// Renders only points of the polygons.
        /// </summary>
        Point = 6912,
        /// <summary>
        /// Renders only edges of the polygons.
        /// </summary>
        Line = 6913,
        /// <summary>
        /// Renders the filled polygons.
        /// </summary>
        Fill = 6914
    }
}

