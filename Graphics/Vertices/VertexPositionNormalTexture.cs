using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace engenious.Graphics
{
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct VertexPositionNormalTexture:IVertexType
	{
		public static readonly VertexDeclaration VertexDeclaration;

		static VertexPositionNormalTexture()
		{
			VertexElement[] elements = new VertexElement[] { new VertexElement (0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),new VertexElement (12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0), new VertexElement (24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0) };
			VertexDeclaration declaration = new VertexDeclaration (elements);
			VertexDeclaration = declaration;
		}
		VertexDeclaration IVertexType.VertexDeclaration
		{
			get
			{
				return VertexDeclaration;
			}
		}
		public VertexPositionNormalTexture (Vector3 position,Vector3 normal,Vector2 textureCoord)
		{
			this.Position = position;
			this.Normal = normal;
			this.TextureCoordinate = textureCoord;
		}
			
		public Vector3 Position{ get; private set;}
		public Vector3 Normal{ get; private set;}
		public Vector2 TextureCoordinate{ get; private set;}
	}
}

