using System;
using OpenTK;

namespace engenious
{
    public struct Ray
    {
        public Vector3 Direction;
        public Vector3 Position;

        public Ray(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }

        public float? Intersects(BoundingBox box)
        {
            return box.Intersects(this);   
        }

        public static bool operator ==(Ray ray1, Ray ray2)
        {
            return ray1.Position == ray2.Position && ray1.Direction.Normalized() == ray2.Direction.Normalized();//TODO: really normalized
        }

        public static bool operator !=(Ray ray1, Ray ray2)
        {
            return ray1.Position != ray2.Position || ray1.Direction.Normalized() != ray2.Direction.Normalized();
        }

        public override int GetHashCode()
        {
            return Direction.GetHashCode() ^ Position.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Ray)
            {
                return (Ray)obj == this;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("[Position: {0},Direction: {1}]", Position.ToString(), Direction.ToString());
        }
    }
}

