using System;
using System.Collections.Generic;

namespace engenious.Graphics
{
    internal class Node
    {
        public Node()
        {
            GlobalTransform = LocalTransform = Matrix.Identity;
        }

        public string Name{ get; set; }

        public List<Mesh> Meshes{ get; set; }

        public Matrix Transformation{ get; set; }

        public Matrix LocalTransform{ get; set; }

        public Matrix GlobalTransform{ get; set; }

        public List<Node> Children{ get; set; }
    }
}

