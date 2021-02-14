namespace engenious.Graphics
{
    /// <summary>
    /// A class containing mesh data on cpu side only.
    /// </summary>
    public class MeshContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeshContent"/> class.
        /// </summary>
        /// <param name="primitiveCount">The numbers of primitives of the mesh.</param>
        /// <param name="vertices">The vertices of the mesh.</param>
        /// <param name="indices">The indices of the mesh, or null if no indexing is used.</param>
        public MeshContent(int primitiveCount, ConditionalVertexArray vertices, int[]? indices = null)
        {
            PrimitiveCount = primitiveCount;
            Vertices = vertices;
            Indices = indices;
        }
        /// <summary>
        /// Gets the number of primitives.
        /// </summary>
        public int PrimitiveCount{ get; }

        /// <summary>
        /// Gets the meshes vertices.
        /// </summary>
        public ConditionalVertexArray Vertices { get; }

        /// <summary>
        /// Gets a value indicating whether this mesh has indices.
        /// </summary>
        public bool HasIndices => Indices != null;

        /// <summary>
        /// Gets this meshes indices.
        /// </summary>
        public int[]? Indices { get; }
    }
}

