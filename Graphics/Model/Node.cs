using System.Collections.Generic;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a node of a model to transform parts of said model.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node(string name)
        {
            Name = name;
            GlobalTransform = LocalTransform = Matrix.Identity;
            Children = new List<Node>();
            Meshes = new List<IMesh>();
        }

        /// <summary>
        /// Gets or sets the name of this node.
        /// </summary>
        public string Name{ get; }

        /// <summary>
        /// Gets or sets the meshes this node influences.
        /// </summary>
        public List<IMesh> Meshes{ get; }

        /// <summary>
        /// Gets or sets the transformation of this node.
        /// </summary>
        public Matrix Transformation{ get; set; }

        /// <summary>
        /// Gets or sets the local transformation in the current animation state.
        /// </summary>
        public Matrix LocalTransform{ get; set; }

        /// <summary>
        /// Gets or sets global transformation in the current animation state.
        /// </summary>
        public Matrix GlobalTransform{ get; set; }

        /// <summary>
        /// Gets or sets the child nodes.
        /// </summary>
        public List<Node> Children{ get; }
    }
}

