namespace engenious.Graphics
{
    /// <summary>
    /// Color palette for multicolor glyph rendering.
    /// </summary>
    public class FontPalette
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FontPalette"/> class.
        /// </summary>
        /// <param name="paletteSize">The number of colors in the palette.</param>
        public FontPalette(int paletteSize)
        {
            Colors = new Color[paletteSize];
        }
        /// <summary>
        /// Gets the color of this palette.
        /// </summary>
        public Color[] Colors { get; }
    }
}