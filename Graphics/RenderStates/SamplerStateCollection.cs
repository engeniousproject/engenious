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

        /// <inheritdoc cref="GraphicsResource.GraphicsDevice"/>
        public new GraphicsDevice GraphicsDevice => base.GraphicsDevice!;

        /// <summary>
        /// Gets an element at the specified index.
        /// </summary>
        /// <param name="index">The index to get the element at.</param>
        public SamplerState? this [int index]
        {
            get => _samplerStates[index];
            set
            {
                if (_samplerStates[index] != value)
                {
                    _samplerStates[index] = value ?? SamplerState.LinearClamp;
                    GL.ActiveTexture(TextureUnit.Texture0 + index);
                    var texture = GraphicsDevice.Textures[index];
                    if (texture != null)
                        _samplerStates[index].Bind(texture);
                    
                }
            }
        }
    }
}

