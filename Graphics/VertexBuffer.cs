using System;
using System.Runtime.InteropServices;
using engenious.Helper;
using OpenToolkit.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// A buffer for vertex data.
    /// </summary>
    public class VertexBuffer : GraphicsResource
    {
        internal int Vbo;
        internal int TempVbo = -1;
        internal VertexAttributes Vao;

        private VertexBuffer(GraphicsDevice graphicsDevice, long vertexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : base(graphicsDevice)
        {
            VertexCount = vertexCount;
            BufferUsage = usage;
        }

        private void ExchangeVao()
        {
            Vao = new VertexAttributes();
            Vao.Vbo = Vbo;
            Vao.Bind();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            VertexAttributes.ApplyAttributes(Vao, VertexDeclaration);

            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBuffer"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to be created on.</param>
        /// <param name="vertexType">The type of the vertices.</param>
        /// <param name="vertexCount">The count of vertices.</param>
        /// <param name="usage">The usage of this <see cref="VertexBuffer"/>.</param>
        /// <exception cref="ArgumentException"></exception>
        public VertexBuffer(GraphicsDevice graphicsDevice, Type vertexType, long vertexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : this(graphicsDevice, vertexCount, usage)
        {
            var tp = Activator.CreateInstance(vertexType) as IVertexType;
            if (tp == null)
                throw new ArgumentException("must be a vertexType");
			
            VertexDeclaration = tp.VertexDeclaration;
            
            GraphicsDevice.ValidateGraphicsThread();
            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                new IntPtr(vertexCount * VertexDeclaration.VertexStride),
                IntPtr.Zero,
                (OpenToolkit.Graphics.OpenGL.BufferUsageHint) BufferUsage);
            ExchangeVao();
            GraphicsDevice.CheckError();
        }

        internal bool Bind()
        {
            if (Vao == null)
                return false;
            Vao.Bind();
            GraphicsDevice.CheckError();
            return true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBuffer"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to be created on.</param>
        /// <param name="vertexDeclaration">A vertex declaration describing the structure of the buffer.</param>
        /// <param name="vertexCount">The count of vertices.</param>
        /// <param name="usage">The usage of this <see cref="VertexBuffer"/>.</param>
        public VertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : this(graphicsDevice, vertexCount, usage)
        {
            VertexDeclaration = vertexDeclaration;
            GraphicsDevice.ValidateGraphicsThread();
        
            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertexCount * VertexDeclaration.VertexStride), IntPtr.Zero, (OpenToolkit.Graphics.OpenGL.BufferUsageHint)BufferUsage);

            ExchangeVao();
            GraphicsDevice.CheckError();
        }

        /// <summary>
        /// Resizes the <see cref="VertexBuffer"/> to a new size with optionally keeping its data.
        /// </summary>
        /// <param name="vertexCount">The new count of vertices needed.</param>
        /// <param name="keepData">Whether to keep the old data or not.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Resize(int vertexCount, bool keepData = false)
        {
            GraphicsDevice.ValidateGraphicsThread();
            
            GL.BindVertexArray(0);
            var tempVbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tempVbo);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                new IntPtr(vertexCount * VertexDeclaration.VertexStride),
                IntPtr.Zero,
                (OpenToolkit.Graphics.OpenGL.BufferUsageHint) BufferUsage);
            GraphicsDevice.CheckError();

            VertexCount = vertexCount;
            GL.DeleteBuffer(Vbo);
            Vbo = tempVbo;
            GraphicsDevice.CheckError();

            if (keepData)
            {
                //TODO:
                throw new NotImplementedException();
            }

        }

        internal void EnsureVao()
        {
            if (Vao != null && Vao.Vbo != Vbo)
            {
                Vao.Vbo = Vbo;
                Vao.Bind();
                GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
                VertexAttributes.ApplyAttributes(Vao, VertexDeclaration);

                GL.BindVertexArray(0);
            }
        }

        /// <summary>
        /// Gets the buffer usage.
        /// </summary>
        public BufferUsageHint BufferUsage{ get; private set; }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public long VertexCount{ get; private set; }

        /// <summary>
        /// Gets the vertex declaration describing this vertex buffer.
        /// </summary>
        public VertexDeclaration VertexDeclaration{ get; private set; }

        /// <summary>
        /// Sets the <see cref="VertexBuffer"/> vertices data.
        /// </summary>
        /// <param name="data">The vertices.</param>
        /// <typeparam name="T">The vertex type.</typeparam>
        public void SetData<T>(T[] data) where T:struct
        {
            GraphicsDevice.ValidateGraphicsThread();
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);

            var buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, new IntPtr(data.Length * VertexDeclaration.VertexStride), buffer.AddrOfPinnedObject());
                //TODO use bufferusage
            buffer.Free();
            GraphicsDevice.CheckError();
        }

        /// <summary>
        /// Sets the <see cref="VertexBuffer"/> vertices data.
        /// </summary>
        /// <param name="ptr">Pointer to copy data from.</param>
        /// <param name="sizeInBytes">The number of bytes to copy from the source pointer.</param>
        public void SetData(IntPtr ptr, long sizeInBytes)
        {
            SetData(ptr, 0, sizeInBytes);
        }

        
        /// <summary>
        /// Sets the <see cref="VertexBuffer"/> vertices data.
        /// </summary>
        /// <param name="ptr">Pointer to copy data from.</param>
        /// <param name="offsetInBytes">The offset destination to copy the vertices to.</param>
        /// <param name="sizeInBytes">The number of bytes to copy from the source pointer.</param>
        public void SetData(IntPtr ptr,long offsetInBytes,long sizeInBytes)
        {
            GraphicsDevice.ValidateGraphicsThread();
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);

            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(offsetInBytes), new IntPtr(sizeInBytes), ptr);
            GraphicsDevice.CheckError();
        }

        /// <summary>
        /// Sets the <see cref="VertexBuffer"/> vertices data.
        /// </summary>
        /// <param name="data">The vertices.</param>
        /// <param name="startIndex">The offset to start copying vertices from.</param>
        /// <param name="elementCount">The count of vertices to copy.</param>
        /// <typeparam name="T">The vertex type.</typeparam>
        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
        {
            GraphicsDevice.ValidateGraphicsThread();
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);

            var buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
            GL.BufferSubData(
                BufferTarget.ArrayBuffer,
                IntPtr.Zero,
                new IntPtr(elementCount * VertexDeclaration.VertexStride),
                buffer.AddrOfPinnedObject() + startIndex * VertexDeclaration.VertexStride); //TODO use bufferusage

            buffer.Free();
            GraphicsDevice.CheckError();
        }

        /// <summary>
        /// Sets the <see cref="VertexBuffer"/> vertices data.
        /// </summary>
        /// <param name="offsetInBytes">The offset destination to copy the vertices to.</param>
        /// <param name="data">The vertices.</param>
        /// <param name="startIndex">The offset to start copying vertices from.</param>
        /// <param name="elementCount">The count of vertices to copy.</param>
        /// <param name="vertexStride">The vertex stride used.</param>
        /// <typeparam name="T">The vertex type.</typeparam>
        public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct
        {
            GraphicsDevice.ValidateGraphicsThread();

            //vao.Bind();//TODO: verify
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);

            var buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
            GL.BufferSubData(
                BufferTarget.ArrayBuffer,
                new IntPtr(offsetInBytes),
                new IntPtr(elementCount * vertexStride),
                buffer.AddrOfPinnedObject() + startIndex * vertexStride);

            buffer.Free();
            GraphicsDevice.CheckError();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            GraphicsDevice.ValidateGraphicsThread();

            GL.DeleteBuffer(Vbo);
            Vao.Dispose();
            base.Dispose();
        }
    }
}

