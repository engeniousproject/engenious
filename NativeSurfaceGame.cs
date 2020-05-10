using System;
using engenious.Helper;
using OpenTK;
using OpenTK.Graphics;

namespace engenious
{
    /// <inheritdoc />
    public abstract class NativeSurfaceGame : Game<NativeSurfaceWrapper>
    {
        private void ConstructContext()
        {
            var windowInfo = Control.WindowInfo;
            _context = Control.Context;

            _context.MakeCurrent(windowInfo);
            (_context as IGraphicsContextInternal)?.LoadAll();
            
        }

        private void CreateSharedContext()
        {
            if (GraphicsDevice?.DriverVendor == null || GraphicsDevice.DriverVendor.IndexOf("amd", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                var secondwindow = new GameWindow();
                ThreadingHelper.Initialize(_context,secondwindow.WindowInfo, 0,0, ContextFlags);
            }
            else
            {
                ThreadingHelper.Initialize(_context,Control.WindowInfo, 0,0, ContextFlags);
            }
            _context.MakeCurrent(Control.WindowInfo);
        }
    }
}