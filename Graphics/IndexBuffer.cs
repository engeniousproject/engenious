using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace engenious.Graphics
{

    public class IndexBuffer :GraphicsResource
    {
        private int ibo;
        //private byte[] buffer;
        public IndexBuffer(GraphicsDevice graphicsDevice, Type indexType, int indexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : base(graphicsDevice)
        {
            int sz = System.Runtime.InteropServices.Marshal.SizeOf(indexType);
            if (sz <= 8)
                this.IndexElementSize = DrawElementsType.UnsignedByte;
            else if (sz <= 16)
                this.IndexElementSize = DrawElementsType.UnsignedShort;
            else if (sz <= 32)
                this.IndexElementSize = DrawElementsType.UnsignedInt;
            else
                throw new ArgumentException("Invalid Type(bigger than 32 bits)");
			
            this.IndexCount = indexCount;
            this.BufferUsage = usage;
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    ibo = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(indexCount * sz), IntPtr.Zero, (OpenTK.Graphics.OpenGL4.BufferUsageHint)BufferUsage);
                });
            GraphicsDevice.CheckError();
            //buffer = new byte[indexCount * (int)IndexElementSize / 8];
        }

        public IndexBuffer(GraphicsDevice graphicsDevice, DrawElementsType indexElementSize, int indexCount, BufferUsageHint usage = BufferUsageHint.StaticDraw)
            : base(graphicsDevice)
        {
            this.IndexElementSize = indexElementSize;
            this.IndexCount = indexCount;
            this.BufferUsage = usage;
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    ibo = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
                    int sz = (indexElementSize == DrawElementsType.UnsignedByte ? 1 : (indexElementSize == DrawElementsType.UnsignedShort ? 2 : 4));
                    GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(indexCount * sz), IntPtr.Zero, (OpenTK.Graphics.OpenGL4.BufferUsageHint)BufferUsage);
                });
            GraphicsDevice.CheckError();
            //buffer = new byte[indexCount * (int)IndexElementSize / 8];
        }

        public BufferUsageHint BufferUsage{ get; private set; }

        public int IndexCount{ get; private set; }

        public DrawElementsType IndexElementSize{ get; private set; }


        public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount) where T : struct //TODO: valuetype?
        {
            if (elementCount == 0 || data.Length == 0)
                return;



            int elSize = Marshal.SizeOf(typeof(T));

            GCHandle buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    Bind();
                    GL.BufferSubData(BufferTarget.ElementArrayBuffer, new IntPtr(offsetInBytes), new IntPtr(elementCount * elSize), buffer.AddrOfPinnedObject() + startIndex * elSize);//TODO:
                });
            buffer.Free();
            GraphicsDevice.CheckError();
            //Buffer.BlockCopy (data, startIndex, buffer, offsetInBytes, elementCount);
        }

        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
        {
            if (elementCount == 0 || data.Length == 0)
                return;


            int elSize = Marshal.SizeOf(typeof(T));

            GCHandle buffer = GCHandle.Alloc(data, GCHandleType.Pinned);
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    Bind();

                    GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, new IntPtr(elementCount * elSize), buffer.AddrOfPinnedObject() + startIndex * elSize);//TODO:
                });
            buffer.Free();
            GraphicsDevice.CheckError();
            //GL.BufferData<T> (BufferTarget.ElementArrayBuffer, new IntPtr (elementCount * Marshal.SizeOf(T)), data[startIndex], BufferUsageHint.StaticDraw);//TODO:
            //Buffer.BlockCopy (data, startIndex, buffer, 0, elementCount);
        }

        public void SetData<T>(T[] data) where T : struct
        {
            if (data.Length == 0)
                return;



            ThreadingHelper.BlockOnUIThread(() =>
                {
                    Bind();
                    GL.BufferSubData<T>(BufferTarget.ElementArrayBuffer, IntPtr.Zero, new IntPtr(IndexCount * Marshal.SizeOf(default(T))), data);//TODO:
                });
            GraphicsDevice.CheckError();
            //Buffer.BlockCopy (data, 0, buffer, 0, data.Length);
        }

        internal void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GraphicsDevice.CheckError();
        }

        public override void Dispose()
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.DeleteBuffer(ibo);
                });
            base.Dispose();
        }
    }
}

