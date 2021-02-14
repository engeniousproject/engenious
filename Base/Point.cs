using System;
using System.ComponentModel;
using OpenTK.Mathematics;

namespace engenious
{
    /// <summary>
    /// A 2D integer point.
    /// </summary>
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct.
        /// </summary>
        /// <param name="x">The X-component of the point.</param>
        /// <param name="y">The Y-component of the point.</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the X-component.
        /// </summary>
        public int X{ get; set; }

        /// <summary>
        /// Gets or sets the Y-component.
        /// </summary>
        public int Y{ get; set; }

        /// <summary>
        /// Gets a <see cref="bool"/> indicating whether the point is at the origin.
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty => X == 0 && Y == 0;

        /// <summary>
        /// Converts the <see cref="Point"/> to a <see cref="Vector2"/>.
        /// </summary>
        /// <returns>The resulting <see cref="Vector2"/>.</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        /// <summary>
        /// A <see cref="Point"/> at the origin.
        /// </summary>
        public static readonly Point Zero = new Point();

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Point: X={X}, Y={Y}]";
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return X ^ Y;//TODO
        }

        /// <inheritdoc />
        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is Point point && Equals(point);
        }

        /// <summary>
        /// Tests two <see cref="Point"/> structs for equality.
        /// </summary>
        /// <param name="a">The first <see cref="Point"/> to test with.</param>
        /// <param name="b">The second <see cref="Point"/> to test with.</param>
        /// <returns><c>true</c> if the points are equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        /// <summary>
        /// Tests two <see cref="Point"/> structs for inequality.
        /// </summary>
        /// <param name="a">The first <see cref="Point"/> to test with.</param>
        /// <param name="b">The second <see cref="Point"/> to test with.</param>
        /// <returns><c>true</c> if the points aren't equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(Point a, Point b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        /// <summary>
        /// Adds up two points.
        /// </summary>
        /// <param name="value1">The first <see cref="Point"/> summand.</param>
        /// <param name="value2">The second <see cref="Point"/> summand.</param>
        /// <returns>The summed up <see cref="Color"/>.</returns>
        public static Point operator +(Point value1, Point value2)
        {
            return new Point(value1.X + value2.X, value1.Y + value2.Y);
        }

        /// <summary>
        /// Divides two points componentwise.
        /// </summary>
        /// <param name="dividend">The <see cref="Point"/> dividend.</param>
        /// <param name="divisor">The <see cref="Point"/> divisor.</param>
        /// <returns>The componentwise divided <see cref="Point"/>.</returns>
        public static Point operator /(Point dividend, Point divisor)
        {
            return new Point(dividend.X / divisor.X, dividend.Y / divisor.Y);
        }

        /// <summary>
        /// Subtracts two points.
        /// </summary>
        /// <param name="value1">The <see cref="Point"/> minuend.</param>
        /// <param name="value2">The <see cref="Point"/> subtrahend.</param>
        /// <returns>The difference of the <see cref="Point"/>s.</returns>
        public static Point operator -(Point value1, Point value2)
        {
            return new Point(value1.X - value2.X, value1.Y - value2.Y);
        }

        /// <summary>
        /// Implicitly converts the <see cref="System.Drawing.Point"/> to a <see cref="Point"/>.
        /// </summary>
        /// <param name="col">The <see cref="System.Drawing.Point"/>.</param>
        /// <returns>The resulting <see cref="Point"/>.</returns>
        public static implicit operator Point(Vector2i col)
        {
            return new Point(col.X, col.Y);
        }
    }

}