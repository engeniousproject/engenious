using System;
using System.Collections.Generic;

namespace engenious
{
    /// <summary>
    /// Defines an axis-aligned 3D-box.
    /// </summary>
    public struct BoundingBox
    {
        /// <summary>
        /// The corner of the bounding box which is maximal on all axes.
        /// </summary>
        public Vector3 Max;
        /// <summary>
        /// The corner of the bounding box which is minimal on all axes.
        /// </summary>
        public Vector3 Min;
        
        
        private Vector3[] _cornerPreAlloc;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBox"/> struct.
        /// </summary>
        /// <param name="min">The corner of the bounding box which is minimal on all axes.</param>
        /// <param name="max">The corner of the bounding box which is maximal on all axes.</param>
        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
            _cornerPreAlloc = new Vector3[8];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBox"/> struct.
        /// </summary>
        /// <param name="minX">The minimal value of all corners X-axis.</param>
        /// <param name="minY">The minimal value of all corners Y-axis.</param>
        /// <param name="minZ">The minimal value of all corners Z-axis.</param>
        /// <param name="maxX">The maximal value of all corners X-axis.</param>
        /// <param name="maxY">The maximal value of all corners Y-axis.</param>
        /// <param name="maxZ">The maximal value of all corners Z-axis.</param>
        public BoundingBox(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            Min = new Vector3(minX, minY, minZ);
            Max = new Vector3(maxX, maxY, maxZ);
            _cornerPreAlloc = new Vector3[8];
        }

        /// <summary>
        /// Tests whether the <see cref="BoundingBox"/> contains a point.
        /// <remarks>This test is border inclusive.</remarks>
        /// </summary>
        /// <param name="vec">The point to test.</param>
        /// <returns><c>true</c> if the point is contained by the <see cref="BoundingBox"/>;otherwise <c>false</c></returns>
        public bool Contains(Vector3 vec)
        {
            return vec.X >= Min.X && vec.X < Max.X &&
            vec.Y >= Min.Y && vec.Y < Max.Y &&
            vec.Z >= Min.Z && vec.Z < Max.Z;
        }

        /// <summary>
        /// Tests whether the <see cref="BoundingBox"/> contains another <see cref="BoundingBox"/>.
        /// <remarks>This test is border inclusive.</remarks>
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="BoundingBox"/> is contained by the <see cref="BoundingBox"/>;otherwise <c>false</c></returns>
        public bool Contains(BoundingBox box)
        {
            return Contains(box.Min) && Contains(box.Max);
        }

        /// <summary>
        /// Tests whether the <see cref="BoundingBox"/> intersects with another <see cref="BoundingBox"/>.
        /// <remarks>This test is border inclusive.</remarks>
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="BoundingBox"/> intersects with the other <see cref="BoundingBox"/>;otherwise <c>false</c></returns>
        public bool Intersects(BoundingBox box)
        {
            if (Min.X > box.Max.X || box.Min.X > Max.X)
                return false;

            if (Min.Y > box.Max.Y || box.Min.Y > Max.Y)
                return false;

            return !(Min.Z > box.Max.Z) && !(box.Min.Z > Max.Z);
        }
        
        /// <summary>
        /// Tests whether the <see cref="BoundingBox"/> intersects with a <see cref="Ray"/>.
        /// </summary>
        /// <param name="ray">The <see cref="Ray"/> to test with.</param>
        /// <returns>The distance at which the intersection occurs, or <c>null</c> if the <see cref="Ray"/> does not intersect with the <see cref="BoundingBox"/>.</returns>
        public float? Intersects(Ray ray)
        {
            const float epsilon = 1e-6f;

            float? tMin = null, tMax = null;

            if (Math.Abs(ray.Direction.X) < epsilon)
            {
                if (ray.Position.X < Min.X || ray.Position.X > Max.X)
                    return null;
            }
            else
            {
                tMin = (Min.X - ray.Position.X) / ray.Direction.X;
                tMax = (Max.X - ray.Position.X) / ray.Direction.X;

                if (tMin > tMax)
                {
                    var temp = tMin;
                    tMin = tMax;
                    tMax = temp;
                }
            }

            if (Math.Abs(ray.Direction.Y) < epsilon)
            {
                if (ray.Position.Y < Min.Y || ray.Position.Y > Max.Y)
                    return null;
            }
            else
            {
                var tMinY = (Min.Y - ray.Position.Y) / ray.Direction.Y;
                var tMaxY = (Max.Y - ray.Position.Y) / ray.Direction.Y;

                if (tMinY > tMaxY)
                {
                    var temp = tMinY;
                    tMinY = tMaxY;
                    tMaxY = temp;
                }

                if ((tMin.HasValue && tMin > tMaxY) || (tMax.HasValue && tMinY > tMax))
                    return null;

                if (!tMin.HasValue || tMinY > tMin)
                    tMin = tMinY;
                if (!tMax.HasValue || tMaxY < tMax)
                    tMax = tMaxY;
            }

            if (Math.Abs(ray.Direction.Z) < epsilon)
            {
                if (ray.Position.Z < Min.Z || ray.Position.Z > Max.Z)
                    return null;
            }
            else
            {
                var tMinZ = (Min.Z - ray.Position.Z) / ray.Direction.Z;
                var tMaxZ = (Max.Z - ray.Position.Z) / ray.Direction.Z;

                if (tMinZ > tMaxZ)
                {
                    var temp = tMinZ;
                    tMinZ = tMaxZ;
                    tMaxZ = temp;
                }

                if ((tMin.HasValue && tMin > tMaxZ) || (tMax.HasValue && tMinZ > tMax))
                    return null;

                if (!tMin.HasValue || tMinZ > tMin)
                    tMin = tMinZ;
                if (!tMax.HasValue || tMaxZ < tMax)
                    tMax = tMaxZ;
            }

            // having a positive tMin and a negative tMax means the ray is inside the box
            // we expect the intesection distance to be 0 in that case
            if ((tMin.HasValue && tMin < 0) && tMax > 0)
                return 0;

            // a negative tMin means that the intersection point is behind the ray's origin
            // we discard these as not hitting the AABB
            if (tMin < 0)
                return null;

            return tMin;
        }

        /// <summary>
        /// Gets all corners of the <see cref="BoundingBox"/>.
        /// </summary>
        /// <returns>All corners of the <see cref="BoundingBox"/>.</returns>
        /// <remarks>
        /// Order of corners:
        ///     - (Min.X, Max.Y, Max.Z)
        ///     - Max
        ///     - (Max.X, Min.Y, Max.Z)
        ///     - (Min.X, Min.Y, Max.Z)
        ///     - (Min.X, Max.Y, Min.Z)
        ///     - (Max.X, Max.Y, Min.Z)
        ///     - (Max.X, Min.Y, Min.Z)
        ///     - Min
        /// </remarks>
        public Vector3[] GetCorners()
        {
            if (_cornerPreAlloc == null)
                _cornerPreAlloc = new Vector3[8];
            _cornerPreAlloc[0] = new Vector3(Min.X, Max.Y, Max.Z);
            _cornerPreAlloc[1] = Max;
            _cornerPreAlloc[2] = new Vector3(Max.X, Min.Y, Max.Z);
            _cornerPreAlloc[3] = new Vector3(Min.X, Min.Y, Max.Z);
            _cornerPreAlloc[4] = new Vector3(Min.X, Max.Y, Min.Z);
            _cornerPreAlloc[5] = new Vector3(Max.X, Max.Y, Min.Z);
            _cornerPreAlloc[6] = new Vector3(Max.X, Min.Y, Min.Z);
            _cornerPreAlloc[7] = Min;

            return _cornerPreAlloc;
        }

        /// <summary>
        /// Creates the smallest possible  <see cref="BoundingBox"/> containing all the passed points.
        /// <remarks>This is border inclusive.</remarks>
        /// </summary>
        /// <param name="points">The points to be contained by the created <see cref="BoundingBox"/>.</param>
        /// <returns>The smallest possible <see cref="BoundingBox"/> containing all the <paramref name="points"/>.</returns>
        public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
        {
            float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue, maxZ = float.MinValue;
            foreach (var point in points)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                minZ = Math.Min(minZ, point.Z);
                                           
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
                maxZ = Math.Max(maxZ, point.Z);
            }
            return new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
        }

        /// <summary>
        /// Creates the smallest possible <see cref="BoundingBox"/> containing an additional <see cref="BoundingBox"/>.
        /// <remarks>This is border inclusive.</remarks>
        /// </summary>
        /// <param name="original">The first <see cref="BoundingBox"/> to combine with the <paramref name="additional"/> <see cref="BoundingBox"/>.</param>
        /// <param name="additional">The <paramref name="additional"/> <see cref="BoundingBox"/> to combine with.</param>
        /// <param name="result">The resulting combined <see cref="BoundingBox"/> created from <paramref name="original"/> and <paramref name="additional"/>.</param>
        public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
        {
            result = new BoundingBox(Math.Min(original.Min.X, additional.Min.X),
                Math.Min(original.Min.Y, additional.Min.Y),
                Math.Min(original.Min.Z, additional.Min.Z),
                Math.Max(original.Max.X, additional.Max.X),
                Math.Max(original.Max.Y, additional.Max.Y),
                Math.Max(original.Max.Z, additional.Max.Z));
        }

        /// <summary>
        /// Creates the smallest possible <see cref="BoundingBox"/> containing all the passed BoundingBoxes.
        /// <remarks>This is border inclusive.</remarks>
        /// </summary>
        /// <param name="boundingBoxes">The BoundingBoxes to create the new <see cref="BoundingBox"/> from</param>
        /// <returns>The smallest possible <see cref="BoundingBox"/> containing all the <paramref name="boundingBoxes"/>.</returns>
        public static BoundingBox CreateMerged(params BoundingBox[] boundingBoxes)
        {
            return CreateMerged((IEnumerable<BoundingBox>)boundingBoxes);
        }

        /// <summary>
        /// Creates the smallest possible <see cref="BoundingBox"/> containing all the passed BoundingBoxes.
        /// <remarks>This is border inclusive.</remarks>
        /// </summary>
        /// <param name="boundingBoxes">The BoundingBoxes to create the new <see cref="BoundingBox"/> from</param>
        /// <returns>The smallest possible <see cref="BoundingBox"/> containing all the <paramref name="boundingBoxes"/>.</returns>
        public static BoundingBox CreateMerged(IEnumerable<BoundingBox> boundingBoxes)
        {
            float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue, maxZ = float.MinValue;
            foreach (var box in boundingBoxes)
            {
                minX = Math.Min(box.Min.X, minX);
                minY = Math.Min(box.Min.Y, minY);
                minZ = Math.Min(box.Min.Z, minZ);
                                    
                maxX = Math.Max(box.Max.X, maxX);
                maxY = Math.Max(box.Max.Y, maxY);
                maxZ = Math.Max(box.Max.Z, maxZ);
            }
            return new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
        }
    }
}

