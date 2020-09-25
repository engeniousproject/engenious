using System.Diagnostics.CodeAnalysis;

namespace engenious.Content.Serialization
{
    /// <summary>
    /// Specifies the texture formats of content texture files.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum TextureContentFormat
    {
        /// <summary>
        /// Png texture format.
        /// </summary>
        Png = 0,
        /// <summary>
        /// Jpeg texture format.
        /// </summary>
        Jpg = 1,
        /// <summary>
        /// Compressed RGBA S3TC-DXT1 texture format.
        /// </summary>
        DXT1 = OpenTK.Graphics.OpenGL.PixelInternalFormat.CompressedRgbaS3tcDxt1Ext,
        /// <summary>
        /// Compressed RGBA S3TC-DXT3 texture format.
        /// </summary>
        DXT3 = OpenTK.Graphics.OpenGL.PixelInternalFormat.CompressedRgbaS3tcDxt3Ext,
        /// <summary>
        /// Compressed RGBA S3TC-DXT5 texture format.
        /// </summary>
        DXT5 = OpenTK.Graphics.OpenGL.PixelInternalFormat.CompressedRgbaS3tcDxt5Ext,
    }
}

