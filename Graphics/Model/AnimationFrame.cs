namespace engenious.Graphics
{
    /// <summary>
    /// Captures a single animation frame/key frame
    /// </summary>
    public class AnimationFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationFrame"/> class.
        /// </summary>
        /// <param name="frame">A floating point time representation for the frame time.</param>
        /// <param name="transform">The transformation object of this frame.</param>
        public AnimationFrame(float frame, AnimationTransform transform)
        {
            Frame = frame;
            Transform = transform;
        }
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

