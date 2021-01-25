using System;
using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// Abstract base class for GPU textures.
    /// </summary>
    public abstract class Texture : GraphicsResource, IEquatable<Texture>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="levelCount">The number of mip-map levels.</param>
        /// <param name="format">The pixel format to use on GPU side.</param>
        protected Texture(GraphicsDevice graphicsDevice, int levelCount = 1, PixelInternalFormat format = PixelInternalFormat.Rgba8)
            : base(graphicsDevice)
        {
            LevelCount = levelCount;
            Format = format;
        }

        private SamplerState _samplerState;

        /// <summary>
        /// Gets or sets the sampler state to use for rendering of this texture.
        /// </summary>
        public SamplerState SamplerState{
            get{
                return _samplerState;
            }
            set
            {
                if (_samplerState == value)
                    return;
                _samplerState?.Unbind();
                var samplerState = value ?? SamplerState.LinearWrap;
                samplerState.Bind(this);
                _samplerState = samplerState;
            }
        }

        internal abstract TextureTarget Target { get; }

        /// <summary>
        /// Gets or sets the pixel format to use on the GPU.
        /// </summary>
        public PixelInternalFormat Format { get; protected set; }

        /// <summary>
        /// Gets or sets the number of mip-map levels.
        /// </summary>
        public int LevelCount { get; protected set; }

        internal abstract void Bind();

        /// <summary>
        /// Binds this texture for use in computation shaders.
        /// </summary>
        /// <param name="unit">The unit to bind this texture to.</param>
        public abstract void BindComputation(int unit=0);

        /// <inheritdoc />
        public abstract bool Equals(Texture other);
    }
}

