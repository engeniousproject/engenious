using System;
using System.Linq;
using System.Reflection;
using engenious.Helper;
using OpenTK.Windowing.Common;

namespace engenious.Input
{
    /// <summary>
    /// Static class to get mouse input.
    /// </summary>
    public static class Mouse
    {
        private static IRenderingSurface _window;
        private static float _deltaPrecise;
        private static MouseScroll _scroll = default;

        private static readonly int MouseButtonCount;
        static Mouse()
        {
            //if (!WrappingHelper.ValidateStructs<OpenTK.Windowing.Common.Input.MouseState,MouseState>())
            //    throw new Exception("test");

            MouseButtonCount = Enum.GetValues(typeof(OpenTK.Windowing.Common.Input.MouseButton)).OfType<OpenTK.Windowing.Common.Input.MouseButton>().Select(x => (int)x)
                .Max();
        }

        internal static void UpdateWindow<TControl>(TControl window)
            where TControl : class, IRenderingSurface
        {
            _window = window;
            window.MouseWheel += delegate(MouseWheelEventArgs e)
            {
                _scroll = new MouseScroll(_scroll.X + e.OffsetX, _scroll.Y + e.OffsetY);
                _deltaPrecise -= e.OffsetX;
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
        }

        private static MouseButton TranslateMouseButton(OpenTK.Windowing.Common.Input.MouseButton button)
        {
            return (MouseButton) button;
        }

        /// <summary>
        /// Gets the current raw mouse state.
        /// </summary>
        /// <returns>The current raw mouse state.</returns>
        public static MouseState GetState()
        {
            var state = _window.WindowInfo.MouseState;
            var actual = new MouseState()
                {IsConnected = true, Position = new Vector2(state.Position.X, state.Position.Y), Scroll = _scroll};
            for (var i = 0; i < MouseButtonCount; i++)
            {
                var original = (OpenTK.Windowing.Common.Input.MouseButton) i;
                if (state[original])
                {
                    actual.EnableBit(i);
                }
                
            }
            return actual;
        }

        /// <summary>
        /// Gets the current mouse cursor state.
        /// </summary>
        /// <returns>The current mouse cursor state.</returns>
        public static MouseState GetCursorState()
        {
            var state = _window.WindowInfo.MouseState;
            var actual = new MouseState()
                {IsConnected = true, Position = new Vector2(state.Position.X, state.Position.Y), Scroll = _scroll};
            for (var i = 0; i < MouseButtonCount; i++)
            {
                var original = (OpenTK.Windowing.Common.Input.MouseButton) i;
                if (state[original])
                {
                    actual.EnableBit(i);
                }
                
            }
            return actual;
        }

        /// <summary>
        /// Sets the mouse cursor position.
        /// </summary>
        /// <param name="x">The x-component for the new cursor position.</param>
        /// <param name="y">The y-component for the new cursor position.</param>
        public static void SetPosition(float x, float y)
        {
            var pos = _window.Vector2ToScreen(new Vector2(x, y));
            _window.WindowInfo.MousePosition = new OpenTK.Mathematics.Vector2(pos.X, pos.Y);
        }
    }
}

