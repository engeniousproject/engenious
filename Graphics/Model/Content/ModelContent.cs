using System;
using System.Collections.Generic;

namespace engenious.Graphics
{
    /// <summary>
    /// A class containing model data on cpu side only.
    /// </summary>
    public class ModelContent : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelContent"/> class.
        /// </summary>
        /// <param name="meshCount">The number of meshes for this model.</param>
        public ModelContent(int meshCount)
        {
            Animations =new List<AnimationContent>();
            Meshes = new MeshContent[meshCount];
            Nodes = new List<NodeContent>();
        }

        /// <summary>
        /// Gets or sets the meshes this model consists of.
        /// </summary>
        public MeshContent[] Meshes{ get; }

        internal NodeContent? RootNode{ get; set; }

        internal List<NodeContent> Nodes{ get; }

        internal List<AnimationContent> Animations{ get; }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}

