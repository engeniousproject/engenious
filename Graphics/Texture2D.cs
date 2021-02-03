using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using engenious.Content;
using engenious.Content.Serialization;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// A 2D GPU texture.
    /// </summary>
    public class Texture2D : Texture
    {
        internal int Texture;
        private readonly PixelInternalFormat _internalFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="width">The texture width.</param>
        /// <param name="height">The texture height.</param>
        /// <param name="mipMaps">The number of mip-map levels.</param>
        /// <param name="internalFormat">The pixel format to use on GPU side.</param>
        public Texture2D(GraphicsDevice graphicsDevice, int width, int height, int mipMaps = 1,
            PixelInternalFormat internalFormat = PixelInternalFormat.Rgba8)
            : base(graphicsDevice, 1, internalFormat)
        {
            _internalFormat = internalFormat;
            Width = width;
            Height = height;
            Bounds = new Rectangle(0, 0, width, height);
            GraphicsDevice.ValidateUiGraphicsThread();

            Texture = GL.GenTexture();

            Bind();
            SetDefaultTextureParameters();
            var isDepthTarget = ((int) internalFormat >= (int) PixelInternalFormat.DepthComponent16 &&
                                  (int) internalFormat <= (int) PixelInternalFormat.DepthComponent32Sgix);
            if (isDepthTarget)
                GL.TexImage2D(
                    Target,
                    0,
                    (OpenTK.Graphics.OpenGL.PixelInternalFormat) internalFormat,
                    width,
                    height,
                    0,
                    OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent,
                    PixelType.Float,
                    IntPtr.Zero);
            else
                GL.TexStorage2D(
                    TextureTarget2d.Texture2D,
                    mipMaps,
                    (SizedInternalFormat) internalFormat,
                    width,
                    height);

            //GL.TexImage2D(Target, 0, (OpenTK.Graphics.OpenGL.PixelInternalFormat)internalFormat, width, height, 0, (OpenTK.Graphics.OpenGL.PixelFormat)Format, PixelType.UnsignedByte, IntPtr.Zero);
        }

        private void SetDefaultTextureParameters()
        {
            GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int) All.Linear);
            GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int) All.Linear);
            //if (GL.SupportsExtension ("Version12")) {
            //GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            //GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);


            /*} else {
				GL.TexParameter (Target, TextureParameterName.TextureWrapS, (int)All.Clamp);
				GL.TexParameter (Target, TextureParameterName.TextureWrapT, (int)All.Clamp);
			}*/
        }

        internal override void Bind()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            GL.BindTexture(Target, Texture);
        }

        /// <inheritdoc />
        public override void BindComputation(int unit = 0)
        {
            GL.BindImageTexture(unit, Texture, 0, false, 0, TextureAccess.WriteOnly,
                (SizedInternalFormat) _internalFormat);
        }

        /// <inheritdoc />
        public override bool Equals(Texture other)
        {
            if (!(other is Texture2D otherTex))
                return false;
            return (Texture == otherTex.Texture);
        }

        internal override TextureTarget Target => TextureTarget.Texture2D;

        /// <summary>
        /// Gets the width of the texture.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the texture.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the texture bounds.
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Sets the textures pixel data.
        /// </summary>
        /// <param name="data">The array containing the pixel data to write.</param>
        /// <param name="level">The mip map level to set the pixel data of.</param>
        /// <typeparam name="T">The type to write pixel data as.</typeparam>
        public void SetData<T>(ReadOnlySpan<T> data, int level = 0)
            where T : unmanaged
        {
            SetData(data,level,OpenTK.Graphics.OpenGL.PixelFormat.Bgra);
        }
        
        /// <summary>
        /// Sets the textures pixel data.
        /// </summary>
        /// <param name="data">The array containing the pixel data to write.</param>
        /// <param name="level">The mip map level to set the pixel data of.</param>
        /// <typeparam name="T">The type to write pixel data as.</typeparam>
        public void SetData<T>(T[] data, int level = 0)
            where T : unmanaged
        {
            SetData(data,level,OpenTK.Graphics.OpenGL.PixelFormat.Bgra);
        }

        internal unsafe void SetData<T>(T[] data, int level,OpenTK.Graphics.OpenGL.PixelFormat format)
            where T : unmanaged
        {
            SetData<T>(data.AsSpan(), level, format);
        }


        internal unsafe void SetData<T>(ReadOnlySpan<T> data, int level,OpenTK.Graphics.OpenGL.PixelFormat format)
            where T : unmanaged
        {
            var hwCompressed =
                format == (OpenTK.Graphics.OpenGL.PixelFormat) TextureContentFormat.DXT1 ||
                format == (OpenTK.Graphics.OpenGL.PixelFormat) TextureContentFormat.DXT3 || format ==
                (OpenTK.Graphics.OpenGL.PixelFormat) TextureContentFormat.DXT5;
            if (!hwCompressed && sizeof(T) * data.Length < Width * Height)
                throw new ArgumentException("Not enough pixel data");

            int width = Width, height = Height;
            for (var i = 0; i < level; i++)
            {
                width /= 2;
                height /= 2;
                if (width == 0 || height == 0)
                    return;
            }
            GraphicsDevice.ValidateUiGraphicsThread();

            Bind();
            var pxType = PixelType.UnsignedByte;
            if (typeof(T) == typeof(Color))
                pxType = PixelType.Float;
            if (hwCompressed)
            {
                var blockSize = (format ==
                                 (OpenTK.Graphics.OpenGL.PixelFormat) TextureContentFormat
                                     .DXT1)
                    ? 8
                    : 16;
                var mipSize = ((width + 3) / 4) * ((height + 3) / 4) * blockSize;
                fixed(T* buffer = &data.GetPinnableReference())
                    GL.CompressedTexSubImage2D(Target, level, 0, 0, width, height,
                        format, mipSize, (IntPtr)buffer);
            }
            else
                fixed(T* buffer = &data.GetPinnableReference())
                    GL.TexSubImage2D(Target, level, 0, 0, width, height, format, pxType,
                        (IntPtr)buffer);

            //GL.TexSubImage2D<T> (Target, 0, 0, 0, Width, Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, getPixelType (typeof(T)), data);
        }

        /// <summary>
        /// Sets the textures pixel data.
        /// </summary>
        /// <param name="data">The array containing the pixel data to write.</param>
        /// <param name="startIndex">The starting index to start reading from.</param>
        /// <param name="elementCount">The maximum number of elements to write.</param>
        /// <typeparam name="T">The type to write pixel data as.</typeparam>
        public void SetData<T>(T[] data, int startIndex, int elementCount) where T : unmanaged
        {
            SetData<T>(data.AsSpan().Slice(startIndex, elementCount), 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra);
        }

        /// <summary>
        /// Sets the textures pixel data.
        /// </summary>
        /// <param name="level">The mip map level to set the pixel data of.</param>
        /// <param name="rect">The region to set the pixels of, or null for the whole texture.</param>
        /// <param name="data">The array containing the pixel data to write.</param>
        /// <param name="startIndex">The starting index to start reading from.</param>
        /// <param name="elementCount">The maximum number of elements to write.</param>
        /// <typeparam name="T">The type to write pixel data as.</typeparam>
        public void SetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount)
            where T : unmanaged
        {
            throw new NotImplementedException("Need to implement offset"); //TODO:
            /*if (rect.HasValue)
				GL.TexSubImage2D<T> (Target, level, rect.Value.X, rect.Value.Y, rect.Value.Width, rect.Value.Height, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, data);
			else
				GL.TexSubImage2D<T> (Target, level, 0, 0, Width, Height, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, data);*/
        }

        
        /// <summary>
        /// Gets the textures pixel data.
        /// </summary>
        /// <param name="data">The array to write the pixel data into.</param>
        /// <param name="level">The mip map level to set the pixel data of.</param>
        /// <typeparam name="T">The type to read pixel data as.</typeparam>
        public void GetData<T>(T[] data, int level = 0) where T : unmanaged
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            Bind();

            GL.GetTexImage(Target, level, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data);
        }
                
        /// <summary>
        /// Gets the textures pixel data.
        /// </summary>
        /// <param name="data">The array to write the pixel data into.</param>
        /// <param name="level">The mip map level to set the pixel data of.</param>
        /// <typeparam name="T">The type to read pixel data as.</typeparam>
        public unsafe void GetData<T>(Span<T> data, int level = 0) where T : unmanaged
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            Bind();
            fixed(T* buffer = &data.GetPinnableReference())
                GL.GetTexImage(Target, level, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte, (IntPtr)buffer);
        }

        /// <summary>
        /// Gets the textures pixel data.
        /// </summary>
        /// <param name="level">The mip map level to get the pixel data from.</param>
        /// <param name="rect">The region to get the pixels of, or null for the whole texture.</param>
        /// <param name="data">The array to write the pixel data into.</param>
        /// <param name="startIndex">The starting index to write into.</param>
        /// <param name="elementCount">The maximum number of elements to read.</param>
        /// <typeparam name="T">The type to read pixel data as.</typeparam>
        public void GetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount)
            where T : unmanaged
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            Bind();
            if (rect.HasValue)
            {
                //TODO: use ? GL.CopyImageSubData(
                var temp = new T[Width * Height];
                GL.GetTexImage(Target, level, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte, temp);
                int z = 0, w = 0, index = 0;

                for (var y = rect.Value.Y; y < rect.Value.Y + rect.Value.Height; ++y)
                {
                    for (var x = rect.Value.X; x < rect.Value.X + rect.Value.Width; ++x)
                    {
                        data[z * rect.Value.Width + w] = temp[(y * Width) + x];
                        ++w;
                        ++index;
                    }
                    ++z;
                    w = 0;
                }
            }
            else
            {
                GL.GetTexImage(Target, level, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte, data);
            }
        }

        /// <summary>
        /// Loads a <see cref="Texture2D"/> from a stream.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="stream">The stream to load the texture from.</param>
        /// <param name="mipMaps">The number of mip-map levels.</param>
        /// <returns>The loaded texture.</returns>
        public static Texture2D FromStream(GraphicsDevice graphicsDevice, Stream stream, int mipMaps = 1)
        {
            using (var bmp = new Bitmap(stream))
            {
                return FromBitmap(graphicsDevice, bmp, mipMaps);
            }
        }

        /// <summary>
        /// Loads a <see cref="Texture2D"/> from a file.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="filename">Path to a file to load the texture from.</param>
        /// <param name="mipMaps">The number of mip-map levels.</param>
        /// <returns>The loaded texture.</returns>
        public static Texture2D FromFile(GraphicsDevice graphicsDevice, string filename, int mipMaps = 1)
        {
            using (var bmp = new Bitmap(filename))
            {
                return FromBitmap(graphicsDevice, bmp, mipMaps);
            }
        }

        /// <summary>
        /// Loads a <see cref="Texture2D"/> from a existing <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="bmp">Path to a file to load the texture from.</param>
        /// <param name="mipMaps">The number of mip-map levels.</param>
        /// <returns>The loaded texture.</returns>
        public static Texture2D FromBitmap(GraphicsDevice graphicsDevice, Bitmap bmp, int mipMaps = 1)
        {
            Texture2D text;
            text = new Texture2D(graphicsDevice, bmp.Width, bmp.Height, mipMaps);
            var bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, text.Width, text.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            graphicsDevice.ValidateUiGraphicsThread();

            text.Bind();
            GL.TexSubImage2D(text.Target, 0, 0, 0, text.Width, text.Height,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
            bmp.UnlockBits(bmpData);


            return text;
        }

        /// <summary>
        /// Converts a <see cref="Texture2D"/> to a <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        /// <param name="text">The texture to convert.</param>
        /// <returns>The converted texture as a <see cref="System.Drawing.Bitmap"/>.</returns>
        public static Bitmap ToBitmap(Texture2D text)
        {
            var bmp = new Bitmap(text.Width, text.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, text.Width, text.Height),
                ImageLockMode.WriteOnly, bmp.PixelFormat);
            text.GraphicsDevice.ValidateUiGraphicsThread();

            text.Bind();
            GL.GetTexImage(text.Target, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, bmpData.Scan0);
            //System.Runtime.InteropServices.Marshal.Copy (bmpData.Scan0, data, 0, data.Length);//TODO: performance
            //TODO: convert pixel formats


            bmp.UnlockBits(bmpData);

            return bmp;
        }

        /// <summary>
        /// Loads a <see cref="Texture2D"/> from a stream.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="stream">The stream to load the texture from.</param>
        /// <param name="width">The width the texture should be stretched to.</param>
        /// <param name="height">The height the texture should be stretched to.</param>
        /// <param name="zoom">Whether to keep the aspect ratio while scaling.</param>
        /// <param name="mipMaps">The number of mip-map levels.</param>
        /// <returns>The loaded texture.</returns>
        public static Texture2D FromStream(GraphicsDevice graphicsDevice, Stream stream, int width, int height,
            bool zoom = false, int mipMaps = 1)
        {
            //TODO:implement correct
            using (var bmp = new Bitmap(stream))
            {
                using (var scaled = new Bitmap(bmp, width, height))
                {
                    //TODO: zoom?
                    return FromBitmap(graphicsDevice, scaled, mipMaps);
                }
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            GL.DeleteTexture(Texture);

            base.Dispose();
        }
    }
}