﻿using System.Runtime.InteropServices;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a basic vertex type containing position and color information as well as texture coordinates.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionColorTexture:IPositionVertex,IColorVertex,ITextureCoordinatesVertex
    {
        /// <summary>
        /// Gets the <see cref="engenious.Graphics.VertexDeclaration"/> for this vertex type.
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration;

        static VertexPositionColorTexture()
        {
            VertexElement[] elements = {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
                new VertexElement(28, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            };
            var declaration = new VertexDeclaration(elements);
            VertexDeclaration = declaration;
        }

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionColorTexture"/> struct.
        /// </summary>
        /// <param name="position">The position of this vertex.</param>
        /// <param name="color">The color of this vertex.</param>
        /// <param name="textureCoord">The texture coordinate of this vertex.</param>
        public VertexPositionColorTexture(Vector3 position, Color color, Vector2 textureCoord)
        {
            TextureCoordinate = textureCoord;
            Position = position;
            Color = color;


        }

        /// <inheritdoc />
        public Vector3 Position { get; set; }

        /// <inheritdoc />
        public Color Color { get; set; }

        /// <inheritdoc />
        public Vector2 TextureCoordinate { get; set; }
    }
}

