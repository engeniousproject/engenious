using System.Text;

namespace engenious.Graphics
{
	/// <summary>
	/// Describes a font glyph for the <see cref="SpriteFont"/> class.
	/// </summary>
	public class FontCharacter
	{
        /// <summary>
		/// Initializes a new instance of the <see cref="FontCharacter"/> class.
		/// </summary>
		/// <param name="character">The character depicted by the glyph.</param>
		/// <param name="glyph">The main glyph for this character.</param>
		/// <param name="advance">The glyph advance.</param>
        /// <param name="glyphLayers">The glyph layers used for multi layered character rendering.</param>
		public FontCharacter (Rune character, FontGlyph glyph, float advance, FontGlyph[] glyphLayers)
		{
			Character = character;
            Glyph = glyph;
			Advance = advance;
            GlyphLayers = glyphLayers;
        }

		/// <summary>
		/// Gets the character.
		/// </summary>
		public Rune Character{ get; }

        /// <summary>
		/// Gets a value indicating how much this character advances the caret.
		/// </summary>
		public float Advance{ get; }
        
        /// <summary>
        /// Gets the main glyph.
        /// </summary>
        public FontGlyph Glyph { get; }
        
        /// <summary>
        /// Gets the glyph layers used for multi layered character rendering.
        /// </summary>
        public FontGlyph[] GlyphLayers { get; }
	}
}

