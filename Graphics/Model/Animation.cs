using System;
using System.Collections.Generic;

namespace engenious.Graphics
{
    internal class Animation
    {
        public float Time{ get; set; }

        public float MaxTime{ get; set; }

        public List<AnimationNode> Channels{ get; set; }

        public void Update(float elapsed)
        {
            Time += elapsed;
            Time %= MaxTime;
            foreach (var channel in Channels)
                channel.ApplyAnimation(Time, MaxTime);
        }
    }
}

