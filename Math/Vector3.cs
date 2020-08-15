using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Fast = System.Numerics;
namespace engenious
{
    /// <summary>
    /// Defines a 3D float Vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [System.ComponentModel.TypeConverter(typeof(Vector3Converter))]
    public unsafe struct Vector3 : IEquatable<Vector3>
    {
        /// <summary>
        /// Gets or sets the x component.
        /// </summary>
        public float X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// Gets or sets the y component.
        /// </summary>
        public float Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// Gets or sets the z component.
        /// </summary>
        public float Z
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3"/> struct with its components set to a scalar.
        /// </summary>
        /// <param name="w">The scalar to set the components to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3(float w)
        {
            X = w;
            Y = w;
            Z = w;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="val">A <see cref="Vector2"/> to take the x and y component from.</param>
        /// <param name="z">The z component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3(Vector2 val, float z)
        {
            X = val.X;
            Y = val.Y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3"/> struct.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3(float x, float y, float z = 0.0f)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Calculates the dot product with another <see cref="Vector3"/>.
        /// </summary>
        /// <param name="value2">The <see cref="Vector3"/> to create the dot product with.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vector3 value2)
        {
            return Dot(this, value2);
        }

        /// <summary>
        /// Calculates the cross product between this <see cref="Vector3"/> and another <see cref="Vector3"/>.
        /// </summary>
        /// <returns>The resulting orthogonal <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Cross(Vector3 value2)
        {
            return Cross(this, value2);
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (float)Math.Sqrt(LengthSquared); }
        }

        /// <summary>
        /// Gets the squared length.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public float LengthSquared => Dot(this, this);

        /// <summary>
        /// Normalizes this <see cref="Vector3"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            this /= Length;
        }

        
        /// <summary>
        /// Gets the normalized <see cref="Vector3"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Normalized()
        {
            return this / Length;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return
                $"[{X.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {Y.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {Z.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}]";
        }

        #region IEquatable implementation

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (obj is Vector3)
                return Equals((Vector3)obj);
            return false;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        #endregion

        /// <summary>
        /// Tests two <see cref="Vector3"/> structs for equality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector3"/> to test with.</param>
        /// <returns><c>true</c> if the vectors are equal; otherwise <c>false</c>.</returns>     
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3 value1, Vector3 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z;
        }

        /// <summary>
        /// Tests two <see cref="Vector3"/> structs for inequality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector3"/> to test with.</param>
        /// <returns><c>true</c> if the vectors aren't equal; otherwise <c>false</c>.</returns>  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3 value1, Vector3 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3"/>.</param>
        /// <param name="value2">The second <see cref="Vector3"/>.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator +(Vector3 value1, Vector3 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;

            Fast.Vector3 res= (*(Fast.Vector3*)tempVal1 + *(Fast.Vector3*)tempVal2);

            var tempRes = &res;
            return *(Vector3*)tempRes;
#else
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
#endif
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The subtrahend <see cref="Vector3"/>.</param>
        /// <param name="value2">The minuend <see cref="Vector3"/>.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(Vector3 value1, Vector3 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;

            Fast.Vector3 res= (*(Fast.Vector3*)tempVal1 - *(Fast.Vector3*)tempVal2);

            var tempRes = &res;
            return *(Vector3*)tempRes;
#else
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
#endif
        }

        /// <summary>
        /// Negates a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vector3"/> to negate.</param>
        /// <returns>The negated <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(Vector3 value)
        {
#if USE_SIMD
            var tempVal = &value;

            Fast.Vector3 res= (-*(Fast.Vector3*)tempVal);

            var tempRes = &res;
            return *(Vector3*)tempRes;
#else
            value.X = -value.X;
            value.Y = -value.Y;
            value.Z = -value.Z;
            return value;
#endif
        }

        /// <summary>
        /// Multiplies a <see cref="Vector3"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector3"/>.</param>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <returns>The scaled <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Vector3 value, float scalar)
        {
#if USE_SIMD
            var tempVal = &value;

            Fast.Vector3 res= (scalar * *(Fast.Vector3*)tempVal);

            var tempRes = &res;
            return *(Vector3*)tempRes;
#else
            value.X *= scalar;
            value.Y *= scalar;
            value.Z *= scalar;
            return value;
#endif
        }

        /// <summary>
        /// Multiplies a <see cref="Vector3"/> by a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <param name="value">The <see cref="Vector3"/>.</param>
        /// <returns>The scaled <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(float scalar, Vector3 value)
        {
            return value * scalar;
        }

        /// <summary>
        /// Multiplies two vectors componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3"/>.</param>
        /// <param name="value2">The second <see cref="Vector3"/>.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Vector3 value1, Vector3 value2)//TODO: ugly as hell
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 =  &value2;

            Fast.Vector3 res= (*(Fast.Vector3*)tempVal1 * *(Fast.Vector3*)tempVal2);

            var tempRes = &res;
            return *(Vector3*)tempRes;
#else
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
#endif
        }

        /// <summary>
        /// Divides a <see cref="Vector3"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector3"/>.</param>
        /// <param name="scalar">The scalar to divide by.</param>
        /// <returns>The scaled down <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(Vector3 value, float scalar)
        {
#if USE_SIMD
            var tempVal = &value;

            Fast.Vector3 res= (*(Fast.Vector3*)tempVal / scalar);

            var tempRes = &res;
            return *(Vector3*)tempRes;
#else
            value.X /= scalar;
            value.Y /= scalar;
            value.Z /= scalar;
            return value;
#endif
        }

        /// <summary>
        /// Divides two vectors componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3"/>.</param>
        /// <param name="value2">The second <see cref="Vector3"/>.</param>
        /// <returns>The resulting <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(Vector3 value1, Vector3 value2)//TODO: ugly as hell?
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;

            Fast.Vector3 res= (*(Fast.Vector3*)tempVal1 / *(Fast.Vector3*)tempVal2);

            var tempRes = &res;
            return *(Vector3*)tempRes;
#else
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            return value1;
#endif
        }

        /// <summary>
        /// Calculates the cross product between two vectors.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns>A <see cref="Vector3"/> orthogonal to both input vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Cross(Vector3 value1, Vector3 value2)
        {
#if USE_SIMD
            Fast.Vector3 a1 = new Fast.Vector3(value1.Y,value1.Z,value1.X);
            Fast.Vector3 a2 = new Fast.Vector3(value2.Z,value2.X,value2.Y);
            Fast.Vector3 b1 = new Fast.Vector3(value1.Z,value1.X,value1.Y);
            Fast.Vector3 b2 = new Fast.Vector3(value2.Y,value2.Z,value2.X);

            Fast.Vector3 res = a1*a2 - b1*b2;

            var tempRes = &res;
            return *(Vector3*)tempRes;
#else
            return new Vector3(value1.Y * value2.Z - value1.Z * value2.Y,
                value1.Z * value2.X - value1.X * value2.Z,
                value1.X * value2.Y - value1.Y * value2.X);
#endif
        }

        /// <summary>
        /// Clamps a <see cref="Vector3"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector3"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <returns>The resulting clamped <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            return Min(Max(min, value), max);
        }

        /// <summary>
        /// Clamps a <see cref="Vector3"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector3"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <param name="output">The resulting clamped <see cref="Vector3"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp(Vector3 value, Vector3 min, Vector3 max, out Vector3 output)
        {
            output = Min(Max(min, value), max);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3"/>.</param>
        /// <param name="value2">The second <see cref="Vector3"/>.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector3 value1, Vector3 value2)
        {
            var res = value1 * value2;
            return res.X + res.Y + res.Z;
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3"/>.</param>
        /// <param name="value2">The second <see cref="Vector3"/>.</param>
        /// <returns>The resulting distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector3 value1, Vector3 value2)
        {
            return (value1 - value2).Length;
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3"/>.</param>
        /// <param name="value2">The second <see cref="Vector3"/>.</param>
        /// <returns>The resulting squared distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Vector3 value1, Vector3 value2)
        {
            return (value1 - value2).LengthSquared;
        }

        /// <summary>
        /// Lerps between two vectors using a given <paramref name="amount"/>.
        /// </summary>
        /// <param name="value1">The <see cref="Vector3"/> to lerp from.</param>
        /// <param name="value2">The <see cref="Vector3"/> to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>The resulting interpolated <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        /// <summary>
        /// Gets the componentwise maximum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3"/>.</param>
        /// <param name="value2">The second <see cref="Vector3"/>.</param>
        /// <returns>The componentwise maximum <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;

            Fast.Vector3 res= Fast.Vector3.Max(*(Fast.Vector3*)tempVal1,*(Fast.Vector3*)tempVal2);
            
            var tempRes = &res;
            return *(Vector3*)tempRes;
#else
            return new Vector3(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y), Math.Max(value1.Z, value2.Z));
#endif
        }

        /// <summary>
        /// Gets the componentwise minimum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3"/>.</param>
        /// <param name="value2">The second <see cref="Vector3"/>.</param>
        /// <returns>The componentwise minimum <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;

            Fast.Vector3 res = Fast.Vector3.Min(*(Fast.Vector3*)tempVal1,*(Fast.Vector3*)tempVal2);

            var tempRes = &res;
            return *(Vector3*)tempRes;
#else
            return new Vector3(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y), Math.Min(value1.Z, value2.Z));
#endif
        }

        /// <summary>
        /// Reflects a <see cref="Vector3"/> by a given normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to reflect.</param>
        /// <param name="normal">The normal indicating a surface to reflect off from.</param>
        /// <returns>The reflected <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            normal.Normalize();
            return 2 * (normal.Dot(vector) * normal - vector); //TODO: normalize normal?
        }

        /// <summary>
        /// Transforms a <see cref="Vector3"/> by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="position">The <see cref="Vector3"/> to transform.</param>
        /// <returns>The transformed <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Transform(Matrix matrix, Vector3 position)
        {
            return new Vector3(position.X * matrix.M11 + position.Y * matrix.M12 + position.Z * matrix.M13 + matrix.M14,
                position.X * matrix.M21 + position.Y * matrix.M22 + position.Z * matrix.M23 + matrix.M24,
                position.X * matrix.M31 + position.Y * matrix.M32 + position.Z * matrix.M33 + matrix.M34);
        }
        
        /// <summary>
        /// Transforms multiple vectors by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="count">The count of vectors to transform.</param>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="positions">A pointer to the vectors to transform.</param>
        /// <param name="output">A pointer to write the resulting vectors to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Transform(int count, ref Matrix matrix, Vector3* positions, Vector3* output)
        {
            for (var i = 0; i < count; i++, positions++, output++)
            {
                *output = Transform(matrix, *positions);
            }
        }

        /// <summary>
        /// Transforms multiple vectors by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="positions">The vectors to transform.</param>
        /// <param name="output">An array to write the resulting vectors to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ref Matrix matrix, Vector3[] positions, Vector3[] output)
        {
            var index = 0;
            foreach (var position in positions)
            {
                output[index++] = Transform(matrix, position);
            }
        }

        /// <summary>
        /// Rotates a given <see cref="Vector3"/> by a given <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="position">The <see cref="Vector3"/> to rotate.</param>
        /// <param name="quaternion">The <see cref="Quaternion"/> to rotate by.</param>
        /// <returns>The rotated <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Transform(Vector3 position, Quaternion quaternion)
        {
            return Transform(Matrix.CreateFromQuaternion(quaternion), position); //TODO: directly transform
        }

        /// <summary>
        /// A <see cref="Vector3"/> with all its components set to 1.
        /// </summary>
        public static readonly Vector3 One = new Vector3(1, 1, 1);

        /// <summary>
        /// A <see cref="Vector3"/> with all its components set to 0.
        /// </summary>
        public static readonly Vector3 Zero;

        /// <summary>
        /// A <see cref="Vector3"/> with its x component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector3 UnitX = new Vector3(1, 0);

        /// <summary>
        /// A <see cref="Vector3"/> with its y component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector3 UnitY = new Vector3(0, 1);

        /// <summary>
        /// A <see cref="Vector3"/> with its z component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);
    }
}

