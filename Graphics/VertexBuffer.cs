using System;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace engenious.Graphics
{
    public class VertexBuffer : GraphicsResource
    {
        private int vbo = -1;
        private int tempVBO = -1;
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
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertexCount * VertexDeclaration.VertexStride), IntPtr.Zero, (OpenTK.Graphics.OpenGL4.BufferUsageHint)BufferUsage);
                });
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    vao = new VertexAttributes();
                    vao.Bind();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    VertexAttributes.ApplyAttributes(vao, VertexDeclaration);

                    GL.BindVertexArray(0);
                }, true);
            GraphicsDevice.CheckError();
        }

        internal bool Bind()
        {
            if (vao == null)
                return false;
            vao.Bind();
            GraphicsDevice.CheckError();
            return true;
        }

        public VertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : this(graphicsDevice, vertexCount, usage)
        {
            this.VertexDeclaration = vertexDeclaration;
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    vbo = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertexCount * VertexDeclaration.VertexStride), IntPtr.Zero, (OpenTK.Graphics.OpenGL4.BufferUsageHint)BufferUsage);
                });
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    vao = new VertexAttributes();
                    vao.Bind();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    VertexAttributes.ApplyAttributes(vao, VertexDeclaration);
                    GL.BindVertexArray(0);

                }, true);
            GraphicsDevice.CheckError();
        }

        public void Resize(int vertexCount, bool keepData = false)
        {
            this.VertexCount = vertexCount;
            tempVBO = vbo;
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    vbo = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(VertexCount * VertexDeclaration.VertexStride), IntPtr.Zero, (OpenTK.Graphics.OpenGL4.BufferUsageHint)BufferUsage);
                });
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    vao.Bind();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    VertexAttributes.ApplyAttributes(vao, VertexDeclaration);

                    GL.BindVertexArray(0);
                }, true);
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.DeleteBuffer(tempVBO);
                    tempVBO = -1;
                }, true);
            if (keepData)
            {
                //TODO:
                throw new NotImplementedException();
            }

        }

        public BufferUsageHint BufferUsage{ get; private set; }

        public int VertexCount{ get; private set; }

        public VertexDeclaration VertexDeclaration{ get; private set; }

        public void SetData<T>(T[] data) where T:struct
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    GCHandle buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
                    GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, new IntPtr(VertexCount * VertexDeclaration.VertexStride), buffer.AddrOfPinnedObject());//TODO use bufferusage
                    buffer.Free();
                }, true);
            GraphicsDevice.CheckError();
        }

        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

                    GCHandle buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
                    GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, new IntPtr(elementCount * VertexDeclaration.VertexStride), buffer.AddrOfPinnedObject() + startIndex * VertexDeclaration.VertexStride);//TODO use bufferusage

                    buffer.Free();
                });
            GraphicsDevice.CheckError();
        }

        public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    //vao.Bind();//TODO: verify
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

                    GCHandle buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
                    GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(offsetInBytes), new IntPtr(elementCount * vertexStride), buffer.AddrOfPinnedObject() + startIndex * vertexStride);

                    buffer.Free();
                });
            GraphicsDevice.CheckError();
        }

        public override void Dispose()
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.DeleteBuffer(vbo);
                });
            vao.Dispose();
            base.Dispose();
        }
    }
}

