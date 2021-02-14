using System;
using System.Runtime.InteropServices;
using System.Threading;
using engenious.Helper;
using engenious.Utility;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// A buffer for vertex data.
    /// </summary>
    public class VertexBuffer : GraphicsResource
    {
        internal int Vbo, NextVbo = -1;
        internal long NextVertexCount;
        internal VertexAttributes? Vao;
        private readonly WaitCallback _waitForExchange;

        private VertexBuffer(GraphicsDevice graphicsDevice, long vertexCapacity, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : base(graphicsDevice)
        {
            VertexCapacity = vertexCapacity;
            BufferUsage = usage;

            VertexDeclaration = null!;
            unsafe
            {
                _waitForExchange = (userData) =>
                {
                    ((AutoResetEvent) userData!).WaitOne();

                    GraphicsDevice.UiThread.QueueWork(CapturingDelegate.Create(&DoExchange, this));
                };
            }
        }

        private void ExchangeVao()
        {
            Vao = new VertexAttributes(GraphicsDevice);
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
        /// <param name="vertexCapacity">The maximum count of vertices.</param>
        /// <param name="usage">The usage of this <see cref="VertexBuffer"/>.</param>
        /// <exception cref="ArgumentException"></exception>
        public VertexBuffer(GraphicsDevice graphicsDevice, Type vertexType, long vertexCapacity, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : this(graphicsDevice, vertexCapacity, usage)
        {
            var tp = Activator.CreateInstance(vertexType) as IVertexType;
            if (tp == null)
                throw new ArgumentException("must be a vertexType");
			
            VertexDeclaration = tp.VertexDeclaration;
            
            GraphicsDevice.ValidateUiGraphicsThread();
            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                new IntPtr(vertexCapacity * VertexDeclaration.VertexStride),
                IntPtr.Zero,
                (OpenTK.Graphics.OpenGL.BufferUsageHint) BufferUsage);
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
        /// <param name="vertexCapacity">The maximum count of vertices.</param>
        /// <param name="usage">The usage of this <see cref="VertexBuffer"/>.</param>
        public VertexBuffer(GraphicsDevice graphicsDevice, VertexDeclaration vertexDeclaration, int vertexCapacity, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : this(graphicsDevice, vertexCapacity, usage)
        {
            VertexDeclaration = vertexDeclaration;
            GraphicsDevice.ValidateUiGraphicsThread();
        
            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertexCapacity * VertexDeclaration.VertexStride), IntPtr.Zero, (OpenTK.Graphics.OpenGL.BufferUsageHint)BufferUsage);

            ExchangeVao();
            GraphicsDevice.CheckError();
        }

        /// <summary>
        /// Resizes the <see cref="VertexBuffer"/> to a new size with optionally keeping its data.
        /// </summary>
        /// <param name="vertexCapacity">The new maximum count of vertices needed.</param>
        /// <param name="keepData">Whether to keep the old data or not.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Resize(int vertexCapacity, bool keepData = false)
        {
            var tempVbo = ResizeInternal(vertexCapacity, keepData);
            GL.DeleteBuffer(Vbo);
            Vbo = tempVbo;
            VertexCapacity = vertexCapacity;
        }
        private int ResizeInternal(long vertexCapacity, bool keepData = false)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            
            GL.BindVertexArray(0);
            var tempVbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tempVbo);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                new IntPtr(vertexCapacity * VertexDeclaration.VertexStride),
                IntPtr.Zero,
                (OpenTK.Graphics.OpenGL.BufferUsageHint) BufferUsage);
            GraphicsDevice.CheckError();

            if (keepData)
            {
                //TODO:
                throw new NotImplementedException();
            }

            return tempVbo;
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

        /// <inheritdoc cref="GraphicsResource.GraphicsDevice"/>
        public new GraphicsDevice GraphicsDevice => base.GraphicsDevice!;

        /// <summary>
        /// Gets the buffer usage.
        /// </summary>
        public BufferUsageHint BufferUsage{ get; private set; }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public long VertexCount{ get; private set; }

        /// <summary>
        /// Gets the number of vertices that can be written without resize.
        /// </summary>
        public long VertexCapacity{ get; private set; }

        /// <summary>
        /// Gets the vertex declaration describing this vertex buffer.
        /// </summary>
        public VertexDeclaration VertexDeclaration{ get; private set; }

        /// <summary>
        /// Sets the <see cref="VertexBuffer"/> vertices data.
        /// </summary>
        /// <param name="data">The vertices.</param>
        /// <typeparam name="T">The vertex type.</typeparam>
        public void SetData<T>(T[] data) where T : unmanaged
        {
            SetData<T>(data.AsSpan());
        }

        /// <summary>
        /// Resets <see cref="VertexCount"/> to 0.
        /// </summary>
        public void Clear()
        {
            VertexCount = 0;
        }

        private static void DoExchange(VertexBuffer that)
        {
            var newVbo = Interlocked.Exchange(ref that.NextVbo, -1);
            if (newVbo == -1)
                return;
            var oldVbo = that.Vbo;

            that.Vbo = newVbo;
            that.VertexCapacity = that.NextVertexCount;
            that.VertexCount = that.NextVertexCount;
            GL.DeleteBuffer(oldVbo);
        }

        /// <summary>
        /// Sets the <see cref="VertexBuffer"/> vertices data resizes the buffer if necessary
        /// and exchanges the data with the current vertex data seamlessly.
        /// </summary>
        /// <param name="data">The vertices.</param>
        /// <typeparam name="T">The vertex type.</typeparam>
        public unsafe void ExchangeData<T>(ReadOnlySpan<T> data) where T : unmanaged
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            int newLength = data.Length;
            var tempVbo = ResizeInternal(newLength, false);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, tempVbo);

            fixed(T* buffer = &data.GetPinnableReference())
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, new IntPtr(data.Length * VertexDeclaration.VertexStride), (IntPtr)buffer);

            var fence = GraphicsDevice.CreateFenceAsync();
            NextVertexCount = newLength;
            NextVbo = tempVbo;

            ThreadPool.QueueUserWorkItem(_waitForExchange, fence);


            GraphicsDevice.CheckError();
        }

        /// <summary>
        /// Sets the <see cref="VertexBuffer"/> vertices data.
        /// </summary>
        /// <param name="data">The vertices.</param>
        /// <typeparam name="T">The vertex type.</typeparam>
        public unsafe void SetData<T>(ReadOnlySpan<T> data) where T : unmanaged
        {
            fixed(T* buffer = &data.GetPinnableReference())
                SetData((IntPtr)buffer, 0, data.Length * VertexDeclaration.VertexStride);
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
        public void SetData(IntPtr ptr, long offsetInBytes, long sizeInBytes)
        {
            VertexCount = sizeInBytes / VertexDeclaration.VertexStride;
            GraphicsDevice.ValidateUiGraphicsThread();
            
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
        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : unmanaged
        {
            SetData<T>(data.AsSpan(startIndex, elementCount));
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
        public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : unmanaged
        {
            SetData<T>(offsetInBytes, data.AsSpan(), startIndex, elementCount, vertexStride);
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
        public unsafe void SetData<T>(int offsetInBytes, ReadOnlySpan<T> data, int startIndex, int elementCount, int vertexStride) where T : unmanaged
        {
            fixed(T* buffer = &data.GetPinnableReference())
                SetData((IntPtr)buffer + startIndex * vertexStride, offsetInBytes, elementCount * vertexStride);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            GL.DeleteBuffer(Vbo);
            Vao?.Dispose();
            base.Dispose();
        }
    }
}

