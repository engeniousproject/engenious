using System;

namespace engenious
{
    /// <summary>
    /// Defines a 2D floating point Rectangle.
    /// </summary>
    public struct RectangleF : IEquatable<RectangleF>
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="RectangleF"/> struct.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the <see cref="RectangleF"/>.
        /// </summary>
        public float X{ get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the <see cref="RectangleF"/>.
        /// </summary>
        public float Y{ get; set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="RectangleF"/>.
        /// </summary>
        public float Width{ get; set; }

        /// <summary>
        /// Gets or sets the height of the <see cref="RectangleF"/>.
        /// </summary>
        public float Height{ get; set; }

        /// <summary>
        /// Gets the x-coordinate of the left bound of the <see cref="RectangleF"/>.
        /// </summary>
        public float Left => X;

        /// <summary>
        /// Gets the x-coordinate of the right bound of the <see cref="RectangleF"/>.
        /// </summary>
        public float Right => X + Width;

        /// <summary>
        /// Gets the y-coordinate of the top bound of the <see cref="RectangleF"/>.
        /// </summary>
        public float Top => Y;

        /// <summary>
        /// Gets the y-coordinate of the bottom bound of the <see cref="RectangleF"/>.
        /// </summary>
        public float Bottom => Y + Height;

        /// <summary>
        /// Gets or sets the size of the <see cref="RectangleF"/>.
        /// </summary>
        public Vector2 Size
        { 
            get => new Vector2(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the location of the <see cref="RectangleF"/>.
        /// </summary>
        public Vector2 Location
        { 
            get => new(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Tests whether the <see cref="RectangleF"/> contains a point.
        /// <remarks>This test is border inclusive.</remarks>
        /// </summary>
        /// <param name="x">The x-component to test for.</param>
        /// <param name="y">The y-component to test for.</param>
        /// <returns><c>true</c> if the point is contained inside the <see cref="RectangleF"/>; otherwise <c>false</c>.</returns>
        public bool Contains(float x, float y)
        {
            return x >= X && x < X + Width && y >= Y && y < Y + Height;
        }

        /// <summary>
        /// Tests whether the <see cref="RectangleF"/> contains a point.
        /// <remarks>This test is border inclusive.</remarks>
        /// </summary>
        /// <param name="location">The location to test for.</param>
        /// <returns><c>true</c> if the point is contained inside the <see cref="RectangleF"/>; otherwise <c>false</c>.</returns>
        public bool Contains(Vector2 location)
        {
            return Contains(location.X, location.Y);
        }

        /// <summary>
        /// Tests whether the <see cref="RectangleF"/> contains another <see cref="RectangleF"/>.
        /// <remarks>This test is border inclusive.</remarks>
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to test for.</param>
        /// <returns><c>true</c> if the <see cref="RectangleF"/> is contained inside the <see cref="RectangleF"/>; otherwise <c>false</c>.</returns>
        public bool Contains(RectangleF rect)
        {
            return Contains(rect.X, rect.Y) && Contains(rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Inflates the <see cref="RectangleF"/> by <paramref name="width"/> and <paramref name="height"/> so that its origin is kept the same.
        /// </summary>
        /// <param name="width">The width to grow by.</param>
        /// <param name="height">The height to grow by.</param>
        public void Inflate(float width, float height)
        {
            X -= width;
            Y -= height;

            Width += width;
            Height += height;
        }

        
        /// <summary>
        /// Inflates the <see cref="RectangleF"/> by <paramref name="size"/> so that its origin is kept the same.
        /// </summary>
        /// <param name="size">The size to grow by.</param>
        public void Inflate(Vector2 size)
        {
            Inflate(size.X, size.Y);
        }

        /// <summary>
        /// Changes the <see cref="RectangleF"/> to be the intersection of itself and the given <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to create the intersection with.</param>
        public void Intersect(RectangleF rect)
        {
            var x = Math.Max(X, rect.X);
            var y = Math.Max(Y, rect.Y);
            var right = Math.Max(Right, rect.Right);
            var bottom = Math.Max(Bottom, rect.Bottom);
            X = x;
            Y = y;

            Width = right - x;
            Height = bottom - y;
        }

        /// <summary>
        /// Tests whether a <see cref="RectangleF"/> intersects with another <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to test intersection with.</param>
        /// <returns><c>true</c> if the rectangles intersect; otherwise <c>false</c>.</returns>
        public bool IntersectsWith(RectangleF rect)
        {
            return (X >= rect.X && X < rect.Right || rect.X >= X && rect.X < Right) &&
            (Y >= rect.Y && Y < rect.Bottom || rect.Y >= Y && rect.Y < Bottom);
        }

        /// <summary>
        /// Offsets the rectangle by a given point.
        /// </summary>
        /// <param name="x">The x-component to offset by.</param>
        /// <param name="y">The y-component to offset by.</param>
        public void Offset(float x, float y)
        {
            X += x;
            Y += y;
        }

        /// <summary>
        /// Offsets the rectangle by a given point.
        /// </summary>
        /// <param name="point">The point to offset by.</param>
        public void Offset(Vector2 point)
        {
            Offset(point.X, point.Y);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Rectangle: X={X}, Y={Y}, Width={Width}, Height={Height}]";
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Width, Height);
        }

        /// <inheritdoc />
        public bool Equals(RectangleF other)
        {
            return X == other.X && Y ==other.Y && Width == other.Width && Height == other.Height;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is RectangleF other && Equals(other);
        }

        /// <summary>
        /// Compares two <see cref="RectangleF"/> structs for equality.
        /// </summary>
        /// <param name="a">The first <see cref="RectangleF"/> to compare.</param>
        /// <param name="b">The second <see cref="RectangleF"/> to compare.</param>
        /// <returns><c>true</c> if the rectangles are the same; otherwise <c>false</c>.</returns>
        public static bool operator ==(RectangleF a, RectangleF b)
        {
            return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
        }

        /// <summary>
        /// Compares two <see cref="RectangleF"/> structs for inequality.
        /// </summary>
        /// <param name="a">The first <see cref="RectangleF"/> to compare.</param>
        /// <param name="b">The second <see cref="RectangleF"/> to compare.</param>
        /// <returns><c>true</c> if the rectangles aren't the same; otherwise <c>false</c>.</returns>
        public static bool operator !=(RectangleF a, RectangleF b)
        {
            return a.X != b.X || a.Y != b.Y || a.Width != b.Width || a.Height != b.Height;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Creates a <see cref="RectangleF"/> from its left, top, right and bottom bounds.
        /// </summary>
        /// <param name="left">The left bound.</param>
        /// <param name="top">The top bound.</param>
        /// <param name="right">The right bound.</param>
        /// <param name="bottom">The bottom bound.</param>
        /// <returns><see cref="RectangleF"/> created from the given bounds.</returns>
        public static RectangleF FromLTRB(float left, float top, float right, float bottom)
        {
            return new RectangleF(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Creates an inflated <see cref="RectangleF"/> from a given <see cref="RectangleF"/>, <paramref name="width"/> and <paramref name="height"/>.
        /// </summary>
        /// <param name="rect">The <see cref="RectangleF"/> to inflate from.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>The inflated <see cref="RectangleF"/>.</returns>
        /// <seealso cref="Inflate(float,float)"/>
        public static RectangleF Inflate(RectangleF rect, float width, float height)
        {
            return new RectangleF(rect.X - width, rect.Y - height, rect.Width + width, rect.Height + height);
        }

        /// <summary>
        /// Creates the intersection <see cref="RectangleF"/> from two given rectangles.
        /// </summary>
        /// <param name="a">The <see cref="RectangleF"/> to create the intersection with.</param>
        /// <param name="b">The <see cref="RectangleF"/> to intersect with.</param>
        /// <returns>The intersected <see cref="RectangleF"/>.</returns>
        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            var x = Math.Max(a.X, b.X);
            var y = Math.Max(a.Y, b.Y);
            var right = Math.Max(a.Right, b.Right);
            var bottom = Math.Max(a.Bottom, b.Bottom);
            return FromLTRB(x, y, right, bottom);
        }

        /// <summary>
        /// A <see cref="RectangleF"/> with its location at the origin zero width and height.
        /// </summary>
        public static readonly RectangleF Empty = new RectangleF(0, 0, 0, 0);

    }

}