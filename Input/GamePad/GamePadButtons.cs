using System;

namespace engenious.Input
{
    /// <summary>
    /// A struct for easy access to the <see cref="Buttons"/> enumeration.
    /// </summary>
    public struct GamePadButtons : IEquatable<GamePadButtons>
    {
        private readonly Buttons _buttons;

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the A button.
        /// </summary>
        public ButtonState A => GetButton(Buttons.A);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the B button.
        /// </summary>
        public ButtonState B => GetButton(Buttons.B);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the Back button.
        /// </summary>
        public ButtonState Back => GetButton(Buttons.Back);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the Right button.
        /// </summary>
        public ButtonState BigButton => GetButton(Buttons.Home);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the left shoulder button.
        /// </summary>
        public ButtonState LeftShoulder => GetButton(Buttons.LeftShoulder);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the left stick button.
        /// </summary>
        public ButtonState LeftStick => GetButton(Buttons.LeftStick);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the right shoulder button.
        /// </summary>
        public ButtonState RightShoulder => GetButton(Buttons.RightShoulder);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the right stick button.
        /// </summary>
        public ButtonState RightStick => GetButton(Buttons.RightStick);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the start button.
        /// </summary>
        public ButtonState Start => GetButton(Buttons.Start);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the X button.
        /// </summary>
        public ButtonState X => GetButton(Buttons.X);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the Y button.
        /// </summary>
        public ButtonState Y => GetButton(Buttons.Y);

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadButtons"/> struct.
        /// </summary>
        /// <param name="state">The gamepad button state.</param>
        public GamePadButtons(Buttons state)
        {
            _buttons = state;
        }

        /// <inheritdoc />
        public bool Equals(GamePadButtons other)
        {
            return _buttons == other._buttons;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is GamePadButtons && Equals((GamePadButtons)obj);
        }

        /// <summary>
        /// Get the <see cref="ButtonState"/> of a given button.
        /// </summary>
        /// <param name="b">The button to get the <see cref="ButtonState"/> of.</param>
        /// <returns>The <see cref="ButtonState"/> of the button.</returns>
        private ButtonState GetButton(Buttons b)
        {
            return (_buttons & b) == 0 ? ButtonState.Released : ButtonState.Pressed;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _buttons.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Convert.ToString((int)_buttons, 2).PadLeft(10, '0');
        }

        /// <summary>
        /// Tests two <see cref="GamePadButtons"/> structs for equality.
        /// </summary>
        /// <param name="left">The first <see cref="GamePadButtons"/> to test with.</param>
        /// <param name="right">The second <see cref="GamePadButtons"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="GamePadButtons"/> structs are equal; otherwise <c>false</c>.</returns>   
        public static bool operator ==(GamePadButtons left, GamePadButtons right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests two <see cref="GamePadButtons"/> structs for inequality.
        /// </summary>
        /// <param name="left">The first <see cref="GamePadButtons"/> to test with.</param>
        /// <param name="right">The second <see cref="GamePadButtons"/> to test with.</param>
        /// <returns><c>true</c> if the <see cref="GamePadButtons"/> structs aren't equal; otherwise <c>false</c>.</returns>   
        public static bool operator !=(GamePadButtons left, GamePadButtons right)
        {
            return !left.Equals(right);
        }
    }
}

