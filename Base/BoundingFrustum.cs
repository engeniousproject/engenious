using System;
using OpenTK;

namespace engenious
{
    public class BoundingFrustum
    {
        public BoundingFrustum(Matrix matrix)
        {
            
            //TODO:
            // Right clipping plane.
            Right = new Plane(matrix.M14 - matrix.M11, matrix.M24 - matrix.M21, matrix.M34 - matrix.M31, matrix.M44 - matrix.M41);
            // Left clipping plane.
            Left = new Plane(matrix.M14 + matrix.M11, matrix.M24 + matrix.M21, matrix.M34 + matrix.M31, matrix.M44 + matrix.M41);
            // Bottom clipping plane.
            Bottom = new Plane(matrix.M14 + matrix.M12, matrix.M24 + matrix.M22, matrix.M34 + matrix.M32, matrix.M44 + matrix.M42);
            // Top clipping plane.
            Top = new Plane(matrix.M14 - matrix.M12, matrix.M24 - matrix.M22, matrix.M34 - matrix.M32, matrix.M44 - matrix.M42);
            // Far clipping plane.
            Far = new Plane(matrix.M14 - matrix.M13, matrix.M24 - matrix.M23, matrix.M34 - matrix.M33, matrix.M44 - matrix.M43);
            // Near clipping plane.
            Near = new Plane(matrix.M14 + matrix.M13, matrix.M24 + matrix.M23, matrix.M34 + matrix.M33, matrix.M44 + matrix.M43);
        }

        public Plane Bottom{ get; private set; }

        public Plane Top{ get; private set; }

        public Plane Far{ get; private set; }

        public Plane Near{ get; private set; }

        public Plane Left{ get; private set; }

        public Plane Right{ get; private set; }

        public Matrix4 Matrix{ get; private set; }


        public bool Intersects(BoundingBox box)
        {
            return false;
        }
    }
}

