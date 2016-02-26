using System;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace engenious.Graphics
{
    public class Texture2D:Texture
    {
        internal int texture;

        public Texture2D(GraphicsDevice graphicsDevice, int width, int height, PixelFormat format = PixelFormat.Rgba)
            : this(graphicsDevice, width, height, (PixelInternalFormat)format, format)
        {

        }

        protected Texture2D(GraphicsDevice graphicsDevice, int width, int height, PixelInternalFormat internalFormat = PixelInternalFormat.Rgba, PixelFormat format = PixelFormat.Rgba)
            : base(graphicsDevice, 1, format)
        {
            Width = width;
            Height = height;
            Bounds = new Rectangle(0, 0, width, height);
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    texture = GL.GenTexture();

                    Bind();
                    setDefaultTextureParameters();
                    GL.TexImage2D(TextureTarget.Texture2D, 0, (OpenTK.Graphics.OpenGL4.PixelInternalFormat)internalFormat, width, height, 0, (OpenTK.Graphics.OpenGL4.PixelFormat)Format, PixelType.UnsignedByte, IntPtr.Zero);
                });
        }

        private void setDefaultTextureParameters()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            //if (GL.SupportsExtension ("Version12")) {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            /*} else {
				GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Clamp);
				GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Clamp);
			}*/
        }

        public Texture2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, PixelFormat format)
            : this(graphicsDevice, width, height, format)
        {
            //TODO: mipmap
        }

        internal override void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, texture);
        }

        internal override void SetSampler(SamplerState state)
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    state = state == null ? SamplerState.LinearClamp : state;
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)state.AddressU);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)state.AddressV);
                });
        }

        public int Width{ get; private set; }

        public int Height{ get; private set; }

        public Rectangle Bounds{ get; private set; }

        private static PixelType getPixelType(Type type)
        {
            if (type == typeof(float) || type == typeof(double))
            {
                return PixelType.Float;
            }
            else if (type == typeof(int))
            {
                return PixelType.Byte;
            }
            else if (type == typeof(uint))
            {
                return PixelType.UnsignedByte;
            }
            return PixelType.UnsignedByte;
        }

        public void SetData<T>(T[] data) where T : struct//ValueType
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
				
                        GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, pxType, handle.AddrOfPinnedObject());
                    });
                handle.Free();
            }
            //GL.TexSubImage2D<T> (TextureTarget.Texture2D, 0, 0, 0, Width, Height, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, getPixelType (typeof(T)), data);
        }

        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct
        {
            throw new NotImplementedException("Need to implement offset");
            //TODO:GL.TexSubImage2D<T> (TextureTarget.Texture2D, 0, 0, 0, Width, Height, OpenTK.Graphics.OpenGL4.PixelFormat.Rgba, PixelType.UnsignedByte, data);
        }

        public void SetData<T>(int level, Nullable<Rectangle> rect, T[] data, int startIndex, int elementCount) where T : struct
        {
            throw new NotImplementedException("Need to implement offset");//TODO:
            /*if (rect.HasValue)
				GL.TexSubImage2D<T> (TextureTarget.Texture2D, level, rect.Value.X, rect.Value.Y, rect.Value.Width, rect.Value.Height, OpenTK.Graphics.OpenGL4.PixelFormat.Rgba, PixelType.UnsignedByte, data);
			else
				GL.TexSubImage2D<T> (TextureTarget.Texture2D, level, 0, 0, Width, Height, OpenTK.Graphics.OpenGL4.PixelFormat.Rgba, PixelType.UnsignedByte, data);*/
        }

        public void GetData<T>(T[] data) where T:struct//ValueType
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    Bind();

                    /*if (glFormat == All.CompressedTextureFormats) {
				throw new NotImplementedException ();
			} else {*/
                    int level = 0;
                    Rectangle? rect = new Rectangle?();//new Rectangle? (new Rectangle (0, 0, Width, Height));
                    if (rect.HasValue)
                    {
                        var temp = new T[this.Width * this.Height];
                        GL.GetTexImage(TextureTarget.Texture2D, level, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, temp);
                        int z = 0, w = 0;

                        for (int y = rect.Value.Y; y < rect.Value.Y + rect.Value.Height; ++y)
                        {
                            for (int x = rect.Value.X; x < rect.Value.X + rect.Value.Width; ++x)
                            {
                                data[z * rect.Value.Width + w] = temp[(y * Width) + x];
                                ++w;
                            }
                            ++z;
                            w = 0;
                        }
                    }
                    else
                    {
                        GL.GetTexImage(TextureTarget.Texture2D, level, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data);
                    }
                });
            //}
        }

        public void GetData<T>(int level, Nullable<Rectangle> rect, T[] data, int startIndex, int elementCount) where T : struct
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    Bind();
                    if (rect.HasValue)
                    {
                        //TODO: use ? GL.CopyImageSubData(
                        var temp = new T[this.Width * this.Height];
                        GL.GetTexImage(TextureTarget.Texture2D, level, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, temp);
                        int z = 0, w = 0;

                        for (int y = rect.Value.Y; y < rect.Value.Y + rect.Value.Height; ++y)
                        {
                            for (int x = rect.Value.X; x < rect.Value.X + rect.Value.Width; ++x)
                            {
                                data[z * rect.Value.Width + w] = temp[(y * Width) + x];
                                ++w;
                            }
                            ++z;
                            w = 0;
                        }
                    }
                    else
                    {
                        GL.GetTexImage(TextureTarget.Texture2D, level, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data);
                    }
                });
        }

        public static Texture2D FromStream(GraphicsDevice graphicsDevice, Stream stream)
        {
            using (Bitmap bmp = new Bitmap(stream))
            {
                return FromBitmap(graphicsDevice, bmp);
            }
        }

        public static Texture2D FromBitmap(GraphicsDevice graphicsDevice, Bitmap bmp)
        {
            Texture2D text = null;
            text = new Texture2D(graphicsDevice, bmp.Width, bmp.Height, PixelFormat.Rgba);
            BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, text.Width, text.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    text.Bind();
                    GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, text.Width, text.Height, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
                });
            bmp.UnlockBits(bmpData);


            return text;
        }

        public static Bitmap ToBitmap(Texture2D text)
        {
            Bitmap bmp = new Bitmap(text.Width, text.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, text.Width, text.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    text.Bind();
                    GL.GetTexImage(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
                });
            //System.Runtime.InteropServices.Marshal.Copy (bmpData.Scan0, data, 0, data.Length);//TODO: performance
            //TODO: convert pixel formats


            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public static Texture2D FromStream(GraphicsDevice graphicsDevice, Stream stream, int width, int height, bool zoom)
        {
            //TODO:implement correct
            using (Bitmap bmp = new Bitmap(stream))
            {
                using (Bitmap scaled = new Bitmap(bmp, width, height))
                {
                    //TODO: zoom?
                    return FromBitmap(graphicsDevice, scaled);
                }
            }
        }

        public override void Dispose()
        {
            ThreadingHelper.BlockOnUIThread(() =>
                {
                    GL.DeleteTexture(texture);
                });

            base.Dispose();
        }
    }
}

