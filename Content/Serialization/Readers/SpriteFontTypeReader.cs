using System;
using engenious.Graphics;

namespace engenious.Content.Serialization
{
    /// <summary>
    /// Content type reader to load <see cref="SpriteFont"/> instances.
    /// </summary>
    [ContentTypeReaderAttribute(typeof(SpriteFont))]
    public class SpriteFontTypeReader : ContentTypeReader<SpriteFont>
    {
        private FontGlyph ReadGlyph(ContentReader reader)
        {
            var textRegion = new RectangleF(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle());
            var offset = reader.ReadVector2();
            var size = reader.ReadVector2();
            var colorIndex = reader.ReadInt32();
            return new FontGlyph(textRegion, offset, size, colorIndex);
        }
        /// <inheritdoc />
        public override SpriteFont? Read(ContentManagerBase managerBase, ContentReader reader, Type? customType = null)
        {
            var texture = reader.Read<Texture2D>(managerBase);
            if (texture == null)
                return null;
            var font = new SpriteFont(texture)
            {
                Spacing = reader.ReadSingle(),
                LineSpacing = reader.ReadSingle(),
                BaseLine = reader.ReadSingle(),
                FontType = (SpriteFontType)reader.ReadByte()
            };
            var hasDefaultChar = reader.ReadBoolean();
            if (hasDefaultChar)
                font.DefaultCharacter = reader.ReadRune();
            else
                font.DefaultCharacter = null;

            var kerningCount = reader.ReadInt32();

            for (var i = 0; i < kerningCount; i++)
            {
                var key = new RunePair(reader.ReadRune(), reader.ReadRune());
                var kerning = reader.ReadSingle();
                font.Kernings.Add(key, kerning);
            }
            var characterMapCount = reader.ReadInt32();
            for (var i = 0; i < characterMapCount; i++)
            {
                var key = reader.ReadRune();
                float advance = reader.ReadSingle();
                var glyph = ReadGlyph(reader);
                var glyphLayers = new FontGlyph[reader.ReadInt32()];
                for (int j = 0; j < glyphLayers.Length; j++)
                {
                    glyphLayers[j] = ReadGlyph(reader);
                }
                var fntChar = new FontCharacter(key, glyph, advance, glyphLayers);
                font.CharacterMap.Add(key, fntChar);
            }

            var paletteCount = reader.ReadInt32();
            for (var i = 0; i < paletteCount; i++)
            {
                var colorCount = reader.ReadInt32();
                var palette = new FontPalette(colorCount);
                for (var j = 0; j < colorCount; j++)
                {
                    palette.Colors[j] = reader.ReadColor();
                }
                font.Palettes.Add(palette);
            }

            return font;
        }

        /// <inheritdoc />
        public SpriteFontTypeReader() : base(2)
        {
        }
    }
}

