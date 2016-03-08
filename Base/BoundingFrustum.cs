using System;
using OpenTK;

namespace engenious
{
    public class BoundingFrustum
    {
        private Plane[] planes = new Plane[6];

        public BoundingFrustum(Matrix matrix)
        {
            
            //TODO:
            Right = new Plane(matrix.M14 - matrix.M11, matrix.M24 - matrix.M21, matrix.M34 - matrix.M31, matrix.M44 - matrix.M41);
            Left = new Plane(matrix.M14 + matrix.M11, matrix.M24 + matrix.M21, matrix.M34 + matrix.M31, matrix.M44 + matrix.M41);
            Bottom = new Plane(matrix.M14 + matrix.M12, matrix.M24 + matrix.M22, matrix.M34 + matrix.M32, matrix.M44 + matrix.M42);
            Top = new Plane(matrix.M14 - matrix.M12, matrix.M24 - matrix.M22, matrix.M34 - matrix.M32, matrix.M44 - matrix.M42);
            Far = new Plane(matrix.M14 - matrix.M13, matrix.M24 - matrix.M23, matrix.M34 - matrix.M33, matrix.M44 - matrix.M43);
            Near = new Plane(matrix.M14 + matrix.M13, matrix.M24 + matrix.M23, matrix.M34 + matrix.M33, matrix.M44 + matrix.M43);
            /*Right = new Plane(matrix.M41 - matrix.M11, matrix.M42 - matrix.M12, matrix.M43 - matrix.M13, matrix.M44 - matrix.M14);
            Left = new Plane(matrix.M41 + matrix.M11, matrix.M42 + matrix.M12, matrix.M43 + matrix.M13, matrix.M44 + matrix.M14);
            Bottom = new Plane(matrix.M41 + matrix.M21, matrix.M42 + matrix.M22, matrix.M43 + matrix.M23, matrix.M44 + matrix.M24);
            Top = new Plane(matrix.M41 - matrix.M21, matrix.M42 - matrix.M22, matrix.M43 - matrix.M23, matrix.M44 - matrix.M24);
            Far = new Plane(matrix.M41 - matrix.M31, matrix.M42 - matrix.M32, matrix.M43 - matrix.M33, matrix.M44 - matrix.M34);
            Near = new Plane(matrix.M41 + matrix.M31, matrix.M42 + matrix.M32, matrix.M43 + matrix.M33, matrix.M44 + matrix.M34);*/

            this.Matrix = matrix;
        }


        public Plane Bottom
        { 
            get{ return planes[5]; }
            private set
            {
                planes[5] = value; 
            }
        }

        public Plane Top
        {
            get{ return planes[4]; }
            private set
            {
                planes[4] = value; 
            }
        }

        public Plane Far
        {
            get{ return planes[3]; }
            private set
            {
                planes[3] = value; 
            }
        }

        public Plane Near
        { 
            get{ return planes[2]; }
            private set
            {
                planes[2] = value; 
            }
        }

        public Plane Left
        { 
            get{ return planes[1]; }
            private set
            {
                planes[1] = value; 
            }
        }

        public Plane Right
        {
            get{ return planes[0]; }
            private set
            {
                planes[0] = value; 
            }
        }

        public Matrix Matrix{ get; private set; }

        public enum CollisionType
        {
            Outside,
            Intersect,
            Inside
        }

        private void pVertex(Vector3 normal, BoundingBox box, out Vector3 vn, out Vector3 vp)
        {
            if (normal.X >= 0)
            {
                vp.X = box.Max.X;
                vn.X = box.Min.X;
            }
            else
            {
                vp.X = box.Min.X;
                vn.X = box.Max.X;
            }
            if (normal.Y >= 0)
            {
                vp.Y = box.Max.Y;
                vn.Y = box.Min.Y;
            }
            else
            {
                vp.Y = box.Min.Y;
                vn.Y = box.Max.Y;
            }
            if (normal.Z >= 0)
            {
                vp.Z = box.Max.Z;
                vn.Z = box.Min.Z;
            }
            else
            {
                vp.Z = box.Min.Z;
                vn.Z = box.Max.Z;
            }

        }

        public bool Contains(BoundingBox box, out CollisionType type)
        {
            type = CollisionType.Outside;
            for (int i = 0; i < planes.Length; i++)
            {
                float d = planes[i].D;
                Vector3 vn, vp;
                Vector3 n = planes[i].Normal;
                pVertex(n, box, out vn, out vp);

                float a = vp.Dot(n) + d;
                if (a < 0)//TODO: validate
                    return false;
                float b = vp.Dot(n) + d;
                if (b < 0)
                    type = CollisionType.Intersect;
            }
            if (type == CollisionType.Outside)
                type = CollisionType.Inside;
            return true;
        }

        public bool Intersects(BoundingBox box)
        {
            //return false;
            //return true;
            CollisionType tmp;
            Contains(box, out tmp);
            return tmp != CollisionType.Outside;
        }
    }
}

