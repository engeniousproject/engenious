using System;
using engenious.Helper;
using engenious.Utility;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    internal class VertexAttributes:IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        internal int Vbo;
        private readonly int _vao;

        public VertexAttributes(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;

            _vao = GL.GenVertexArray();
        }

        private static void Add(VertexElement el, int stride, int perInstances = -1)
        {
            switch (el.VertexElementUsage)
            {
                case VertexElementUsage.Color:
                    
                    GL.EnableVertexAttribArray((int)el.VertexElementUsage);
                    GL.VertexAttribPointer((int)el.VertexElementUsage, el.ByteCount / el.GetGlVertexDataTypeSize(), el.GetGlVertexDataType(), el.IsNormalized, stride, new IntPtr(el.Offset));
                    break;
                case VertexElementUsage.Position:
                    GL.EnableVertexAttribArray((int)el.VertexElementUsage);
                    GL.VertexAttribPointer((int)el.VertexElementUsage, el.ByteCount / el.GetGlVertexDataTypeSize(), el.GetGlVertexDataType(), el.IsNormalized, stride, new IntPtr(el.Offset));
                    break;
                case VertexElementUsage.Normal:
                    GL.EnableVertexAttribArray((int)el.VertexElementUsage);
                    GL.VertexAttribPointer((int)el.VertexElementUsage, el.ByteCount / el.GetGlVertexDataTypeSize(), el.GetGlVertexDataType(), el.IsNormalized, stride, new IntPtr(el.Offset));
                    break;
                case VertexElementUsage.TextureCoordinate:
                    GL.EnableVertexAttribArray((int)el.VertexElementUsage);
                    GL.VertexAttribPointer((int)el.VertexElementUsage, el.ByteCount / el.GetGlVertexDataTypeSize(), el.GetGlVertexDataType(), el.IsNormalized, stride, new IntPtr(el.Offset));
                    break;
                default:
                    int index = (int) el.VertexElementUsage + el.UsageIndex;
                    GL.VertexAttribPointer(index, el.ByteCount / el.GetGlVertexDataTypeSize(), el.GetGlVertexDataType(), el.IsNormalized, stride, new IntPtr(el.Offset));
                    GL.EnableVertexAttribArray(index);
                       
                    break;
            }
            if (perInstances > 0)
                GL.VertexAttribDivisor(el.UsageIndex, perInstances);
        }

        public void Bind()
        {
            GL.BindVertexArray(_vao);
        }

        public static void ApplyAttributes(VertexAttributes attribs, VertexDeclaration declaration)
        {
            attribs.Bind();
            foreach (var el in declaration.VertexElements)
            {
                Add(el, declaration.VertexStride,declaration.InstanceDivisor);
            }
            GL.BindVertexArray(0);
        }

        public bool IsDisposed{ get; private set; }

        private static void DeleteVertexArray(VertexAttributes va)
        {
            GL.DeleteVertexArray(va._vao);
        }
        public unsafe void Dispose()
        {
            if (!IsDisposed)
                _graphicsDevice.UiThread.QueueWork(CapturingDelegate.Create(&DeleteVertexArray, this));
            IsDisposed = true;
        }
    }
}

