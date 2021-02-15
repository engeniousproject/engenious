using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Fast = System.Numerics;
namespace engenious
{
    /// <summary>
    /// Defines a 3D double Vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [System.ComponentModel.TypeConverter(typeof(Vector3dConverter))]
    public unsafe struct Vector3d:IEquatable<Vector3d>
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
        /// Gets or sets the z component.
        /// </summary>
        public double Z
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]set;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3d"/> struct with its components set to a scalar.
        /// </summary>
        /// <param name="w">The scalar to set the components to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(double w)
        {
            X = w;
            Y = w;
            Z = w;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3d"/> struct.
        /// </summary>
        /// <param name="val">A <see cref="Vector2d"/> to take the x and y component from.</param>
        /// <param name="z">The z component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(Vector2d val, float z)
        {
            X = val.X;
            Y = val.Y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new <see cref="Vector3d"/> struct.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        /// <param name="z">The z component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(double x, double y, double z=0.0f)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Calculates the dot product with another <see cref="Vector3d"/>.
        /// </summary>
        /// <param name="value2">The <see cref="Vector3d"/> to create the dot product with.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Dot(Vector3d value2)
        {
            return Dot(this,value2);
        }

        /// <summary>
        /// Calculates the cross product between this <see cref="Vector3d"/> and another <see cref="Vector3d"/>.
        /// </summary>
        /// <returns>The resulting orthogonal <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Cross(Vector3d value2)
        {
            return Cross(this,value2);
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public double Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (double)Math.Sqrt(LengthSquared);
        }

        /// <summary>
        /// Gets the squared length.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public double LengthSquared => Dot(this,this);

        /// <summary>
        /// Normalizes this <see cref="Vector3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            this /= Length;
        }
        
        /// <summary>
        /// Gets the normalized <see cref="Vector3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d Normalized()
        {
            return this / Length;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
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
        public override bool Equals(object? obj)
        {
            return (obj is Vector3d vector3d) && Equals(vector3d);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3d other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        #endregion

        /// <summary>
        /// Tests two <see cref="Vector3d"/> structs for equality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3d"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector3d"/> to test with.</param>
        /// <returns><c>true</c> if the vectors are equal; otherwise <c>false</c>.</returns>     
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3d value1, Vector3d value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z;
        }

        /// <summary>
        /// Tests two <see cref="Vector3"/> structs for inequality.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3d"/> to test with.</param>
        /// <param name="value2">The second <see cref="Vector3d"/> to test with.</param>
        /// <returns><c>true</c> if the vectors aren't equal; otherwise <c>false</c>.</returns>  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3d value1, Vector3d value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3d"/>.</param>
        /// <param name="value2">The second <see cref="Vector3d"/>.</param>
        /// <returns>The resulting <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator +(Vector3d value1, Vector3d value2)
        {
#if USE_SIMD
            Fast.Vector3d res= (*(Fast.Vector3d*)&value1 + *(Fast.Vector3d*)&value2);
            return *(Vector3d*)&res;
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
        /// <param name="value1">The subtrahend <see cref="Vector3d"/>.</param>
        /// <param name="value2">The minuend <see cref="Vector3d"/>.</param>
        /// <returns>The resulting <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(Vector3d value1, Vector3d value2)
        {
#if USE_SIMD
            Fast.Vector3d res= (*(Fast.Vector3d*)&value1 - *(Fast.Vector3d*)&value2);
            return *(Vector3d*)&res;
#else
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
#endif
        }

        /// <summary>
        /// Negates a <see cref="Vector3d"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vector3d"/> to negate.</param>
        /// <returns>The negated <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(Vector3d value)
        {
#if USE_SIMD
            Fast.Vector3d res= (-*(Fast.Vector3d*)&value);
            return *(Vector3d*)&res;
#else
            value.X = -value.X;
            value.Y = -value.Y;
            value.Z = -value.Z;
            return value;
#endif
        }

        /// <summary>
        /// Multiplies a <see cref="Vector3d"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector3d"/>.</param>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <returns>The scaled <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(Vector3d value, double scalar)
        {
#if USE_SIMD
            Fast.Vector3d res= (scalar * *(Fast.Vector3d*)&value);
            return *(Vector3d*)&res;
#else
            value.X *= scalar;
            value.Y *= scalar;
            value.Z *= scalar;
            return value;
#endif
        }

        /// <summary>
        /// Multiplies a <see cref="Vector3d"/> by a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to multiply by.</param>
        /// <param name="value">The <see cref="Vector3d"/>.</param>
        /// <returns>The scaled <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(double scalar, Vector3d value)
        {
            return value * scalar;
        }

        /// <summary>
        /// Multiplies two vectors componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3d"/>.</param>
        /// <param name="value2">The second <see cref="Vector3d"/>.</param>
        /// <returns>The resulting <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(Vector3d value1, Vector3d value2)//TODO: ugly as hell
        {
#if USE_SIMD
            Fast.Vector3d res= (*(Fast.Vector3d*)&value1 * *(Fast.Vector3d*)&value2);
            return *(Vector3d*)&res;
#else
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
#endif
        }

        /// <summary>
        /// Divides a <see cref="Vector3d"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Vector3d"/>.</param>
        /// <param name="scalar">The scalar to divide by.</param>
        /// <returns>The scaled down <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator /(Vector3d value, double scalar)
        {
#if USE_SIMD
            Fast.Vector3d res= (*(Fast.Vector3d*)&value / scalar);
            return *(Vector3d*)&res;
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
        /// <param name="value1">The first <see cref="Vector3d"/>.</param>
        /// <param name="value2">The second <see cref="Vector3d"/>.</param>
        /// <returns>The resulting <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator /(Vector3d value1, Vector3d value2)//TODO: ugly as hell?
        {
#if USE_SIMD
            Fast.Vector3d res= (*(Fast.Vector3d*)&value1 / *(Fast.Vector3d*)&value2);
            return *(Vector3d*)&res;
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
        /// <returns>A <see cref="Vector3d"/> orthogonal to both input vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Cross(Vector3d value1, Vector3d value2)
        {
#if USE_SIMD
            Fast.Vector3d a1 = new Fast.Vector3d(value1.Y,value1.Z,value1.X);
            Fast.Vector3d a2 = new Fast.Vector3d(value2.Z,value2.X,value2.Y);
            Fast.Vector3d b1 = new Fast.Vector3d(value1.Z,value1.X,value1.Y);
            Fast.Vector3d b2 = new Fast.Vector3d(value2.Y,value2.Z,value2.X);

            Fast.Vector3d res = a1*a2 - b1*b2;
            return *(Vector3d*)&res;
#else
            return new Vector3d(value1.Y*value2.Z - value1.Z*value2.Y,
                value1.Z*value2.X - value1.X*value2.Z,
                value1.X*value2.Y - value1.Y*value2.X);
#endif
        }

        /// <summary>
        /// Clamps a <see cref="Vector3d"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector3d"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <returns>The resulting clamped <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Clamp(Vector3d value, Vector3d min, Vector3d max)
        {
            return Min(Max(min,value),max);
        }

        /// <summary>
        /// Clamps a <see cref="Vector3d"/> to given min and max values.
        /// </summary>
        /// <param name="value">The <see cref="Vector3d"/> to clamp.</param>
        /// <param name="min">The minimum values to clamp to.</param>
        /// <param name="max">The maximum values to clamp to.</param>
        /// <param name="output">The resulting clamped <see cref="Vector3d"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clamp(Vector3d value, Vector3d min, Vector3d max, out Vector3d output)
        {
            output = Min(Max(min,value),max);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3d"/>.</param>
        /// <param name="value2">The second <see cref="Vector3d"/>.</param>
        /// <returns>The resulting dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(Vector3d value1, Vector3d value2)
        {
            var res = value1*value2;
            return res.X+res.Y+res.Z;
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3d"/>.</param>
        /// <param name="value2">The second <see cref="Vector3d"/>.</param>
        /// <returns>The resulting distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(Vector3d value1, Vector3d value2)
        {
            return (value1 - value2).Length;
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3d"/>.</param>
        /// <param name="value2">The second <see cref="Vector3d"/>.</param>
        /// <returns>The resulting squared distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceSquared(Vector3d value1, Vector3d value2)
        {
            return (value1 - value2).LengthSquared;
        }

        /// <summary>
        /// Lerps between two vectors using a given <paramref name="amount"/>.
        /// </summary>
        /// <param name="value1">The <see cref="Vector3d"/> to lerp from.</param>
        /// <param name="value2">The <see cref="Vector3d"/> to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>The resulting interpolated <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Lerp(Vector3d value1, Vector3d value2, double amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        /// <summary>
        /// Gets the componentwise maximum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3d"/>.</param>
        /// <param name="value2">The second <see cref="Vector3d"/>.</param>
        /// <returns>The componentwise maximum <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Max(Vector3d value1, Vector3d value2)
        {
#if USE_SIMD
            Fast.Vector3d res= Fast.Vector3d.Max(*(Fast.Vector3d*)&value1,*(Fast.Vector3d*)&value2);
            return *(Vector3d*)&res;
#else
            return new Vector3d(Math.Max(value1.X, value2.X), Math.Max(value1.Y, value2.Y), Math.Max(value1.Z, value2.Z));
#endif
        }

        /// <summary>
        /// Gets the componentwise minimum of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector3d"/>.</param>
        /// <param name="value2">The second <see cref="Vector3d"/>.</param>
        /// <returns>The componentwise minimum <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Min(Vector3d value1, Vector3d value2)
        {
#if USE_SIMD
            Fast.Vector3d res = Fast.Vector3d.Min(*(Fast.Vector3d*)&value1,*(Fast.Vector3d*)&value2);
            return *(Vector3d*)&res;
#else
            return new Vector3d(Math.Min(value1.X, value2.X), Math.Min(value1.Y, value2.Y), Math.Min(value1.Z, value2.Z));
#endif
        }

        /// <summary>
        /// Reflects a <see cref="Vector3d"/> by a given normal.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3d"/> to reflect.</param>
        /// <param name="normal">The normal indicating a surface to reflect off from.</param>
        /// <returns>The reflected <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Reflect(Vector3d vector, Vector3d normal)
        {
            normal.Normalize();
            return 2 * (normal.Dot(vector) * normal - vector); //TODO: normalize normal?
        }

        /// <summary>
        /// Transforms a <see cref="Vector3d"/> by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="position">The <see cref="Vector3d"/> to transform.</param>
        /// <returns>The transformed <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Transform(Matrix matrix, Vector3d position)
        {
            return new Vector3d(position.X * matrix.M11 + position.Y * matrix.M12 + position.Z * matrix.M13 + matrix.M14,
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
        public static unsafe void Transform(int count, ref Matrix matrix, Vector3d* positions, Vector3d* output)
        {
            for (var i = 0; i < count; i++, positions++, output++)
            {
                *output = Transform(matrix, *positions);
            }
        }

        /// <summary>
        /// Transforms multiple vectors by a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="positions">The vectors to transform.</param>
        /// <param name="matrix">The <see cref="Matrix"/> to transform by.</param>
        /// <param name="output">An array to write the resulting vectors to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Vector3d[] positions, ref Matrix matrix, Vector3d[] output)
        {
            var index = 0;
            foreach (var position in positions)
            {
                output[index++] = Transform(matrix, position);
            }
        }

        /// <summary>
        /// Rotates a given <see cref="Vector3d"/> by a given <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="position">The <see cref="Vector3d"/> to rotate.</param>
        /// <param name="quaternion">The <see cref="Quaternion"/> to rotate by.</param>
        /// <returns>The rotated <see cref="Vector3d"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d Transform(Vector3d position,Quaternion quaternion)
        {
            return Transform(Matrix.CreateFromQuaternion(quaternion), position); //TODO: directly transform
        }

        /// <summary>
        /// A <see cref="Vector3d"/> with all its components set to 1.
        /// </summary>
        public static readonly Vector3d One = new Vector3d(1, 1, 1);

        /// <summary>
        /// A <see cref="Vector3d"/> with all its components set to 0.
        /// </summary>
        public static readonly Vector3d Zero;

        /// <summary>
        /// A <see cref="Vector3d"/> with its x component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector3d UnitX = new Vector3d(1, 0);

        /// <summary>
        /// A <see cref="Vector3d"/> with its y component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector3d UnitY = new Vector3d(0, 1);

        /// <summary>
        /// A <see cref="Vector3d"/> with its z component set to 1, all others set to 0.
        /// </summary>
        public static readonly Vector3d UnitZ = new Vector3d(0, 0, 1);
    }
}

