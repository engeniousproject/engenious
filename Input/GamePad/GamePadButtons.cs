using System;
using System.Collections.Generic;
using System.Text;

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
        /// Gets the <see cref="ButtonState"/> of the X button.
        /// </summary>
        public ButtonState X => GetButton(Buttons.X);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the Y button.
        /// </summary>
        public ButtonState Y => GetButton(Buttons.Y);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the left bumper button.
        /// </summary>
        public ButtonState LeftBumper => GetButton(Buttons.LeftBumper);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the right bumper button.
        /// </summary>
        public ButtonState RightBumper => GetButton(Buttons.RightBumper);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the Back button.
        /// </summary>
        public ButtonState Back => GetButton(Buttons.Back);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the start button.
        /// </summary>
        public ButtonState Start => GetButton(Buttons.Start);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the guide button.
        /// </summary>
        public ButtonState Guide => GetButton(Buttons.Guide);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the left thumb button.
        /// </summary>
        public ButtonState LeftThumb => GetButton(Buttons.LeftThumb);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> of the right thumb button.
        /// </summary>
        public ButtonState RightThumb => GetButton(Buttons.RightThumb);

        /// <summary>
        /// Gets the <see cref="ButtonState"/> at the given index.
        /// </summary>
        /// <param name="button">The button index.</param>
        /// <returns>The <see cref="ButtonState"/> at the given index.</returns>
        public ButtonState this[int button] => GetButton(button);

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
        public override bool Equals(object? obj)
        {
            return obj is GamePadButtons buttons && Equals(buttons);
        }

        /// <summary>
        /// Gets a value indicating whether the given <see cref="Buttons"/> flag is pressed down.
        /// </summary>
        /// <param name="b">The buttons to check.</param>
        /// <returns></returns>
        /// <remarks>
        /// If multiple buttons are given the returned value is only <c>true</c>,
        /// if all buttons given in the flag are pressed.
        /// </remarks>
        public bool IsButtonDown(Buttons b)
        {
            return GetButton(b) == ButtonState.Pressed;
        }

        /// <summary>
        /// Get the <see cref="ButtonState"/> of a given button.
        /// </summary>
        /// <param name="b">The button to get the <see cref="ButtonState"/> of.</param>
        /// <returns>The <see cref="ButtonState"/> of the button.</returns>
        public ButtonState GetButton(Buttons b)
        {
            return (_buttons & b) == b ? ButtonState.Pressed : ButtonState.Released;
        }

        private ButtonState GetButton(int b)
        {
            return ((long)_buttons & b) == b ? ButtonState.Pressed : ButtonState.Released;
        }

        public int[] PressedButtons()
        {
            var pressedButtons = new List<int>();
            long btns = (long)_buttons;
            for (var i = 0; btns > 0; i++)
            {
                if ((btns & 1) != 0)
                    pressedButtons.Add(i);

                btns >>= 1;
            }

            return pressedButtons.ToArray();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _buttons.GetHashCode();
        }

        /// <inheritdoc />
        public unsafe override string ToString()
        {
            StringBuilder sb = new();
            foreach (var i in PressedButtons())
            {
                long enumValue = 1 << i;
                if (Enum.IsDefined(typeof(Buttons), enumValue))
                    sb.Append(((Buttons)enumValue).ToString());
                else
                    sb.Append(i);

                sb.Append(' ');
            }

            //return Convert.ToString((int)_buttons, 2).PadLeft(10, '0');
            return sb.ToString();
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

