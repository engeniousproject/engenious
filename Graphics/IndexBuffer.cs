using System;
using System.Runtime.InteropServices;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// A buffer to index vertex data.
    /// </summary>
    public class IndexBuffer : GraphicsResource
    {
        private int _ibo;
        //private byte[] buffer;
        private readonly int _elementSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexBuffer"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to be created on.</param>
        /// <param name="indexType">The type of the indices.</param>
        /// <param name="indexCount">The count of indices.</param>
        /// <param name="usage">The usage of this <see cref="IndexBuffer"/>.</param>
        public IndexBuffer(GraphicsDevice graphicsDevice, Type indexType, int indexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : base(graphicsDevice)
        {
            _elementSize = Marshal.SizeOf(indexType);
            if (_elementSize <= 1)
                IndexElementSize = DrawElementsType.UnsignedByte;
            else if (_elementSize <= 2)
                IndexElementSize = DrawElementsType.UnsignedShort;
            else if (_elementSize <= 4)
                IndexElementSize = DrawElementsType.UnsignedInt;
            else
                throw new ArgumentException("Invalid Type(bigger than 32 bits)");
			
            IndexCount = indexCount;
            BufferUsage = usage;
            graphicsDevice.ValidateGraphicsThread();
            _ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                new IntPtr(indexCount * _elementSize),
                IntPtr.Zero,
                (OpenTK.Graphics.OpenGL.BufferUsageHint) BufferUsage);
            GraphicsDevice.CheckError();
            //buffer = new byte[indexCount * (int)IndexElementSize / 8];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexBuffer"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to be created on.</param>
        /// <param name="indexElementSize">The basic size of the indices.</param>
        /// <param name="indexCount">The count of indices</param>
        /// <param name="usage">The usage of this <see cref="IndexBuffer"/>.</param>
        public IndexBuffer(GraphicsDevice graphicsDevice, DrawElementsType indexElementSize, int indexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : base(graphicsDevice)
        {
            IndexElementSize = indexElementSize;
            IndexCount = indexCount;
            BufferUsage = usage;
            graphicsDevice.ValidateGraphicsThread();
            _ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
            _elementSize = (indexElementSize == DrawElementsType.UnsignedByte ? 1 : (indexElementSize == DrawElementsType.UnsignedShort ? 2 : 4));
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                new IntPtr(indexCount * _elementSize),
                IntPtr.Zero,
                (OpenTK.Graphics.OpenGL.BufferUsageHint) BufferUsage);
            GraphicsDevice.CheckError();
            //buffer = new byte[indexCount * (int)IndexElementSize / 8];
        }

        /// <summary>
        /// Gets the buffer usage.
        /// </summary>
        public BufferUsageHint BufferUsage{ get; private set; }

        /// <summary>
        /// Gets the number of indices.
        /// </summary>
        public int IndexCount{ get; private set; }

        /// <summary>
        /// Gets the basic size of a single index.
        /// </summary>
        public DrawElementsType IndexElementSize{ get; private set; }

        /// <summary>
        /// Sets the <see cref="IndexBuffer"/> indices data.
        /// </summary>
        /// <param name="offsetInBytes">The offset destination to copy the indices to.</param>
        /// <param name="data">The indices.</param>
        /// <param name="startIndex">The offset to start copying indices from.</param>
        /// <param name="elementCount">The count of indices to copy.</param>
        /// <typeparam name="T">The index type.</typeparam>
        public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount) where T : unmanaged
        {
            SetData<T>(offsetInBytes, data.AsSpan(), startIndex, elementCount);
        }
        
        
        /// <summary>
        /// Sets the <see cref="IndexBuffer"/> indices data.
        /// </summary>
        /// <param name="offsetInBytes">The offset destination to copy the indices to.</param>
        /// <param name="data">The indices.</param>
        /// <param name="startIndex">The offset to start copying indices from.</param>
        /// <param name="elementCount">The count of indices to copy.</param>
        /// <typeparam name="T">The index type.</typeparam>
        public unsafe void SetData<T>(int offsetInBytes, ReadOnlySpan<T> data, int startIndex, int elementCount) where T : unmanaged
        {
            if (elementCount == 0 || data.Length == 0)
                return;

            GraphicsDevice.ValidateGraphicsThread();
            Bind();
            
            fixed(T* buffer = &data.GetPinnableReference())
                GL.BufferSubData(
                    BufferTarget.ElementArrayBuffer,
                    new IntPtr(offsetInBytes),
                    new IntPtr(elementCount * sizeof(T)),
                    (IntPtr)buffer + startIndex * sizeof(T)); //TODO:
            GraphicsDevice.CheckError();
            //Buffer.BlockCopy (data, startIndex, buffer, offsetInBytes, elementCount);
        }

        /// <summary>
        /// Sets the <see cref="IndexBuffer"/> indices data.
        /// </summary>
        /// <param name="data">The indices.</param>
        /// <param name="startIndex">The offset to start copying indices from.</param>
        /// <param name="elementCount">The count of indices to copy.</param>
        /// <typeparam name="T">The index type.</typeparam>
        public unsafe void SetData<T>(T[] data, int startIndex, int elementCount) where T : unmanaged
        {
            if (elementCount == 0 || data.Length == 0)
                return;


            var elSize = Marshal.SizeOf(typeof(T));

            var buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
            GraphicsDevice.ValidateGraphicsThread();
            Bind();

            GL.BufferSubData(
                BufferTarget.ElementArrayBuffer,
                IntPtr.Zero,
                new IntPtr(elementCount * sizeof(T)),
                buffer.AddrOfPinnedObject() + startIndex * sizeof(T)); //TODO:
            buffer.Free();
            GraphicsDevice.CheckError();
            //GL.BufferData<T> (BufferTarget.ElementArrayBuffer, new IntPtr (elementCount * sizeof(T)), data[startIndex], BufferUsageHint.StaticDraw);//TODO:
            //Buffer.BlockCopy (data, startIndex, buffer, 0, elementCount);
        }

        /// <summary>
        /// Sets the <see cref="IndexBuffer"/> indices data.
        /// </summary>
        /// <param name="data">The indices.</param>
        /// <typeparam name="T">The index type.</typeparam>
        public void SetData<T>(T[] data) where T : unmanaged
        {
            SetData<T>(data.AsSpan());
        }
        
        /// <summary>
        /// Sets the <see cref="IndexBuffer"/> indices data.
        /// </summary>
        /// <param name="data">The indices.</param>
        /// <typeparam name="T">The index type.</typeparam>
        public unsafe void SetData<T>(ReadOnlySpan<T> data) where T : unmanaged
        {
            if (data.Length == 0)
                return;

            GraphicsDevice.ValidateGraphicsThread();
            Bind();
            fixed(T* buffer = &data.GetPinnableReference())
                GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, new IntPtr(data.Length * sizeof(T)), (IntPtr)buffer); //TODO:

            GraphicsDevice.CheckError();
            //Buffer.BlockCopy (data, 0, buffer, 0, data.Length);
        }

        /// <summary>
        /// Resizes the <see cref="IndexBuffer"/> to a new size with optionally keeping its data.
        /// </summary>
        /// <param name="indexCount">The new count of indices needed.</param>
        /// <param name="keepData">Whether to keep the old data or not.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Resize(int indexCount, bool keepData = false)
        {
            
            int tempIBO;
            GraphicsDevice.ValidateGraphicsThread();
            tempIBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, tempIBO);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                new IntPtr(indexCount * _elementSize),
                IntPtr.Zero,
                (OpenTK.Graphics.OpenGL.BufferUsageHint) BufferUsage);


            GL.DeleteBuffer(_ibo);
            _ibo = tempIBO;

            IndexCount = indexCount;

            if (keepData)
            {
                //TODO:
                throw new NotImplementedException();
            }

        }


        internal void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
            GraphicsDevice.CheckError();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            GraphicsDevice.ValidateGraphicsThread();
            GL.DeleteBuffer(_ibo);
            base.Dispose();
        }
    }
}

