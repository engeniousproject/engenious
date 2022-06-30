using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace engenious.Graphics
{
    /// <summary>
    /// Helper struct to enumerate through runes of a <see cref="StringBuilder"/>.
    /// </summary>
    public readonly struct StringBuilderRuneEnumerable : IEnumerable<(Rune current, Rune? next)>
    {
        private readonly StringBuilder _text;
        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilderRuneEnumerable"/> struct.
        /// </summary>
        /// <param name="text">The text to enumerate through.</param>
        public StringBuilderRuneEnumerable(StringBuilder text)
        {
            _text = text;
        }

        /// <summary>
        /// The allocation free enumerator to enumerate through runes.
        /// </summary>
        public struct Enumerator : IEnumerator<(Rune current, Rune? next)>
        {
            private readonly StringBuilder _text;
            private int _startIndex;
            private (Rune current, Rune? next, int nextruneAdvance)? _current;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="text">The text to enumerate through.</param>
            public Enumerator(StringBuilder text)
            {
                _text = text;
                _startIndex = 0;
                _current = null;
            }

            private (char? f, char? s, int length) GetTwoChars(int index)
            {
                if (index >= _text.Length)
                    return (null, null, 0);
                
                if (index + 1 < _text.Length)
                    return (_text[index], _text[index + 1], 2);
                return (_text[index], null, 1);
            }

            private static void WriteTo(Span<char> chars, char? f, char? s)
            {
                if (f is not null)
                    chars[0] = f.Value;
                if (s is not null)
                    chars[1] = s.Value;
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                Rune? firstRune = null;
                if (_current is null)
                {
                    if (_text.Length - _startIndex > 0)
                    {
                        var twoChars = GetTwoChars(_startIndex);
                        Span<char> currentSpan = stackalloc char[twoChars.length];
                        WriteTo(currentSpan, twoChars.f, twoChars.s);
                        
                        if (Rune.DecodeFromUtf16(currentSpan, out var rune, out var read) != OperationStatus.Done)
                            throw new Exception("Invalid string data");
                        firstRune = rune;
                        _startIndex += read;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    firstRune = _current.Value.next;
                    _startIndex += _current.Value.nextruneAdvance;
                }

                if (firstRune is null)
                {
                    return false;
                }

                Rune? nextRune = null;
                int nextRuneAdvance = 0;
                if (_text.Length - _startIndex > 0)
                {
                    var twoChars = GetTwoChars(_startIndex);
                    Span<char> currentSpan = stackalloc char[twoChars.length];
                    WriteTo(currentSpan, twoChars.f, twoChars.s);
                    if(Rune.DecodeFromUtf16(currentSpan, out var tmpRune, out nextRuneAdvance) != OperationStatus.Done)
                        throw new Exception("Invalid string data");
                    nextRune = tmpRune;
                }

                _current = (firstRune.Value, nextRune, nextRuneAdvance);
                return true;
            }

            /// <inheritdoc />
            public void Reset()
            {
                _startIndex = 0;
                _current = null;
            }

            object IEnumerator.Current => Current;

            /// <inheritdoc />
            public (Rune current, Rune? next) Current => _current is null ? default : (_current.Value.current, _current.Value.next);

            /// <inheritdoc />
            public void Dispose()
            {
            }
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(_text);
        }

        IEnumerator<(Rune current, Rune? next)> IEnumerable<(Rune current, Rune? next)>.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}