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
                font.DefaultCharacter = reader.ReadChar();
            else
                font.DefaultCharacter = null;

            var kerningCount = reader.ReadInt32();

            for (var i = 0; i < kerningCount; i++)
            {
                var key = reader.ReadInt32();
                var kerning = reader.ReadSingle();
                font.Kernings.Add(key, kerning);
            }
            var characterMapCount = reader.ReadInt32();
            for (var i = 0; i < characterMapCount; i++)
            {
                var key = reader.ReadChar();
                var offset = reader.ReadVector2();
                var size = reader.ReadVector2();
                var fntChar = new FontCharacter(key, new RectangleF(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()), offset, size, reader.ReadSingle());
                font.CharacterMap.Add(key, fntChar);
            }

            return font;
        }

        /// <inheritdoc />
        public SpriteFontTypeReader() : base(1)
        {
        }
    }
}

