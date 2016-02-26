using System;

namespace engenious
{
    public struct Rectangle
    {
        public Rectangle(Point location, Point size)
            : this(location.X, location.Y, size.X, size.Y)
        {
        }

        public Rectangle(Point location, Size size)
            : this(location.X, location.Y, size.Width, size.Height)
        {
        }

        public Rectangle(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public int X{ get; set; }

        public int Y{ get; set; }

        public int Width{ get; set; }

        public int Height{ get; set; }

        public int Left{ get { return X; } }

        public int Right{ get { return X + Width; } }

        public int Top{ get { return Y; } }

        public int Bottom{ get { return Y + Height; } }

        public Size Size
        { 
            get { return new Size(Width, Height); }
            set
            {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        public Point Location
        { 
            get { return new Point(X, Y); } 
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public bool Contains(int x, int y)
        {
            return x >= X && x < X + Width && y >= Y && y < Y + Height;
        }

        public bool Contains(Point location)
        {
            return Contains(location.X, location.Y);
        }

        public bool Contains(Rectangle rect)
        {
            return Contains(rect.X, rect.Y) && Contains(rect.Right, rect.Bottom);
        }

        public void Inflate(int width, int height)
        {
            X -= width;
            Y -= height;

            Width += width;
            Height += height;
        }

        public void Inflate(Size size)
        {
            Inflate(size.Width, size.Height);
        }

        public void Intersect(Rectangle rect)
        {
            int x = Math.Max(X, rect.X);
            int y = Math.Max(Y, rect.Y);
            int right = Math.Max(Right, rect.Right);
            int bottom = Math.Max(Bottom, rect.Bottom);
            this.X = x;
            this.Y = y;

            this.Width = right - x;
            this.Height = bottom - y;
        }

        public bool IntersectsWith(Rectangle rect)
        {
            return (X >= rect.X && X < rect.Right || rect.X >= X && rect.X < Right) &&
            (Y >= rect.Y && Y < rect.Bottom || rect.Y >= Y && rect.Y < Bottom);
        }

        public void Offset(int x, int y)
        {
            X += x;
            Y += y;
        }

        public void Offset(Point location)
        {
            Offset(location.X, location.Y);
        }

        public override string ToString()
        {
            return string.Format("[Rectangle: X={0}, Y={1}, Width={2}, Height={3}]", X, Y, Width, Height);
        }

        public override int GetHashCode()
        {
            return (Height + Width) ^ X + Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Rectangle)
            {
                Rectangle sec = (Rectangle)obj;
                return X == sec.X && Y == sec.Y && Width == sec.Width && Height == sec.Height;
            }
            return false;
        }

        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
        }

        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return a.X != b.X || a.Y != b.Y || a.Width != b.Width || a.Height != b.Height;
        }

        public static Rectangle FromLTRB(int left, int top, int right, int bottom)
        {
            return new Rectangle(left, top, right - left, bottom - top);
        }

        public static Rectangle Inflate(Rectangle rect, int width, int height)
        {
            return new Rectangle(rect.X - width, rect.Y - height, rect.Width + width, rect.Height + height);
        }


        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            int x = Math.Max(a.X, b.X);
            int y = Math.Max(a.Y, b.Y);
            int right = Math.Max(a.Right, b.Right);
            int bottom = Math.Max(a.Bottom, b.Bottom);
            return Rectangle.FromLTRB(x, y, right, bottom);
        }

        public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);


        /*public static Rectangle Ceiling(RectangleF value)
        {
        }*/
    }

}