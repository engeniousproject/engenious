using System;

namespace engenious.Input
{
    [Flags]
    public enum GamePadHat : long
    {
        Centered = 1 << 0,
        Up = 1 << 1,
        Right = 1 << 2,
        Down = 1 << 4,
        Left = 1 << 8,
        RightUp = Right | Up,
        RightDown = Right | Down,
        LeftUp = Left | Up,
        LeftDown = Left | Down,
    }
}
