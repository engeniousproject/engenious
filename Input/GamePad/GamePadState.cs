using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;

namespace engenious.Input
{
    /// <summary>
    /// Defines a gamepad state.
    /// </summary>
    public struct GamePadState : IEnumerable<Buttons>, IEquatable<GamePadState>
    {
        private Buttons _buttons;
        private float _leftStickX;
        private float _leftStickY;
        private float _rightStickX;
        private float _rightStickY;
        private float _leftTrigger;
        private float _rightTrigger;
        private bool _isConnected;
        private string _name;
        private ArraySegment<GamePadHat> _hats;
        private bool _isGamePad;

        /// <summary>
        /// Gets the gamepad buttons.
        /// </summary>
        public GamePadButtons Buttons => new(_buttons);

        /// <summary>
        /// Gets the gamepad D-Pad buttons.
        /// </summary>
        public GamePadDPad DPad => new(_buttons);

        /// <summary>
        /// Gets a value indicating whether the gamepad is connected.
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// Gets the name of the gamepad.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets a value indicating whether the connected joystick has a gamepad mapping.
        /// </summary>
        public bool IsGamePad => _isGamePad;

        /// <summary>
        /// Gets the gamepad thumbsticks.
        /// </summary>
        public GamePadThumbSticks ThumbSticks => new(_leftStickX, _leftStickY, _rightStickX, _rightStickY);

        /// <summary>
        /// Gets the gamepad triggers.
        /// </summary>
        public GamePadTriggers Triggers => new(_leftTrigger, _rightTrigger);

        /// <summary>
        /// Gets a gamepad hat at a specified index.
        /// </summary>
        /// <param name="index">The index of the hat.</param>
        /// <returns>The hat at the given index.</returns>
        public GamePadHat Hat(int index) => _hats[index];

        internal void SetAxis(GamePadAxes axis, float value)
        {
            if ((axis & GamePadAxes.LeftX) != GamePadAxes.None)
            {
                _leftStickX = value;
            }
            if ((axis & GamePadAxes.LeftY) != GamePadAxes.None)
            {
                _leftStickY = value;
            }
            if ((axis & GamePadAxes.RightX) != GamePadAxes.None)
            {
                _rightStickX = value;
            }
            if ((axis & GamePadAxes.RightY) != GamePadAxes.None)
            {
                _rightStickY = value;
            }
            if ((axis & GamePadAxes.LeftTrigger) != GamePadAxes.None)
            {
                _leftTrigger = value;
            }
            if ((axis & GamePadAxes.RightTrigger) != GamePadAxes.None)
            {
                _rightTrigger = value;
            }
        }

        internal void SetButton(Buttons button, bool pressed)
        {
            if (pressed)
            {
                _buttons |= button;
                return;
            }
            _buttons &= ~button;
        }

        internal void SetConnected(bool connected)
        {
            _isConnected = connected;
        }

        internal void SetName(string name)
        {
            _name = name;
        }

        internal void SetIsGamePad(bool isGamePad)
        {
            _isGamePad = isGamePad;
        }

        internal void SetHats(ArraySegment<GamePadHat> hats)
        {
            if (_hats.Array != null)
            {
                ArrayPool<GamePadHat>.Shared.Return(_hats.Array);
            }
            _hats = hats;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{{Name: {Name}; IsConnected: {IsConnected}; Sticks: {ThumbSticks}; Buttons: {Buttons}; DPad: {DPad}; Trigger: {Triggers}}}";
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is GamePadState state && Equals(state);
        }

        /// <inheritdoc />
        public bool Equals(GamePadState other)
        {
            return Buttons.Equals(other.Buttons) &&
                   IsConnected == other.IsConnected &&
                   Name == other.Name &&
                   IsGamePad == other.IsGamePad &&
                   ThumbSticks.Equals(other.ThumbSticks) &&
                   Triggers.Equals(other.Triggers);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(Buttons, IsConnected, Name, IsGamePad, ThumbSticks, Triggers);
        }

        /// <inheritdoc />
        public static bool operator ==(GamePadState left, GamePadState right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc />
        public static bool operator !=(GamePadState left, GamePadState right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public IEnumerator<Buttons> GetEnumerator()
        {
            long btns = (long)_buttons;
            for (var i = 0; btns > 0; i++)
            {
                if ((btns & 1) != 0)
                    yield return (Buttons)(long)(1 << i);

                btns >>= 1;
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}