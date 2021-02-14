using System;

namespace engenious.Graphics
{
	/// <summary>
	/// Describes a vertex declaration for describing buffer structure.
	/// </summary>
	public class VertexDeclaration : GraphicsResource
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="VertexDeclaration"/> class.
		/// </summary>
		/// <param name="vertexStride">The stride of this declaration inside a buffer.</param>
		/// <param name="elements">The elements describing the buffer.</param>
		public VertexDeclaration (int vertexStride, params VertexElement[] elements)
		{
			VertexStride = vertexStride;
			VertexElements = elements ?? throw new ArgumentNullException();
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="VertexDeclaration"/> class.
		/// </summary>
		/// <param name="elements">The elements describing the buffer.</param>
		public VertexDeclaration (params VertexElement[] elements)
		{
			VertexElements = elements ?? throw new ArgumentNullException();
			VertexStride = 0;
			foreach (var element in elements)
				VertexStride += element.ByteCount;
		}

		/// <summary>
		/// Gets the vertex elements describing the buffer structure.
		/// </summary>
		public VertexElement[] VertexElements { get; }

		/// <summary>
		/// Gets the stride of the vertex elements.
		/// </summary>
		public int VertexStride{ get; private set;}

		/// <summary>
		/// Gets or sets a divisor used for instancing.
		/// </summary>
        public int InstanceDivisor{get;set;}=-1;
	}
}

