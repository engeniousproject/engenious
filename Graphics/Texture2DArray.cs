using System;
using System.Runtime.InteropServices;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace engenious.Graphics
{
    /// <summary>
    /// An array of 2D GPU textures.
    /// </summary>
    public class Texture2DArray : TextureArray
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2DArray"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="levels">The number mip-map levels.</param>
        /// <param name="width">The width of the contained textures.</param>
        /// <param name="height">The height of the contained textures.</param>
        /// <param name="layers">The number of layers for this texture array.</param>
        /// <param name="format">The pixel format to use on GPU side.</param>
        public Texture2DArray(GraphicsDevice graphicsDevice, int levels, int width, int height, int layers, PixelInternalFormat format = PixelInternalFormat.Rgba8)
            : base(graphicsDevice, format: format)
        {
            GL.TexStorage3D(TextureTarget3d.Texture2DArray, levels, (SizedInternalFormat)Format, width, height, Math.Max(layers,1));

            Width = width;
            Height = height;
            LayerCount = layers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2DArray"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="levels">The number mip-map levels.</param>
        /// <param name="width">The width of the contained textures.</param>
        /// <param name="height">The height of the contained textures.</param>
        /// <param name="textures">The 2D textures to copy to this texture array.</param>
        public Texture2DArray(GraphicsDevice graphicsDevice,int levels,int width,int height,Texture2D[] textures)
            :this(graphicsDevice,levels,width,height,textures.Length)
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            var layer=0;

            var createMipMaps=false;
            foreach(var text in textures){
                if (text.LevelCount < levels)
                    createMipMaps = true;
                int mipWidth =text.Width,mipHeight=text.Height;
                for (var i=0;i<1 && createMipMaps || !createMipMaps && i < levels;i++){
                    GL.CopyImageSubData(text.Texture,ImageTarget.Texture2D,i,0,0,0,Texture,ImageTarget.Texture2DArray,i,0,0,layer,mipWidth,mipHeight,1);
                    mipWidth/=2;
                    mipHeight /=2;
                }
                layer++;
            }
            if (createMipMaps){
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
            }
            SetDefaultTextureParameters();
        }
        private void SetDefaultTextureParameters()
        {
            GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(Target,TextureParameterName.TextureMagFilter,(int)All.Linear);
            GL.TexParameter(Target,TextureParameterName.TextureMinFilter,(int)All.LinearMipmapLinear);
        }

        /// <summary>
        /// Gets the width of the contained textures.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the contained textures.
        /// </summary>
        public int Height { get; }

        internal override TextureTarget Target => TextureTarget.Texture2DArray;

        /// <inheritdoc />
        public override void BindComputation(int unit = 0)
        {
            GL.BindImageTexture(unit, Texture, 0, false, 0, TextureAccess.WriteOnly,
                (SizedInternalFormat)Format);
        }
        
        /// <summary>
        /// Copy this texture to the given texture.
        /// </summary>
        /// <param name="dest">The destination texture to copy to.</param>
        /// <param name="srcRect">The rectangle region to copy from.</param>
        /// <param name="srcZ">The z starting position to copy from.</param>
        /// <param name="destPos">The destination position to copy to.</param>
        /// <param name="sourceLevel">The source level to copy from.</param>
        /// <param name="destinationLevel">The destination level to copy to.</param>
        public void CopyTo(Texture2D dest, Rectangle srcRect, int srcZ, Point destPos,
            int sourceLevel = 0, int destinationLevel = 0)
        {
            CopyTo(dest, srcRect.X, srcRect.Y, srcZ, srcRect.Width, srcRect.Height, destPos.X, destPos.Y, sourceLevel, destinationLevel);
        }

        /// <summary>
        /// Copy this texture to the given texture.
        /// </summary>
        /// <param name="dest">The destination texture to copy to.</param>
        /// <param name="srcX">The x starting position to copy from.</param>
        /// <param name="srcY">The y starting position to copy from.</param>
        /// <param name="srcZ">The z starting position to copy from.</param>
        /// <param name="srcWidth">The width of the region to copy from.</param>
        /// <param name="srcHeight">The height of the region to copy from.</param>
        /// <param name="destX">The destination x-position to copy to.</param>
        /// <param name="destY">The destination y-position to copy to.</param>
        /// <param name="sourceLevel">The source level to copy from.</param>
        /// <param name="destinationLevel">The destination level to copy to.</param>
        public void CopyTo(Texture2D dest, int srcX, int srcY, int srcZ, int srcWidth, int srcHeight, int destX, int destY, int sourceLevel = 0, int destinationLevel = 0)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            
            GL.CopyImageSubData(Texture, (ImageTarget)Target, sourceLevel, srcX, srcY, srcZ, dest.Texture,
                (ImageTarget)dest.Target, destinationLevel, destX, destY, 0, srcWidth, srcHeight, 1);
        }

        /// <summary>
        /// Copy this texture to the given texture.
        /// </summary>
        /// <param name="dest">The destination texture to copy to.</param>
        /// <param name="srcRect">The rectangle region to copy from.</param>
        /// <param name="srcZ">The z starting position to copy from.</param>
        /// <param name="srcDepth">The depth of the region to copy from.</param>
        /// <param name="destPos">The destination position to copy to.</param>
        /// <param name="destZ">The destination z-position to copy to.</param>
        /// <param name="sourceLevel">The source level to copy from.</param>
        /// <param name="destinationLevel">The destination level to copy to.</param>
        public void CopyTo(Texture2DArray dest, Rectangle srcRect, int srcZ, int srcDepth, Point destPos, int destZ,
            int sourceLevel = 0, int destinationLevel = 0)
        {
            CopyTo(dest, srcRect.X, srcRect.Y, srcZ, srcRect.Width, srcRect.Height, srcDepth, destPos.X, destPos.Y, destZ, sourceLevel, destinationLevel);
        }

        /// <summary>
        /// Copy this texture to the given texture.
        /// </summary>
        /// <param name="dest">The destination texture to copy to.</param>
        /// <param name="srcX">The x starting position to copy from.</param>
        /// <param name="srcY">The y starting position to copy from.</param>
        /// <param name="srcZ">The z starting position to copy from.</param>
        /// <param name="srcWidth">The width of the region to copy from.</param>
        /// <param name="srcHeight">The height of the region to copy from.</param>
        /// <param name="srcDepth">The depth of the region to copy from.</param>
        /// <param name="destX">The destination x-position to copy to.</param>
        /// <param name="destY">The destination y-position to copy to.</param>
        /// <param name="destZ">The destination z-position to copy to.</param>
        /// <param name="sourceLevel">The source level to copy from.</param>
        /// <param name="destinationLevel">The destination level to copy to.</param>
        public void CopyTo(Texture2DArray dest, int srcX, int srcY, int srcZ, int srcWidth, int srcHeight, int srcDepth, int destX, int destY, int destZ, int sourceLevel = 0, int destinationLevel = 0)
        {
            GraphicsDevice.ValidateUiGraphicsThread();
            
            GL.CopyImageSubData(Texture, (ImageTarget)Target, sourceLevel, srcX, srcY, srcZ, dest.Texture,
                (ImageTarget)dest.Target, destinationLevel, destX, destY, destZ, srcWidth, srcHeight, srcDepth);
        }

        /// <summary>
        /// Sets the textures pixel data.
        /// </summary>
        /// <param name="data">The array containing the pixel data to write.</param>
        /// <param name="layer">The layer to write to.</param>
        /// <param name="level">The mip-map level to write to.</param>
        /// <typeparam name="T">The type to write pixel data as.</typeparam>
        public unsafe void SetData<T>(Image<Rgba32> data, int layer, int level = 0) where T : unmanaged
        {
            if (data.ToContinuousImage().DangerousTryGetSinglePixelMemory(out var pixelData))
            {
                var span = pixelData.Span;
                SetData<Rgba32>(span, layer, level);
            }
        }

        /// <summary>
        /// Sets the textures pixel data.
        /// </summary>
        /// <param name="data">The array containing the pixel data to write.</param>
        /// <param name="layer">The layer to write to.</param>
        /// <param name="level">The mip-map level to write to.</param>
        /// <typeparam name="T">The type to write pixel data as.</typeparam>
        public unsafe void SetData<T>(ReadOnlySpan<T> data, int layer, int level=0) where T : unmanaged
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            Bind();
            var pxType = PixelType.UnsignedByte;
            if (typeof(T) == typeof(Color))
                pxType = PixelType.Float;

            fixed(T* buffer = &data.GetPinnableReference())
                GL.TexSubImage3D(Target, level, 0, 0,layer, Width, Height,1, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, pxType, (IntPtr)buffer);
        }
        
        /// <summary>
        /// Sets the textures pixel data.
        /// </summary>
        /// <param name="data">The array containing the pixel data to write.</param>
        /// <param name="layer">The layer to write to.</param>
        /// <param name="level">The mip-map level to write to.</param>
        /// <typeparam name="T">The type to write pixel data as.</typeparam>
        public void SetData<T>(T[] data, int layer, int level=0) where T : unmanaged
        {
            SetData<T>(data.AsSpan(), layer, level);
        }
    }
}
