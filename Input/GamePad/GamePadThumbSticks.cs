using System;

namespace engenious.Input
{
    /// <summary>
    /// Defines gamepad thumbstick states.
    /// </summary>
    public struct GamePadThumbSticks : IEquatable<GamePadThumbSticks>
    {
        private readonly float _leftX, _leftY;
        private readonly float _rightX, _rightY;

        private const short LeftThumbDeadZone = 7864;//0.24f * short.MaxValue;//MonoGame
        private const short RightThumbDeadZone = 8683;

        /// <summary>
        /// Gets the position of the left thumbstick.
        /// </summary>
        public Vector2 Left => new Vector2(_leftX, _leftY);

        /// <summary>
        /// Gets the position of the right thumbstick.
        /// </summary>
        public Vector2 Right => new Vector2(_rightX, _rightY);

        internal GamePadThumbSticks(float leftX, float leftY, float rightX, float rightY)
        {
            _leftX = ExcludeAxisDeadZone(leftX, LeftThumbDeadZone);//TODO: circular dead zone?
            _leftY = ExcludeAxisDeadZone(leftY, LeftThumbDeadZone);
            _rightX = ExcludeAxisDeadZone(rightX, RightThumbDeadZone);
            _rightY = ExcludeAxisDeadZone(rightY, RightThumbDeadZone);
        }
        private static float ExcludeAxisDeadZone(float value, short deadZone)
        {
            return value;
            // TODO:
            //if (value < -deadZone)
            //    value += deadZone;
            //else if (value > deadZone)
            //    value -= deadZone;
            //else
            //    return 0;
            //return (short)(value / (short.MaxValue - deadZone));
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is GamePadThumbSticks sticks && Equals(sticks);
        }

        /// <inheritdoc />
        public bool Equals(GamePadThumbSticks other)
        {
            return _leftX == other._leftX && _leftY == other._leftY && _rightX == other._rightX && _rightY == other._rightY;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(_leftX, _leftY, _rightX, _rightY);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{{Left: ({0:f4}; {1:f4}); Right: ({2:f4}; {3:f4})}}", Left.X, Left.Y, Right.X, Right.Y);
        }

        /// <summary>
        /// Tests two <see cref="GamePadThumbSticks"/> structs for equality.
        /// </summary>
        /// <param name="left">The first <see cref="GamePadThumbSticks"/> to test with.</param>
        /// <param name="right">The second <see cref="GamePadThumbSticks"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="GamePadThumbSticks"/> structs are equal; otherwise <c>false</c>.</returns>   
        public static bool operator ==(GamePadThumbSticks left, GamePadThumbSticks right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests two <see cref="GamePadThumbSticks"/> structs for inequality.
        /// </summary>
        /// <param name="left">The first <see cref="GamePadThumbSticks"/> to test with.</param>
        /// <param name="right">The second <see cref="GamePadThumbSticks"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="GamePadThumbSticks"/> structs aren't equal; otherwise <c>false</c>.</returns>  
        public static bool operator !=(GamePadThumbSticks left, GamePadThumbSticks right)
        {
            return !left.Equals(right);
        }
    }
}

