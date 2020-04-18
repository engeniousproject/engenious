using System.Collections.Generic;

namespace engenious.Graphics
{
    /// <summary>
    /// Class containing animation information for a model.
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        public Animation()
        {
            Channels = new List<AnimationNode>();
        }
        /// <summary>
        /// Gets the current time the animation is at.
        /// </summary>
        public float Time{ get;internal set; }

        /// <summary>
        /// Gets or sets the duration the animation should require.
        /// </summary>
        public float MaxTime{ get; set; }

        /// <summary>
        /// Gets a list of animation nodes this model has.
        /// </summary>
        public List<AnimationNode> Channels { get; }

        /// <summary>
        /// Advances the animation by a given time.
        /// </summary>
        /// <param name="elapsed">The time to advance the animation by.</param>
        public void Update(float elapsed)
        {
            Time = elapsed;
            Time %= MaxTime;
            foreach (var channel in Channels)
                channel.ApplyAnimation(Time, MaxTime);
        }
    }
}

