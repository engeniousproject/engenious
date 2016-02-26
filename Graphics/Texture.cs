using System;
using OpenTK.Graphics.OpenGL4;

namespace engenious.Graphics
{
    public abstract class Texture:GraphicsResource
    {
        public Texture(GraphicsDevice graphicsDevice, int levelCount = 1, PixelFormat format = PixelFormat.Rgba)
            : base(graphicsDevice)
        {
            this.LevelCount = levelCount;
            this.Format = format;
        }

        public PixelFormat Format { get; protected set; }

        public int LevelCount { get; protected set; }

        internal abstract void SetSampler(SamplerState state);

        internal abstract void Bind();
    }
}

