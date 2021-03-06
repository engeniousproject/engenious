namespace engenious
{
    /// <summary>
    /// Provides initial settings for <see cref="Game"/>.
    /// </summary>
    public class GameSettings
    {
        /// <summary>
        /// Gets or sets a value indicating the number of samples that should be used.
        /// </summary>
        public int NumberOfSamples { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether we want to only use offscreen rendering.
        /// </summary>
        public bool Offscreen { get; set; }

        /// <summary>
        /// Gets the default game settings.
        /// </summary>
        public static GameSettings Default = new();
    }
}