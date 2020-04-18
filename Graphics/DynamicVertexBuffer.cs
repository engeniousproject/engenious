using System;

namespace engenious.Graphics
{
	/// <summary>
	/// A dynamic version of the <see cref="VertexBuffer"/> class.
	/// </summary>
	public class DynamicVertexBuffer:VertexBuffer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicVertexBuffer"/> class.
		/// </summary>
		/// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to be created on.</param>
		/// <param name="vertexType">The type of the vertices.</param>
		/// <param name="vertexCount">The count of vertices.</param>
		public DynamicVertexBuffer (GraphicsDevice graphicsDevice, Type vertexType, int vertexCount)
			: base (graphicsDevice, vertexType, vertexCount, BufferUsageHint.DynamicDraw)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicVertexBuffer"/> class.
		/// </summary>
		/// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to be created on.</param>
		/// <param name="vertexDeclaration">A vertex declaration describing the structure of the buffer.</param>
		/// <param name="vertexCount">The count of vertices.</param>
		public DynamicVertexBuffer (GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount)
			: base (graphicsDevice, vertexDeclaration, vertexCount, BufferUsageHint.DynamicDraw)
		{
		}
	}
}

