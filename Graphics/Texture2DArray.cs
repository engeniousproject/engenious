using System;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace engenious.Graphics
{
    public class Texture2DArray:TextureArray
    {
        public Texture2DArray(GraphicsDevice graphicsDevice,int width,int height,int layerCount=1,int levelCount=1,PixelInternalFormat format=PixelInternalFormat.Rgba8)
            :base(graphicsDevice,layerCount,levelCount,format)
        {
            Width = width;
            Height = height;
            GL.TexStorage3D(TextureTarget3d.Texture2DArray,levelCount,(SizedInternalFormat)format,width,height,layerCount);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
        }
        public int Width{get;private set;}
        public int Height{get;private set;}

        public void SetData<T>(T[] data,int layer,int level=0) where T : struct//ValueType
        {
            if (Marshal.SizeOf(typeof(T)) * data.Length < Width * Height)
                throw new ArgumentException("Not enough pixel data");

            unsafe
            {
                GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                ThreadingHelper.BlockOnUIThread(() =>
                    {
                        Bind();
                        PixelType pxType = PixelType.UnsignedByte;
                        if (typeof(T) == typeof(Color))
                            pxType = PixelType.Float;

                        GL.TexSubImage2D(TextureTarget.Texture2DArray,0,0,0,Width,Height,OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,pxType,handle.AddrOfPinnedObject());
                        //GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, pxType, handle.AddrOfPinnedObject());
                    });
                handle.Free();
            }
            //GL.TexSubImage2D<T> (TextureTarget.Texture2D, 0, 0, 0, Width, Height, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, getPixelType (typeof(T)), data);
        }
    }
}

