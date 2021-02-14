using System;

namespace engenious
{
    /// <summary>
    /// Defines a 2D size.
    /// </summary>
    public struct Size : IEquatable<Size>
    {
        /// <summary>
        /// Initializes a new <see cref="Size"/> struct from a point.
        /// </summary>
        /// <param name="point">The point to take the <see cref="Width"/> and <see cref="Height"/> from.</param>
        public Size(Point point)
        {
            Width = point.X;
            Height = point.Y;
        }

        /// <summary>
        /// Initializes a new <see cref="Size"/> struct from a point.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        public int Width{ get; set; }

        /// <summary>
        /// Gets or sets the Height.
        /// </summary>
        public int Height{ get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Size: Width={Width}, Height={Height}]";
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Width ^ (Height * 397);//TODO
        }

        /// <inheritdoc />
        public bool Equals(Size other)
        {
            return Width == other.Width && Height == other.Height;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is Size size && Equals(size);
        }

        /// <summary>
        /// Compares two <see cref="Size"/> structs for equality.
        /// </summary>
        /// <param name="a">The first <see cref="Size"/> to compare.</param>
        /// <param name="b">The second <see cref="Size"/> to compare.</param>
        /// <returns><c>true</c> if the rectangles are the same; otherwise <c>false</c>.</returns>
        public static bool operator ==(Size a, Size b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Compares two <see cref="Size"/> structs for inequality.
        /// </summary>
        /// <param name="a">The first <see cref="Size"/> to compare.</param>
        /// <param name="b">The second <see cref="Size"/> to compare.</param>
        /// <returns><c>true</c> if the size aren't the same; otherwise <c>false</c>.</returns>
        public static bool operator !=(Size a, Size b)
        {
            return a.Width != b.Width || a.Height != b.Height;
        }

        /// <summary>
        /// Adds up two sizes.
        /// </summary>
        /// <param name="value1">The first <see cref="Size"/> summand.</param>
        /// <param name="value2">The second <see cref="Size"/> summand.</param>
        /// <returns>The summed up <see cref="Size"/>.</returns>
        public static Size operator +(Size value1, Size value2)
        {
            return new Size(value1.Width + value2.Width, value1.Height + value2.Height);
        }

        /// <summary>
        /// Divides two sizes componentwise.
        /// </summary>
        /// <param name="dividend">The <see cref="Size"/> dividend.</param>
        /// <param name="divisor">The <see cref="Size"/> divisor.</param>
        /// <returns>The componentwise divided <see cref="Size"/>.</returns>
        public static Size operator /(Size dividend, Size divisor)
        {
            return new Size(dividend.Width / divisor.Width, dividend.Height / divisor.Height);
        }

        /// <summary>
        /// Multiplies two sizes componentwise.
        /// </summary>
        /// <param name="value1">The first <see cref="Size"/> factor.</param>
        /// <param name="value2">The second <see cref="Size"/> factor.</param>
        /// <returns>The componentwise multiplied <see cref="Size"/>.</returns>
        public static Size operator *(Size value1, Size value2)
        {
            return new Size(value1.Width * value2.Width, value1.Height * value2.Height);
        }

        /// <summary>
        /// Subtracts two sizes.
        /// </summary>
        /// <param name="value1">The <see cref="Size"/> minuend.</param>
        /// <param name="value2">The <see cref="Size"/> subtrahend.</param>
        /// <returns>The difference of the <see cref="Size"/>s.</returns>
        public static Size operator -(Size value1, Size value2)
        {
            return new Size(value1.Width - value2.Width, value1.Height - value2.Height);
        }

        /// <summary>
        /// Implicitly converts the <see cref="System.Drawing.Size"/> to a <see cref="Size"/>.
        /// </summary>
        /// <param name="col">The <see cref="System.Drawing.Size"/>.</param>
        /// <returns>The resulting <see cref="Size"/>.</returns>
        public static implicit operator Size(System.Drawing.Size col)
        {
            return new Size(col.Width, col.Width);
        }
    }
}

