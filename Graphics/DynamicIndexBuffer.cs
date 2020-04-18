using System;

namespace engenious.Graphics
{
	/// <summary>
	/// A dynamic version of the <see cref="IndexBuffer"/> class.
	/// </summary>
	public class DynamicIndexBuffer : IndexBuffer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicIndexBuffer"/> class.
		/// </summary>
		/// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to be created on.</param>
		/// <param name="indexType">The type of the indices.</param>
		/// <param name="indexCount">The count of indices.</param>
		public DynamicIndexBuffer (GraphicsDevice graphicsDevice, Type indexType, int indexCount)
			: base (graphicsDevice, indexType, indexCount, BufferUsageHint.DynamicDraw)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicIndexBuffer"/> class.
		/// </summary>
		/// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to be created on.</param>
		/// <param name="indexElementSize">The basic size of the indices.</param>
		/// <param name="indexCount">The count of indices</param>
		public DynamicIndexBuffer (GraphicsDevice graphicsDevice, DrawElementsType indexElementSize, int indexCount)
			: base (graphicsDevice, indexElementSize, indexCount, BufferUsageHint.DynamicDraw)
		{
		}
	}
}

