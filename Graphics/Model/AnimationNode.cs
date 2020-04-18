using System;
using System.Linq;
using System.Collections.Generic;

namespace engenious.Graphics
{
    /// <summary>
    /// Animation node containing animation information for model nodes.
    /// </summary>
    public class AnimationNode
    {
        private bool _sorted = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationNode"/> class.
        /// </summary>
        public AnimationNode()
        {
            Frames = new List<AnimationFrame>();
        }

        /// <summary>
        /// Gets or sets the model node, which is to be animated, for this animation node.
        /// </summary>
        public Node Node{ get; set; }

        /// <summary>
        /// Gets the animation frames for this node.
        /// </summary>
        public List<AnimationFrame> Frames{ get; }

        /// <summary>
        /// Sorts animation frames by their timestamp.
        /// </summary>
        public void Sort()
        {
            if (_sorted)
                return;
            _sorted = true;
            Frames.Sort((a,b) => a.Frame.CompareTo(b.Frame));
        }

        /// <summary>
        /// Gets or sets whether the animation should be in repeat mode.
        /// </summary>
        public bool Repeat{get;set;}=true;

        /// <summary>
        /// Applies the animation and sets the animation state of this node to a specific time.
        /// </summary>
        /// <param name="time">The current time the node should be set to.</param>
        /// <param name="maxTime">The maximum time an animation takes.</param>
        public void ApplyAnimation(float time, float maxTime)
        {
            Sort();
            var frameIndex = Math.Max(Frames.FindIndex(f => f.Frame >= time)-1,0);
            var frame = Frames[frameIndex];
            AnimationFrame nextFrame=null;
            if (Repeat){
                nextFrame = Frames[(frameIndex + 1) % Frames.Count];
            }
            else if(frameIndex < Frames.Count-1)
                nextFrame = Frames[frameIndex + 1];
            else
                return;
            
            var diff = time-frame.Frame;
            var frameTime = nextFrame.Frame - frame.Frame;

            /*if (diff == 0)
            {
                Node.LocalTransform = frame.Transform.ToMatrix();
            }
            else if (diff > 0)*/
            var percent=diff / frameTime;
            if (Node.Name.Contains("$"))
            {
                Console.WriteLine(frameIndex + " - " + percent);
            }
            {
                //Matrix m1 = frame.Transform.ToMatrix();
                //Matrix m2 = nextFrame.Transform.ToMatrix();
                //Node.LocalTransform = Matrix.Lerp(m1,m2,percent);
                Node.LocalTransform = AnimationTransform.Lerp(frame.Transform, nextFrame.Transform, percent).ToMatrix();
            }
        }
    }
}

