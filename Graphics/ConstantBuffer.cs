using System;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using engenious.Helper;

namespace engenious.Graphics
{
    /// <summary>
    /// A constant buffer for sending data to the GPU.
    /// </summary>
    public class ConstantBuffer
    {
        internal int Ubo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantBuffer"/> class.
        /// </summary>
        /// <param name="size">The size of the <see cref="ConstantBuffer"/>.</param>
        public ConstantBuffer(int size)
        {
            using (Execute.OnUiContext)
            {
                Ubo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
                GL.BufferData(BufferTarget.UniformBuffer, new IntPtr(size), IntPtr.Zero, OpenTK.Graphics.OpenGL.BufferUsageHint.DynamicDraw);
            }
        }

        /// <summary>
        /// Updates the buffer data.
        /// </summary>
        /// <param name="data">A pointer to new data.</param>
        /// <param name="size">The size to copy to the <see cref="ConstantBuffer"/>.</param>
        public unsafe void Update(IntPtr data, uint size)
        {
            using (Execute.OnUiContext)
            {
                GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
                var ptr = GL.MapBuffer(BufferTarget.UniformBuffer, BufferAccess.WriteOnly);

                System.Runtime.CompilerServices.Unsafe.CopyBlock((void*) ptr, (void*) data, size);
                //Buffer.BlockCopy(
                GL.UnmapBuffer(BufferTarget.UniformBuffer);
            }
        }

        /// <summary>
        /// Updates the buffer data.
        /// </summary>
        /// <param name="data">The data to copy.</param>
        /// <typeparam name="T">The data type.</typeparam>
        public unsafe void Update<T>(T data) where T : struct
        {
            using (Execute.OnUiContext)
            {
                GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
                var ptr = GL.MapBuffer(BufferTarget.UniformBuffer, BufferAccess.WriteOnly);

                System.Runtime.CompilerServices.Unsafe.Write((void*) ptr, data);

                GL.UnmapBuffer(BufferTarget.UniformBuffer);
            }
        }

        /// <summary>
        /// Updates the buffer data.
        /// </summary>
        /// <param name="data">The data array to copy.</param>
        /// <typeparam name="T">The data type.</typeparam>
        public unsafe void Update<T>(T[] data) where T : struct
        {
            using (Execute.OnUiContext)
            {
                GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
                var ptr = GL.MapBuffer(BufferTarget.UniformBuffer, BufferAccess.WriteOnly);

                System.Runtime.CompilerServices.Unsafe.Write((void*) ptr, data);

                GL.UnmapBuffer(BufferTarget.UniformBuffer);
            }
        }
    }
}

