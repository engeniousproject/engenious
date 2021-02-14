using System;
using engenious.Helper;
using OpenTK.Windowing.Desktop;

namespace engenious
{
    /// <inheritdoc />
    public abstract class NativeSurfaceGame : Game<NativeSurfaceWrapper>
    {
        private void ConstructContext()
        {
            var windowInfo = Control.WindowInfo;
            Context = Control.Context;

            Context.MakeCurrent();
            
        }
    }
}