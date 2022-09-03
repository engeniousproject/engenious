namespace engenious.Graphics
{
    /// <summary>
    /// A single renderable glyph.
    /// </summary>
    public class FontGlyph
    {
        /// <summary>Initializes a new instance of the <see cref="FontGlyph"/> class.</summary>
        /// <param name="textureSize">The texture size of the font glyph atlas.</param>
        /// <param name="textureRegionPx">The texture region the glyph is found at in pixels.</param>
        /// <param name="offset">The offsets of this character.</param>
        /// <param name="size">The glyph size.</param>
        /// <param name="colorIndex">The color index used for drawing this glyph.</param>
        public FontGlyph(Rectangle textureSize, Rectangle textureRegionPx, Vector2 offset, Vector2 size, int colorIndex)
            : this(new RectangleF ((float)(textureRegionPx.X) / textureSize.Width, (float)(textureRegionPx.Y) / textureSize.Height, (float)(textureRegionPx.Width) / textureSize.Width, (float)(textureRegionPx.Height) / textureSize.Height), offset, size, colorIndex)
        {
            
        }

        /// <summary>Initializes a new instance of the <see cref="FontGlyph"/> class.</summary>
        /// <param name="textureRegion">The texture region the glyph is found at in uv-coordinates.</param>
        /// <param name="offset">The offsets of this character.</param>
        /// <param name="size">The glyph size.</param>
        /// <param name="colorIndex">The color index used for drawing this glyph.</param>
        public FontGlyph(RectangleF textureRegion, Vector2 offset, Vector2 size, int colorIndex)
        {
            TextureRegion = textureRegion;
            Offset = offset;
            Size = size;
            ColorIndex = colorIndex;
        }

        /// <summary>
        /// Gets the texture region the glyph is found at.
        /// </summary>
        public RectangleF TextureRegion{ get; }

        /// <summary>
        /// Gets the offsets of this character.
        /// </summary>
        public Vector2 Offset{ get; }

        /// <summary>
        /// Gets the size of this character.
        /// </summary>
        public Vector2 Size { get; }
        
        /// <summary>
        /// Gets the color index used for drawing this glyph.
        /// <remarks>Use foreground color when set to <c>-1</c>.</remarks>
        /// </summary>
        public int ColorIndex { get; }
    }
}