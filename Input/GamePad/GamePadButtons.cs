using System;

namespace engenious.Input
{
    public struct GamePadButtons : IEquatable<GamePadButtons>
    {
        Buttons buttons;
        //
        // Properties
        //
        public ButtonState A
        {
            get
            {
                return this.GetButton(Buttons.A);
            }
        }

        public ButtonState B
        {
            get
            {
                return this.GetButton(Buttons.B);
            }
        }

        public ButtonState Back
        {
            get
            {
                return this.GetButton(Buttons.Back);
            }
        }

        public ButtonState BigButton
        {
            get
            {
                return this.GetButton(Buttons.Home);
            }
        }

        public ButtonState LeftShoulder
        {
            get
            {
                return this.GetButton(Buttons.LeftShoulder);
            }
        }

        public ButtonState LeftStick
        {
            get
            {
                return this.GetButton(Buttons.LeftStick);
            }
        }

        public ButtonState RightShoulder
        {
            get
            {
                return this.GetButton(Buttons.RightShoulder);
            }
        }

        public ButtonState RightStick
        {
            get
            {
                return this.GetButton(Buttons.RightStick);
            }
        }

        public ButtonState Start
        {
            get
            {
                return this.GetButton(Buttons.Start);
            }
        }

        public ButtonState X
        {
            get
            {
                return this.GetButton(Buttons.X);
            }
        }

        public ButtonState Y
        {
            get
            {
                return this.GetButton(Buttons.Y);
            }
        }

        //
        // Constructors
        //
        public GamePadButtons(Buttons state)
        {
            this.buttons = state;
        }

        //
        // Methods
        //
        public bool Equals(GamePadButtons other)
        {
            return this.buttons == other.buttons;
        }

        public override bool Equals(object obj)
        {
            return obj is GamePadButtons && this.Equals((GamePadButtons)obj);
        }

        private ButtonState GetButton(Buttons b)
        {
            if ((this.buttons & b) == (Buttons)0)
            {
                return ButtonState.Released;
            }
            return ButtonState.Pressed;
        }

        public override int GetHashCode()
        {
            return this.buttons.GetHashCode();
        }

        public override string ToString()
        {
            return Convert.ToString((int)this.buttons, 2).PadLeft(10, '0');
        }

        //
        // Operators
        //
        public static bool operator ==(GamePadButtons left, GamePadButtons right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GamePadButtons left, GamePadButtons right)
        {
            return !left.Equals(right);
        }
    }
}

