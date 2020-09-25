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
            _context = Control.Context;

            _context.MakeCurrent();
            
        }

        private void CreateSharedContext()
        {
            if (GraphicsDevice?.DriverVendor == null || GraphicsDevice.DriverVendor.IndexOf("amd", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                var secondwindow = new GameWindow(GameWindowSettings.Default, NativeWindowSettings.Default);
                ThreadingHelper.Initialize(_context,secondwindow);
            }
            else
            {
                ThreadingHelper.Initialize(_context,(NativeWindow)Control.WindowInfo);
            }
            _context.MakeCurrent();
        }
    }
}