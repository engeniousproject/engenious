﻿using System;

namespace engenious.Input
{
    /// <summary>
    /// Defines gamepad thumbstick states.
    /// </summary>
    public struct GamePadThumbSticks : IEquatable<GamePadThumbSticks>
    {
        private const float ConversionFactor = 1.0f / short.MaxValue;
        private readonly short _leftX, _leftY;
        private readonly short _rightX, _rightY;

        private const short LeftThumbDeadZone = 7864;//0.24f * short.MaxValue;//MonoGame
        private const short RightThumbDeadZone = 8683;

        /// <summary>
        /// Gets the position of the left thumbstick.
        /// </summary>
        public Vector2 Left => new Vector2(_leftX * ConversionFactor, _leftY * ConversionFactor);

        /// <summary>
        /// Gets the position of the right thumbstick.
        /// </summary>
        public Vector2 Right => new Vector2(_rightX * ConversionFactor, _rightY * ConversionFactor);
        
        internal GamePadThumbSticks(short leftX, short leftY, short rightX, short rightY)
        {
            _leftX = ExcludeAxisDeadZone(leftX, LeftThumbDeadZone);//TODO: circular dead zone?
            _leftY = ExcludeAxisDeadZone(leftY, LeftThumbDeadZone);
            _rightX = ExcludeAxisDeadZone(rightX, RightThumbDeadZone);
            _rightY = ExcludeAxisDeadZone(rightY, RightThumbDeadZone);
        }
        private static short ExcludeAxisDeadZone(short value, short deadZone)
        {
            if (value < -deadZone)
                value += deadZone;
            else if (value > deadZone)
                value -= deadZone;
            else
                return 0;
            return (short)(value / (short.MaxValue - deadZone));
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
            return _leftX.GetHashCode() ^ _leftY.GetHashCode() ^ _rightX.GetHashCode() ^ _rightY.GetHashCode();
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

