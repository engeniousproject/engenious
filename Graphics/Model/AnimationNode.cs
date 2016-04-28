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

        int lastFrame;

        public void ApplyAnimation(float time, float maxTime)
        {
            Sort();
            int frameIndex = Frames.FindIndex(f => f.Frame >= time);
            AnimationFrame frame = Frames[frameIndex];
            AnimationFrame nextFrame = Frames[(frameIndex + 1) % Frames.Count];
            float diff = time - frame.Frame;
            float frameTime = nextFrame.Frame - frame.Frame;
            //if (frameIndex == Frames.Count - 1)
            {
                //TODO
            }
            //else// if (diff == 0)
            {
                Node.LocalTransform = frame.Transform;
            }
            //else if (diff > 0)
            {
                //Node.LocalTransform = Matrix.Lerp(frame.Transform, nextFrame.Transform, diff / frameTime);
            }
            lastFrame = frameIndex;
        }
    }
}

