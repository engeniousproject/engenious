using System;

namespace engenious.Input
{
    [Flags]
    internal enum GamePadAxes : byte
    {
        None = 0,
        LeftX = 1 << 0,
        LeftY = 1 << 1,
        RightX = 1 << 2,
        RightY = 1 << 3,
        LeftTrigger = 1 << 4,
        RightTrigger = 1 << 5,
        AxisLast = RightTrigger,
    }
}

