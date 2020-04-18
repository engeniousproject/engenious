using System;

namespace engenious.Input
{
    /// <summary>
    /// Defines a gamepad D-Pad.
    /// </summary>
    public struct GamePadDPad : IEquatable<GamePadDPad>
    {
        private DPadButtons _buttons;

        /// <summary>
        /// Gets or sets whether the down D-Pad button is pressed.
        /// </summary>
        public bool IsDown
        {
            get
            {
                return (byte)(_buttons & DPadButtons.Down) != 0;
            }
            internal set
            {
                SetButton(DPadButtons.Down, value);
            }
        }

        /// <summary>
        /// Gets or sets whether the left D-Pad button is pressed.
        /// </summary>
        public bool IsLeft
        {
            get
            {
                return (byte)(_buttons & DPadButtons.Left) != 0;
            }
            internal set
            {
                SetButton(DPadButtons.Left, value);
            }
        }

        /// <summary>
        /// Gets or sets whether the right D-Pad button is pressed.
        /// </summary>
        public bool IsRight
        {
            get
            {
                return (byte)(_buttons & DPadButtons.Right) != 0;
            }
            internal set
            {
                SetButton(DPadButtons.Right, value);
            }
        }

        /// <summary>
        /// Gets or sets whether the up D-Pad button is pressed.
        /// </summary>
        public bool IsUp
        {
            get
            {
                return (byte)(_buttons & DPadButtons.Up) != 0;
            }
            internal set
            {
                SetButton(DPadButtons.Up, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the down D-Pad button.
        /// </summary>
        public ButtonState Down => !IsDown ? ButtonState.Released : ButtonState.Pressed;

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the left D-Pad button.
        /// </summary>
        public ButtonState Left => !IsLeft ? ButtonState.Released : ButtonState.Pressed;

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the right D-Pad button.
        /// </summary>
        public ButtonState Right => !IsRight ? ButtonState.Released : ButtonState.Pressed;

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the up D-Pad button.
        /// </summary>
        public ButtonState Up => !IsUp ? ButtonState.Released : ButtonState.Pressed;
        
        internal GamePadDPad(Buttons state)
        {
            _buttons = (DPadButtons)(state & (Buttons.DPadUp | Buttons.DPadDown | Buttons.DPadLeft | Buttons.DPadRight));
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is GamePadDPad && Equals((GamePadDPad)obj);
        }

        /// <inheritdoc />
        public bool Equals(GamePadDPad other)
        {
            return _buttons == other._buttons;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _buttons.GetHashCode();
        }

        private void SetButton(DPadButtons button, bool value)
        {
            if (value)
            {
                _buttons |= button;
                return;
            }
            _buttons &= ~button;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{{{0}{1}{2}{3}}}", IsUp ? "U" : string.Empty, IsLeft ? "L" : string.Empty, IsDown ? "D" : string.Empty, IsRight ? "R" : string.Empty);
        }

        /// <summary>
        /// Tests two <see cref="GamePadDPad"/> structs for equality.
        /// </summary>
        /// <param name="left">The first <see cref="GamePadDPad"/> to test with.</param>
        /// <param name="right">The second <see cref="GamePadDPad"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="GamePadDPad"/> structs are equal; otherwise <c>false</c>.</returns>   
        public static bool operator ==(GamePadDPad left, GamePadDPad right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests two <see cref="GamePadDPad"/> structs for inequality.
        /// </summary>
        /// <param name="left">The first <see cref="GamePadDPad"/> to test with.</param>
        /// <param name="right">The second <see cref="GamePadDPad"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="GamePadDPad"/> structs aren't equal; otherwise <c>false</c>.</returns>   
        public static bool operator !=(GamePadDPad left, GamePadDPad right)
        {
            return !left.Equals(right);
        }

        [Flags]
        private enum DPadButtons : byte
        {
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }
    }
}

