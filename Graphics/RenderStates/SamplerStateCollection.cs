using engenious.Graphics;
using OpenTK.Graphics.OpenGL;

namespace engenious
{
    /// <summary>
    /// A collection of the <see cref="SamplerState"/> class.
    /// </summary>
    public class SamplerStateCollection : GraphicsResource
    {
        private readonly SamplerState[] _samplerStates;

        internal SamplerStateCollection(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            var maxTextures = GL.GetInteger(GetPName.MaxTextureImageUnits);
            _samplerStates = new SamplerState[maxTextures];
        }

        /// <summary>
        /// Gets an element at the specified index.
        /// </summary>
        /// <param name="index">The index to get the element at.</param>
        public SamplerState this [int index]
        {
            get { return _samplerStates[index]; }
            set
            {
                if (_samplerStates[index] != value)
                {
                    _samplerStates[index] = value ?? SamplerState.LinearClamp;
                    GL.ActiveTexture(TextureUnit.Texture0 + index);
                    if (GraphicsDevice.Textures[index] != null)
                        _samplerStates[index].Bind(GraphicsDevice.Textures[index]);
                    
                }
            }
        }
    }
}

