﻿namespace engenious
{
    /// <summary>
    /// Defines a Bounding frustum.
    /// </summary>
    public class BoundingFrustum
    {
        private readonly Plane[] _planes = new Plane[6];
        private Matrix _matrix;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingFrustum"/> class using a matrix creating a frustum.
        /// <remarks>Usually a matrix resulting from <c>View * Projection</c>.</remarks>
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to create the frustum from.</param>
        public BoundingFrustum(Matrix matrix)
        {
            Matrix = matrix;
        }

        /// <summary>
        /// Gets the bottom plane of the frustum.
        /// </summary>
        public Plane Bottom
        {
            get => _planes[5];
            private set => _planes[5] = value;
        }

        /// <summary>
        /// Gets the top plane of the frustum.
        /// </summary>
        public Plane Top
        {
            get => _planes[4];
            private set => _planes[4] = value;
        }

        /// <summary>
        /// Gets the far plane of the frustum.
        /// </summary>
        public Plane Far
        {
            get => _planes[3];
            private set => _planes[3] = value;
        }

        /// <summary>
        /// Gets the near plane of the frustum.
        /// </summary>
        public Plane Near
        {
            get => _planes[2];
            private set => _planes[2] = value;
        }

        /// <summary>
        /// Gets the left plane of the frustum.
        /// </summary>
        public Plane Left
        {
            get => _planes[1];
            private set => _planes[1] = value;
        }

        /// <summary>
        /// Gets the right plane of the frustum.
        /// </summary>
        public Plane Right
        {
            get => _planes[0];
            private set => _planes[0] = value;
        }

        /// <summary>
        /// Gets the matrix which constructed the <see cref="BoundingFrustum"/>.
        /// </summary>
        public Matrix Matrix
        {
            get => _matrix; set
            {
                _matrix = value;
                
                Right  = new Plane( value.M41 + value.M11, value.M42 + value.M12, value.M43 + value.M13, value.M44 + value.M14);
                Left   = new Plane( value.M41 - value.M11, value.M42 - value.M12, value.M43 - value.M13, value.M44 - value.M14);
                Top    = new Plane( value.M41 - value.M21, value.M42 - value.M22, value.M43 - value.M23, value.M44 - value.M24);
                Bottom = new Plane( value.M41 + value.M21, value.M42 + value.M22, value.M43 + value.M23, value.M44 + value.M24);
                Near   = new Plane( value.M41 - value.M31, value.M42 - value.M32, value.M43 - value.M33, value.M44 - value.M34);
                Far    = new Plane( value.M31, value.M32, value.M33, value.M34);
            }
        }

        private static void PVertex(Vector3 normal, BoundingBox box, out Vector3 vn, out Vector3 vp)
        {
            float vNx, vNy, vNz, vPx, vPy, vPz;
            if (normal.X >= 0)
            {
                vPx = box.Max.X;
                vNx = box.Min.X;
            }
            else
            {
                vPx = box.Min.X;
                vNx = box.Max.X;
            }
            if (normal.Y >= 0)
            {
                vPy = box.Max.Y;
                vNy = box.Min.Y;
            }
            else
            {
                vPy = box.Min.Y;
                vNy = box.Max.Y;
            }
            if (normal.Z >= 0)
            {
                vPz = box.Max.Z;
                vNz = box.Min.Z;
            }
            else
            {
                vPz = box.Min.Z;
                vNz = box.Max.Z;
            }
            vp = new Vector3(vPx, vPy, vPz);
            vn = new Vector3(vNx, vNy, vNz);
        }

        /// <summary>
        /// Tests whether and in which way the <see cref="BoundingBox"/> collides with the <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to test the containment with.</param>
        /// <param name="type">The resulting <see cref="CollisionType"/>.</param>
        /// <returns><c>true</c> if the <see cref="BoundingBox"/> is contained in the <see cref="BoundingFrustum"/>;otherwise <c>false</c></returns>
        public bool Contains(BoundingBox box, out CollisionType type)
        {
            type = CollisionType.Outside;
            for (var i = 0; i < _planes.Length; i++)
            {
                var d = _planes[i].D;
                var n = _planes[i].Normal;
                PVertex(n, box, out _, out var vp);

                var a = vp.Dot(n) + d;
                if (a < 0)//TODO: validate
                    return false;
                var b = vp.Dot(n) + d;
                if (b < 0)
                    type = CollisionType.Intersect;
            }
            if (type == CollisionType.Outside)
                type = CollisionType.Inside;
            return true;
        }

        /// <summary>
        /// Tests whether the given <see cref="BoundingBox"/> intersects with the <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="BoundingBox"/> intersects with the <see cref="BoundingFrustum"/>;otherwise <c>false</c></returns>
        public bool Intersects(BoundingBox box)
        {
            _ = Contains(box, out var tmp);
            return tmp != CollisionType.Outside;
        }
    }
}