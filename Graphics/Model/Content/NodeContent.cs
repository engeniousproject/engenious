using System.Collections.Generic;

namespace engenious.Graphics
{
    internal class NodeContent
    {
        public NodeContent(string name, NodeContent? parent = null)
        {
            Name = name;
            Parent = parent;
            GlobalTransform = LocalTransform = Matrix.Identity;
            Meshes = new List<int>();
            Children = new List<NodeContent>();
        }

        public string Name{ get; set; }

        public List<int> Meshes{ get; }

        public Matrix Transformation{ get; set; }

        public Matrix LocalTransform{ get; set; }

        public Matrix GlobalTransform{ get; set; }

        public List<NodeContent> Children{ get; }
        public NodeContent? Parent { get; internal set; }
    }
}

