namespace engenious
{
    /// <summary>
    /// Defines a 3D plane.
    /// </summary>
    public struct Plane
    {
        /// <summary>
        /// The distance of the plane from the origin in the direction of the <see cref="Normal"/>.
        /// </summary>
        public readonly float D;

        /// <summary>
        /// The plane normal.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> struct.
        /// </summary>
        /// <param name="a">The X-Component of the <see cref="Normal"/>.</param>
        /// <param name="b">The Y-Component of the <see cref="Normal"/>.</param>
        /// <param name="c">The Z-Component of the <see cref="Normal"/>.</param>
        /// <param name="d">The distance of the plane from the origin in the direction of the <see cref="Normal"/>.</param>
        public Plane(float a, float b, float c, float d)
        {
            Normal = new Vector3(a, b, c);
            var len = Normal.Length;
            D = (d / len);
            Normal /= len;
        }

        /// <summary>
        /// Calculates the distance from the <see cref="Plane"/> to a point.
        /// </summary>
        /// <param name="pt">The point to calculate the distance from.</param>
        /// <returns>The distance from the point to the <see cref="Plane"/>.</returns>
        public float DistanceToPoint(Vector3 pt)
        {
            return Normal.Dot(pt) + D;
        }
    }
}

