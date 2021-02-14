using System.Collections.Generic;

namespace engenious.Graphics
{
    internal class AnimationContent
    {
        public AnimationContent()
        {
            Channels = new List<AnimationNodeContent>();
        }

        public float Time{ get; set; }

        public float MaxTime{ get; set; }

        public List<AnimationNodeContent> Channels{ get; set; }
    }
}

