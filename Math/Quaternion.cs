using System;
using System.Runtime.InteropServices;

namespace engenious
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Quaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;


        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Quaternion operator +(Quaternion val1, Quaternion val2)
        {
            val1.X += val2.X;
            val1.Y += val2.Y;
            val1.Z += val2.Z;
            val1.W += val2.W;
            return val1;
        }

        public static Quaternion operator*(Quaternion val, float scale)
        {
            return new Quaternion(val.X * scale, val.Y * scale, val.Z * scale, val.W * scale);
        }

        public static Quaternion operator*(Quaternion val1, Quaternion val2)
        {
            OpenTK.Quaternion q;

            Quaternion result = new Quaternion();
            result.W = val1.W * val2.W - val1.X * val2.X - val1.Y * val2.Y - val1.Z * val2.Z;
            result.X = val1.W * val2.X + val1.X * val2.W + val1.Y * val2.Z - val1.Z * val2.Y;
            result.Y = val1.W * val2.Y + val1.Y * val2.W + val1.X * val2.Z - val1.Z * val2.X;
            result.Z = val1.W * val2.Z + val1.Z * val2.W + val1.X * val2.Y - val1.Y * val2.X;
            return result;
        }
    }
}

