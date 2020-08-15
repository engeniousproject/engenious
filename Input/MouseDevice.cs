namespace engenious
{
    /// <summary>
    /// Specifies a mouse device.
    /// </summary>
    public class MouseDevice
    {
        private readonly IRenderingSurface _surface;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseDevice"/> class.
        /// </summary>
        /// <param name="dev">The underlying mouse device.</param>
        public MouseDevice(IRenderingSurface surface)
        {
            _surface = surface;
        }

        /// <summary>
        /// Gets the mouse x cursor position.
        /// </summary>
        public int X => (int)_surface.WindowInfo.MousePosition.X;
        
        /// <summary>
        /// Gets the mouse y cursor position.
        /// </summary>
        public int Y => (int)_surface.WindowInfo.MousePosition.Y;
    }
}

