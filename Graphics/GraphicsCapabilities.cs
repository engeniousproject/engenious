using OpenTK.Graphics.OpenGL;

namespace engenious.Graphics
{
    /// <summary>
    /// A class for reading capabilities of a <see cref="GraphicsDevice"/>.
    /// </summary>
    public class GraphicsCapabilities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsDevice"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to check capabilities for.</param>
        public GraphicsCapabilities(GraphicsDevice graphicsDevice)
        {
            SupportsTextureFilterAnisotropic = graphicsDevice.Extensions.Contains("GL_EXT_texture_filter_anisotropic");
            float anisotropy = 0;
            if (SupportsTextureFilterAnisotropic)
            {
                GL.GetFloat((GetPName)All.MaxTextureMaxAnisotropyExt,out anisotropy);
            }
            MaxTextureAnisotropy = anisotropy;

            SupportsTextureMaxLevel = true;//TODO other platforms?
        }

        /// <summary>
        /// Gets a value indicating whether anisotropic texture filtering is supported.
        /// </summary>
        public bool SupportsTextureFilterAnisotropic { get; }

        /// <summary>
        /// Gets a value indicating the maximum supported texture anisotropy.
        /// </summary>
        public float MaxTextureAnisotropy { get; }

        /// <summary>
        /// Gets a value indicating whether setting a mip-map maximum level is supported.
        /// </summary>
        public bool SupportsTextureMaxLevel { get; }
    }
}