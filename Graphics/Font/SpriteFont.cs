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
        internal Dictionary<int,int> Kernings;
        internal Dictionary<char,FontCharacter> CharacterMap;
        internal Texture2D Texture;

        internal SpriteFont(Texture2D texture)
        {
            Texture = texture;
            Kernings = new Dictionary<int, int>();
            CharacterMap = new Dictionary<char, FontCharacter>();
        }

        internal static int GetKerningKey(char first, char second)
        {
            return (first << 16) | second;
        }

        private ReadOnlyCollection<char> _characters;

        /// <summary>
        /// Gets a list of supported characters.
        /// </summary>
        public ReadOnlyCollection<char> Characters => _characters ?? (_characters = new ReadOnlyCollection<char>(CharacterMap.Keys.ToList()));

        /// <summary>
        /// Gets or sets the default character to render if a specific character cannot be depicted by this <see cref="SpriteFont"/>.
        /// </summary>
        public char? DefaultCharacter { get; set; }

        /// <summary>
        /// Gets or sets the vertical spacing between lines.
        /// </summary>
        public int LineSpacing { get; set; }

        /// <summary>
        /// Gets the base line position.
        /// </summary>
        public int BaseLine{ get; internal set; }

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
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (!CharacterMap.TryGetValue(c, out var fontChar))
                {
                    if (!DefaultCharacter.HasValue || !CharacterMap.TryGetValue(DefaultCharacter.Value, out fontChar))
                    {
                        continue;
                    }
                }

                if (fontChar == null)
                    continue;
                width += fontChar.Advance;
                if (i < text.Length - 1)
                {
                    if (Kernings.TryGetValue(GetKerningKey(c, text[i + 1]), out var kerning))
                        width += kerning;
                }
            }
            return new Vector2(width, LineSpacing);//TODO height?
        }

        /// <summary>
        /// Measures the dimensions needed to render a string with this <see cref="SpriteFont"/>.
        /// </summary>
        /// <remarks>Currently does not account for line breaks.</remarks>
        /// <param name="text">The text to measure.</param>
        /// <returns>The dimensions of the string when rendered with this <see cref="SpriteFont"/>.</returns>
        public Vector2 MeasureString(string text)
        {
            return MeasureString(text, 0, text.Length);
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
            return MeasureString(text, startIndex, text.Length - startIndex);
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
            var width = 0.0f;
            for (var i = startIndex; i < startIndex + length; i++)
            {
                var c = text[i];
                if (!CharacterMap.TryGetValue(c, out var fontChar))
                {
                    if (!DefaultCharacter.HasValue || !CharacterMap.TryGetValue(DefaultCharacter.Value, out fontChar))
                    {
                        continue;
                    }
                }

                if (fontChar == null)
                    continue;
                width += fontChar.Advance;
                if (i < text.Length - 1)
                {
                    if (Kernings.TryGetValue(GetKerningKey(c, text[i + 1]), out var kerning))
                        width += kerning;
                }
            }
            return new Vector2(width, LineSpacing);//TODO height?
        }

        /// <summary>
        /// Measures the dimensions needed to render a string with this <see cref="SpriteFont"/>.
        /// </summary>
        /// <remarks>Currently does not account for line breaks.</remarks>
        /// <param name="text">The text to measure.</param>
        /// <param name="startIndex">The index to start measuring from.</param>
        /// <param name="length">The length to measure.</param>
        /// <returns>The dimensions of the string when rendered with this <see cref="SpriteFont"/>.</returns>
        public Vector2 MeasureString(ReadOnlySpan<char> text)
        {
            var width = 0.0f;
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (!CharacterMap.TryGetValue(c, out var fontChar))
                {
                    if (!DefaultCharacter.HasValue || !CharacterMap.TryGetValue(DefaultCharacter.Value, out fontChar))
                    {
                        continue;
                    }
                }

                if (fontChar == null)
                    continue;
                width += fontChar.Advance;
                if (i < text.Length - 1)
                {
                    if (Kernings.TryGetValue(GetKerningKey(c, text[i + 1]), out var kerning))
                        width += kerning;
                }
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

