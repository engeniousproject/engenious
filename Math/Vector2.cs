using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Fast = System.Numerics;
// ReSharper disable CompareOfFloatsByEqualityOperator
namespace engenious
{
    /// <summary>
    /// Defines a 2D float Vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [TypeConverter(typeof(Vector2Converter))]
    public unsafe struct Vector2 : IEquatable<Vector2>
    {
        /// <summary>
        /// Gets or sets the x component.
        /// </summary>
        public float X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]set;
        }

        /// <summary>
        /// Gets or sets the y component.
        /// </summary>
        public float Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]set;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector2"/> struct with its components set to a scalar.
        /// </summary>
        /// <param name="w">The scalar to set the components to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(float w)
        {
            X = Y = w;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector2"/> struct.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates the dot product with another <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value2">The <see cref="Vector2"/> to create the dot product with.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Dot(Vector2 value2)
        {
            return Dot(this,value2);
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> orthogonal to this <see cref="Vector2"/>.
        /// </summary>
        /// <returns>The orthogonal <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Cross()
        {
            return new Vector2(Y, -X);
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        [Browsable(false)]
        public float Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get{return (float)Math.Sqrt(LengthSquared);}
        }

        /// <summary>
        /// Gets the squared length.
        /// </summary>
        [Browsable(false)]
        public float LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get{return Dot(this,this);}
        }

        /// <summary>
        /// Normalizes this <see cref="Vector2"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            this /= Length;
        }
        
        /// <summary>
        /// Gets the normalized <see cref="Vector2"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Normalized()
        {
            return this / Length;
        }

        /// <summary>
        /// Transforms this <see cref="Vector2"/> by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <returns>The transformed <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 Transform(Matrix matrix)
        {
            return new Vector2((X * matrix.M11) + (Y * matrix.M21) + matrix.M41, (X * matrix.M12) + (Y * matrix.M22) + matrix.M42);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        #region IEquatable implementation

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (obj is Vector2)
                return Equals((Vector2)obj);
            return false;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        #endregion

        /// <summary>
        /// Tests two <see cref="Vector2"/> structs for equality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector2"/> to test with.</param>
        /// <returns><c>true</c> if the vectors are equal; otherwise <c>false</c>.</returns>        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2 value1, Vector2 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }

        /// <summary>
        /// Tests two <see cref="Vector2"/> structs for inequality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector2"/> to test with.</param>
        /// <returns><c>true</c> if the vectors aren't equal; otherwise <c>false</c>.</returns>  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2 value1, Vector2 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator +(Vector2 value1, Vector2 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector2 res= (*(Fast.Vector2*)tempVal1 + *(Fast.Vector2*)tempVal2);

            var tempRes = &res;
            return *(Vector2*)tempRes;
#else
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
#endif
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The subtrahend <see cref="Vector2"/>.</param>
        /// <param name="value2">The minuend <see cref="Vector2"/>.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 value1, Vector2 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector2 res= (*(Fast.Vector2*)tempVal1 - *(Fast.Vector2*)tempVal2);

            var tempRes = &res;
            return *(Vector2*)tempRes;
#else
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
#endif
        }

        /// <summary>
        /// Negates a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vector2"/> to negate.</param>
        /// <returns>The negated <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator -(Vector2 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Multiplies a <see cref="Vector2"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector2"/>.</param>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <returns>The scaled <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Vector2 value, float scalar)
        {
#if USE_SIMD
            var tempVal = &value;
            Fast.Vector2 res= (*(Fast.Vector2*)tempVal * scalar);

            var tempRes = &res;
            return *(Vector2*)tempRes;
#else
            value.X *= scalar;
            value.Y *= scalar;
            return value;
#endif
        }

        /// <summary>
        /// Multiplies a <see cref="Vector2"/> by a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <param name="value">The <see cref="Vector2"/>.</param>
        /// <returns>The scaled <see cref="Vector2"/>.</returns>
        public static Vector2 operator *(float scalar, Vector2 value)
        {
            return value * scalar;
        }

        /// <summary>
        /// Multiplies two vectors componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator *(Vector2 value1, Vector2 value2)//TODO: ugly as hell
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector2 res= (*(Fast.Vector2*)tempVal1 * *(Fast.Vector2*)tempVal2);

            var tempRes = &res;
            return *(Vector2*)tempRes;
#else
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
#endif
        }

        /// <summary>
        /// Divides a <see cref="Vector2"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector2"/>.</param>
        /// <param name="scalar">The scalar to divide by.</param>
        /// <returns>The scaled down <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(Vector2 value, float scalar)
        {
#if USE_SIMD
            var tempVal = &value;
            Fast.Vector2 res= (*(Fast.Vector2*)tempVal / scalar);

            var tempRes = &res;
            return *(Vector2*)tempRes;
#else
            value.X /= scalar;
            value.Y /= scalar;
            return value;
#endif
        }

        /// <summary>
        /// Divides two vectors componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 operator /(Vector2 value1, Vector2 value2)//TODO: ugly as hell?
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector2 res= (*(Fast.Vector2*)tempVal1 / *(Fast.Vector2*)tempVal2);

            var tempRes = &res;
            return *(Vector2*)tempRes;
#else
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
#endif
        }

        /// <summary>
        /// Clamps a <see cref="Vector2"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector2"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <returns>The resulting clamped <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
        {
            return Min(Max(value,min),max);
        }

        /// <summary>
        /// Clamps a <see cref="Vector2"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector2"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <param name="output">The resulting clamped <see cref="Vector2"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp(Vector2 value, Vector2 min, Vector2 max, out Vector2 output)
        {
            output = Min(Max(value,min),max);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector2 value1, Vector2 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector2 res= (*(Fast.Vector2*)tempVal1 * *(Fast.Vector2*)tempVal2);
            return res.X+res.Y;
#else
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1.X+value1.Y;
#endif
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The resulting distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector2 value1, Vector2 value2)
        {
            return (value1 - value2).Length;
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The resulting squared distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Vector2 value1, Vector2 value2)
        {
            return (value1 - value2).LengthSquared;
        }

        /// <summary>
        /// Lerps between two vectors using a given <paramref name="amount"/>.
        /// </summary>
        /// <param name="value1">The <see cref="Vector2"/> to lerp from.</param>
        /// <param name="value2">The <see cref="Vector2"/> to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>The resulting interpolated <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        /// <summary>
        /// Gets the componentwise maximum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The componentwise maximum <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Max(Vector2 value1, Vector2 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;

            Fast.Vector2 res = Fast.Vector2.Max(*(Fast.Vector2*)tempVal1,*(Fast.Vector2*)tempVal2);

            var tempRes = &res;
            return *(Vector2*)tempRes;
#else
            return new Vector2(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y));
#endif
        }

        /// <summary>
        /// Gets the componentwise minimum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The componentwise minimum <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Min(Vector2 value1, Vector2 value2)
        {
#if USE_SIMD
            var tempVal1 = &value1;
            var tempVal2 = &value2;
            Fast.Vector2 res = Fast.Vector2.Min(*(Fast.Vector2*)tempVal1,*(Fast.Vector2*)tempVal2);

            var tempRes = &res;
            return *(Vector2*)tempRes;
#else
            return new Vector2(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y));
#endif
        }

        /// <summary>
        /// Reflects a <see cref="Vector2"/> by a given normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2"/> to reflect.</param>
        /// <param name="normal">The normal indicating a surface to reflect off from.</param>
        /// <returns>The reflected <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
        {
            normal.Normalize();
            return 2 * (normal.Dot(vector) * normal - vector); //TODO: normalize normal?
        }

        /// <summary>
        /// Transforms a <see cref="Vector2"/> by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">The <see cref="Vector2"/> to transform.</param>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <returns>The transformed <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Transform(Vector2 position, Matrix matrix)
        {
            //{v1*x1+v2*y1+w1,v1*x2+v2*y2+w2,v1*x3+v2*y3+w3,v1*x4+v2*y4+w4}
            //{v1*x1+v2*x2+x4,v1*y1+v2*y2+y4,v1*z1+v2*z2+z4,v1*w1+v2*w2+w4}
            return new Vector2(position.X * matrix.M11 + position.Y * matrix.M12 + matrix.M14, position.X * matrix.M21 + position.Y * matrix.M22 + matrix.M24);//TODO: SIMD
        }

        /// <summary>
        /// Transforms multiple vectors by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="count">The count of vectors to transform.</param>
        /// <param name="positions">A pointer to the vectors to transform.</param>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="output">A pointer to write the resulting vectors to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Transform(int count, Vector2* positions, ref Matrix matrix, Vector2* output)
        {
            for (var i = 0; i < count; i++, positions++, output++)
            {
                *output = new Vector2(
                    (float)((*positions).X * (double)matrix.M11 + (*positions).Y * (double)matrix.M21) + matrix.M41,
                    (float)((*positions).X * (double)matrix.M12 + (*positions).Y * (double)matrix.M22) + matrix.M42);//TODO: SIMD
            }
        }

        /// <summary>
        /// Transforms multiple vectors by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="positions">The vectors to transform.</param>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="output">An array to write the resulting vectors to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Vector2[] positions, ref Matrix matrix, Vector2[] output)
        {
            var index = 0;
            foreach (var position in positions)//TODO: SIMD
            {
                output[index++] = new Vector2(position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41, position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42);
            }
        }

        /// <summary>
        /// A <see cref="Vector2"/> with all its components set to 1.
        /// </summary>
        public static readonly Vector2 One = new Vector2(1, 1);

        /// <summary>
        /// A <see cref="Vector2"/> with all its components set to 0.
        /// </summary>
        public static readonly Vector2 Zero;

        /// <summary>
        /// A <see cref="Vector2"/> with its x component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector2 UnitX = new Vector2(1, 0);

        /// <summary>
        /// A <see cref="Vector2"/> with its y component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector2 UnitY = new Vector2(0, 1);
    }
}

