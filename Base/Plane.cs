using System;
using OpenTK;

namespace engenious
{
    public struct Plane
    {
        public Vector3 D;
        public Vector3 Normal;

        public Plane(float a, float b, float c, float d)
        {
            Normal = new Vector3(a, b, c);//TODO: verify?
            float p = Math.Abs(d) / Normal.Length;
            D = p * Normal;
        }
    }
}

