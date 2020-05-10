using System;
using System.Reflection;
using engenious.Helper;
using OpenTK;
using OpenTK.Input;

namespace engenious.Input
{
    /// <summary>
    /// Static class to get mouse input.
    /// </summary>
    public static class Mouse
    {

        private static IRenderingSurface _window;
        private static float _deltaPrecise;
        static Mouse()
        {
            if (!WrappingHelper.ValidateStructs<OpenTK.Input.MouseState,MouseState>())
                throw new Exception("test");
        }

        internal static void UpdateWindow<TControl>(TControl window)
            where TControl : class, IRenderingSurface
        {
            _window = window;
            window.MouseWheel += delegate(object sender, MouseWheelEventArgs e)
            {
                _deltaPrecise-=(e.DeltaPrecise);
            };
        }
        
        /// <summary>
        /// Gets the current raw mouse state for a given mouse index.
        /// </summary>
        /// <param name="index">The mouse index of the mouse to get the state of.</param>
        /// <returns>The current raw mouse state for the given mouse index.</returns>
        public static MouseState GetState(int index)
        {
            return GetState();
            //TODO multiple mice
            /*OpenTK.Input.MouseState state = OpenTK.Input.Mouse.GetState(index);
            MouseState actual = *(MouseState*)(&state);
            
            actual.X = _window.Mouse.X;
            actual.Y = _window.Mouse.Y;
            return actual;*/

        }

        /// <summary>
        /// Gets the current raw mouse state.
        /// </summary>
        /// <returns>The current raw mouse state.</returns>
        public static unsafe MouseState GetState()
        {
            var state = OpenTK.Input.Mouse.GetState();
            var actual = *(MouseState*)(&state);
            return actual;

        }

        /// <summary>
        /// Gets the current mouse cursor state.
        /// </summary>
        /// <returns>The current mouse cursor state.</returns>
        public static unsafe MouseState GetCursorState()
        {
            var state = OpenTK.Input.Mouse.GetCursorState();
            var actual = *(MouseState*)(&state);
            return actual;
        }

        /// <summary>
        /// Sets the mouse position in screen coordinates.
        /// </summary>
        /// <param name="x">The x coordinate to move the mouse position to.</param>
        /// <param name="y">The y coordinate to move the mouse position to.</param>
        public static void SetPosition(double x, double y)
        {
            var pt = _window.PointToScreen(new System.Drawing.Point((int)x, (int)y));
            x = pt.X + (x - (int)x);
            y = pt.Y + (y - (int)y);
            OpenTK.Input.Mouse.SetPosition(x,y);
        }
    }
}

