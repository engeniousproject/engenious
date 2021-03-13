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
		/// <param name="textureSize">The size of the texture(pixels) where the font glyph is located on.</param>
		/// <param name="textureRegionPx">The texture region(pixels) where the font glyph is located.</param>
		/// <param name="offset">The glyph offset.</param>
		/// <param name="size">The glyph size.</param>
		/// <param name="advance">The glyph advance</param>
		public FontCharacter (char character, Rectangle textureSize, Rectangle textureRegionPx, Vector2 offset, Vector2 size, float advance)
		{
			Character = character;
			TextureRegion = new RectangleF ((float)(textureRegionPx.X) / textureSize.Width, (float)(textureRegionPx.Y) / textureSize.Height, (float)(textureRegionPx.Width) / textureSize.Width, (float)(textureRegionPx.Height) / textureSize.Height);
			Offset = offset;
			Size = size;
			Advance = advance;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FontCharacter"/> class.
		/// </summary>
		/// <param name="character">The character depicted by the glyph.</param>
		/// <param name="textureRegion">The texture region(texels) where the font glyph is located.</param>
		/// <param name="offset">The glyph offset.</param>
		/// <param name="size">The glyph size.</param>
		/// <param name="advance">The glyph advance</param>
		public FontCharacter (char character, RectangleF textureRegion, Vector2 offset, Vector2 size, float advance)
		{
			Character = character;
			TextureRegion = textureRegion;
			Offset = offset;
			Size = size;
			Advance = advance;
		}

		/// <summary>
		/// Gets the character.
		/// </summary>
		public char Character{ get; }

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
		/// Gets a value indicating how much this character advances the caret.
		/// </summary>
		public float Advance{ get; private set; }
	}
}

