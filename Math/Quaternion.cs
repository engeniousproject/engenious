using System;
using System.Runtime.InteropServices;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace engenious
{
    /// <summary>
    /// Defines a Quaternion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        /// <inheritdoc />
        public bool Equals(Quaternion other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is Quaternion quaternion && Equals(quaternion);
        }

        /// <inheritdoc />
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

        /// <summary>
        /// The x-component.
        /// </summary>
        public float X;
        /// <summary>
        /// The y-component.
        /// </summary>
        public float Y;
        /// <summary>
        /// The z-component.
        /// </summary>
        public float Z;
        /// <summary>
        /// The w-component.
        /// </summary>
        public float W;

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct from a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">Rotation <see cref="Matrix"/> to convert to a <see cref="Quaternion"/>.</param>
        public Quaternion(Matrix matrix)
        {
            //matrix.Transpose();
            var tr = matrix.M11 + matrix.M22 + matrix.M33;

            if (tr > 0)
            { 
                var s = (float)(Math.Sqrt(tr + 1.0f) * 2); // S=4*qw
                W = 0.25f * s;
                X = (matrix.M32 - matrix.M23) / s;
                Y = (matrix.M13 - matrix.M31) / s;
                Z = (matrix.M21 - matrix.M12) / s;
            }
            else if ((matrix.M11 > matrix.M22) & (matrix.M11 > matrix.M33))
            { 
                var s = (float)(Math.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33) * 2); // S=4*qx
                W = (matrix.M32 - matrix.M23) / s;
                X = 0.25f * s;
                Y = (matrix.M12 + matrix.M21) / s;
                Z = (matrix.M13 + matrix.M31) / s;
            }
            else if (matrix.M22 > matrix.M33)
            { 
                var s = (float)(Math.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33) * 2); // S=4*qy
                W = (matrix.M13 - matrix.M31) / s;
                X = (matrix.M12 + matrix.M21) / s;
                Y = 0.25f * s;
                Z = (matrix.M23 + matrix.M32) / s;
            }
            else
            { 
                var s = (float)(Math.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22) * 2); // S=4*qz
                W = (matrix.M21 - matrix.M12) / s;
                X = (matrix.M13 + matrix.M31) / s;
                Y = (matrix.M23 + matrix.M32) / s;
                Z = 0.25f * s;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="x">The x-component.</param>
        /// <param name="y">The y-component.</param>
        /// <param name="z">The z-component.</param>
        /// <param name="w">The w-component.</param>
        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="val1">The first <see cref="Quaternion"/>.</param>
        /// <param name="val2">The second <see cref="Quaternion"/>.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion operator +(Quaternion val1, Quaternion val2)
        {
            val1.X += val2.X;
            val1.Y += val2.Y;
            val1.Z += val2.Z;
            val1.W += val2.W;
            return val1;
        }

        /// <summary>
        /// Scales a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="val">The <see cref="Quaternion"/>.</param>
        /// <param name="scale">The scalar to scale by.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion operator*(Quaternion val, float scale)
        {
            return new Quaternion(val.X * scale, val.Y * scale, val.Z * scale, val.W * scale);
        }

        /// <summary>
        /// Multiplies two quaternions.
        /// </summary>
        /// <param name="val1">The first <see cref="Quaternion"/>.</param>
        /// <param name="val2">The second <see cref="Quaternion"/>.</param>
        /// <returns>The resulting <see cref="Quaternion"/>.</returns>
        public static Quaternion operator*(Quaternion val1, Quaternion val2)
        {
            return new Quaternion(val1.W * val2.X + val1.X * val2.W + val1.Y * val2.Z - val1.Z * val2.Y, val1.W * val2.Y + val1.Y * val2.W + val1.Z * val2.X - val1.X * val2.Z, val1.W * val2.Z + val1.Z * val2.W + val1.X * val2.Y - val1.Y * val2.X, val1.W * val2.W - val1.X * val2.X - val1.Y * val2.Y - val1.Z * val2.Z);
        }

        /// <summary>
        /// Lerps between two quaternions using a given <paramref name="amount"/>.
        /// </summary>
        /// <param name="quaternion1">The <see cref="Quaternion"/> to lerp from.</param>
        /// <param name="quaternion2">The <see cref="Quaternion"/> to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>The resulting interpolated <see cref="Quaternion"/>.</returns>
        public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, float amount)//copied from MonoGame
        {
            var num = amount;
            var num2 = 1f - num;
            var quaternion = new Quaternion();
            var num5 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
            if (num5 >= 0f)
            {
                quaternion.X = (num2 * quaternion1.X) + (num * quaternion2.X);
                quaternion.Y = (num2 * quaternion1.Y) + (num * quaternion2.Y);
                quaternion.Z = (num2 * quaternion1.Z) + (num * quaternion2.Z);
                quaternion.W = (num2 * quaternion1.W) + (num * quaternion2.W);
            }
            else
            {
                quaternion.X = (num2 * quaternion1.X) - (num * quaternion2.X);
                quaternion.Y = (num2 * quaternion1.Y) - (num * quaternion2.Y);
                quaternion.Z = (num2 * quaternion1.Z) - (num * quaternion2.Z);
                quaternion.W = (num2 * quaternion1.W) - (num * quaternion2.W);
            }
            var num4 = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
            if (num4 == 0)
                return quaternion;
            var num3 = (float)(1.0 / Math.Sqrt(num4));
            quaternion.X *= num3;
            quaternion.Y *= num3;
            quaternion.Z *= num3;
            quaternion.W *= num3;
            return quaternion;
        }

        /// <summary>
        /// Converts this <see cref="Quaternion"/> to a rotation <see cref="Matrix"/>.
        /// </summary>
        /// <returns>The created rotation <see cref="Matrix"/>.</returns>
        public Matrix ToMatrix()
        {
            var m=new Matrix();

            var x2 = 2*X*X;
            var y2 = 2*Y*Y;
            var z2 = 2*Z*Z;

            var xy = 2*X*Y;
            var xz = 2*X*Z;
            var xw = 2*X*W;

            var yz = 2*Y*Z;
            var yw = 2*Y*W;

            var zw = 2*Z*W;


            m.M11 = 1 - y2-z2;
            m.M12 = xy-zw;
            m.M13 = xz+ yw;

            m.M21 = xy+zw;
            m.M22 = 1-x2-z2;
            m.M23 = yz-xw;

            m.M31 = xz-yw;
            m.M32 = yz+xw;
            m.M33 = 1-x2-y2;
            //TODO: transpose?
            return m;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return
                $"[{X.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {Y.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {Z.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {W.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}]";
        }

        /// <summary>
        /// Tests two <see cref="Quaternion"/> structs for equality.
        /// </summary>
        /// <param name="q1">The first <see cref="Quaternion"/> to test with.</param>
        /// <param name="q2">The second <see cref="Quaternion"/> to test with.</param>
        /// <returns><c>true</c> if the quaternions are equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Quaternion q1, Quaternion q2)
        {
            return q1.X == q2.X && q1.Y == q2.Y && q1.Z == q2.Z && q1.W == q2.W;
        }

        /// <summary>
        /// Tests two <see cref="Quaternion"/> structs for inequality.
        /// </summary>
        /// <param name="q1">The first <see cref="Quaternion"/> to test with.</param>
        /// <param name="q2">The second <see cref="Quaternion"/> to test with.</param>
        /// <returns><c>true</c> if the quaternions aren't equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Quaternion q1, Quaternion q2)
        {
            return q1.X != q2.X || q1.Y != q2.Y || q1.Z != q2.Z || q1.W != q2.W;
        }

        /// <summary>
        /// The identity <see cref="Quaternion"/>.
        /// </summary>
        public static readonly Quaternion Identity = new Quaternion(0,0,0,1);
    }
}

