using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Fast = System.Numerics;
// ReSharper disable CompareOfdoublesByEqualityOperator
namespace engenious
{
    /// <summary>
    /// Defines a 2D double Vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [TypeConverter(typeof(Vector2dConverter))]
    public unsafe struct Vector2d : IEquatable<Vector2d>
    {
        /// <summary>
        /// Gets or sets the x component.
        /// </summary>
        public double X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]set;
        }

        /// <summary>
        /// Gets or sets the y component.
        /// </summary>
        public double Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]set;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector2d"/> struct with its components set to a scalar.
        /// </summary>
        /// <param name="w">The scalar to set the components to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d(double w)
        {
            X = Y = w;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector2d"/> struct.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates the dot product with another <see cref="Vector2d"/>.
        /// </summary>
        /// <param name="value2">The <see cref="Vector2d"/> to create the dot product with.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Dot(Vector2d value2)
        {
            return Dot(this,value2);
        }

        /// <summary>
        /// Returns a <see cref="Vector2d"/> orthogonal to this <see cref="Vector2d"/>.
        /// </summary>
        /// <returns>The orthogonal <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Cross()
        {
            return new Vector2d(X, -Y);
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        [Browsable(false)]
        public double Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (double)Math.Sqrt(LengthSquared);
        }

        /// <summary>
        /// Gets the squared length.
        /// </summary>
        [Browsable(false)]
        public double LengthSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Dot(this,this);
        }

        /// <summary>
        /// Normalizes this <see cref="Vector2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            this /= Length;
        }
        
        /// <summary>
        /// Gets the normalized <see cref="Vector2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Normalized()
        {
            return this / Length;
        }

        /// <summary>
        /// Transforms this <see cref="Vector2d"/> by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <returns>The transformed <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2d Transform(Matrix matrix)
        {
            return new Vector2d((X * matrix.M11) + (Y * matrix.M12) + matrix.M14, (X * matrix.M21) + (Y * matrix.M22) + matrix.M24);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        #region IEquatable implementation

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return (obj is Vector2d vector2d) && Equals(vector2d);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2d other)
        {
            return X == other.X && Y == other.Y;
        }

        #endregion

        /// <summary>
        /// Tests two <see cref="Vector2d"/> structs for equality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2d"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector2d"/> to test with.</param>
        /// <returns><c>true</c> if the vectors are equal; otherwise <c>false</c>.</returns>     
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2d value1, Vector2d value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }

        /// <summary>
        /// Tests two <see cref="Vector2d"/> structs for inequality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2d"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector2d"/> to test with.</param>
        /// <returns><c>true</c> if the vectors aren't equal; otherwise <c>false</c>.</returns>  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2d value1, Vector2d value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2d"/>.</param>
        /// <param name="value2">The second <see cref="Vector2d"/>.</param>
        /// <returns>The resulting <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator +(Vector2d value1, Vector2d value2)
        {
#if USE_SIMD
            Fast.Vector2d res= (*(Fast.Vector2d*)&value1 + *(Fast.Vector2d*)&value2);
            return *(Vector2d*)&res;
#else
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
#endif
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The subtrahend <see cref="Vector2d"/>.</param>
        /// <param name="value2">The minuend <see cref="Vector2d"/>.</param>
        /// <returns>The resulting <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator -(Vector2d value1, Vector2d value2)
        {
#if USE_SIMD
            Fast.Vector2d res= (*(Fast.Vector2d*)&value1 - *(Fast.Vector2d*)&value2);
            return *(Vector2d*)&res;
#else
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
#endif
        }

        /// <summary>
        /// Negates a <see cref="Vector2d"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vector2d"/> to negate.</param>
        /// <returns>The negated <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator -(Vector2d value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Multiplies a <see cref="Vector2d"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector2d"/>.</param>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <returns>The scaled <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator *(Vector2d value, double scalar)
        {
#if USE_SIMD
            Fast.Vector2d res= (*(Fast.Vector2d*)&value * scalar);
            return *(Vector2d*)&res;
#else
            value.X *= scalar;
            value.Y *= scalar;
            return value;
#endif
        }

        /// <summary>
        /// Multiplies a <see cref="Vector2d"/> by a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <param name="value">The <see cref="Vector2d"/>.</param>
        /// <returns>The scaled <see cref="Vector2d"/>.</returns>
        public static Vector2d operator *(double scalar, Vector2d value)
        {
            return value * scalar;
        }

        /// <summary>
        /// Multiplies two vectors componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2d"/>.</param>
        /// <param name="value2">The second <see cref="Vector2d"/>.</param>
        /// <returns>The resulting <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator *(Vector2d value1, Vector2d value2)//TODO: ugly as hell
        {
#if USE_SIMD
            Fast.Vector2d res= (*(Fast.Vector2d*)&value1 * *(Fast.Vector2d*)&value2);
            return *(Vector2d*)&res;
#else
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
#endif
        }

        /// <summary>
        /// Divides a <see cref="Vector2d"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector2d"/>.</param>
        /// <param name="scalar">The scalar to divide by.</param>
        /// <returns>The scaled down <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator /(Vector2d value, double scalar)
        {
#if USE_SIMD
            Fast.Vector2d res= (*(Fast.Vector2d*)&value / scalar);
            return *(Vector2d*)&res;
#else
            value.X /= scalar;
            value.Y /= scalar;
            return value;
#endif
        }

        /// <summary>
        /// Divides two vectors componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2d"/>.</param>
        /// <param name="value2">The second <see cref="Vector2d"/>.</param>
        /// <returns>The resulting <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator /(Vector2d value1, Vector2d value2)//TODO: ugly as hell?
        {
#if USE_SIMD
            Fast.Vector2d res= (*(Fast.Vector2d*)&value1 / *(Fast.Vector2d*)&value2);
            return *(Vector2d*)&res;
#else
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
#endif
        }

        /// <summary>
        /// Clamps a <see cref="Vector2d"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector2d"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <returns>The resulting clamped <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Clamp(Vector2d value, Vector2d min, Vector2d max)
        {
            return Min(Max(value,min),max);
        }

        /// <summary>
        /// Clamps a <see cref="Vector2d"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector2d"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <param name="output">The resulting clamped <see cref="Vector2d"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp(Vector2d value, Vector2d min, Vector2d max, out Vector2d output)
        {
            output = Min(Max(value,min),max);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2d"/>.</param>
        /// <param name="value2">The second <see cref="Vector2d"/>.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(Vector2d value1, Vector2d value2)
        {
#if USE_SIMD
            Fast.Vector2d res= (*(Fast.Vector2d*)&value1 * *(Fast.Vector2d*)&value2);
            return res.X+res.Y;
#else
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1.X+value2.Y;
#endif
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2d"/>.</param>
        /// <param name="value2">The second <see cref="Vector2d"/>.</param>
        /// <returns>The resulting distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Vector2d value1, Vector2d value2)
        {
            return (value1 - value2).Length;
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2d"/>.</param>
        /// <param name="value2">The second <see cref="Vector2d"/>.</param>
        /// <returns>The resulting squared distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceSquared(Vector2d value1, Vector2d value2)
        {
            return (value1 - value2).LengthSquared;
        }

        /// <summary>
        /// Lerps between two vectors using a given <paramref name="amount"/>.
        /// </summary>
        /// <param name="value1">The <see cref="Vector2d"/> to lerp from.</param>
        /// <param name="value2">The <see cref="Vector2d"/> to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>The resulting interpolated <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Lerp(Vector2d value1, Vector2d value2, double amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        /// <summary>
        /// Gets the componentwise maximum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2d"/>.</param>
        /// <param name="value2">The second <see cref="Vector2d"/>.</param>
        /// <returns>The componentwise maximum <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Max(Vector2d value1, Vector2d value2)
        {
#if USE_SIMD
            Fast.Vector2d res = Fast.Vector2d.Max(*(Fast.Vector2d*)&value1,*(Fast.Vector2d*)&value2);
            return *(Vector2d*)&res;
#else
            return new Vector2d(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y));
#endif
        }

        /// <summary>
        /// Gets the componentwise minimum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2d"/>.</param>
        /// <param name="value2">The second <see cref="Vector2d"/>.</param>
        /// <returns>The componentwise minimum <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Min(Vector2d value1, Vector2d value2)
        {
#if USE_SIMD
            Fast.Vector2d res = Fast.Vector2d.Min(*(Fast.Vector2d*)&value1,*(Fast.Vector2d*)&value2);
            return *(Vector2d*)&res;
#else
            return new Vector2d(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y));
#endif
        }

        /// <summary>
        /// Reflects a <see cref="Vector2d"/> by a given normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector2d"/> to reflect.</param>
        /// <param name="normal">The normal indicating a surface to reflect off from.</param>
        /// <returns>The reflected <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Reflect(Vector2d vector, Vector2d normal)
        {
            normal.Normalize();
            return 2 * (normal.Dot(vector) * normal - vector); //TODO: normalize normal?
        }

        /// <summary>
        /// Transforms a <see cref="Vector2d"/> by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="position">The <see cref="Vector2d"/> to transform.</param>
        /// <returns>The transformed <see cref="Vector2d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Transform(Matrix matrix, Vector2d position)
        {
            //{v1*x1+v2*y1+w1,v1*x2+v2*y2+w2,v1*x3+v2*y3+w3,v1*x4+v2*y4+w4}
            //{v1*x1+v2*x2+x4,v1*y1+v2*y2+y4,v1*z1+v2*z2+z4,v1*w1+v2*w2+w4}
            return new Vector2d(position.X * matrix.M11 + position.Y * matrix.M12 + matrix.M14, position.X * matrix.M21 + position.Y * matrix.M22 + matrix.M24);//TODO: SIMD
        }

        /// <summary>
        /// Transforms multiple vectors by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="count">The count of vectors to transform.</param>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="positions">A pointer to the vectors to transform.</param>
        /// <param name="output">A pointer to write the resulting vectors to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Transform(int count, ref Matrix matrix, Vector2d* positions, Vector2d* output)
        {
            for (var i = 0; i < count; i++, positions++, output++)
            {
                *output = Transform(matrix, *positions);//TODO: SIMD
            }
        }

        /// <summary>
        /// Transforms multiple vectors by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="positions">The vectors to transform.</param>
        /// <param name="output">An array to write the resulting vectors to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ref Matrix matrix, Vector2d[] positions, Vector2d[] output)
        {
            var index = 0;
            foreach (var position in positions)//TODO: SIMD
            {
                output[index++] = Transform(matrix, position);
            }
        }
        
        /// <summary>
        /// A <see cref="Vector2d"/> with all its components set to 1.
        /// </summary>
        public static readonly Vector2d One = new Vector2d(1, 1);

        /// <summary>
        /// A <see cref="Vector2d"/> with all its components set to 0.
        /// </summary>
        public static readonly Vector2d Zero;

        /// <summary>
        /// A <see cref="Vector2d"/> with its x component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector2d UnitX = new Vector2d(1, 0);

        /// <summary>
        /// A <see cref="Vector2d"/> with its y component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector2d UnitY = new Vector2d(0, 1);
    }
}

