using System;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace engenious.Graphics
{
    public class VertexBuffer : GraphicsResource
    {
        private int vbo;
        internal VertexAttributes vao = null;

        private VertexBuffer(GraphicsDevice graphicsDevice, int vertexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : base(graphicsDevice)
        {

            this.VertexCount = vertexCount;
            this.BufferUsage = usage;
        }

        public VertexBuffer(GraphicsDevice graphicsDevice, Type vertexType, int vertexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : this(graphicsDevice, vertexCount, usage)
        {
            IVertexType tp = Activator.CreateInstance(vertexType) as IVertexType;
            if (tp == null)
                throw new ArgumentException("must be a vertexType");
			
            this.VertexDeclaration = tp.VertexDeclaration;
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    vbo = GL.GenBuffer();

                    vao = new VertexAttributes();
                    vao.Bind();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertexCount * VertexDeclaration.VertexStride), IntPtr.Zero, (OpenTK.Graphics.OpenGL4.BufferUsageHint)BufferUsage);
                });
        }

        public VertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : this(graphicsDevice, vertexCount, usage)
        {
            this.VertexDeclaration = vertexDeclaration;
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    vbo = GL.GenBuffer();

                    vao = new VertexAttributes();
                    vao.Bind();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertexCount * VertexDeclaration.VertexStride), IntPtr.Zero, (OpenTK.Graphics.OpenGL4.BufferUsageHint)BufferUsage);
                });
        }

        public BufferUsageHint BufferUsage{ get; private set; }

        public int VertexCount{ get; private set; }

        public VertexDeclaration VertexDeclaration{ get; private set; }

        public void SetData<T>(T[] data) where T:struct
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    vao.Bind();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

                    GL.BufferSubData<T>(BufferTarget.ArrayBuffer, IntPtr.Zero, new IntPtr(VertexCount * VertexDeclaration.VertexStride), data);//TODO use bufferusage

                    VertexAttributes.ApplyAttributes(vao, VertexDeclaration);
                });
        }

        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    vao.Bind();//TODO: verify
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

                    GCHandle buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
                    GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, new IntPtr(elementCount * VertexDeclaration.VertexStride), buffer.AddrOfPinnedObject() + startIndex * VertexDeclaration.VertexStride);//TODO use bufferusage

                    buffer.Free();

                    VertexAttributes.ApplyAttributes(vao, VertexDeclaration);
                });
        }

        public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    vao.Bind();//TODO: verify
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

                    GCHandle buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
                    GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(offsetInBytes), new IntPtr(elementCount * vertexStride), buffer.AddrOfPinnedObject() + startIndex * vertexStride);

                    buffer.Free();

                    VertexAttributes.ApplyAttributes(vao, VertexDeclaration);
                });
        }

        public override void Dispose()
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.DeleteBuffer(vbo);
                });
            base.Dispose();
        }
    }
}

