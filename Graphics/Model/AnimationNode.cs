using System;
using System.Linq;
using System.Collections.Generic;

namespace engenious.Graphics
{
    internal class AnimationNode
    {
        private bool sorted = false;

        public Node Node{ get; set; }

        public List<AnimationFrame> Frames{ get; set; }

        public void Sort()
        {
            if (sorted)
                return;
            sorted = true;
            Frames = Frames.OrderBy(f => f.Frame).ToList();
        }

        public void ApplyAnimation(float time, float maxTime)
        {
            Sort();
            int frameIndex = (Frames.FindIndex(f => f.Frame >= time) + Frames.Count - 1)% Frames.Count;
            AnimationFrame frame = Frames[frameIndex];
            AnimationFrame nextFrame = Frames[(frameIndex + 1) % Frames.Count];
            float diff = time-frame.Frame;
            float frameTime = nextFrame.Frame - frame.Frame;
            if (diff == 0)
            {
                Node.LocalTransform = frame.Transform.ToMatrix();
            }
            else if (diff > 0)
            {
                Node.LocalTransform = AnimationTransform.Lerp(frame.Transform, nextFrame.Transform, diff / frameTime).ToMatrix();
            }
        }
    }
}

