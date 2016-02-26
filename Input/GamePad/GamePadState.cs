using System;

namespace engenious.Input
{
    public struct GamePadState : IEquatable<GamePadState>
    {
        const float RangeMultiplier = 1.0f / (short.MaxValue + 1);

        Buttons buttons;
        int packet_number;
        short left_stick_x;
        short left_stick_y;
        short right_stick_x;
        short right_stick_y;
        byte left_trigger;
        byte right_trigger;
        bool is_connected;
        //
        // Properties
        //
        public GamePadButtons Buttons
        {
            get
            {
                return new GamePadButtons(this.buttons);
            }
        }

        public GamePadDPad DPad
        {
            get
            {
                return new GamePadDPad(this.buttons);
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.is_connected;
            }
        }

        public int PacketNumber
        {
            get
            {
                return this.packet_number;
            }
        }

        public GamePadThumbSticks ThumbSticks
        {
            get
            {
                return new GamePadThumbSticks(this.left_stick_x, this.left_stick_y, this.right_stick_x, this.right_stick_y);
            }
        }

        public GamePadTriggers Triggers
        {
            get
            {
                return new GamePadTriggers(this.left_trigger, this.right_trigger);
            }
        }

        //
        // Methods
        //
        public bool Equals(GamePadState other)
        {
            return this.ThumbSticks == other.ThumbSticks && this.Buttons == other.Buttons && this.DPad == other.DPad && this.IsConnected == other.IsConnected;
        }

        public override bool Equals(object obj)
        {
            return obj is GamePadState && this.Equals((GamePadState)obj);
        }

        public override int GetHashCode()
        {
            return this.ThumbSticks.GetHashCode() ^ this.Buttons.GetHashCode() ^ this.DPad.GetHashCode() ^ this.IsConnected.GetHashCode();
        }

        private bool IsAxisValid(GamePadAxes axis)
        {
            return axis >= (GamePadAxes)0 && axis < (GamePadAxes.LeftY | GamePadAxes.RightX);
        }

        private bool IsDPadValid(int index)
        {
            return index >= 0 && index < 2;
        }

        internal void SetAxis(GamePadAxes axis, short value)
        {
            if ((byte)(axis & GamePadAxes.LeftX) != 0)
            {
                this.left_stick_x = value;
            }
            if ((byte)(axis & GamePadAxes.LeftY) != 0)
            {
                this.left_stick_y = value;
            }
            if ((byte)(axis & GamePadAxes.RightX) != 0)
            {
                this.right_stick_x = value;
            }
            if ((byte)(axis & GamePadAxes.RightY) != 0)
            {
                this.right_stick_y = value;
            }
            if ((byte)(axis & GamePadAxes.LeftTrigger) != 0)
            {
                this.left_trigger = (byte)(value - -32768 >> 8);
            }
            if ((byte)(axis & GamePadAxes.RightTrigger) != 0)
            {
                this.right_trigger = (byte)(value - -32768 >> 8);
            }
        }

        internal void SetButton(Buttons button, bool pressed)
        {
            if (pressed)
            {
                this.buttons |= button;
                return;
            }
            this.buttons &= ~button;
        }

        internal void SetConnected(bool connected)
        {
            this.is_connected = connected;
        }

        internal void SetPacketNumber(int number)
        {
            this.packet_number = number;
        }

        internal void SetTriggers(byte left, byte right)
        {
            this.left_trigger = left;
            this.right_trigger = right;
        }

        public override string ToString()
        {
            return string.Format("{{Sticks: {0}; Buttons: {1}; DPad: {2}; IsConnected: {3}}}", new object[]
                {
                    this.ThumbSticks,
                    this.Buttons,
                    this.DPad,
                    this.IsConnected
                });
        }
    }
}

