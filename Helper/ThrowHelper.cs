using System;

namespace engenious.Helper
{
    internal static class ThrowHelper
    {
        internal static void ThrowNotOnGraphicsThreadException()
        {
            throw new GraphicsThreadException();
        }

        public static void ThrowArgumentOutOfRangeException(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }
    }
}