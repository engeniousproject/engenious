using System.Collections.Generic;
using System.Linq;

namespace engenious.Graphics
{
    internal class AnimationNodeContent
    {
        public AnimationNodeContent(NodeContent node)
        {
            Frames = new List<AnimationFrame>();
            Node = node;
        }
        public NodeContent Node { get; }

        public List<AnimationFrame> Frames{ get; }

        public void Sort()
        {
            Frames.Sort((a, b) => a.Frame.CompareTo(b));
        }
    }
}

