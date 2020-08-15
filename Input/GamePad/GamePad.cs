using System;
using OpenToolkit.Windowing.GraphicsLibraryFramework;

namespace engenious.Input
{
    /// <summary>
    /// Static class to get gamepad input.
    /// </summary>
    public class GamePad
    {
        private static IRenderingSurface _window;
        internal static void UpdateWindow<TControl>(TControl window)
            where TControl : class, IRenderingSurface
        {
            _window = window;
        }
        /// <summary>
        /// Get the current gamepad state by a given gamepad index.
        /// </summary>
        /// <param name="index">The index of the gamepad to get the state of.</param>
        /// <returns>The current gamepad state of the gamepad specified by <paramref name="index"/>.</returns>
        public static unsafe GamePadState GetState(int index = 0)
        {
            throw new NotSupportedException();
            /*var state = _window.WindowInfo.JoystickStates[index];
            

            var actual = *(GamePadState*)&state;
            //TODO:actual.ThumbSticks.Left = new Vector2(Math.M
            return actual;*/
        }
    }
}

