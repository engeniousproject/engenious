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
        public ModelContent()
        {
            Animations =new List<AnimationContent>();
        }

        /// <summary>
        /// Gets or sets the meshes this model consists of.
        /// </summary>
        public MeshContent[] Meshes{ get; set; }

        internal NodeContent RootNode{ get; set; }

        internal List<NodeContent> Nodes{ get; set; }

        internal List<AnimationContent> Animations{ get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}

