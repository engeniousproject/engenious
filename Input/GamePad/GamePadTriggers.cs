using System;

namespace engenious.Input
{
    /// <summary>
    /// Defines the gamepad trigger state.
    /// </summary>
    public struct GamePadTriggers : IEquatable<GamePadTriggers>
    {
        private const float ConversionFactor = 1.0f / byte.MaxValue;
        private readonly byte _left;

        private readonly byte _right;

        /// <summary>
        /// Gets the left trigger value.
        /// </summary>
        public float Left => _left * ConversionFactor;

        /// <summary>
        /// Gets the right trigger value.
        /// </summary>
        public float Right => _right * ConversionFactor;

        internal GamePadTriggers(byte left, byte right)
        {
            _left = left;
            _right = right;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is GamePadTriggers && Equals((GamePadTriggers)obj);
        }

        /// <inheritdoc />
        public bool Equals(GamePadTriggers other)
        {
            return _left == other._left && _right == other._right;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _left.GetHashCode() ^ _right.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{{Left: {0:f2}; Right: {1:f2}}}", Left, Right);
        }

        /// <summary>
        /// Tests two <see cref="GamePadTriggers"/> structs for equality.
        /// </summary>
        /// <param name="left">The first <see cref="GamePadTriggers"/> to test with.</param>
        /// <param name="right">The second <see cref="GamePadTriggers"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="GamePadTriggers"/> structs are equal; otherwise <c>false</c>.</returns>  
        public static bool operator ==(GamePadTriggers left, GamePadTriggers right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests two <see cref="GamePadTriggers"/> structs for inequality.
        /// </summary>
        /// <param name="left">The first <see cref="GamePadTriggers"/> to test with.</param>
        /// <param name="right">The second <see cref="GamePadTriggers"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="GamePadTriggers"/> structs aren't equal; otherwise <c>false</c>.</returns> 
        public static bool operator !=(GamePadTriggers left, GamePadTriggers right)
        {
            return !left.Equals(right);
        }
    }
}

