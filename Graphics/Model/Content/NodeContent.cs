using System;
using System.Collections.Generic;

namespace engenious.Graphics
{
    internal class NodeContent
    {
        public NodeContent()
        {
            GlobalTransform = LocalTransform = Matrix.Identity;
        }

        public string Name{ get; set; }

        public List<int> Meshes{ get; set; }

        public Matrix Transformation{ get; set; }

        public Matrix LocalTransform{ get; set; }

        public Matrix GlobalTransform{ get; set; }

        public List<NodeContent> Children{ get; set; }
    }
}

