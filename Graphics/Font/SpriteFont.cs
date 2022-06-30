using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a sprite font for text rendering.
    /// </summary>
    public sealed class SpriteFont : IDisposable
    {
        internal Dictionary<RunePair, float> Kernings;
        internal Dictionary<Rune, FontCharacter> CharacterMap;
        internal Texture2D Texture;
        internal SpriteFontType FontType;
        private ReadOnlyCollection<Rune>? _characters;

        internal SpriteFont(Texture2D texture)
        {
            Texture = texture;
            Kernings = new Dictionary<RunePair, float>();
            CharacterMap = new Dictionary<Rune, FontCharacter>();
        }

        /// <summary>
        /// Gets a list of supported characters.
        /// </summary>
        public ReadOnlyCollection<Rune> Characters => _characters ??= new ReadOnlyCollection<Rune>(CharacterMap.Keys.ToList());

        /// <summary>
        /// Gets or sets the default character to render if a specific character cannot be depicted by this <see cref="SpriteFont"/>.
        /// </summary>
        public Rune? DefaultCharacter { get; set; }

        /// <summary>
        /// Gets or sets the vertical spacing between lines.
        /// </summary>
        public float LineSpacing { get; set; }

        /// <summary>
        /// Gets the base line position.
        /// </summary>
        public float BaseLine{ get; internal set; }

        /// <summary>
        /// Gets or sets the horizontal spacing between characters.
        /// </summary>
        public float Spacing { get; set; }

        /// <summary>
        /// Measures the dimensions needed to render a string with this <see cref="SpriteFont"/>.
        /// </summary>
        /// <remarks>Currently does not account for line breaks.</remarks>
        /// <param name="text">The text to measure.</param>
        /// <returns>The dimensions of the string when rendered with this <see cref="SpriteFont"/>.</returns>
        public Vector2 MeasureString(StringBuilder text)
        {
            var width = 0.0f;
            foreach (var (rune, nextRune) in new StringBuilderRuneEnumerable(text))
            {
                width = CalculateCharacterWidth(rune, nextRune, width);
            }
            return new Vector2(width, LineSpacing);//TODO height?
        }

        private float CalculateCharacterWidth(Rune c, Rune? nextChar, float width)
        {
            if (!CharacterMap.TryGetValue(c, out var fontChar))
            {
                if (!DefaultCharacter.HasValue || !CharacterMap.TryGetValue(DefaultCharacter.Value, out fontChar))
                {
                    return width;
                }
            }

            width += fontChar.Advance;
            if (nextChar is not null && Kernings.TryGetValue(new RunePair(c, nextChar.Value), out var kerning))
                width += kerning;

            return width;
        }

        /// <summary>
        /// Measures the dimensions needed to render a string with this <see cref="SpriteFont"/>.
        /// </summary>
        /// <remarks>Currently does not account for line breaks.</remarks>
        /// <param name="text">The text to measure.</param>
        /// <returns>The dimensions of the string when rendered with this <see cref="SpriteFont"/>.</returns>
        public Vector2 MeasureString(string text)
        {
            return MeasureString(text.AsSpan());
        }

        /// <summary>
        /// Measures the dimensions needed to render a string with this <see cref="SpriteFont"/>.
        /// </summary>
        /// <remarks>Currently does not account for line breaks.</remarks>
        /// <param name="text">The text to measure.</param>
        /// <param name="startIndex">The index to start measuring from.</param>
        /// <returns>The dimensions of the string when rendered with this <see cref="SpriteFont"/>.</returns>
        public Vector2 MeasureString(string text, int startIndex)
        {
            return MeasureString(text.AsSpan(startIndex));
        }
        /// <summary>
        /// Measures the dimensions needed to render a string with this <see cref="SpriteFont"/>.
        /// </summary>
        /// <remarks>Currently does not account for line breaks.</remarks>
        /// <param name="text">The text to measure.</param>
        /// <param name="startIndex">The index to start measuring from.</param>
        /// <param name="length">The length to measure.</param>
        /// <returns>The dimensions of the string when rendered with this <see cref="SpriteFont"/>.</returns>
        public Vector2 MeasureString(string text, int startIndex, int length)
        {
            return MeasureString(text.AsSpan(startIndex, length));
        }

        /// <summary>
        /// Measures the dimensions needed to render a string with this <see cref="SpriteFont"/>.
        /// </summary>
        /// <remarks>Currently does not account for line breaks.</remarks>
        /// <param name="text">The text to measure.</param>
        /// <returns>The dimensions of the string when rendered with this <see cref="SpriteFont"/>.</returns>
        public Vector2 MeasureString(ReadOnlySpan<char> text)
        {
            var width = 0.0f;
            foreach (var (rune, nextRune) in new CharSpanRuneEnumerable(text))
            {
                width = CalculateCharacterWidth(rune, nextRune, width);
            }
            return new Vector2(width, LineSpacing);//TODO height?
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Texture?.Dispose();
        }
    }
}

