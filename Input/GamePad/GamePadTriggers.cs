using System;

namespace engenious.Input
{
    public struct GamePadTriggers : IEquatable<GamePadTriggers>
    {
        const float ConversionFactor = 1.0f / byte.MaxValue;
        byte left;
        byte right;
        //
        // Properties
        //
        public float Left
        {
            get
            {
                return (float)this.left * ConversionFactor;
            }
        }

        public float Right
        {
            get
            {
                return (float)this.right * ConversionFactor;
            }
        }

        //
        // Constructors
        //
        internal GamePadTriggers(byte left, byte right)
        {
            this.left = left;
            this.right = right;
        }

        //
        // Methods
        //
        public override bool Equals(object obj)
        {
            return obj is GamePadTriggers && this.Equals((GamePadTriggers)obj);
        }

        public bool Equals(GamePadTriggers other)
        {
            return this.left == other.left && this.right == other.right;
        }

        public override int GetHashCode()
        {
            return this.left.GetHashCode() ^ this.right.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{{Left: {0:f2}; Right: {1:f2}}}", this.Left, this.Right);
        }

        //
        // Operators
        //
        public static bool operator ==(GamePadTriggers left, GamePadTriggers right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GamePadTriggers left, GamePadTriggers right)
        {
            return !left.Equals(right);
        }
    }
}

