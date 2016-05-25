using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace engenious.Graphics
{
    public class Texture2DArray : Texture
    {
        private int texture;
        public Texture2DArray(GraphicsDevice graphicsDevice, int levels, int width, int height, int layers)
            : base(graphicsDevice)
        {
            texture = GL.GenTexture();
            
            GL.BindTexture(TextureTarget.Texture2DArray, texture);

            GL.TexStorage3D(TextureTarget3d.Texture2DArray, levels, SizedInternalFormat.Rgba8, width, height, layers);

            Width = width;
            Height = height;
            LayerCount = layers;
        }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int LayerCount { get; private set; }

        internal override void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2DArray, texture);
        }
        internal override void SetSampler(SamplerState state)
        {
            //TODO:throw new NotImplementedException();
        }
        public void SetData<T>(T[] data,int layer,int level=0)where T : struct
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            ThreadingHelper.BlockOnUIThread(() =>
            {
                Bind();
                PixelType pxType = PixelType.UnsignedByte;
                if (typeof(T) == typeof(Color))
                    pxType = PixelType.Float;

                    GL.TexSubImage3D(TextureTarget.Texture2DArray, level, 0, 0,layer, Width, Height,1, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, pxType, handle.AddrOfPinnedObject());
            });
            handle.Free();
        }
    }
}