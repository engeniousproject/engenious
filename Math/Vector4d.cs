using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Fast = System.Numerics;
// ReSharper disable CompareOfFloatByEqualityOperator
namespace engenious
{
    /// <summary>
    /// Defines a 4D double Vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [System.ComponentModel.TypeConverter(typeof(Vector4dConverter))]
    public unsafe struct Vector4d : IEquatable<Vector4d>
    {
        /// <summary>
        /// Gets or sets the x component.
        /// </summary>
        public double X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// Gets or sets the y component.
        /// </summary>
        public double Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// Gets or sets the z component.
        /// </summary>
        public double Z
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// Gets or sets the w component.
        /// </summary>
        public double W
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector4d"/> struct with its components set to a scalar.
        /// </summary>
        /// <param name="val">The scalar to set the components to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d(double val)
        {
            X = val;
            Y = val;
            Z = val;
            W = val;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector4d"/> struct.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        /// <param name="w">The w component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        
        /// <summary>
        /// Initializes a new <see cref="Vector4d"/> struct.
        /// </summary>
        /// <param name="val">A <see cref="Vector2d"/> to take the x and y component from.</param>
        /// <param name="z">The z component.</param>
        /// <param name="w">The w component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d(Vector2d val, double z, double w)
        {
            X = val.X;
            Y = val.Y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector4d"/> struct.
        /// </summary>
        /// <param name="val">A <see cref="Vector3d"/> to take the x, y and z component from.</param>
        /// <param name="w">The w component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d(Vector3d val, double w)
        {
            X = val.X;
            Y = val.Y;
            Z = val.Z;
            W = w;
        }

        /// <summary>
        /// Calculates the dot product with another <see cref="Vector4d"/>.
        /// </summary>
        /// <param name="value2">The <see cref="Vector4d"/> to create the dot product with.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Dot(Vector4d value2)
        {
            return Dot(this, value2);
        }

        /// <summary>
        /// Calculates the cross product between this <see cref="Vector4d"/> and another <see cref="Vector4d"/>.
        /// </summary>
        /// <returns>The resulting orthogonal <see cref="Vector4d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Cross(Vector4d value2, Vector4d value3)
        {
            return Cross(this, value2, value3);
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public double Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (double)Math.Sqrt(LengthSquared); }
        }

        /// <summary>
        /// Gets the squared length.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public double LengthSquared => Dot(this, this);

        /// <summary>
        /// Normalizes this <see cref="Vector4d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            this /= Length;
        }
        
        /// <summary>
        /// Gets the normalized <see cref="Vector4d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4d Normalized()
        {
            return this / Length;
        }


        #region IEquatable implementation

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (obj is Vector4)
                return Equals((Vector4)obj);
            return false;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4d other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        #endregion

        /// <summary>
        /// Tests two <see cref="Vector4d"/> structs for equality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4d"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector4d"/> to test with.</param>
        /// <returns><c>true</c> if the vectors are equal; otherwise <c>false</c>.</returns>     
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4d value1, Vector4d value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W;
        }

        /// <summary>
        /// Tests two <see cref="Vector4"/> structs for inequality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4d"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector4d"/> to test with.</param>
        /// <returns><c>true</c> if the vectors aren't equal; otherwise <c>false</c>.</returns>  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4d value1, Vector4d value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z || value1.W != value2.W;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4d"/>.</param>
        /// <param name="value2">The second <see cref="Vector4d"/>.</param>
        /// <returns>The resulting <see cref="Vector4d"/>.</returns>
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

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The subtrahend <see cref="Vector4d"/>.</param>
        /// <param name="value2">The minuend <see cref="Vector4d"/>.</param>
        /// <returns>The resulting <see cref="Vector4d"/>.</returns>
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

        /// <summary>
        /// Negates a <see cref="Vector4d"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vector4d"/> to negate.</param>
        /// <returns>The negated <see cref="Vector4d"/>.</returns>
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

        /// <summary>
        /// Multiplies a <see cref="Vector4d"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector4d"/>.</param>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <returns>The scaled <see cref="Vector4d"/>.</returns>
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

        /// <summary>
        /// Multiplies a <see cref="Vector4d"/> by a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <param name="value">The <see cref="Vector4d"/>.</param>
        /// <returns>The scaled <see cref="Vector4d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d operator *(double scalar, Vector4d value)
        {
            return value * scalar;
        }

        /// <summary>
        /// Multiplies two vectors componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4d"/>.</param>
        /// <param name="value2">The second <see cref="Vector4d"/>.</param>
        /// <returns>The resulting <see cref="Vector4d"/>.</returns>
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

        /// <summary>
        /// Divides a <see cref="Vector4d"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector4d"/>.</param>
        /// <param name="scalar">The scalar to divide by.</param>
        /// <returns>The scaled down <see cref="Vector4d"/>.</returns>
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

        /// <summary>
        /// Divides two vectors componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4d"/>.</param>
        /// <param name="value2">The second <see cref="Vector4d"/>.</param>
        /// <returns>The resulting <see cref="Vector4d"/>.</returns>
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

        /// <summary>
        /// Calculates the cross product between 3 vectors.
        /// </summary>
        /// <param name="value1">The first operand.</param>
        /// <param name="value2">The second operand.</param>
        /// <param name="value3">The third operand</param>
        /// <returns>The resulting cross product.</returns>
        /// <exception cref="NotImplementedException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Cross(Vector4d value1, Vector4d value2, Vector4d value3)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clamps a <see cref="Vector4d"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector4d"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <returns>The resulting clamped <see cref="Vector4d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Clamp(Vector4d value, Vector4d min, Vector4d max)
        {
            return Min(Max(min, value), max);
        }

        /// <summary>
        /// Clamps a <see cref="Vector4d"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector4d"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <param name="output">The resulting clamped <see cref="Vector4d"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp(Vector4d value, Vector4d min, Vector4d max, out Vector4d output)
        {
            output = Min(Max(min, value), max);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4d"/>.</param>
        /// <param name="value2">The second <see cref="Vector4d"/>.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(Vector4d value1, Vector4d value2)
        {
            var res = value1 * value2;
            return res.X + res.Y + res.Z + res.W;
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4d"/>.</param>
        /// <param name="value2">The second <see cref="Vector4d"/>.</param>
        /// <returns>The resulting distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Vector4d value1, Vector4d value2)
        {
            return (value1 - value2).Length;
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4d"/>.</param>
        /// <param name="value2">The second <see cref="Vector4d"/>.</param>
        /// <returns>The resulting squared distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceSquared(Vector4d value1, Vector4d value2)
        {
            return (value1 - value2).LengthSquared;
        }

        /// <summary>
        /// Lerps between two vectors using a given <paramref name="amount"/>.
        /// </summary>
        /// <param name="value1">The <see cref="Vector4d"/> to lerp from.</param>
        /// <param name="value2">The <see cref="Vector4d"/> to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>The resulting interpolated <see cref="Vector4d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Lerp(Vector4d value1, Vector4d value2, double amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        /// <summary>
        /// Gets the componentwise maximum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4d"/>.</param>
        /// <param name="value2">The second <see cref="Vector4d"/>.</param>
        /// <returns>The componentwise maximum <see cref="Vector4d"/>.</returns>
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

        /// <summary>
        /// Gets the componentwise minimum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4d"/>.</param>
        /// <param name="value2">The second <see cref="Vector4d"/>.</param>
        /// <returns>The componentwise minimum <see cref="Vector4d"/>.</returns>
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

        /// <summary>
        /// Reflects a <see cref="Vector4d"/> by a given normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4d"/> to reflect.</param>
        /// <param name="normal">The normal indicating a surface to reflect off from.</param>
        /// <returns>The reflected <see cref="Vector4d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Reflect(Vector4d vector, Vector4d normal)
        {
            normal.Normalize();
            return 2 * (normal.Dot(vector) * normal - vector); //TODO: normalize normal?
        }
        
        /// <summary>
        /// Transforms multiple vectors by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="count">The count of vectors to transform.</param>
        /// <param name="positions">A pointer to the vectors to transform.</param>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="output">A pointer to write the resulting vectors to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Transform(int count, Vector4d* positions, ref Matrix matrix, Vector4d* output)
        {
            for (var i = 0; i < count; i++, positions++, output++)
            {
                *output = new Vector4d(
                    (*positions).X * matrix.M11 + (*positions).Y * matrix.M12 + (*positions).Z * matrix.M13 + (*positions).W * matrix.M14,
                    (*positions).X * matrix.M21 + (*positions).Y * matrix.M22 + (*positions).Z * matrix.M23 + (*positions).W * matrix.M24,
                    (*positions).X * matrix.M31 + (*positions).Y * matrix.M32 + (*positions).Z * matrix.M33 + (*positions).W * matrix.M34,
                    (*positions).X * matrix.M41 + (*positions).Y * matrix.M42 + (*positions).Z * matrix.M43 + (*positions).W * matrix.M44);//TODO: SIMD
            }
        }

        /// <summary>
        /// Transforms multiple vectors by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="positions">The vectors to transform.</param>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="output">An array to write the resulting vectors to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Vector4d[] positions, ref Matrix matrix, Vector4d[] output)
        {
            var index = 0;
            foreach (var position in positions)//TODO: SIMD
            {
                output[index++] = new Vector4d(position.X * matrix.M11 + position.Y * matrix.M12 + position.Z * matrix.M13 + position.W * matrix.M14,
                    position.X * matrix.M21 + position.Y * matrix.M22 + position.Z * matrix.M23 + position.W * matrix.M24,
                    position.X * matrix.M31 + position.Y * matrix.M32 + position.Z * matrix.M33 + position.W * matrix.M34,
                    position.X * matrix.M41 + position.Y * matrix.M42 + position.Z * matrix.M43 + position.W * matrix.M44);
            }
        }

        /// <summary>
        /// Rotates a given <see cref="Vector4d"/> by a given <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="position">The <see cref="Vector4d"/> to rotate.</param>
        /// <param name="quaternion">The <see cref="Quaternion"/> to rotate by.</param>
        /// <returns>The rotated <see cref="Vector4d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Transform(Vector4d position, Quaternion quaternion)
        {
            return Transform(position, Matrix.CreateFromQuaternion(quaternion));
        }

        /// <summary>
        /// Transforms a <see cref="Vector4d"/> by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">The <see cref="Vector4d"/> to transform.</param>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <returns>The transformed <see cref="Vector4d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4d Transform(Vector4d position, Matrix matrix)
        {

            return new Vector4d(position.X * matrix.M11 + position.Y * matrix.M12 + position.Z * matrix.M13 + position.W * matrix.M14,
                position.X * matrix.M21 + position.Y * matrix.M22 + position.Z * matrix.M23 + position.W * matrix.M24,
                position.X * matrix.M31 + position.Y * matrix.M32 + position.Z * matrix.M33 + position.W * matrix.M34,
                position.X * matrix.M41 + position.Y * matrix.M42 + position.Z * matrix.M43 + position.W * matrix.M44);
        }

        /// <summary>
        /// A <see cref="Vector4d"/> with all its components set to 1.
        /// </summary>
        public static readonly Vector4d One = new Vector4d(1, 1, 1, 1);

        /// <summary>
        /// A <see cref="Vector4d"/> with all its components set to 0.
        /// </summary>
        public static readonly Vector4d Zero;

        /// <summary>
        /// A <see cref="Vector4d"/> with its x component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector4d UnitX = new Vector4d(1, 0, 0, 0);

        /// <summary>
        /// A <see cref="Vector4d"/> with its y component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector4d UnitY = new Vector4d(0, 1, 0, 0);

        /// <summary>
        /// A <see cref="Vector4d"/> with its z component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector4d UnitZ = new Vector4d(0, 0, 1, 0);

        /// <summary>
        /// A <see cref="Vector4d"/> with its w component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector4d UnitW = new Vector4d(0, 0, 0, 1);

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override string ToString()
        {
            return
                $"[{X.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {Y.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {Z.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}]";
        }

    }
}
