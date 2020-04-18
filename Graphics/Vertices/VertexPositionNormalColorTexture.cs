using System.Runtime.InteropServices;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a basic vertex type containing position information, colors, normals and texture coordinates.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct VertexPositionNormalColorTexture:IPositionVertex,INormalVertex,IColorVertex,ITextureCoordinatesVertex
    {
        /// <summary>
        /// Gets the <see cref="engenious.Graphics.VertexDeclaration"/> for this vertex type.
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration;

        static VertexPositionNormalColorTexture()
        {
            VertexElement[] elements = { new VertexElement (0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),new VertexElement (12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),new VertexElement (24, VertexElementFormat.Color, VertexElementUsage.Color, 0), new VertexElement (40, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0) };
            var declaration = new VertexDeclaration (elements);
            VertexDeclaration = declaration;
        }
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosition"/> struct.
        /// </summary>
        /// <param name="position">The position of this vertex.</param>
        /// <param name="normal">The normal direction of this vertex.</param>
        /// <param name="color">The color of this vertex.</param>
        /// <param name="textureCoord">The texture coordinate of this vertex.</param>
        public VertexPositionNormalColorTexture (Vector3 position,Vector3 normal,Color color,Vector2 textureCoord)
        {
            Position = position;
            Normal = normal;
            Color = color;
            TextureCoordinate = textureCoord;
        }

        /// <inheritdoc />
        public Vector3 Position{ get; set;}

        /// <inheritdoc />
        public Vector3 Normal{ get; set;}

        /// <inheritdoc />
        public Color Color{ get; set;}

        /// <inheritdoc />
        public Vector2 TextureCoordinate{ get; set;}
    }
}

