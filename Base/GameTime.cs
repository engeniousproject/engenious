using System;

namespace engenious
{
    /// <summary>
    /// Defines <see cref="GameTime"/> containing time elapsed since the last frame/update and time since game start.
    /// </summary>
    public struct GameTime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameTime"/> class.
        /// </summary>
        /// <param name="totalGameTime">The total elapsed game time.</param>
        /// <param name="elapsedGameTime">The game time elapsed since the last frame/update.</param>
        public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
        {
            ElapsedGameTime = elapsedGameTime;
            TotalGameTime = totalGameTime;
        }

        internal void Update(double time)
        {
            ElapsedGameTime = new TimeSpan((long)(time * TimeSpan.TicksPerSecond));
            TotalGameTime += ElapsedGameTime;
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> indicating the elapsed game time since the last frame/update.
        /// </summary>
        public TimeSpan ElapsedGameTime { get; set; }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> indicating the elapsed game time since game start.
        /// </summary>
        public TimeSpan TotalGameTime { get; set; }
    }
}

