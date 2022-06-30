using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace engenious.Graphics
{
    /// <summary>
    /// Helper struct to enumerate through runes of a char span.
    /// </summary>
    public readonly ref struct CharSpanRuneEnumerable
    {
        private readonly ReadOnlySpan<char> _text;
        /// <summary>
        /// Initializes a new instance of the <see cref="CharSpanRuneEnumerable"/> struct.
        /// </summary>
        /// <param name="text">The text to enumerate through.</param>
        public CharSpanRuneEnumerable(ReadOnlySpan<char> text)
        {
            _text = text;
        }

        /// <summary>
        /// The allocation free enumerator to enumerate through runes.
        /// </summary>
        public ref struct Enumerator
        {
            private readonly ReadOnlySpan<char> _text;
            private ReadOnlySpan<char> _currentPart;
            private (Rune current, Rune? next, int nextruneAdvance)? _current;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="text">The text to enumerate through.</param>
            public Enumerator(ReadOnlySpan<char> text)
            {
                _text = _currentPart = text;
                _current = null;
            }

            /// <inheritdoc cref="IEnumerator{T}.MoveNext" />
            public bool MoveNext()
            {
                Rune? firstRune = null;
                if (_current is null)
                {
                    if (_currentPart.Length > 0)
                    {
                        if (Rune.DecodeFromUtf16(_currentPart, out var rune, out var read) != OperationStatus.Done)
                            throw new Exception("Invalid string data");
                        firstRune = rune;
                        _currentPart = _currentPart[read..];
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    firstRune = _current.Value.next;
                    _currentPart = _currentPart[_current.Value.nextruneAdvance..];
                }

                if (firstRune is null)
                {
                    return false;
                }

                Rune? nextRune = null;
                int nextRuneAdvance = 0;
                if (_currentPart.Length > 0)
                {
                    if(Rune.DecodeFromUtf16(_currentPart, out var tmpRune, out nextRuneAdvance) != OperationStatus.Done)
                        throw new Exception("Invalid string data");
                    nextRune = tmpRune;
                }

                _current = (firstRune.Value, nextRune, nextRuneAdvance);
                return true;
            }

            /// <inheritdoc cref="IEnumerator{T}.Reset" />
            public void Reset()
            {
                _currentPart = _text;
                _current = null;
            }

            /// <inheritdoc cref="IEnumerator{T}.Current" />
            public (Rune current, Rune? next) Current => _current is null ? default : (_current.Value.current, _current.Value.next);

            /// <inheritdoc cref="IEnumerator{T}.Dispose" />
            public void Dispose()
            {
            }
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(_text);
        }
    }
}