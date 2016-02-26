using System;

namespace engenious.Input
{
    public struct GamePadThumbSticks : IEquatable<GamePadThumbSticks>
    {
        const float ConversionFactor = 1.0f / short.MaxValue;
        short left_x, left_y;
        short right_x, right_y;
        //
        // Properties
        //
        public Vector2 Left
        {
            get
            {
                return new Vector2((float)this.left_x * ConversionFactor, (float)this.left_y * ConversionFactor);
            }
        }

        public Vector2 Right
        {
            get
            {
                return new Vector2((float)this.right_x * ConversionFactor, (float)this.right_y * ConversionFactor);
            }
        }

        //
        // Constructors
        //
        internal GamePadThumbSticks(short left_x, short left_y, short right_x, short right_y)
        {
            this.left_x = left_x;
            this.left_y = left_y;
            this.right_x = right_x;
            this.right_y = right_y;
        }

        //
        // Methods
        //
        public override bool Equals(object obj)
        {
            return obj is GamePadThumbSticks && this.Equals((GamePadThumbSticks)obj);
        }

        public bool Equals(GamePadThumbSticks other)
        {
            return this.left_x == other.left_x && this.left_y == other.left_y && this.right_x == other.right_x && this.right_y == other.right_y;
        }

        public override int GetHashCode()
        {
            return this.left_x.GetHashCode() ^ this.left_y.GetHashCode() ^ this.right_x.GetHashCode() ^ this.right_y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{{Left: ({0:f4}; {1:f4}); Right: ({2:f4}; {3:f4})}}", new object[]
                {
                    this.Left.X,
                    this.Left.Y,
                    this.Right.X,
                    this.Right.Y
                });
        }

        //
        // Operators
        //
        public static bool operator ==(GamePadThumbSticks left, GamePadThumbSticks right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GamePadThumbSticks left, GamePadThumbSticks right)
        {
            return !left.Equals(right);
        }
    }
}

