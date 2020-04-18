using System.Runtime.InteropServices;

namespace engenious.Graphics
{
	/// <summary>
	/// Describes a basic vertex type containing position information and texture coordinates.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct VertexPositionTexture:IPositionVertex,ITextureCoordinatesVertex
	{
		/// <summary>
		/// Gets the <see cref="engenious.Graphics.VertexDeclaration"/> for this vertex type.
		/// </summary>
		public static readonly VertexDeclaration VertexDeclaration;

		static VertexPositionTexture()
		{
			VertexElement[] elements = { new VertexElement (0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), new VertexElement (12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0) };
			var declaration = new VertexDeclaration (elements);
			VertexDeclaration = declaration;
		}
		VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

		/// <summary>
		/// Initializes a new instance of the <see cref="VertexPosition"/> struct.
		/// </summary>
		/// <param name="position">The position of this vertex.</param>
		/// <param name="textureCoord">The texture coordinate of this vertex.</param>
		public VertexPositionTexture (Vector3 position,Vector2 textureCoord)
		{
			TextureCoordinate = textureCoord;
			Position = position;
		}

		/// <inheritdoc />
		public Vector3 Position{ get; set;}

		/// <inheritdoc />
		public Vector2 TextureCoordinate{ get; set;}
	}
}

