namespace engenious.Graphics
{
    /// <summary>
    /// Captures a single animation frame/key frame
    /// </summary>
    public class AnimationFrame
    {
        /// <summary>
        /// Gets or sets the transformation of this <see cref="AnimationFrame"/>.
        /// </summary>
        public AnimationTransform Transform{ get; set; }

        /// <summary>
        /// Gets or sets the time of this <see cref="AnimationFrame"/>.
        /// </summary>
        public float Frame{ get; set; }
    }
}

