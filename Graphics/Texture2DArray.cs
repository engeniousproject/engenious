using System;
using System.Runtime.InteropServices;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// An array of 2D GPU textures.
    /// </summary>
    public class Texture2DArray : TextureArray
    {
        private readonly int _texture;
        private readonly PixelInternalFormat _internalFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2DArray"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="levels">The number mip-map levels.</param>
        /// <param name="width">The width of the contained textures.</param>
        /// <param name="height">The height of the contained textures.</param>
        /// <param name="layers">The number of layers for this texture array.</param>
        public Texture2DArray(GraphicsDevice graphicsDevice, int levels, int width, int height, int layers)
            : base(graphicsDevice)
        {
            using (Execute.OnUiContext)
            {
                _texture = GL.GenTexture();
                
                GL.BindTexture(Target, _texture);
                _internalFormat = PixelInternalFormat.Rgba8;//TODO dynamic format
                GL.TexStorage3D(TextureTarget3d.Texture2DArray, levels, SizedInternalFormat.Rgba8, width, height, Math.Max(layers,1));
            }
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
            using (Execute.OnUiContext)
            {
                var layer=0;

                var createMipMaps=false;
                foreach(var text in textures){
                    if (text.LevelCount < levels)
                        createMipMaps = true;
                    int mipWidth =text.Width,mipHeight=text.Height;
                    for (var i=0;i<1 && createMipMaps || !createMipMaps && i < levels;i++){
                        GL.CopyImageSubData(text.Texture,ImageTarget.Texture2D,i,0,0,0,_texture,ImageTarget.Texture2DArray,i,0,0,layer,mipWidth,mipHeight,1);
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
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the contained textures.
        /// </summary>
        public int Height { get; private set; }

        internal override void Bind()
        {
            GL.BindTexture(Target, _texture);
        }
        internal override TextureTarget Target => TextureTarget.Texture2DArray;

        /// <inheritdoc />
        public override void BindComputation(int unit = 0)
        {
            GL.BindImageTexture(unit, _texture, 0, false, 0, TextureAccess.WriteOnly,
                (SizedInternalFormat) _internalFormat);
        }

        
        /// <summary>
        /// Sets the textures pixel data.
        /// </summary>
        /// <param name="data">The array containing the pixel data to write.</param>
        /// <param name="layer">The layer to write to.</param>
        /// <param name="level">The mip-map level to write to.</param>
        /// <typeparam name="T">The type to write pixel data as.</typeparam>
        public void SetData<T>(T[] data,int layer,int level=0)where T : struct
        {
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            using (Execute.OnUiContext)
            {
                Bind();
                var pxType = PixelType.UnsignedByte;
                if (typeof(T) == typeof(Color))
                    pxType = PixelType.Float;

                GL.TexSubImage3D(Target, level, 0, 0,layer, Width, Height,1, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, pxType, handle.AddrOfPinnedObject());
            }
            handle.Free();
        }
    }
}
