namespace engenious.Audio
{
    /// <summary>
    /// Defines an audio listener.
    /// </summary>
    public class AudioListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioListener"/> class.
        /// </summary>
        public AudioListener()
        {
            Position = new Vector3();
            Up = Vector3.UnitY;
            Forward = Vector3.UnitZ;
        }

        /// <summary>
        /// Gets or sets the forward direction of the listener.
        /// </summary>
        public Vector3 Forward { get; set; }

        /// <summary>
        /// Gets or sets the position direction of the listener.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the up vector direction of the listener.
        /// </summary>
        public Vector3 Up { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the listener.
        /// </summary>
        public Vector3 Velocity { get; set; }
    }
}

