using System;
using OpenTK;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Reflection;

namespace engenious.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPosition:IVertexType
    {
        public static readonly VertexDeclaration VertexDeclaration;

        static VertexPosition()
        {
            VertexElement[] elements = new VertexElement[] { new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0) };
            VertexDeclaration declaration = new VertexDeclaration(elements);
            VertexDeclaration = declaration;
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexDeclaration;
            }
        }

        public VertexPosition(Vector3 position)
        {
            this.Position = position;
        }

        public Vector3 Position;
    }
}

