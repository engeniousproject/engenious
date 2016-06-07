using System;
using OpenTK;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Reflection;

namespace engenious.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionNormal:IVertexType
    {
        public static readonly VertexDeclaration VertexDeclaration;

        static VertexPositionNormal()
        {
            VertexElement[] elements = new VertexElement[] { new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0) };
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

        public VertexPositionNormal(Vector3 position, Vector3 normal)
        {
            this.Normal = normal;
            this.Position = position;
        }

        public Vector3 Position;
        public Vector3 Normal;
        //public Vector3 Position{ get; private set;}
        //public Color Color{ get; private set;}
    }
}

