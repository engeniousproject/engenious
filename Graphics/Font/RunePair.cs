using System;
using System.Text;

namespace engenious.Graphics
{
    internal readonly struct RunePair : IEquatable<RunePair>
    {
        public RunePair(Rune first, Rune second)
        {
            First = first;
            Second = second;
        }

        public Rune First { get; }
        public Rune Second { get; }

        public bool Equals(RunePair other)
        {
            return First.Equals(other.First) && Second.Equals(other.Second);
        }

        public override bool Equals(object? obj)
        {
            return obj is RunePair other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ((ulong)First.Value << 32 | (uint)Second.Value).GetHashCode();
        }

        public override string ToString()
        {
            return $"{First} - {Second}";
        }
    }
}