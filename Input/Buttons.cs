using System;

namespace engenious.Input
{
    /// <summary>
    /// Specifies gamepad buttons.
    /// </summary>
    [Flags]
    public enum Buttons : long
    {
        A = 1 << 0,
        B = 1 << 1,
        X = 1 << 2,
        Y = 1 << 3,
        LeftBumper = 1 << 4,
        RightBumper = 1 << 5,
        Back = 1 << 6,
        Start = 1 << 7,
        Guide = 1 << 8,
        LeftThumb = 1 << 9,
        RightThumb = 1 << 10,
        DPadUp = 1 << 11,
        DPadRight = 1 << 12,
        DPadDown = 1 << 13,
        DPadLeft = 1 << 14,
        Last = DPadLeft,
        Cross = A,
        Circle = B,
        Square = X,
        Triangle = Y,
    }
}

