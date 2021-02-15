using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Fast = System.Numerics;
// ReSharper disable CompareOfFloatsByEqualityOperator
namespace engenious
{
    /// <summary>
    /// Defines a 4D float Vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [System.ComponentModel.TypeConverter(typeof(Vector4Converter))]
    public unsafe struct Vector4 : IEquatable<Vector4>
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
        /// Gets or sets the w component.
        /// </summary>
        public float W
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector4"/> struct with its components set to a scalar.
        /// </summary>
        /// <param name="val">The scalar to set the components to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4(float val)
        {
            X = val;
            Y = val;
            Z = val;
            W = val;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        /// <param name="w">The w component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="val">A <see cref="Vector2"/> to take the x and y component from.</param>
        /// <param name="z">The z component.</param>
        /// <param name="w">The w component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4(Vector2 val, float z, float w)
        {
            X = val.X;
            Y = val.Y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector4"/> struct.
        /// </summary>
        /// <param name="val">A <see cref="Vector3"/> to take the x, y and z component from.</param>
        /// <param name="w">The w component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4(Vector3 val, float w)
        {
            X = val.X;
            Y = val.Y;
            Z = val.Z;
            W = w;
        }

        /// <summary>
        /// Calculates the dot product with another <see cref="Vector4"/>.
        /// </summary>
        /// <param name="value2">The <see cref="Vector4"/> to create the dot product with.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vector4 value2)
        {
            return Dot(this, value2);
        }

        /// <summary>
        /// Calculates the cross product between this <see cref="Vector4"/> and another <see cref="Vector4"/>.
        /// </summary>
        /// <returns>The resulting orthogonal <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4 Cross(Vector4 value2, Vector4 value3)
        {
            return Cross(this, value2, value3);
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (float)Math.Sqrt(LengthSquared);
        }

        /// <summary>
        /// Gets the squared length.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public float LengthSquared => Dot(this, this);

        /// <summary>
        /// Normalizes this <see cref="Vector4"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            this /= Length;
        }
        
        /// <summary>
        /// Gets the normalized <see cref="Vector4"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4 Normalized()
        {
            return this / Length;
        }


        #region IEquatable implementation

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return (obj is Vector4 vector4) && Equals(vector4);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        #endregion

        /// <summary>
        /// Tests two <see cref="Vector4"/> structs for equality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector4"/> to test with.</param>
        /// <returns><c>true</c> if the vectors are equal; otherwise <c>false</c>.</returns>     
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4 value1, Vector4 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W;
        }

        /// <summary>
        /// Tests two <see cref="Vector4"/> structs for inequality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector4"/> to test with.</param>
        /// <returns><c>true</c> if the vectors aren't equal; otherwise <c>false</c>.</returns>  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4 value1, Vector4 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z || value1.W != value2.W;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4"/>.</param>
        /// <param name="value2">The second <see cref="Vector4"/>.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator +(Vector4 value1, Vector4 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector4 res = *(Fast.Vector4*)tempVal1 + *(Fast.Vector4*)tempVal2;

            var tempRes = &res;
            return *(Vector4*)tempRes;
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
        /// <param name="value1">The subtrahend <see cref="Vector4"/>.</param>
        /// <param name="value2">The minuend <see cref="Vector4"/>.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator -(Vector4 value1, Vector4 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;

            Fast.Vector4 res = *(Fast.Vector4*)tempVal1 - *(Fast.Vector4*)tempVal2;

            var tempRes = &res;
            return *(Vector4*)tempRes;
#else
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            value1.W -= value2.W;
            return value1;
#endif
        }

        /// <summary>
        /// Negates a <see cref="Vector4"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vector4"/> to negate.</param>
        /// <returns>The negated <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator -(Vector4 value)
        {
#if USE_SIMD
            var tempVal = &value;
            Fast.Vector4 res = -*(Fast.Vector4*)tempVal;

            var tempRes = &res;
            return *(Vector4*)tempRes;
#else
            value.X = -value.X;
            value.Y = -value.Y;
            value.Z = -value.Z;
            value.W = -value.W;
            return value;
#endif
        }

        /// <summary>
        /// Multiplies a <see cref="Vector4"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector4"/>.</param>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <returns>The scaled <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator *(Vector4 value, float scalar)
        {
#if USE_SIMD
            var tempVal = &value;
            Fast.Vector4 res = *(Fast.Vector4*)tempVal * scalar;

            var tempRes = &res;
            return *(Vector4*)tempRes;
#else
            value.X *= scalar;
            value.Y *= scalar;
            value.Z *= scalar;
            value.W *= scalar;
            return value;
#endif
        }

        /// <summary>
        /// Multiplies a <see cref="Vector4"/> by a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <param name="value">The <see cref="Vector4"/>.</param>
        /// <returns>The scaled <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator *(float scalar, Vector4 value)
        {
            return value * scalar;
        }

        /// <summary>
        /// Multiplies two vectors componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4"/>.</param>
        /// <param name="value2">The second <see cref="Vector4"/>.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator *(Vector4 value1, Vector4 value2)//TODO: ugly as hell
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector4 res = *(Fast.Vector4*)tempVal1 * *(Fast.Vector4*)tempVal2;

            var tempRes = &res;
            return *(Vector4*)tempRes;
#else
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            value1.W *= value2.W;
            return value1;
#endif
        }

        /// <summary>
        /// Divides a <see cref="Vector4"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector4"/>.</param>
        /// <param name="scalar">The scalar to divide by.</param>
        /// <returns>The scaled down <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator /(Vector4 value, float scalar)
        {
#if USE_SIMD
            var tempVal = &value;
            Fast.Vector4 res = *(Fast.Vector4*)tempVal / scalar;

            var tempRes = &res;
            return *(Vector4*)tempRes;
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
        /// <param name="value1">The first <see cref="Vector4"/>.</param>
        /// <param name="value2">The second <see cref="Vector4"/>.</param>
        /// <returns>The resulting <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator /(Vector4 value1, Vector4 value2)//TODO: ugly as hell?
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector4 res = *(Fast.Vector4*)tempVal1 / *(Fast.Vector4*)tempVal2;

            var tempRes = &res;
            return *(Vector4*)tempRes;
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
        public static Vector4 Cross(Vector4 value1, Vector4 value2, Vector4 value3)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clamps a <see cref="Vector4"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector4"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <returns>The resulting clamped <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max)
        {
            return Min(Max(min, value), max);
        }

        /// <summary>
        /// Clamps a <see cref="Vector4"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector4"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <param name="output">The resulting clamped <see cref="Vector4"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp(Vector4 value, Vector4 min, Vector4 max, out Vector4 output)
        {
            output = Min(Max(min, value), max);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4"/>.</param>
        /// <param name="value2">The second <see cref="Vector4"/>.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector4 value1, Vector4 value2)
        {
            var res = value1 * value2;
            return res.X + res.Y + res.Z + res.W;
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4"/>.</param>
        /// <param name="value2">The second <see cref="Vector4"/>.</param>
        /// <returns>The resulting distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector4 value1, Vector4 value2)
        {
            return (value1 - value2).Length;
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4"/>.</param>
        /// <param name="value2">The second <see cref="Vector4"/>.</param>
        /// <returns>The resulting squared distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Vector4 value1, Vector4 value2)
        {
            return (value1 - value2).LengthSquared;
        }

        /// <summary>
        /// Lerps between two vectors using a given <paramref name="amount"/>.
        /// </summary>
        /// <param name="value1">The <see cref="Vector4"/> to lerp from.</param>
        /// <param name="value2">The <see cref="Vector4"/> to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>The resulting interpolated <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        /// <summary>
        /// Gets the componentwise maximum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4"/>.</param>
        /// <param name="value2">The second <see cref="Vector4"/>.</param>
        /// <returns>The componentwise maximum <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Max(Vector4 value1, Vector4 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector4 res = Fast.Vector4.Max(*(Fast.Vector4*)tempVal1,*(Fast.Vector4*)tempVal2);

            var tempRes = &res;
            return *(Vector4*)tempRes;
#else
            return new Vector4(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y), Math.Max(value1.Z, value2.Z), Math.Max(value1.W, value2.W));
#endif
        }

        /// <summary>
        /// Gets the componentwise minimum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector4"/>.</param>
        /// <param name="value2">The second <see cref="Vector4"/>.</param>
        /// <returns>The componentwise minimum <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Min(Vector4 value1, Vector4 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector4 res = Fast.Vector4.Min(*(Fast.Vector4*)tempVal1,*(Fast.Vector4*)tempVal2);

            var tempRes = &res;
            return *(Vector4*)tempRes;
#else
            return new Vector4(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y), Math.Min(value1.Z, value2.Z), Math.Min(value1.W, value2.W));
#endif
        }

        /// <summary>
        /// Reflects a <see cref="Vector4"/> by a given normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> to reflect.</param>
        /// <param name="normal">The normal indicating a surface to reflect off from.</param>
        /// <returns>The reflected <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Reflect(Vector4 vector, Vector4 normal)
        {
            normal.Normalize();
            return 2 * (normal.Dot(vector) * normal - vector); //TODO: normalize normal?
        }
        
        /// <summary>
        /// Transforms multiple vectors by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="count">The count of vectors to transform.</param>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="positions">A pointer to the vectors to transform.</param>
        /// <param name="output">A pointer to write the resulting vectors to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Transform(int count, ref Matrix matrix, Vector4* positions, Vector4* output)
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
        public static void Transform(ref Matrix matrix, Vector4[] positions, Vector4[] output)
        {
            var index = 0;
            foreach (var position in positions)
            {
                output[index++] = Transform(matrix, position);
            }
        }

        /// <summary>
        /// Rotates a given <see cref="Vector4"/> by a given <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="position">The <see cref="Vector4"/> to rotate.</param>
        /// <param name="quaternion">The <see cref="Quaternion"/> to rotate by.</param>
        /// <returns>The rotated <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Transform(Vector4 position, Quaternion quaternion)
        {
            return Transform(Matrix.CreateFromQuaternion(quaternion), position); //TODO: directly transform
        }

        /// <summary>
        /// Transforms a <see cref="Vector4"/> by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="position">The <see cref="Vector4"/> to transform.</param>
        /// <returns>The transformed <see cref="Vector4"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Transform(Matrix matrix, Vector4 position)
        {
            return new Vector4(position.X * matrix.M11 + position.Y * matrix.M12 + position.Z * matrix.M13 + position.W * matrix.M14,
                position.X * matrix.M21 + position.Y * matrix.M22 + position.Z * matrix.M23 + position.W * matrix.M24,
                position.X * matrix.M31 + position.Y * matrix.M32 + position.Z * matrix.M33 + position.W * matrix.M34,
                position.X * matrix.M41 + position.Y * matrix.M42 + position.Z * matrix.M43 + position.W * matrix.M44);
        }

        /// <summary>
        /// A <see cref="Vector4"/> with all its components set to 1.
        /// </summary>
        public static readonly Vector4 One = new Vector4(1, 1, 1, 1);

        /// <summary>
        /// A <see cref="Vector4"/> with all its components set to 0.
        /// </summary>
        public static readonly Vector4 Zero;

        /// <summary>
        /// A <see cref="Vector4"/> with its x component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector4 UnitX = new Vector4(1, 0, 0, 0);

        /// <summary>
        /// A <see cref="Vector4"/> with its y component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector4 UnitY = new Vector4(0, 1, 0, 0);

        /// <summary>
        /// A <see cref="Vector4"/> with its z component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector4 UnitZ = new Vector4(0, 0, 1, 0);

        /// <summary>
        /// A <see cref="Vector4"/> with its w component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector4 UnitW = new Vector4(0, 0, 0, 1);

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
                $"[{X.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {Y.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {Z.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}, {W.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)}]";
        }

    }
}
