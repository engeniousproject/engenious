using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using engenious.Content;
using engenious.Content.Serialization;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace engenious.Graphics
{
    /// <summary>
    /// A 2D GPU texture.
    /// </summary>
    public class Texture2D : Texture, IEquatable<Texture2D>
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
        public override bool Equals(Texture? other)
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
        /// Loads a <see cref="Texture2D"/> from a stream.
        /// </summary>
        /// <param name="stream">The stream to load the texture from.</param>
        public void LoadFrom(Stream stream)
        {
            using var bmp = Image.Load<Rgba32>(ImageSharpHelper.Config, stream);
            LoadFrom(bmp);
        }

        /// <summary>
        /// Loads a <see cref="Texture2D"/> from a file.
        /// </summary>
        /// <param name="filename">Path to a file to load the texture from.</param>
        public void LoadFrom(string filename)
        {
            using var bmp = Image.Load<Rgba32>(ImageSharpHelper.Config, filename);
            LoadFrom(bmp);
        }

        /// <summary>
        /// Loads a <see cref="Texture2D"/> from a image.
        /// </summary>
        /// <param name="image">Image to load the texture from.</param>
        public void LoadFrom(Image image)
        {
            if (Width != image.Width || Height != image.Height)
                throw new ArgumentOutOfRangeException(nameof(image));

            LoadFrom(image.ToContinuousImage<Rgba32>());
        }

        /// <summary>
        /// Loads a <see cref="Texture2D"/> from a bitmap in RGBA32 format.
        /// </summary>
        /// <param name="image">Image to load the texture from.</param>
        public void LoadFrom(Image<Rgba32> image)
        {
            if (image.ToContinuousImage().DangerousTryGetSinglePixelMemory(out var data))
            {
                var span = data.Span;
                GraphicsDevice.ValidateUiGraphicsThread();
                Bind();
                GL.TexSubImage2D(Target, 0, 0, 0, Width, Height,
                    OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, ref span.GetPinnableReference());
            }
        }

        /// <summary>
        /// Copy this texture to the given texture.
        /// </summary>
        /// <param name="dest">The destination texture to copy to.</param>
        /// <param name="srcRect">The rectangle region to copy from.</param>
        /// <param name="destPos">The destination position to copy to.</param>
        /// <param name="sourceLevel">The source level to copy from.</param>
        /// <param name="destinationLevel">The destination level to copy to.</param>
        public void CopyTo(Texture2D dest, Rectangle srcRect, Point destPos,
            int sourceLevel = 0, int destinationLevel = 0)
        {
            CopyTo(dest, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, destPos.X, destPos.Y, sourceLevel, destinationLevel);
        }

        /// <summary>
        /// Copy this texture to the given texture.
        /// </summary>
        /// <param name="dest">The destination texture to copy to.</param>
        /// <param name="srcX">The x starting position to copy from.</param>
        /// <param name="srcY">The y starting position to copy from.</param>
        /// <param name="srcWidth">The width of the region to copy from.</param>
        /// <param name="srcHeight">The height of the region to copy from.</param>
        /// <param name="destX">The destination x-position to copy to.</param>
        /// <param name="destY">The destination y-position to copy to.</param>
        /// <param name="sourceLevel">The source level to copy from.</param>
        /// <param name="destinationLevel">The destination level to copy to.</param>
        public void CopyTo(Texture2D dest, int srcX, int srcY, int srcWidth, int srcHeight, int destX, int destY, int sourceLevel = 0, int destinationLevel = 0)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            
            GL.CopyImageSubData(Texture, (ImageTarget)Target, sourceLevel, srcX, srcY, 0, dest.Texture,
                (ImageTarget)dest.Target, destinationLevel, destX, destY, 0, srcWidth, srcHeight, 1);
        }

        /// <summary>
        /// Copy this texture to the given texture.
        /// </summary>
        /// <param name="dest">The destination texture to copy to.</param>
        /// <param name="srcRect">The rectangle region to copy from.</param>
        /// <param name="destPos">The destination position to copy to.</param>
        /// <param name="destZ">The destination z-position to copy to.</param>
        /// <param name="sourceLevel">The source level to copy from.</param>
        /// <param name="destinationLevel">The destination level to copy to.</param>
        public void CopyTo(Texture2DArray dest, Rectangle srcRect, Point destPos, int destZ,
            int sourceLevel = 0, int destinationLevel = 0)
        {
            CopyTo(dest, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, destPos.X, destPos.Y, destZ, sourceLevel, destinationLevel);
        }

        /// <summary>
        /// Copy this texture to the given texture.
        /// </summary>
        /// <param name="dest">The destination texture to copy to.</param>
        /// <param name="srcX">The x starting position to copy from.</param>
        /// <param name="srcY">The y starting position to copy from.</param>
        /// <param name="srcWidth">The width of the region to copy from.</param>
        /// <param name="srcHeight">The height of the region to copy from.</param>
        /// <param name="destX">The destination x-position to copy to.</param>
        /// <param name="destY">The destination y-position to copy to.</param>
        /// <param name="destZ">The destination z-position to copy to.</param>
        /// <param name="sourceLevel">The source level to copy from.</param>
        /// <param name="destinationLevel">The destination level to copy to.</param>
        public void CopyTo(Texture2DArray dest, int srcX, int srcY, int srcWidth, int srcHeight, int destX, int destY, int destZ, int sourceLevel = 0, int destinationLevel = 0)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            
            GL.CopyImageSubData(Texture, (ImageTarget)Target, sourceLevel, srcX, srcY, 0, dest.Texture,
                (ImageTarget)dest.Target, destinationLevel, destX, destY, destZ, srcWidth, srcHeight, 1);
        }

        /// <summary>
        /// Sets the textures pixel data.
        /// </summary>
        /// <param name="data">The array containing the pixel data to write.</param>
        /// <param name="level">The mip map level to set the pixel data of.</param>
        public void SetData(Image<Rgba32> data, int level = 0)
        {
            if (data.ToContinuousImage().DangerousTryGetSinglePixelMemory(out var pixelData))
            {
                var span = pixelData.Span;
                SetData<Rgba32>(span, level);
            }
        }

        /// <summary>
        /// Sets the textures pixel data.
        /// </summary>
        /// <param name="data">The array containing the pixel data to write.</param>
        /// <param name="level">The mip map level to set the pixel data of.</param>
        /// <typeparam name="T">The type to write pixel data as.</typeparam>
        public void SetData<T>(ReadOnlySpan<T> data, int level = 0)
            where T : unmanaged
        {
            SetData(data,level,OpenTK.Graphics.OpenGL.PixelFormat.Rgba);
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
            SetData(data,level,OpenTK.Graphics.OpenGL.PixelFormat.Rgba);
        }

        internal unsafe void SetData<T>(T[] data, int level,OpenTK.Graphics.OpenGL.PixelFormat format)
            where T : unmanaged
        {
            SetData<T>(data.AsSpan(), level, format);
        }

        internal unsafe void SetData<T>(ReadOnlySpan<T> data, int level, OpenTK.Graphics.OpenGL.PixelFormat format)
            where T : unmanaged
        {
            int width = Width, height = Height;
            for (var i = 0; i < level; i++)
            {
                width /= 2;
                height /= 2;
                if (width == 0 || height == 0)
                    return;
            }

            SetData<T>(data, level, format, 0, 0, width, height);
        }

        internal unsafe void SetData<T>(ReadOnlySpan<T> data, int level, OpenTK.Graphics.OpenGL.PixelFormat format, int x, int y, int width, int height)
            where T : unmanaged
        {
            var hwCompressed =
                format == (OpenTK.Graphics.OpenGL.PixelFormat) TextureContentFormat.DXT1 ||
                format == (OpenTK.Graphics.OpenGL.PixelFormat) TextureContentFormat.DXT3 || format ==
                (OpenTK.Graphics.OpenGL.PixelFormat) TextureContentFormat.DXT5;
            if (!hwCompressed && sizeof(T) * data.Length < width * height)
                throw new ArgumentException("Not enough pixel data");
            
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
                    GL.CompressedTexSubImage2D(Target, level, x, y, width, height,
                        format, mipSize, (IntPtr)buffer);
            }
            else
                fixed(T* buffer = &data.GetPinnableReference())
                    GL.TexSubImage2D(Target, level, x, y, width, height, format, pxType,
                        (IntPtr)buffer);

            //GL.TexSubImage2D<T> (Target, 0, 0, 0, Width, Height, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, getPixelType (typeof(T)), data);
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
            SetData<T>(data.AsSpan().Slice(startIndex, elementCount), 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba);
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

            if (rect.HasValue)
            {
                var recVal = rect.Value;
                SetData<T>(data.AsSpan(startIndex, elementCount), level, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, recVal.X, recVal.Y, recVal.Width, recVal.Height);
            }
            else
            {
                SetData<T>(data.AsSpan(startIndex, elementCount), level, OpenTK.Graphics.OpenGL.PixelFormat.Rgba);
            }
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

            GL.GetTexImage(Target, level, OpenTK.Graphics.OpenGL.PixelFormat.Rgba,
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
                GL.GetTexImage(Target, level, OpenTK.Graphics.OpenGL.PixelFormat.Rgba,
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
                GL.GetTexImage(Target, level, OpenTK.Graphics.OpenGL.PixelFormat.Rgba,
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
                GL.GetTexImage(Target, level, OpenTK.Graphics.OpenGL.PixelFormat.Rgba,
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
            using var bmp = Image.Load<Rgba32>(ImageSharpHelper.Config, stream);
            return FromBitmap(graphicsDevice, bmp, mipMaps);
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
            using var bmp = Image.Load<Rgba32>(ImageSharpHelper.Config, filename);
            return FromBitmap(graphicsDevice, bmp, mipMaps);
        }

        /// <summary>
        /// Loads a <see cref="Texture2D"/> from a existing <see cref="Image"/>.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="image">Image to create the texture from.</param>
        /// <param name="mipMaps">The number of mip-map levels.</param>
        /// <returns>The loaded texture.</returns>
        public static Texture2D FromBitmap(GraphicsDevice graphicsDevice, Image image, int mipMaps = 1)
        {
            var text = new Texture2D(graphicsDevice, image.Width, image.Height, mipMaps);
            text.LoadFrom(image);
            return text;
        }

        /// <summary>
        /// Converts a <see cref="Texture2D"/> to a <see cref="Image{RGBA32}"/>.
        /// </summary>
        /// <param name="text">The texture to convert.</param>
        /// <returns>The converted texture as a <see cref="Image{RGBA32}"/>.</returns>
        public static Image<Rgba32> ToBitmap(Texture2D text)
        {
            var bmp = new Image<Rgba32>(ImageSharpHelper.Config, text.Width, text.Height);
            if (bmp.DangerousTryGetSinglePixelMemory(out var data))
            {
                text.GraphicsDevice.ValidateUiGraphicsThread();

                text.Bind();
                GL.GetTexImage(text.Target, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba,
                    PixelType.UnsignedByte, ref data);
            }

            //System.Runtime.InteropServices.Marshal.Copy (bmpData.Scan0, data, 0, data.Length);//TODO: performance
            //TODO: convert pixel formats
            

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
            using (var bmp = Image.Load(stream))
            {
                bmp.Mutate((p) => p.Resize(width, height));
                //TODO: zoom?
                return FromBitmap(graphicsDevice, bmp, mipMaps);
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            GL.DeleteTexture(Texture);

            base.Dispose();
        }

        /// <inheritdoc />
        public bool Equals(Texture2D? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Texture == other.Texture;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Texture2D text && Equals(text);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Texture;
        }
    }
}