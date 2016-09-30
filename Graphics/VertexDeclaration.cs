using System;

namespace engenious.Graphics
{
	public class VertexDeclaration:GraphicsResource
	{
		public VertexDeclaration (int vertexStride,params VertexElement[] elements)
		{
			this.VertexStride = vertexStride;
			this.VertexElements = elements;
		}
		public VertexDeclaration (params VertexElement[] elements)
		{
			if (elements == null)
				return;
			this.VertexElements = elements;
			VertexStride = 0;
			foreach (var element in elements)
				VertexStride += element.ByteCount;
		}
		public VertexElement[] VertexElements{get;private set;}
		public int VertexStride{ get; private set;}
        public int InstanceDivisor{get;set;}=-1;
	}
}

