using System;

namespace engenious.Input
{
    /// <summary>
    /// Specifies gamepad buttons.
    /// </summary>
    [Flags]
    public enum Buttons
    {
        /// <summary>
        /// The D-Pdd up button.
        /// </summary>
        DPadUp = 1,
        /// <summary>
        /// The D-Pdd down button.
        /// </summary>
        DPadDown = 2,
        /// <summary>
        /// The D-Pdd left button.
        /// </summary>
        DPadLeft = 4,
        /// <summary>
        /// The D-Pdd right button.
        /// </summary>
        DPadRight = 8,
        /// <summary>
        /// The start button.
        /// </summary>
        Start = 16,
        /// <summary>
        /// The back button.
        /// </summary>
        Back = 32,
        /// <summary>
        /// The left stick button.
        /// </summary>
        LeftStick = 64,
        /// <summary>
        /// The right stick button.
        /// </summary>
        RightStick = 128,
        /// <summary>
        /// The left shoulder button.
        /// </summary>
        LeftShoulder = 256,
        /// <summary>
        /// The right shoulder button.
        /// </summary>
        RightShoulder = 512,
        /// <summary>
        /// The home button.
        /// </summary>
        Home = 2048,
        /// <summary>
        /// The big button.
        /// </summary>
        BigButton = 2048,
        /// <summary>
        /// The A button.
        /// </summary>
        A = 4096,
        /// <summary>
        /// The B button.
        /// </summary>
        B = 8192,
        /// <summary>
        /// The X button.
        /// </summary>
        X = 16384,
        /// <summary>
        /// The Y button.
        /// </summary>
        Y = 32768,
        /// <summary>
        /// The left thumbstick left button.
        /// </summary>
        LeftThumbstickLeft = 2097152,
        /// <summary>
        /// The right trigger button.
        /// </summary>
        RightTrigger = 4194304,
        /// <summary>
        /// The left trigger button.
        /// </summary>
        LeftTrigger = 8388608,
        /// <summary>
        /// The right thumbstick up button.
        /// </summary>
        RightThumbstickUp = 16777216,
        /// <summary>
        /// The right thumbstick down button.
        /// </summary>
        RightThumbstickDown = 33554432,
        /// <summary>
        /// The right thumbstick right button.
        /// </summary>
        RightThumbstickRight = 67108864,
        /// <summary>
        /// The right thumbstick left button.
        /// </summary>
        RightThumbstickLeft = 134217728,
        /// <summary>
        /// The left thumbstick up button.
        /// </summary>
        LeftThumbstickUp = 268435456,
        /// <summary>
        /// The left thumbstick down button.
        /// </summary>
        LeftThumbstickDown = 536870912,
        /// <summary>
        /// The left thumbstick right button.
        /// </summary>
        LeftThumbstickRight = 1073741824
    }
}

