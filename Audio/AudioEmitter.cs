namespace engenious.Audio
{
    /// <summary>
    /// Defines an audio emitter.
    /// </summary>
    public class AudioEmitter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioEmitter"/> class.
        /// </summary>
        public AudioEmitter()
        {
            Position = new Vector3();
            Up = Vector3.UnitY;
            Forward = Vector3.UnitZ;

            DopplerScale = 1.0f;
        }

        /// <summary>
        /// Gets or sets the doppler scale.
        /// </summary>
        public float DopplerScale{ get; set; }

        /// <summary>
        /// Gets or sets the forward direction.
        /// </summary>
        public Vector3 Forward { get; set; }

        /// <summary>
        /// Gets or sets emitters position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the emitters up vector.
        /// </summary>
        public Vector3 Up { get; set; }

        /// <summary>
        /// Gets or sets the emitters velocity.
        /// </summary>
        public Vector3 Velocity { get; set; }
    }
}

