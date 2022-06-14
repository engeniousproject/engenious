using System;
using engenious.Helper;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// An array of GPU textures.
    /// </summary>
    public abstract class TextureArray : Texture, IEquatable<TextureArray>
    {
        /// <summary>
        /// The maximum number of texture arrays possible.
        /// </summary>
        protected static int MaxTextureArrays;

        static TextureArray()
        {
            MaxTextureArrays = GL.GetInteger(GetPName.MaxArrayTextureLayers);
        }

        internal readonly int Texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureArray"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="layerCount">The number of layers for this texture array.</param>
        /// <param name="levelCount">The number of mip-map levels</param>
        /// <param name="format">The pixel format to use on GPU side.</param>
        protected TextureArray(GraphicsDevice graphicsDevice, int layerCount = 1, int levelCount = 1,
            PixelInternalFormat format = PixelInternalFormat.Rgba8)
            : base(graphicsDevice, levelCount, format)
        {
            GraphicsDevice.ValidateUiGraphicsThread();

            LayerCount = layerCount;
            Texture = GL.GenTexture();
            GL.BindTexture(Target, Texture);
            SetDefaultTextureParameters();
        }

        private void SetDefaultTextureParameters()
        {
            GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        /// <summary>
        /// Gets or sets the number of layers for this texture array.
        /// </summary>
        public int LayerCount { get; protected set; }

        #region implemented abstract members of Texture

        internal override void Bind()
        {
            GL.BindTexture(Target, Texture);
        }

        /// <inheritdoc />
        public override void BindComputation(int unit = 0)
        {
            GL.BindImageTexture(unit, Texture, 0, false, 0, TextureAccess.WriteOnly,
                (SizedInternalFormat)Format);
        }

        /// <inheritdoc />
        public override bool Equals(Texture? other)
        {
            return other is TextureArray array && Equals(array);
        }

        #endregion

        /// <inheritdoc />
        public bool Equals(TextureArray? other)
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
            return obj is TextureArray array && Equals(array);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Texture;
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