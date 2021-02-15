namespace engenious.Input
{
    /// <summary>
    /// Static class to get mouse input.
    /// </summary>
    public static class Mouse
    {
        private static IRenderingSurface? _window;

        internal static void UpdateWindow<TControl>(TControl window)
            where TControl : class, IRenderingSurface
        {
            _window = window;
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

        private static MouseButton TranslateMouseButton(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton button)
        {
            return (MouseButton) button;
        }

        /// <summary>
        /// Gets the current raw mouse state.
        /// </summary>
        /// <returns>The current raw mouse state.</returns>
        public static MouseState GetState()
        {
            return _window!.WindowInfo!.MouseState;
        }

        /// <summary>
        /// Gets the current mouse cursor state.
        /// </summary>
        /// <returns>The current mouse cursor state.</returns>
        public static MouseState GetCursorState()
        {
            return _window!.WindowInfo!.MouseState;
        }

        /// <summary>
        /// Sets the mouse cursor position.
        /// </summary>
        /// <param name="x">The x-component for the new cursor position.</param>
        /// <param name="y">The y-component for the new cursor position.</param>
        public static void SetPosition(float x, float y)
        {
            _window!.WindowInfo!.MousePosition = new Vector2(x, y);
        }
    }
}

