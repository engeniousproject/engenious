using System;
using engenious.Graphics;

namespace engenious.Graphics
{
    public class MeshContent
    {
        public MeshContent()
        {
        }

        public int PrimitiveCount{ get; set; }

        public VertexPositionNormalTexture[] Vertices{ get; set; }
    }
}

