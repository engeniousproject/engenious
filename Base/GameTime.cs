using System;

namespace engenious
{
    public class GameTime
    {
        public GameTime()
            : this(TimeSpan.Zero, TimeSpan.Zero)
        {
            
        }


        public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
        {
            ElapsedGameTime = elapsedGameTime;
            TotalGameTime = totalGameTime;
        }

        internal void Update(double time)
        {
            ElapsedGameTime = new TimeSpan((long)(time * 10000000));
            TotalGameTime += ElapsedGameTime;
        }

        public TimeSpan ElapsedGameTime { get; set; }

        public TimeSpan TotalGameTime { get; set; }
    }
}

