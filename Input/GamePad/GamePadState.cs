using System;

namespace engenious.Input
{
    /// <summary>
    /// Defines a gamepad state.
    /// </summary>
    public struct GamePadState : IEquatable<GamePadState>
    {
        private const float RangeMultiplier = 1.0f / (short.MaxValue + 1);

        private Buttons _buttons;
        private int _packetNumber;
        private short _leftStickX;
        private short _leftStickY;
        private short _rightStickX;
        private short _rightStickY;
        private byte _leftTrigger;
        private byte _rightTrigger;
        private bool _isConnected;

        /// <summary>
        /// Gets the gamepad buttons.
        /// </summary>
        public GamePadButtons Buttons => new GamePadButtons(_buttons);

        /// <summary>
        /// Gets the gamepad D-Pad buttons.
        /// </summary>
        public GamePadDPad DPad => new GamePadDPad(_buttons);

        /// <summary>
        /// Gets a value indicating whether the gamepad is connected.
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// Gets the gamepad packet number.
        /// </summary>
        public int PacketNumber => _packetNumber;

        /// <summary>
        /// Gets the gamepad thumbsticks.
        /// </summary>
        public GamePadThumbSticks ThumbSticks => new GamePadThumbSticks(_leftStickX, _leftStickY, _rightStickX, _rightStickY);

        /// <summary>
        /// Gets the gamepad triggers.
        /// </summary>
        public GamePadTriggers Triggers => new GamePadTriggers(_leftTrigger, _rightTrigger);

        /// <inheritdoc />
        public bool Equals(GamePadState other)
        {
            return ThumbSticks == other.ThumbSticks && Buttons == other.Buttons && DPad == other.DPad && IsConnected == other.IsConnected;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is GamePadState state && Equals(state);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ThumbSticks.GetHashCode() ^ Buttons.GetHashCode() ^ DPad.GetHashCode() ^ IsConnected.GetHashCode();
        }

        private bool IsAxisValid(GamePadAxes axis)
        {
            return axis >= 0 && axis < (GamePadAxes.LeftY | GamePadAxes.RightX);
        }

        private bool IsDPadValid(int index)
        {
            return index >= 0 && index < 2;
        }

        internal void SetAxis(GamePadAxes axis, short value)
        {
            if ((byte)(axis & GamePadAxes.LeftX) != 0)
            {
                _leftStickX = value;
            }
            if ((byte)(axis & GamePadAxes.LeftY) != 0)
            {
                _leftStickY = value;
            }
            if ((byte)(axis & GamePadAxes.RightX) != 0)
            {
                _rightStickX = value;
            }
            if ((byte)(axis & GamePadAxes.RightY) != 0)
            {
                _rightStickY = value;
            }
            if ((byte)(axis & GamePadAxes.LeftTrigger) != 0)
            {
                _leftTrigger = (byte)(value - -32768 >> 8);
            }
            if ((byte)(axis & GamePadAxes.RightTrigger) != 0)
            {
                _rightTrigger = (byte)(value - -32768 >> 8);
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

        internal void SetPacketNumber(int number)
        {
            _packetNumber = number;
        }

        internal void SetTriggers(byte left, byte right)
        {
            _leftTrigger = left;
            _rightTrigger = right;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{{Sticks: {ThumbSticks}; Buttons: {Buttons}; DPad: {DPad}; IsConnected: {IsConnected}}}";
        }
    }
}

