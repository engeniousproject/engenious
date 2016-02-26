using System;
using OpenTK;
using System.Collections.Generic;

namespace engenious
{
    public struct BoundingBox
    {
        public Vector3 Max;
        public Vector3 Min;

        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public BoundingBox(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            Min = new Vector3(minX, minY, minZ);
            Max = new Vector3(maxX, maxY, maxZ);
        }

        public bool Contains(Vector3 vec)
        {
            return vec.X >= Min.X && vec.X < Max.X &&
            vec.Y >= Min.Y && vec.Y < Max.Y &&
            vec.Z >= Min.Z && vec.Z < Max.Z;
        }

        public bool Contains(BoundingBox box)
        {
            return Contains(box.Min) && Contains(box.Max);
        }

        private static void swap(ref float val1, ref float val2)
        {
            float tmp = val1;
            val1 = val2;
            val2 = tmp;
        }

        public float? Intersects(Ray ray)
        {
            float txmin = (Min.X - ray.Position.X) / ray.Direction.X;
            float txmax = (Max.X - ray.Position.X) / ray.Direction.X;

            if (txmin > txmax)
                swap(ref txmin, ref txmax);

            float tymin = (Min.Y - ray.Position.Y) / ray.Direction.Y;
            float tymax = (Max.Y - ray.Position.Y) / ray.Direction.Y;

            if (tymin > tymax)
                swap(ref tymin, ref tymax);

            if ((txmin > tymax) || (tymin > txmax))
                return null;

            if (tymin > txmin)
                txmin = tymin;

            if (tymax < txmax)
                txmax = tymax;

            float tzmin = (Min.Z - ray.Position.Z) / ray.Direction.Z;
            float tzmax = (Max.Z - ray.Position.Z) / ray.Direction.Z;

            if (tzmin > tzmax)
                swap(ref tzmin, ref tzmax);

            if ((txmin > tzmax) || (tzmin > txmax))
                return null;

            if (tzmin > txmin)
                txmin = tzmin;

            if (tzmax < txmax)
                txmax = tzmax;

            return (float)Math.Sqrt(txmin * txmin + tymin * tymin + tzmin * tzmin); //TODO: verify?
        }

        public Vector3[] GetCorners()
        {
            return new Vector3[]
            { new Vector3(Min.X, Max.Y, Max.Z), Max, new Vector3(Max.X, Min.Y, Max.Z), new Vector3(Min.X, Min.Y, Max.Z),
                new Vector3(Min.X, Max.Y, Min.Z), Max, new Vector3(Max.X, Min.Y, Min.Z), new Vector3(Min.X, Min.Y, Min.Z),
            };//TODO: verify?
        }

        public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
        {
            float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue, maxZ = float.MinValue;
            foreach (Vector3 point in points)
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

        public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
        {
            result = new BoundingBox(Math.Min(original.Min.X, additional.Min.X),
                Math.Min(original.Min.Y, additional.Min.Y),
                Math.Min(original.Min.Z, additional.Min.Z),
                Math.Max(original.Max.X, additional.Max.X),
                Math.Max(original.Max.Y, additional.Max.Y),
                Math.Max(original.Max.Z, additional.Max.Z));
        }

        public static BoundingBox CreateMerged(params BoundingBox[] boundingBoxes)
        {
            return CreateMerged(boundingBoxes);
        }

        public static BoundingBox CreateMerged(IEnumerable<BoundingBox> boundingBoxes)
        {
            float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue, maxZ = float.MinValue;
            foreach (BoundingBox box in boundingBoxes)
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

