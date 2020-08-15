namespace engenious.Helper
{
    internal static class ThrowHelper
    {
        internal static void ThrowNotOnGraphicsThreadException()
        {
            throw new GraphicsThreadException();
        }
    }
}