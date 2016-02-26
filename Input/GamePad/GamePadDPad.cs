using System;

namespace engenious.Input
{
    public struct GamePadDPad : IEquatable<GamePadDPad>
    {

        DPadButtons buttons;
        //
        // Properties
        //
        public ButtonState Down
        {
            get
            {
                if (!this.IsDown)
                {
                    return ButtonState.Released;
                }
                return ButtonState.Pressed;
            }
        }

        public bool IsDown
        {
            get
            {
                return (byte)(this.buttons & GamePadDPad.DPadButtons.Down) != 0;
            }
            internal set
            {
                this.SetButton(GamePadDPad.DPadButtons.Down, value);
            }
        }

        public bool IsLeft
        {
            get
            {
                return (byte)(this.buttons & GamePadDPad.DPadButtons.Left) != 0;
            }
            internal set
            {
                this.SetButton(GamePadDPad.DPadButtons.Left, value);
            }
        }

        public bool IsRight
        {
            get
            {
                return (byte)(this.buttons & GamePadDPad.DPadButtons.Right) != 0;
            }
            internal set
            {
                this.SetButton(GamePadDPad.DPadButtons.Right, value);
            }
        }

        public bool IsUp
        {
            get
            {
                return (byte)(this.buttons & GamePadDPad.DPadButtons.Up) != 0;
            }
            internal set
            {
                this.SetButton(GamePadDPad.DPadButtons.Up, value);
            }
        }

        public ButtonState Left
        {
            get
            {
                if (!this.IsLeft)
                {
                    return ButtonState.Released;
                }
                return ButtonState.Pressed;
            }
        }

        public ButtonState Right
        {
            get
            {
                if (!this.IsRight)
                {
                    return ButtonState.Released;
                }
                return ButtonState.Pressed;
            }
        }

        public ButtonState Up
        {
            get
            {
                if (!this.IsUp)
                {
                    return ButtonState.Released;
                }
                return ButtonState.Pressed;
            }
        }

        //
        // Constructors
        //
        internal GamePadDPad(Buttons state)
        {
            this.buttons = (GamePadDPad.DPadButtons)(state & (Buttons.DPadUp | Buttons.DPadDown | Buttons.DPadLeft | Buttons.DPadRight));
        }

        //
        // Methods
        //
        public override bool Equals(object obj)
        {
            return obj is GamePadDPad && this.Equals((GamePadDPad)obj);
        }

        public bool Equals(GamePadDPad other)
        {
            return this.buttons == other.buttons;
        }

        public override int GetHashCode()
        {
            return this.buttons.GetHashCode();
        }

        private void SetButton(GamePadDPad.DPadButtons button, bool value)
        {
            if (value)
            {
                this.buttons |= button;
                return;
            }
            this.buttons &= ~button;
        }

        public override string ToString()
        {
            return string.Format("{{{0}{1}{2}{3}}}", new object[]
                {
                    this.IsUp ? "U" : string.Empty,
                    this.IsLeft ? "L" : string.Empty,
                    this.IsDown ? "D" : string.Empty,
                    this.IsRight ? "R" : string.Empty
                });
        }

        //
        // Operators
        //
        public static bool operator ==(GamePadDPad left, GamePadDPad right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GamePadDPad left, GamePadDPad right)
        {
            return !left.Equals(right);
        }

        //
        // Nested Types
        //
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

