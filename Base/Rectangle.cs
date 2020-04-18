using System;

namespace engenious
{
    /// <summary>
    /// Defines a 2D integer rectangle.
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="location">The upper left corner of the <see cref="Rectangle"/>.</param>
        /// <param name="size">The size of the <see cref="Rectangle"/>.</param>
        public Rectangle(Point location, Point size)
            : this(location.X, location.Y, size.X, size.Y)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="location">The upper left corner of the <see cref="Rectangle"/>.</param>
        /// <param name="size">The size of the <see cref="Rectangle"/>.</param>
        public Rectangle(Point location, Size size)
            : this(location.X, location.Y, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the <see cref="Rectangle"/>.
        /// </summary>
        public int X{ get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the <see cref="Rectangle"/>.
        /// </summary>
        public int Y{ get; set; }

        /// <summary>
        /// Gets or sets the Width of the <see cref="Rectangle"/>.
        /// </summary>
        public int Width{ get; set; }

        /// <summary>
        /// Gets or sets the Height of the <see cref="Rectangle"/>.
        /// </summary>
        public int Height{ get; set; }

        /// <summary>
        /// Gets the x-coordinate of the left bound of the <see cref="Rectangle"/>.
        /// </summary>
        public int Left => X;

        /// <summary>
        /// Gets the x-coordinate of the right bound of the <see cref="Rectangle"/>.
        /// </summary>
        public int Right => X + Width;

        /// <summary>
        /// Gets the y-coordinate of the top bound of the <see cref="Rectangle"/>.
        /// </summary>
        public int Top => Y;

        /// <summary>
        /// Gets the y-coordinate of the bottom bound of the <see cref="Rectangle"/>.
        /// </summary>
        public int Bottom => Y + Height;

        /// <summary>
        /// Gets or sets the size of the <see cref="Rectangle"/>.
        /// </summary>
        public Size Size
        { 
            get { return new Size(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the location of the <see cref="Rectangle"/>.
        /// </summary>
        public Point Location
        { 
            get { return new Point(X, Y); } 
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Tests whether the <see cref="Rectangle"/> contains a point.
        /// <remarks>This test is border inclusive.</remarks>
        /// </summary>
        /// <param name="x">The x-component to test for.</param>
        /// <param name="y">The y-component to test for.</param>
        /// <returns><c>true</c> if the point is contained inside the <see cref="Rectangle"/>; otherwise <c>false</c>.</returns>
        public bool Contains(int x, int y)
        {
            return x >= X && x < X + Width && y >= Y && y < Y + Height;
        }

        /// <summary>
        /// Tests whether the <see cref="Rectangle"/> contains a point.
        /// <remarks>This test is border inclusive.</remarks>
        /// </summary>
        /// <param name="location">The location to test for.</param>
        /// <returns><c>true</c> if the point is contained inside the <see cref="Rectangle"/>; otherwise <c>false</c>.</returns>
        public bool Contains(Point location)
        {
            return Contains(location.X, location.Y);
        }

        /// <summary>
        /// Tests whether the <see cref="Rectangle"/> contains another <see cref="Rectangle"/>.
        /// <remarks>This test is border inclusive.</remarks>
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to test for.</param>
        /// <returns><c>true</c> if the <see cref="Rectangle"/> is contained inside the <see cref="Rectangle"/>; otherwise <c>false</c>.</returns>
        public bool Contains(Rectangle rect)
        {
            return Contains(rect.X, rect.Y) && Contains(rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Inflates the <see cref="Rectangle"/> by <paramref name="width"/> and <paramref name="height"/> so that its origin is kept the same.
        /// <remarks>Origin is only kept with even inputs; otherwise it grows less in X/Y direction.</remarks>
        /// </summary>
        /// <param name="width">The width to grow by.</param>
        /// <param name="height">The height to grow by.</param>
        public void Inflate(int width, int height)
        {
            X -= width / 2;
            Y -= height / 2;

            Width += (width + 1) / 2;
            Height += (height + 1) / 2;
        }

        /// <summary>
        /// Inflates the <see cref="Rectangle"/> by <paramref name="size"/> so that its origin is kept the same.
        /// <remarks>Origin is only kept with even inputs; otherwise it grows less in X/Y direction.</remarks>
        /// </summary>
        /// <param name="size">The size to grow by.</param>
        public void Inflate(Size size)
        {
            Inflate(size.Width, size.Height);
        }

        /// <summary>
        /// Changes the <see cref="Rectangle"/> to be the intersection of itself and the given <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to create the intersection with.</param>
        public void Intersect(Rectangle rect)
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
        /// Tests whether a <see cref="Rectangle"/> intersects with another <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to test intersection with.</param>
        /// <returns><c>true</c> if the rectangles intersect; otherwise <c>false</c>.</returns>
        public bool IntersectsWith(Rectangle rect)
        {
            return (X >= rect.X && X < rect.Right || rect.X >= X && rect.X < Right) &&
            (Y >= rect.Y && Y < rect.Bottom || rect.Y >= Y && rect.Y < Bottom);
        }

        /// <summary>
        /// Offsets the rectangle by a given point.
        /// </summary>
        /// <param name="x">The x-component to offset by.</param>
        /// <param name="y">The y-component to offset by.</param>
        public void Offset(int x, int y)
        {
            X += x;
            Y += y;
        }

        /// <summary>
        /// Offsets the rectangle by a given point.
        /// </summary>
        /// <param name="point">The point to offset by.</param>
        public void Offset(Point point)
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
            return (((((Height * 397) ^ Width) * 397) ^ Y) * 397) ^ X;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Rectangle)
            {
                var sec = (Rectangle)obj;
                return X == sec.X && Y == sec.Y && Width == sec.Width && Height == sec.Height;
            }
            return false;
        }

        /// <summary>
        /// Compares two <see cref="Rectangle"/> structs for equality.
        /// </summary>
        /// <param name="a">The first <see cref="Rectangle"/> to compare.</param>
        /// <param name="b">The second <see cref="Rectangle"/> to compare.</param>
        /// <returns><c>true</c> if the rectangles are the same; otherwise <c>false</c>.</returns>
        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
        }

        /// <summary>
        /// Compares two <see cref="Rectangle"/> structs for inequality.
        /// </summary>
        /// <param name="a">The first <see cref="Rectangle"/> to compare.</param>
        /// <param name="b">The second <see cref="Rectangle"/> to compare.</param>
        /// <returns><c>true</c> if the rectangles aren't the same; otherwise <c>false</c>.</returns>
        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return a.X != b.X || a.Y != b.Y || a.Width != b.Width || a.Height != b.Height;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Creates a <see cref="Rectangle"/> from its left, top, right and bottom bounds.
        /// </summary>
        /// <param name="left">The left bound.</param>
        /// <param name="top">The top bound.</param>
        /// <param name="right">The right bound.</param>
        /// <param name="bottom">The bottom bound.</param>
        /// <returns><see cref="Rectangle"/> created from the given bounds.</returns>
        public static Rectangle FromLTRB(int left, int top, int right, int bottom)
        {
            return new Rectangle(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Creates an inflated <see cref="Rectangle"/> from a given <see cref="Rectangle"/>, <paramref name="width"/> and <paramref name="height"/>.
        /// <remarks>Origin is only kept with even inputs; otherwise it grows less in X/Y direction.</remarks>
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to inflate from.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>The inflated <see cref="Rectangle"/>.</returns>
        /// <seealso cref="Inflate(int,int)"/>
        public static Rectangle Inflate(Rectangle rect, int width, int height)
        {
            return new Rectangle(rect.X - width / 2, rect.Y - height / 2, rect.Width + (width + 1) / 2, rect.Height + (height + 1) / 2);
        }

        /// <summary>
        /// Creates the intersection <see cref="Rectangle"/> from two given rectangles.
        /// </summary>
        /// <param name="a">The <see cref="Rectangle"/> to create the intersection with.</param>
        /// <param name="b">The <see cref="Rectangle"/> to intersect with.</param>
        /// <returns>The intersected <see cref="Rectangle"/>.</returns>
        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            var x = Math.Max(a.X, b.X);
            var y = Math.Max(a.Y, b.Y);
            var right = Math.Max(a.Right, b.Right);
            var bottom = Math.Max(a.Bottom, b.Bottom);
            return FromLTRB(x, y, right, bottom);
        }

        /// <summary>
        /// A <see cref="Rectangle"/> with its location at the origin zero width and height.
        /// </summary>
        public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);
    }

}