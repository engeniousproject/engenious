using System;
using OpenTK.Graphics.OpenGL4;

namespace engenious.Graphics
{
    public abstract class Texture:GraphicsResource
    {
        public Texture(GraphicsDevice graphicsDevice, int levelCount = 1, PixelInternalFormat format = PixelInternalFormat.Rgba8)
            : base(graphicsDevice)
        {
            this.LevelCount = levelCount;
            this.Format = format;
        }

        private SamplerState samplerState;
        public SamplerState SamplerState{
            get{
                return samplerState;
            }
            set{
                if (value == null)
                    samplerState = SamplerState.LinearWrap;
                else
                    samplerState = value;
                SetSampler(samplerState);
            }
        }

        public PixelInternalFormat Format { get; protected set; }

        public int LevelCount { get; protected set; }

        internal abstract void SetSampler(SamplerState state);

        internal abstract void Bind();
    }
}

