using System;
using engenious.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace engenious
{
    public class SamplerStateCollection : GraphicsResource
    {
        private SamplerState[] samplerStates;

        internal SamplerStateCollection(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            int maxTextures = GL.GetInteger(GetPName.MaxTextureImageUnits);
            samplerStates = new SamplerState[maxTextures];
        }

        public SamplerState this [int index]
        {
            get { return samplerStates[index]; }
            set
            {
                if (samplerStates[index] != value)
                {
                    samplerStates[index] = value == null ? SamplerState.LinearClamp : value;
                    GL.ActiveTexture(TextureUnit.Texture0 + index);
                    if (GraphicsDevice.Textures[index] != null)
                        GraphicsDevice.Textures[index].SetSampler(value);
                    
                }
            }
        }
    }
}

