using System;
using System.Drawing;
using OpenTK;

namespace engenious.Graphics
{
	public class FontCharacter
	{
		public FontCharacter (char character, Rectangle textureSize, Rectangle textureRegionPX, Vector2 offset, float advance)
		{
			this.Character = character;
			this.TextureRegion = new RectangleF ((float)textureRegionPX.X / textureSize.Width, (float)textureRegionPX.Y / textureSize.Height, (float)textureRegionPX.Width / textureSize.Width, (float)textureRegionPX.Height / textureSize.Height);
			this.Offset = offset;
			this.Advance = advance;
		}

		public FontCharacter (char character, RectangleF textureRegion, Vector2 offset, float advance)
		{
			this.Character = character;
			this.TextureRegion = textureRegion;
			this.Offset = offset;
			this.Advance = advance;
		}

		public char Character{ get; private set; }

		public RectangleF TextureRegion{ get; private set; }

		public Vector2 Offset{ get; private set; }

		public float Advance{ get; private set; }
	}
}

