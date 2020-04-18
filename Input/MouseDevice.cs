namespace engenious
{
    /// <summary>
    /// Specifies a mouse device.
    /// </summary>
    public class MouseDevice
    {
        private readonly OpenTK.Input.MouseDevice _dev;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseDevice"/> class.
        /// </summary>
        /// <param name="dev">The underlying mouse device.</param>
        public MouseDevice(OpenTK.Input.MouseDevice dev)
        {
            _dev = dev;
        }

        /// <summary>
        /// Gets the mouse x cursor position.
        /// </summary>
        public int X => _dev.X;
        
        /// <summary>
        /// Gets the mouse y cursor position.
        /// </summary>
        public int Y => _dev.Y;
    }
}

