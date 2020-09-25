using OpenTK.Graphics.ES20;

namespace engenious
{
    /// <summary>
    /// Defines a 3D ray.
    /// </summary>
    public struct Ray
    {
        /// <summary>
        /// The direction the ray points to.
        /// </summary>
        public Vector3 Direction;
        
        /// <summary>
        /// The position the ray starts at.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Initializes a new <see cref="Ray"/> struct.
        /// </summary>
        /// <param name="position">The starting position of the ray.</param>
        /// <param name="direction">The direction the ray points to.</param>
        public Ray(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }

        /// <summary>
        /// Tests whether the <see cref="Ray"/> intersects with a <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to test with.</param>
        /// <returns>The distance at which the intersection occurs, or <c>null</c> if the <see cref="Ray"/> does not intersect with the <see cref="BoundingBox"/>.</returns>
        public float? Intersects(BoundingBox box)
        {
            return box.Intersects(this);   
        }

        /// <summary>
        /// Tests two <see cref="Ray"/> structs for equality.
        /// </summary>
        /// <param name="ray1">The first <see cref="Ray"/> to test with.</param>
        /// <param name="ray2">The second <see cref="Ray"/> to test with.</param>
        /// <returns><c>true</c> if the rays are equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Ray ray1, Ray ray2)
        {
            
            bool posEqual = ray1.Position == ray2.Position;
            if (!posEqual)
                return false;
            //ray1.Direction.Normalized() == ray2.Direction.Normalized();
            // test if direction is the same
            var div = ray1.Direction * (ray2.Direction.X * ray2.Direction.Y * ray2.Direction.Z) / ray2.Direction;
            return div.X > 0 && (int)div.X == (int)div.Y && (int)div.Y == (int)div.Z;
        }

        /// <summary>
        /// Tests two <see cref="Ray"/> structs for inequality.
        /// </summary>
        /// <param name="ray1">The first <see cref="Ray"/> to test with.</param>
        /// <param name="ray2">The second <see cref="Ray"/> to test with.</param>
        /// <returns><c>true</c> if the rays aren't equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Ray ray1, Ray ray2)
        {
            return ray1.Position != ray2.Position || ray1.Direction.Normalized() != ray2.Direction.Normalized();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Direction.GetHashCode()*397) ^ Position.GetHashCode();
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Ray)
            {
                return (Ray)obj == this;
            }
            return false;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Position: {Position.ToString()},Direction: {Direction.ToString()}]";
        }
    }
}

