using System.Runtime.InteropServices;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a basic vertex type containing position information and normals.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionNormal:IPositionVertex,INormalVertex
    {
        /// <summary>
        /// Gets the <see cref="engenious.Graphics.VertexDeclaration"/> for this vertex type.
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration;

        static VertexPositionNormal()
        {
            VertexElement[] elements = { new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0) };
            var declaration = new VertexDeclaration(elements);
            VertexDeclaration = declaration;
        }

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosition"/> struct.
        /// </summary>
        /// <param name="position">The position of this vertex.</param>
        /// <param name="normal">The normal direction of this vertex.</param>
        public VertexPositionNormal(Vector3 position, Vector3 normal)
        {
            Normal = normal;
            Position = position;
        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public Vector3 Normal { get; set; }
    }
}