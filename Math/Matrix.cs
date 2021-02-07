using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Fast = System.Numerics;
namespace engenious
{
    /// <summary>
    /// Defines a 4x4 Matrix.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    [TypeConverter(typeof(MatrixConverter))]
    public struct Matrix :IEquatable<Matrix>
    {
        [FieldOffset(0)]
        private unsafe fixed float items[16];
        /// <summary>
        /// The element in the first row and first column.
        /// </summary>
        [FieldOffset(0)]
        public float M11;
        /// <summary>
        /// The element in the first row and second column.
        /// </summary>
        [FieldOffset(4)]
        public float M21;
        /// <summary>
        /// The element in the first row and third column.
        /// </summary>
        [FieldOffset(8)]
        public float M31;
        /// <summary>
        /// The element in the first row and fourth column.
        /// </summary>
        [FieldOffset(12)]
        public float M41;
        /// <summary>
        /// The element in the second row and first column.
        /// </summary>
        [FieldOffset(16)]
        public float M12;
        /// <summary>
        /// The element in the second row and second column.
        /// </summary>
        [FieldOffset(20)]
        public float M22;
        /// <summary>
        /// The element in the second row and third column.
        /// </summary>
        [FieldOffset(24)]
        public float M32;
        /// <summary>
        /// The element in the second row and fourth column.
        /// </summary>
        [FieldOffset(28)]
        public float M42;

        /// <summary>
        /// The element in the third row and first column.
        /// </summary>
        [FieldOffset(32)]
        public float M13;
        /// <summary>
        /// The element in the third row and second column.
        /// </summary>
        [FieldOffset(36)]
        public float M23;
        /// <summary>
        /// The element in the third row and third column.
        /// </summary>
        [FieldOffset(40)]
        public float M33;
        /// <summary>
        /// The element in the third row and fourth column.
        /// </summary>
        [FieldOffset(44)]
        public float M43;

        /// <summary>
        /// The element in the fourth row and first column.
        /// </summary>
        [FieldOffset(48)]
        public float M14;
        /// <summary>
        /// The element in the fourth row and second column.
        /// </summary>
        [FieldOffset(52)]
        public float M24;
        /// <summary>
        /// The element in the fourth row and third column.
        /// </summary>
        [FieldOffset(56)]
        public float M34;
        /// <summary>
        /// The element in the fourth row and fourth column.
        /// </summary>
        [FieldOffset(60)]
        public float M44;

        /// <summary>
        /// The first row.
        /// </summary>
        [FieldOffset(0)]
        public Vector4 Row0;
        /// <summary>
        /// The second row.
        /// </summary>
        [FieldOffset(16)]
        public Vector4 Row1;
        /// <summary>
        /// The third row.
        /// </summary>
        [FieldOffset(32)]
        public Vector4 Row2;
        /// <summary>
        /// The fourth row.
        /// </summary>
        [FieldOffset(48)]
        public Vector4 Row3;

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> struct from 16-components.
        /// </summary>
        /// <param name="m11">The element in the first row and first column.</param>
        /// <param name="m21">The element in the second row and first column.</param>
        /// <param name="m31">The element in the third row and first column.</param>
        /// <param name="m41">The element in the fourth row and first column.</param>
        /// <param name="m12">The element in the first row and second column.</param>
        /// <param name="m22">The element in the second row and second column.</param>
        /// <param name="m32">The element in the third row and second column.</param>
        /// <param name="m42">The element in the fourth row and second column.</param>
        /// <param name="m13">The element in the first row and third column.</param>
        /// <param name="m23">The element in the second row and third column.</param>
        /// <param name="m33">The element in the third row and third column.</param>
        /// <param name="m43">The element in the fourth row and third column.</param>
        /// <param name="m14">The element in the first row and fourth column.</param>
        /// <param name="m24">The element in the second row and fourth column.</param>
        /// <param name="m34">The element in the third row and fourth column.</param>
        /// <param name="m44">The element in the fourth row and fourth column.</param>
        public Matrix(float m11, float m21, float m31, float m41,
                      float m12, float m22, float m32, float m42,
                      float m13, float m23, float m33, float m43,
                      float m14, float m24, float m34, float m44)
            : this()
        {
            M11 = m11;
            M21 = m21;
            M31 = m31;
            M41 = m41;

            M12 = m12;
            M22 = m22;
            M32 = m32;
            M42 = m42;

            M13 = m13;
            M23 = m23;
            M33 = m33;
            M43 = m43;

            M14 = m14;
            M24 = m24;
            M34 = m34;
            M44 = m44;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> struct from 4-rows.
        /// </summary>
        /// <param name="row0">The first row.</param>
        /// <param name="row1">The second row.</param>
        /// <param name="row2">The third row.</param>
        /// <param name="row3">The fourth row.</param>
        public Matrix(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
            : this()
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }

        /// <summary>
        /// Gets or sets the translation vector of the <see cref="Matrix"/>.
        /// </summary>
        public Vector3 Translation
        {
            get => new Vector3(M41, M42, M43);
            set
            {
                M14 = value.X;
                M24 = value.Y;
                M34 = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets the first column vector of the <see cref="Matrix"/>.
        /// </summary>
        public Vector4 Column0
        {
            get => new Vector4(Row0.X, Row1.X, Row2.X, Row3.X);
            set
            {
                Row0.X = value.X;
                Row1.X = value.Y;
                Row2.X = value.Z;
                Row3.X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets the second column vector of the <see cref="Matrix"/>.
        /// </summary>
        public Vector4 Column1
        {
            get => new Vector4(Row0.Y, Row1.Y, Row2.Y, Row3.Y);
            set
            {
                Row0.Y = value.X;
                Row1.Y = value.Y;
                Row2.Y = value.Z;
                Row3.Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets the third column vector of the <see cref="Matrix"/>.
        /// </summary>
        public Vector4 Column2
        {
            get => new Vector4(Row0.Z, Row1.Z, Row2.Z, Row3.Z);
            set
            {
                Row0.Z = value.X;
                Row1.Z = value.Y;
                Row2.Z = value.Z;
                Row3.Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets the fourth column vector of the <see cref="Matrix"/>.
        /// </summary>
        public Vector4 Column3
        {
            get => new Vector4(Row0.W, Row1.W, Row2.W, Row3.W);
            set
            {
                Row0.W = value.X;
                Row1.W = value.Y;
                Row2.W = value.Z;
                Row3.W = value.W;
            }
        }

        /// <summary>
        /// Gets the determinant of the <see cref="Matrix"/>.
        /// </summary>
        public float Determinant =>   M11 * M22 * M33 * M44 - M11 * M22 * M43 * M34 + M11 * M32 * M43 * M24 - M11 * M32 * M23 * M44
                                    + M11 * M42 * M23 * M34 - M11 * M42 * M33 * M24 - M21 * M32 * M43 * M14 + M21 * M32 * M13 * M44
                                    - M21 * M42 * M13 * M34 + M21 * M42 * M33 * M14 - M21 * M12 * M33 * M44 + M21 * M12 * M43 * M34
                                    + M31 * M42 * M13 * M24 - M31 * M42 * M23 * M14 + M31 * M12 * M23 * M44 - M31 * M12 * M43 * M24
                                    + M31 * M22 * M43 * M14 - M31 * M22 * M13 * M44 - M41 * M12 * M23 * M34 + M41 * M12 * M33 * M24
                                    - M41 * M22 * M33 * M14 + M41 * M22 * M13 * M34 - M41 * M32 * M13 * M24 + M41 * M32 * M23 * M14;

        /// <summary>
        /// Gets or sets an element in a specific row and column.
        /// </summary>
        /// <param name="columnIndex">The column.</param>
        /// <param name="rowIndex">The row</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="columnIndex"/> or <paramref name="rowIndex"/> is out of range.</exception>
        public unsafe float this [int columnIndex, int rowIndex]
        {
            get
            {
                if (rowIndex < 0 || columnIndex < 0 || rowIndex >= 4 || columnIndex >= 4)
                    throw new IndexOutOfRangeException();
                return items[rowIndex * 4 + columnIndex];
            }
            set
            {
                if (rowIndex < 0 || columnIndex < 0 || rowIndex >= 4 || columnIndex >= 4)
                    throw new IndexOutOfRangeException();
                items[rowIndex * 4 + columnIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets an element at an index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="index"/> is out of range.</exception>
        public unsafe float this [int index]
        {
            get
            {
                if (index < 0 || index >= 16)
                    throw new IndexOutOfRangeException();
                return items[index];
            }
            set
            {
                if (index < 0 || index >= 16)
                    throw new IndexOutOfRangeException();
                items[index] = value;
            }
        }

        /// <summary>
        /// Transposes the <see cref="Matrix"/>.
        /// </summary>
        public void Transpose()
        {
            this = Transposed();
        }

        /// <summary>
        /// Gets the transpose of this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>The transposed <see cref="Matrix"/>.</returns>
        public Matrix Transposed()
        {
            return new Matrix(M11, M12, M13, M14,
                              M21, M22, M23, M24,
                              M31, M32, M33, M34,
                              M41, M42, M43, M44);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(
                HashCode.Combine(M11, M21, M31, M41),
                HashCode.Combine(M12, M22, M32, M42),
                HashCode.Combine(M13, M23, M33, M43),
                HashCode.Combine(M14, M24, M34, M44)
            );
        }

        #region IEquatable implementation

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if (other is Matrix)
                return Equals((Matrix)other);
            return false;
        }

        /// <inheritdoc />
        public bool Equals(Matrix other)
        {
            return this == other;
        }

        #endregion

        /// <summary>
        /// Tests two <see cref="Matrix"/> structs for equality.
        /// </summary>
        /// <param name="value1">The first <see cref="Matrix"/> to test with.</param>
        /// <param name="value2">The second <see cref="Matrix"/> to test with.</param>
        /// <returns><c>true</c> if the matrices are equal; otherwise <c>false</c>.</returns>
        public static unsafe bool operator==(Matrix value1, Matrix value2)
        {
            for (var i = 0; i < 16; i++)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value1.items[i] != value2.items[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Tests two <see cref="Matrix"/> structs for inequality.
        /// </summary>
        /// <param name="value1">The first <see cref="Matrix"/> to test with.</param>
        /// <param name="value2">The second <see cref="Matrix"/> to test with.</param>
        /// <returns><c>true</c> if the matrices aren't equal; otherwise <c>false</c>.</returns>
        public static unsafe bool operator!=(Matrix value1, Matrix value2)
        {
            for (var i = 0; i < 16; i++)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value1.items[i] != value2.items[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Adds two matrices.
        /// </summary>
        /// <param name="value1">The first <see cref="Matrix"/>.</param>
        /// <param name="value2">The second <see cref="Matrix"/>.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static unsafe Matrix operator+(Matrix value1, Matrix value2)
        {
            for (var i = 0; i < 16; i++)
            {
                value1.items[i] += value2.items[i];
            }
            return value1;
        }

        /// <summary>
        /// Subtracts two matrices.
        /// </summary>
        /// <param name="value1">The first <see cref="Matrix"/>.</param>
        /// <param name="value2">The second <see cref="Matrix"/>.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static unsafe Matrix operator-(Matrix value1, Matrix value2)
        {
            for (var i = 0; i < 16; i++)
            {
                value1.items[i] -= value2.items[i];
            }
            return value1;
        }

        /// <summary>
        /// Multiplies a <see cref="Matrix"/> by a scalar.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="value">The <see cref="Matrix"/>.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static unsafe Matrix operator*(float scalar, Matrix value)
        {
            for (var i = 0; i < 16; i++)
            {
                value.items[i] *= scalar;
            }
            return value;
        }

        /// <summary>
        /// Multiplies a <see cref="Matrix"/> by a scalar.
        /// </summary>
        /// <param name="value">The <see cref="Matrix"/>.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static Matrix operator*(Matrix value, float scalar)
        {
            return scalar * value;
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="value1">The first <see cref="Matrix"/>.</param>
        /// <param name="value2">The second <see cref="Matrix"/>.</param>
        /// <returns>The resulting <see cref="Matrix"/>.</returns>
        public static unsafe Matrix operator*(Matrix value1, Matrix value2)
        {
            var multiply = new Matrix();
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    float sum = 0;
                    for (var k = 0; k < 4; k++)
                    {
                        sum = sum + value1.items[i + k * 4] * value2.items[k + j * 4];
                    }

                    multiply[i + j * 4] = sum;
                }
            }

            return multiply;
        }

        /// <summary>
        /// Creates a perspective <see cref="Matrix"/> with a given field of view.
        /// </summary>
        /// <param name="fovY">The field of view angle[radians].</param>
        /// <param name="aspect">The aspect ratio.</param>
        /// <param name="near">The distance of the near plane.</param>
        /// <param name="far">The distance of the far plane.</param>
        /// <param name="result">The resulting perspective <see cref="Matrix"/>.</param>
        public static void CreatePerspectiveFieldOfView(float fovY, float aspect, float near, float far, out Matrix result)
        {

            var tangent = (float)Math.Tan(fovY / 2); // tangent of half fovY
            var height = near * tangent;         // half height of near plane
            var width = height * aspect;          // half width of near plane

            // params: left, right, bottom, top, near, far
            CreatePerspectiveOffCenter(-width, width, height, -height, near, far, out result);

        }

        /// <summary>
        /// Creates a perspective <see cref="Matrix"/> with a given field of view.
        /// </summary>
        /// <param name="fovY">The field of view angle[radians].</param>
        /// <param name="aspect">The aspect ratio.</param>
        /// <param name="near">>The distance of the near plane.</param>
        /// <param name="far">The distance of the far plane.</param>
        /// <returns>The resulting perspective <see cref="Matrix"/>.</returns>
        public static Matrix CreatePerspectiveFieldOfView(float fovY, float aspect, float near, float far)
        {
            var tangent = (float)Math.Tan(fovY / 2); // tangent of half fovY
            var height = near * tangent;         // half height of near plane
            var width = height * aspect;          // half width of near plane

            // params: left, right, bottom, top, near, far
            CreatePerspectiveOffCenter(-width, width, height, -height, near, far, out var result);
            return result;
        }

        /// <summary>
        /// Creates a perspective off-center <see cref="Matrix"/> with a given left, top, right and bottom planes.
        /// </summary>
        /// <param name="left">>The left plane.</param>
        /// <param name="right">>The right plane.</param>
        /// <param name="bottom">>The bottom plane.</param>
        /// <param name="top">>The top plane.</param>
        /// <param name="near">>The distance of the near plane.</param>
        /// <param name="far">The distance of the far plane.</param>
        /// <param name="result">The resulting perspective <see cref="Matrix"/>.</param>
        public static unsafe void CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far, out Matrix result)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (left  == right)
                throw new ArgumentOutOfRangeException($"{nameof(left)} or {nameof(right)}");
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (bottom == top)
                throw new ArgumentOutOfRangeException($"{nameof(bottom)} or {nameof(top)}");
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (near == far)
                throw new ArgumentOutOfRangeException($"{nameof(near)} or {nameof(far)}");
            var m = Identity;
            m.M11 = 2.0f * near / (right - left);
            m.M22 = 2.0f * near / (top - bottom);
            m.M13 = (right + left) / (right - left);
            m.M23 = (top + bottom) / (top - bottom);
            m.M33 = -(far + near) / (far - near);
            m.M43 = -1;
            m.M14 = 0;
            m.M24 = 0;
            m.M34 = -2 * (far * near) / (far - near);
            m.M44 = 0;
            result = m;
        }

        /// <summary>
        /// Creates an orthographic <see cref="Matrix"/>.
        /// </summary>
        /// <param name="width">The width of the view.</param>
        /// <param name="height">The height of the view.</param>
        /// <param name="near">The distance to the near plane.</param>
        /// <param name="far">The distance to the far plane.</param>
        /// <returns>The resulting orthographic <see cref="Matrix"/>.</returns>
        public static unsafe Matrix CreateOrthographic(float width, float height, float near, float far)
        {
            var res = Identity;
            res.M11 = 2f / width;
            res.M22 = -2f / height;
            res.M33 = 1f / (near - far);
            res.M14 = res.M24 = 0;
            res.M34 = -(far + near) / (far - near);

            return res;
        }

        /// <summary>
        /// Creates an orthographic off-center <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left">The left bound of the view.</param>
        /// <param name="right">The right bound of the view.</param>
        /// <param name="bottom">The bottom bound of the view.</param>
        /// <param name="top">The top bound of the view.</param>
        /// <param name="near">The distance to the near plane.</param>
        /// <param name="far">The distance to the far plane.</param>
        /// <returns>The resulting orthographic <see cref="Matrix"/>.</returns>
        public static unsafe Matrix CreateOrthographicOffCenter(float left, float right, float bottom, float top, float near, float far)
        {


            var res = Identity;
            res.M11 = 2.0f / (right - left);
            res.M22 = 2.0f / (top - bottom);
            res.M33 = -2.0f / (far - near);
            res.M14 = -(right + left) / (right - left);
            res.M24 = -(top + bottom) / (top - bottom);
            res.M34 = -(far + near) / (far - near);
            return res;
        }

        /// <summary>
        /// Creates an orthographic off-center <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left">The left bound of the view.</param>
        /// <param name="right">The right bound of the view.</param>
        /// <param name="bottom">The bottom bound of the view.</param>
        /// <param name="top">The top bound of the view.</param>
        /// <param name="near">The distance to the near plane.</param>
        /// <param name="far">The distance to the far plane.</param>
        /// <param name="result">The resulting orthographic <see cref="Matrix"/>.</param>
        public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float near, float far, out Matrix result)
        {
            result = CreateOrthographicOffCenter(left, right, bottom, top, near, far);
        }

        /// <summary>
        /// Creates a look-at view <see cref="Matrix"/>.
        /// </summary>
        /// <param name="eyePos">The camera position.</param>
        /// <param name="lookAt">The position the camera looks at.</param>
        /// <param name="up">The up-vector of the camera.</param>
        /// <returns>The resulting look-at view <see cref="Matrix"/>.</returns>
        public static unsafe Matrix CreateLookAt(Vector3 eyePos, Vector3 lookAt, Vector3 up)
        {
            var forward = (lookAt - eyePos).Normalized();
            up = up.Normalized();
            var side = forward.Cross(up).Normalized();


            var newUp = side.Cross(forward).Normalized();

            var m = default(Matrix);
            m.M11 = side.X;
            m.M12 = side.Y;
            m.M13 = side.Z;

            m.M21 = newUp.X;
            m.M22 = newUp.Y;
            m.M23 = newUp.Z;

            m.M31 = -forward.X;
            m.M32 = -forward.Y;
            m.M33 = -forward.Z;

            m.M41 = m.M42 = m.M43 = 0.0f;

            m.M14 = -side.Dot(eyePos);
            m.M24 = -newUp.Dot(eyePos);
            m.M34 = forward.Dot(eyePos);
            m.M44 = 1.0f;


            return m;
        }

        /// <summary>
        /// Creates a rotation <see cref="Matrix"/> from a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="quat">The <see cref="Quaternion"/>.</param>
        /// <returns>The resulting rotation <see cref="Matrix"/>.</returns>
        public static Matrix CreateFromQuaternion(Quaternion quat)
        {
            return CreateFromQuaternion(quat.X, quat.Y, quat.Z, quat.W);
        }

        /// <summary>
        /// Creates a rotation <see cref="Matrix"/> from quaternion coordinates.
        /// </summary>
        /// <param name="x">The x-component of the quaternion.</param>
        /// <param name="y">The y-component of the quaternion.</param>
        /// <param name="z">The z-component of the quaternion.</param>
        /// <param name="w">The w-component of the quaternion.</param>
        /// <returns>The resulting rotation <see cref="Matrix"/>.</returns>
        public static Matrix CreateFromQuaternion(float x, float y, float z, float w)
        {
            var n = w * w + x * x + y * y + z * z;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var s = n == 0 ? 0 : 2 / n;

            var sw = s * w;
            var sx = s * x;
            var sy = s * y;

            var wx = sw * x;
            var wy = sw * y;
            var wz = sw * z;
            var xx = sx * x;
            var xy = sx * y;
            var xz = sx * z;
            var yy = sy * y;
            var yz = sy * z;
            var zz = s * z * z;

            return new Matrix(1 - (yy + zz), xy - wz, xz + wy, 0,
                xy + wz, 1 - (xx + zz), yz - wx, 0,
                xz - wy, yz + wx, 1 - (xx + yy), 0,
                0, 0, 0, 1).Transposed();
        }

        /// <summary>
        /// Creates a scaling <see cref="Matrix"/>.
        /// </summary>
        /// <param name="x">The x scale component.</param>
        /// <param name="y">The y scale component.</param>
        /// <param name="z">The z scale component.</param>
        /// <returns>The resulting scaling <see cref="Matrix"/>.</returns>
        public static Matrix CreateScaling(float x, float y, float z)
        {
            return new Matrix(x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, z, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Creates a scaling <see cref="Matrix"/> from a scaling vector.
        /// </summary>
        /// <param name="vec">The scaling vector.</param>
        /// <returns>The resulting scaling <see cref="Matrix"/>.</returns>
        public static Matrix CreateScaling(Vector3 vec)
        {
            return CreateScaling(vec.X, vec.Y, vec.Z);
        }

        /// <summary>
        /// Creates a translation <see cref="Matrix"/> from a translation vector.
        /// </summary>
        /// <param name="translation">The translation vector.</param>
        /// <returns>The resulting translation <see cref="Matrix"/>.</returns>
        public static Matrix CreateTranslation(Vector3 translation)
        {
            return CreateTranslation(translation.X, translation.Y, translation.Z);
        }

        /// <summary>
        /// Creates a translation <see cref="Matrix"/>.
        /// </summary>
        /// <param name="x">The x translation component.</param>
        /// <param name="y">The y translation component.</param>
        /// <param name="z">The z translation component.</param>
        /// <returns>The resulting translation <see cref="Matrix"/>.</returns>
        public static Matrix CreateTranslation(float x, float y, float z)
        {
            var res = Identity;
            res.M14 = x;
            res.M24 = y;
            res.M34 = z;
            return res;

        }

        /// <summary>
        /// Creates a rotation <see cref="Matrix"/> rotating around the x-axis.
        /// </summary>
        /// <param name="rot">The rotation amount around the x-axis.</param>
        /// <returns>The resulting rotation <see cref="Matrix"/>.</returns>
        public static Matrix CreateRotationX(float rot)
        {
            var ret = Identity;
            ret.M22 = ret.M33 = (float)Math.Cos(rot);
            ret.M23 = (float)Math.Sin(rot);
            ret.M32 = -ret.M23;
            return ret;
        }

        /// <summary>
        /// Creates a rotation <see cref="Matrix"/> rotating around the y-axis.
        /// </summary>
        /// <param name="rot">The rotation amount around the y-axis.</param>
        /// <returns>The resulting rotation <see cref="Matrix"/>.</returns>
        public static Matrix CreateRotationY(float rot)
        {
            var ret = Identity;
            ret.M11 = ret.M33 = (float)Math.Cos(rot);
            ret.M31 = (float)Math.Sin(rot);
            ret.M13 = -ret.M31;
            return ret;
        }

        /// <summary>
        /// Creates a rotation <see cref="Matrix"/> rotating around the z-axis.
        /// </summary>
        /// <param name="rot">The rotation amount around the z-axis.</param>
        /// <returns>The resulting rotation <see cref="Matrix"/>.</returns>
        public static Matrix CreateRotationZ(float rot)
        {
            var ret = Identity;
            ret.M11 = ret.M22 = (float)Math.Cos(rot);
            ret.M21 = (float)Math.Sin(rot);
            ret.M12 = -ret.M21;
            return ret;
        }

        /// <summary>
        /// Lerps between two matrices using a given <paramref name="amount"/>.
        /// </summary>
        /// <param name="val1">The <see cref="Matrix"/> to lerp from.</param>
        /// <param name="val2">The <see cref="Matrix"/> to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>The resulting interpolated <see cref="Matrix"/>.</returns>
        public static unsafe Matrix Lerp(Matrix val1, Matrix val2, float amount)
        {
            var res = new Matrix();
            for (var i = 0; i < 16; i++)
            {
                res.items[i] = val1.items[i] + (val2.items[i] - val1.items[i]) * amount;
            }
            return res;
        }

        /// <summary>
        /// Gets the inversion <see cref="Matrix"/> of a given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="m">The <see cref="Matrix"/> to invert</param>
        /// <returns>The inverted <see cref="Matrix"/>.</returns>
        /// <exception cref="ArgumentException">Throws if the given <see cref="Matrix"/> is not invertible.</exception>
        public static Matrix Invert(Matrix m)
        {
            float det;
            var inv = default(Matrix);
            inv.M11 = m.M22 * m.M33 * m.M44 -
            m.M22 * m.M43 * m.M34 -
            m.M23 * m.M32 * m.M44 +
            m.M23 * m.M42 * m.M34 +
            m.M24 * m.M32 * m.M43 -
            m.M24 * m.M42 * m.M33;
            inv.M12 = -m.M12 * m.M33 * m.M44 +
            m.M12 * m.M43 * m.M34 +
            m.M13 * m.M32 * m.M44 -
            m.M13 * m.M42 * m.M34 -
            m.M14 * m.M32 * m.M43 +
            m.M14 * m.M42 * m.M33;
            inv.M13 = m.M12 * m.M23 * m.M44 -
            m.M12 * m.M43 * m.M24 -
            m.M13 * m.M22 * m.M44 +
            m.M13 * m.M42 * m.M24 +
            m.M14 * m.M22 * m.M43 -
            m.M14 * m.M42 * m.M23;
            inv.M14 = -m.M12 * m.M23 * m.M34 +
            m.M12 * m.M33 * m.M24 +
            m.M13 * m.M22 * m.M34 -
            m.M13 * m.M32 * m.M24 -
            m.M14 * m.M22 * m.M33 +
            m.M14 * m.M32 * m.M23;
            inv.M21 = -m.M21 * m.M33 * m.M44 +
            m.M21 * m.M43 * m.M34 +
            m.M23 * m.M31 * m.M44 -
            m.M23 * m.M41 * m.M34 -
            m.M24 * m.M31 * m.M43 +
            m.M24 * m.M41 * m.M33;
            inv.M22 = m.M11 * m.M33 * m.M44 -
            m.M11 * m.M43 * m.M34 -
            m.M13 * m.M31 * m.M44 +
            m.M13 * m.M41 * m.M34 +
            m.M14 * m.M31 * m.M43 -
            m.M14 * m.M41 * m.M33;
            inv.M23 = -m.M11 * m.M23 * m.M44 +
            m.M11 * m.M43 * m.M24 +
            m.M13 * m.M21 * m.M44 -
            m.M13 * m.M41 * m.M24 -
            m.M14 * m.M21 * m.M43 +
            m.M14 * m.M41 * m.M23;
            inv.M24 = m.M11 * m.M23 * m.M34 -
            m.M11 * m.M33 * m.M24 -
            m.M13 * m.M21 * m.M34 +
            m.M13 * m.M31 * m.M24 +
            m.M14 * m.M21 * m.M33 -
            m.M14 * m.M31 * m.M23;
            inv.M31 = m.M21 * m.M32 * m.M44 -
            m.M21 * m.M42 * m.M34 -
            m.M22 * m.M31 * m.M44 +
            m.M22 * m.M41 * m.M34 +
            m.M24 * m.M31 * m.M42 -
            m.M24 * m.M41 * m.M32;
            inv.M32 = -m.M11 * m.M32 * m.M44 +
            m.M11 * m.M42 * m.M34 +
            m.M12 * m.M31 * m.M44 -
            m.M12 * m.M41 * m.M34 -
            m.M14 * m.M31 * m.M42 +
            m.M14 * m.M41 * m.M32;
            inv.M33 = m.M11 * m.M22 * m.M44 -
            m.M11 * m.M42 * m.M24 -
            m.M12 * m.M21 * m.M44 +
            m.M12 * m.M41 * m.M24 +
            m.M14 * m.M21 * m.M42 -
            m.M14 * m.M41 * m.M22;
            inv.M34 = -m.M11 * m.M22 * m.M34 +
            m.M11 * m.M32 * m.M24 +
            m.M12 * m.M21 * m.M34 -
            m.M12 * m.M31 * m.M24 -
            m.M14 * m.M21 * m.M32 +
            m.M14 * m.M31 * m.M22;
            inv.M41 = -m.M21 * m.M32 * m.M43 +
            m.M21 * m.M42 * m.M33 +
            m.M22 * m.M31 * m.M43 -
            m.M22 * m.M41 * m.M33 -
            m.M23 * m.M31 * m.M42 +
            m.M23 * m.M41 * m.M32;
            inv.M42 = m.M11 * m.M32 * m.M43 -
            m.M11 * m.M42 * m.M33 -
            m.M12 * m.M31 * m.M43 +
            m.M12 * m.M41 * m.M33 +
            m.M13 * m.M31 * m.M42 -
            m.M13 * m.M41 * m.M32;
            inv.M43 = -m.M11 * m.M22 * m.M43 +
            m.M11 * m.M42 * m.M23 +
            m.M12 * m.M21 * m.M43 -
            m.M12 * m.M41 * m.M23 -
            m.M13 * m.M21 * m.M42 +
            m.M13 * m.M41 * m.M22;
            inv.M44 = m.M11 * m.M22 * m.M33 -
            m.M11 * m.M32 * m.M23 -
            m.M12 * m.M21 * m.M33 +
            m.M12 * m.M31 * m.M23 +
            m.M13 * m.M21 * m.M32 -
            m.M13 * m.M31 * m.M22;
            det = m.M11 * inv.M11 + m.M21 * inv.M12 + m.M31 * inv.M13 + m.M41 * inv.M14;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (det == 0)
                throw new ArgumentException("Not invertible",nameof(m));

            det = 1.0f / det;

            for (var i = 0; i < 16; i++)
                inv[i] = inv[i] * det;
            return inv;
        }

        /// <summary>
        /// The identity <see cref="Matrix"/>.
        /// </summary>
        public static readonly Matrix Identity = new Matrix(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Matrix: {{{Column0},{Column1},{Column2},{Column3}}}]";
        }
    }
}
