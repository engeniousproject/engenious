using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Fast = System.Numerics;
// ReSharper disable CompareOfFloatByEqualityOperator
namespace engenious
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [System.ComponentModel.TypeConverter(typeof(Vector4dConverter))]
    public unsafe struct Vector4d
    {
        public double X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }
        public double Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }
        public double Z
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }
        public double W
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d(double val)
        {
            X = val;
            Y = val;
            Z = val;
            W = val;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d(Vector2d val, double z, double w)
        {
            X = val.X;
            Y = val.Y;
            Z = z;
            W = w;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d(Vector3d val, double w)
        {
            X = val.X;
            Y = val.Y;
            Z = val.Z;
            W = w;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Dot(Vector4d value2)
        {
            return Dot(this, value2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Cross(Vector4d value2, Vector4d value3)
        {
            return Cross(this, value2, value3);
        }
        [System.ComponentModel.Browsable(false)]
        public double Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (double)Math.Sqrt(LengthSquared); }
        }

        [System.ComponentModel.Browsable(false)]
        public double LengthSquared => Dot(this, this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            this /= Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Normalized()
        {
            return this / Length;
        }


        #region IEquatable implementation
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (obj is Vector4)
                return Equals((Vector4)obj);
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4d other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        #endregion
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4d value1, Vector4d value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4d value1, Vector4d value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z || value1.W != value2.W;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator +(Vector4d value1, Vector4d value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector4d res = *(Fast.Vector4d*)tempVal1 + *(Fast.Vector4d*)tempVal2;

            var tempRes = &res;
            return *(Vector4d*)tempRes;
#else
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            value1.W += value2.W;
            return value1;
#endif
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator -(Vector4d value1, Vector4d value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;

            Fast.Vector4d res = *(Fast.Vector4d*)tempVal1 - *(Fast.Vector4d*)tempVal2;

            var tempRes = &res;
            return *(Vector4d*)tempRes;
#else
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            value1.W -= value2.W;
            return value1;
#endif
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator -(Vector4d value)
        {
#if USE_SIMD
            var tempVal = &value;
            Fast.Vector4d res = -*(Fast.Vector4d*)tempVal;

            var tempRes = &res;
            return *(Vector4d*)tempRes;
#else
            value.X = -value.X;
            value.Y = -value.Y;
            value.Z = -value.Z;
            value.W = -value.W;
            return value;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator *(Vector4d value, double scalar)
        {
#if USE_SIMD
            var tempVal = &value;
            Fast.Vector4d res = *(Fast.Vector4d*)tempVal * scalar;

            var tempRes = &res;
            return *(Vector4d*)tempRes;
#else
            value.X *= scalar;
            value.Y *= scalar;
            value.Z *= scalar;
            value.W *= scalar;
            return value;
#endif
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator *(double scalar, Vector4d value)
        {
            return value * scalar;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator *(Vector4d value1, Vector4d value2)//TODO: ugly as hell
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector4d res = *(Fast.Vector4d*)tempVal1 * *(Fast.Vector4d*)tempVal2;

            var tempRes = &res;
            return *(Vector4d*)tempRes;
#else
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            value1.W *= value2.W;
            return value1;
#endif
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator /(Vector4d value, double scalar)
        {
#if USE_SIMD
            var tempVal = &value;
            Fast.Vector4d res = *(Fast.Vector4d*)tempVal / scalar;

            var tempRes = &res;
            return *(Vector4d*)tempRes;
#else
            value.X /= scalar;
            value.Y /= scalar;
            value.Z /= scalar;
            value.W /= scalar;
            return value;
#endif
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator /(Vector4d value1, Vector4d value2)//TODO: ugly as hell?
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector4d res = *(Fast.Vector4d*)tempVal1 / *(Fast.Vector4d*)tempVal2;

            var tempRes = &res;
            return *(Vector4d*)tempRes;
#else
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            value1.W /= value2.W;
            return value1;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Cross(Vector4d value1, Vector4d value2, Vector4d value3)
        {
            throw new NotImplementedException();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Clamp(Vector4d value, Vector4d min, Vector4d max)
        {
            return Min(Max(min, value), max);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp(Vector4d value, Vector4d min, Vector4d max, out Vector4d output)
        {
            output = Min(Max(min, value), max);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(Vector4d value1, Vector4d value2)
        {
            var res = value1 * value2;
            return res.X + res.Y + res.Z + res.W;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Vector4d value1, Vector4d value2)
        {
            return (value1 - value2).Length;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceSquared(Vector4d value1, Vector4d value2)
        {
            return (value1 - value2).LengthSquared;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Lerp(Vector4d value1, Vector4d value2, double amount)
        {
            return value1 + (value2 - value1) * amount;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Max(Vector4d value1, Vector4d value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector4d res = Fast.Vector4d.Max(*(Fast.Vector4d*)tempVal1,*(Fast.Vector4d*)tempVal2);

            var tempRes = &res;
            return *(Vector4d*)tempRes;
#else
            return new Vector4d(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y), Math.Max(value1.Z, value2.Z), Math.Max(value1.W, value2.W));
#endif
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Min(Vector4d value1, Vector4d value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector4d res = Fast.Vector4d.Min(*(Fast.Vector4d*)tempVal1,*(Fast.Vector4d*)tempVal2);

            var tempRes = &res;
            return *(Vector4d*)tempRes;
#else
            return new Vector4d(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y), Math.Min(value1.Z, value2.Z), Math.Min(value1.W, value2.W));
#endif
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Reflect(Vector4d vector, Vector4d normal)
        {
            normal.Normalize();
            return 2 * (normal.Dot(vector) * normal - vector); //TODO: normalize normal?
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Transform(Vector4d position, Quaternion quaternion)
        {
            return Transform(position, Matrix.CreateFromQuaternion(quaternion));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Transform(Vector4d position, Matrix matrix)
        {

            return new Vector4d(position.X * matrix.M11 + position.Y * matrix.M12 + position.Z * matrix.M13 + position.W * matrix.M14,
                position.X * matrix.M21 + position.Y * matrix.M22 + position.Z * matrix.M23 + position.W * matrix.M24,
                position.X * matrix.M31 + position.Y * matrix.M32 + position.Z * matrix.M33 + position.W * matrix.M34,
                position.X * matrix.M41 + position.Y * matrix.M42 + position.Z * matrix.M43 + position.W * matrix.M44);
        }

        public static readonly Vector4d One = new Vector4d(1, 1, 1, 1);
        public static readonly Vector4d Zero;

        public static readonly Vector4d UnitX = new Vector4d(1, 0, 0, 0);
        public static readonly Vector4d UnitY = new Vector4d(0, 1, 0, 0);
        public static readonly Vector4d UnitZ = new Vector4d(0, 0, 1, 0);
        public static readonly Vector4d UnitW = new Vector4d(0, 0, 0, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ W.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return
                $"[{X.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {Y.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {Z.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}]";
        }

    }
}
